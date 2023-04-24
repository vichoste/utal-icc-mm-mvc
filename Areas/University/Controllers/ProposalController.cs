using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.University.Controllers;

[Area("University"), Authorize]
public class ProposalController : Controller {
	private readonly IccDbContext _dbContext;
	private readonly UserManager<IccUser> _userManager;

	public ProposalController(IccDbContext dbContext, UserManager<IccUser> userManager) {
		this._dbContext = dbContext;
		this._userManager = userManager;
	}

	private async Task PopulateAssistants(IccUser guide) {
		var assistants = (
			await this._userManager.GetUsersInRoleAsync("Assistant"))
				.Where(u => u != guide && !u.IsDeactivated)
				.OrderBy(u => u.LastName)
				.ToList();
		this.ViewData[$"Assistants"] = assistants.Select(u => new SelectListItem {
			Text = $"{u.FirstName} {u.LastName}",
			Value = u.Id
		});
	}

	[Authorize(Roles = "Student")]
	public async Task<IActionResult> Guides(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var parameters = new[] { "FirstName", "LastName", "Email", "Specialization" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
		} else {
			searchString = currentFilter;
		}
		this.ViewData["CurrentFilter"] = searchString;
		var users = (await this._userManager.GetUsersInRoleAsync("Guide"))
			.Where(u => !u.IsDeactivated)
				.Select(u => {
					var teacher = u as IccTeacher;
					return new IccTeacherViewModel {
						Id = teacher!.Id,
						FirstName = teacher.FirstName,
						LastName = teacher.LastName,
						Rut = teacher.Rut,
						Email = teacher.Email,
						Specialization = teacher.Specialization,
					};
				}
		).AsQueryable();
		var paginator = Paginator<IccUserViewModel>.Create(users, pageNumber ?? 1, 6);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccUserViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccUserViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Students(string id, string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = this._dbContext.IccTeacherMemoirs!
			.Include(m => m.GuideTeacher)
			.Include(m => m.Candidates)
			.FirstOrDefault(m => m.Id == id
				&& m.GuideTeacher!.Id == user.Id
				&& m.Phase == IccMemoir.MemoirPhase.Visible);
		this.ViewData["Memoir"] = memoir;
		var parameters = new[] { "FirstName", "LastName", "Email" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
		} else {
			searchString = currentFilter;
		}
		this.ViewData["CurrentFilter"] = searchString;
		var candidates = memoir!.Candidates!.Where(u => !u!.IsDeactivated).Select(u => new IccUserViewModel {
			Id = u!.Id,
			FirstName = u.FirstName,
			LastName = u.LastName,
			Email = u.Email,
		}).AsQueryable();
		var paginator = Paginator<IccUserViewModel>.Create(candidates, pageNumber ?? 1, 10);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccUserViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccUserViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		string[]? parameters = null;
		if (this.User.IsInRole("Student")) {
			parameters = new[] { "Title", "GuideName", "UpdatedAt" };
		} else if (this.User.IsInRole("Guide")) {
			parameters = new[] { "Title", "MemoristName", "UpdatedAt" };
		}
		foreach (var parameter in parameters!) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
		} else {
			searchString = currentFilter;
		}
		this.ViewData["CurrentFilter"] = searchString;
		IQueryable<IccMemoirViewModel> memoirs = null!;
		if (this.User.IsInRole("Student")) {
			memoirs = this._dbContext.IccStudentMemoirs!
				.Include(m => m.GuideTeacher)
				.Where(m => m.GuideTeacher!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide).AsNoTracking()
				.Select(m => new IccMemoirViewModel {
					Id = m.Id,
					Title = m.Title,
					Phase = m.Phase.ToString(),
					GuideName = $"{m.Guide!.FirstName} {m.Guide!.LastName}",
					UpdatedAt = m.UpdatedAt
				});
		} else if (this.User.IsInRole("Guide")) {
			memoirs = this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.Include(m => m.Memorist).AsNoTracking()
				.Select(m => new IccMemoirViewModel {
					Id = m.Id,
					Title = m.Title,
					Phase = m.Phase.ToString(),
					MemoristName = $"{m.Memorist!.FirstName} {m.Memorist!.LastName}",
					UpdatedAt = m.UpdatedAt
				});
		}
		var paginator = Paginator<IccMemoirViewModel>.Create(memoirs, pageNumber ?? 1, 6);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccMemoirViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccMemoirViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> List(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		string[]? parameters = null;
		if (this.User.IsInRole("Student")) {
			parameters = new[] { "Title", "GuideName", "UpdatedAt" };
		} else if (this.User.IsInRole("Guide")) {
			parameters = new[] { "Title", "MemoristName", "UpdatedAt" };
		}
		foreach (var parameter in parameters!) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
		} else {
			searchString = currentFilter;
		}
		this.ViewData["CurrentFilter"] = searchString;
		IQueryable<IccMemoirViewModel> memoirs = null!;
		if (this.User.IsInRole("Student")) {
			memoirs = this._dbContext.IccMemoirs!
				.Where(m => m.Phase == IccMemoir.MemoirPhase.PublishedByGuide)
				.Include(m => m.Guide).AsNoTracking()
				.Select(m => new IccMemoirViewModel {
					Id = m.Id,
					Title = m.Title,
					Phase = m.Phase.ToString(),
					GuideName = $"{m.Guide!.FirstName} {m.Guide!.LastName}",
					UpdatedAt = m.UpdatedAt
				});
		} else if (this.User.IsInRole("Guide")) {
			memoirs = this._dbContext.IccMemoirs!
				.Include(m => m.Guide)
				.Where(m => m.Guide!.Id == user.Id
					&& (m.Phase == IccMemoir.MemoirPhase.SentToGuide || m.Phase == IccMemoir.MemoirPhase.ApprovedByGuide))
				.Include(m => m.Memorist).AsNoTracking()
				.Select(m => new IccMemoirViewModel {
					Id = m.Id,
					Title = m.Title,
					Phase = m.Phase.ToString(),
					MemoristName = $"{m.Memorist!.FirstName} {m.Memorist!.LastName}",
					UpdatedAt = m.UpdatedAt
				});
		}
		var paginator = Paginator<IccMemoirViewModel>.Create(memoirs, pageNumber ?? 1, 6);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccMemoirViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccMemoirViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	[Authorize(Roles = "Student")]
	public async Task<IActionResult> Applications(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var parameters = new[] { "Title", "GuideName", "UpdatedAt" };
		foreach (var parameter in parameters!) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
		} else {
			searchString = currentFilter;
		}
		this.ViewData["CurrentFilter"] = searchString;
		var memoirs = this._dbContext.IccMemoirs!
			.Include(m => m.Memorist)
			.Where(m => (m!.Candidates!.Contains(user) || m.Memorist == user)
				&& (m.Phase == IccMemoir.MemoirPhase.PublishedByGuide || m.Phase == IccMemoir.MemoirPhase.ReadyByGuide)
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
				&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Guide).AsNoTracking()
			.Select(m => new IccMemoirViewModel {
				Id = m.Id,
				Title = m.Title,
				Phase = m.Phase.ToString(),
				GuideName = $"{m.Guide!.FirstName} {m.Guide!.LastName}",
				UpdatedAt = m.UpdatedAt
			}
		);
		var paginator = Paginator<IccMemoirViewModel>.Create(memoirs, pageNumber ?? 1, 6);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccMemoirViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccMemoirViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Create() {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		await this.PopulateAssistants(user!);
		return this.View(new IccMemoirViewModel());
	}

	[Authorize(Roles = "Student")]
	public async Task<IActionResult> CreateWithGuide(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var guide = await this._userManager.FindByIdAsync(id) as IccTeacher;
		if (guide is null) {
			this.TempData["ErrorMessage"] = "Error al obtener al profesor guía.";
			return this.RedirectToAction("Guide", "Proposal", new { area = "University" });
		}
		if (guide.IsDeactivated) {
			this.TempData["ErrorMessage"] = "El profesor guía está desactivado.";
			return this.RedirectToAction("Guide", "Proposal", new { area = "University" });
		}
		await this.PopulateAssistants(guide);
		var output = new IccStudentMemoirViewModel {
			GuideTeacher = new IccTeacherViewModel {
				Id = guide.Id,
				FirstName = guide.FirstName,
				LastName = guide.LastName,
				Email = guide.Email,
				Office = guide.Office,
				Schedule = guide.Schedule,
				Specialization = guide.Specialization,
			}
		};
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Create([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccUser? guide = null!;
		if (this.User.IsInRole("Student")) {
			guide = await this._userManager.FindByIdAsync(input.GuideTeacher!.Id);
			if (guide is null) {
				this.ViewData["ErrorMessage"] = "Error al obtener al profesor guía.";
				return this.RedirectToAction("Guide", "Proposal", new { area = "University" });
			}
			if (guide.IsDeactivated) {
				this.ViewData["ErrorMessage"] = "El profesor guía está desactivado.";
				return this.RedirectToAction("Guide", "Proposal", new { area = "University" });
			}
		} else if (this.User.IsInRole("Guide")) {
			guide = (await this._userManager.GetUserAsync(this.User))!;
		}
		var assistants = input.AssistantTeachers!.Select(async a => await this._userManager.FindByIdAsync(a!)).Select(a => a.Result).Where(u => !u!.IsDeactivated).ToList();
		var memoir = new Memoir {
			Id = Guid.NewGuid().ToString(),
			Title = input.Title,
			Description = input.Description,
			Owner = user,
			Guide = guide,
			Assistants = assistants,
			CreatedAt = DateTimeOffset.Now,
			UpdatedAt = DateTimeOffset.Now,
		};
		if (this.User.IsInRole("Student")) {
			memoir.Memorist = await this._userManager.GetUserAsync(this.User);
			memoir.Phase = IccMemoir.MemoirPhase.DraftByStudent;
		} else if (this.User.IsInRole("Guide")) {
			memoir.Requirements = input.Requirements;
			memoir.Phase = IccMemoir.MemoirPhase.DraftByGuide;
		}
		_ = await this._dbContext.IccMemoirs!.AddAsync(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu propouesta ha sido registrada correctamente.";
		return this.RedirectToAction("Index", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> Edit(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.Include(m => m.Assistants).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id && (m.Phase == IccMemoir.MemoirPhase.DraftByStudent || m.Phase == IccMemoir.MemoirPhase.RejectedByGuide));
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.Include(m => m.Assistants).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id && (m.Phase == IccMemoir.MemoirPhase.DraftByGuide || m.Phase == IccMemoir.MemoirPhase.RejectedByGuide));
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (memoir.Guide!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "El profesor guía está desactivado.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		await this.PopulateAssistants(memoir.Guide!);
		var output = new IccMemoirViewModel {
			Id = memoir.Id,
			Title = memoir.Title,
			Description = memoir.Description,
			Assistants = memoir.Assistants!.Select(a => a!.Id).ToList(),
			CreatedAt = memoir.CreatedAt,
			UpdatedAt = memoir.UpdatedAt
		};
		if (this.User.IsInRole("Student")) {
			output.GuideId = memoir.Guide!.Id;
			output.GuideName = $"{memoir.Guide!.FirstName} {memoir.Guide!.LastName}";
			output.GuideEmail = memoir.Guide!.Email;
			output.Office = memoir.Guide!.Office;
			output.Schedule = memoir.Guide!.Schedule;
			output.Specialization = memoir.Guide!.Specialization;
		} else if (this.User.IsInRole("Guide")) {
			output.Requirements = memoir.Requirements;
		}
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.Include(m => m.Assistants)
				.FirstOrDefaultAsync(m => m.Id == input.Id && (m.Phase == IccMemoir.MemoirPhase.DraftByStudent || m.Phase == IccMemoir.MemoirPhase.RejectedByGuide));
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Assistants)
				.FirstOrDefaultAsync(m => m.Id == input.Id && (m.Phase == IccMemoir.MemoirPhase.DraftByGuide || m.Phase == IccMemoir.MemoirPhase.RejectedByGuide));
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (memoir.Guide!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "El profesor guía está desactivado.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		var assistants = input.Assistants!.Select(async a => await this._userManager.FindByIdAsync(a!)).Select(a => a.Result).Where(u => !u!.IsDeactivated).ToList();
		var canEdit = true;
		foreach (var assistant in assistants) {
			if (assistant!.IsDeactivated) {
				this.ViewBag.WarningMessage = $"El profesor co-guía {assistant.FirstName} {assistant.LastName} está desactivado.";
				canEdit = false;
				break;
			}
		}
		if (canEdit) {
			memoir.Title = input.Title;
			memoir.Description = input.Description;
			if (this.User.IsInRole("Guide")) {
				memoir.Requirements = input.Requirements;
			}
			memoir.Assistants = assistants;
			if (memoir.Phase == IccMemoir.MemoirPhase.RejectedByGuide) {
				memoir.Phase = IccMemoir.MemoirPhase.SentToGuide;
			}
			memoir.UpdatedAt = DateTimeOffset.Now;
			_ = this._dbContext.IccMemoirs!.Update(memoir);
			_ = await this._dbContext.SaveChangesAsync();
		}
		await this.PopulateAssistants(memoir.Guide!);
		var output = new IccMemoirViewModel {
			Id = memoir.Id,
			Title = memoir.Title,
			Description = memoir.Description,
			Assistants = memoir.Assistants!.Select(a => a!.Id).ToList(),
			CreatedAt = memoir.CreatedAt,
			UpdatedAt = memoir.UpdatedAt
		};
		if (this.User.IsInRole("Student")) {
			output.GuideId = memoir.Guide!.Id;
			output.GuideName = $"{memoir.Guide!.FirstName} {memoir.Guide!.LastName}";
			output.GuideEmail = memoir.Guide!.Email;
			output.Office = memoir.Guide!.Office;
			output.Schedule = memoir.Guide!.Schedule;
			output.Specialization = memoir.Guide!.Specialization;
		} else if (this.User.IsInRole("Guide")) {
			output.Requirements = memoir.Requirements;
		}
		this.ViewBag.SuccessMessage = "Tu propuesta ha sido editada correctamente.";
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> Delete(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Id == id
					&& m.Phase == IccMemoir.MemoirPhase.DraftByStudent);
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Id == id
					&& m.Phase == IccMemoir.MemoirPhase.DraftByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
		};
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Delete([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.FirstOrDefaultAsync(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Id == input.Id
					&& m.Phase == IccMemoir.MemoirPhase.DraftByStudent);
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.FirstOrDefaultAsync(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Id == input.Id
					&& m.Phase == IccMemoir.MemoirPhase.DraftByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		_ = this._dbContext.IccMemoirs!.Remove(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu propuesta ha sido eliminada correctamente.";
		return this.RedirectToAction("Index", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> Send(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id && m.Phase == IccMemoir.MemoirPhase.DraftByStudent);
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id && m.Phase == IccMemoir.MemoirPhase.DraftByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (memoir.Guide!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "El profesor guía está desactivado.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
		};
		if (this.User.IsInRole("Student")) {
			output.GuideName = $"{memoir.Guide!.FirstName} {memoir.Guide!.LastName}";
		}
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Send([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.FirstOrDefaultAsync(m => m.Id == input.Id && m.Phase == IccMemoir.MemoirPhase.DraftByStudent);
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Owner)
				.Where(m => m.Owner!.Id == this._userManager.GetUserId(this.User)
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee && m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee && m.Phase != IccMemoir.MemoirPhase.InProgress
					&& m.Phase != IccMemoir.MemoirPhase.Abandoned && m.Phase != IccMemoir.MemoirPhase.Completed
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByDirector && m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Guide)
				.FirstOrDefaultAsync(m => m.Id == input.Id && m.Phase == IccMemoir.MemoirPhase.DraftByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (memoir.Guide!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "El profesor guía está desactivado.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (this.User.IsInRole("Student")) {
			memoir.Phase = IccMemoir.MemoirPhase.SentToGuide;
		} else if (this.User.IsInRole("Guide")) {
			memoir.Phase = IccMemoir.MemoirPhase.PublishedByGuide;
		}
		memoir.WhoRejected = null;
		memoir.Reason = string.Empty;
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		if (this.User.IsInRole("Student")) {
			this.TempData["SuccessMessage"] = "Tu propuesta ha sido enviada correctamente.";
		} else if (this.User.IsInRole("Guide")) {
			this.TempData["SuccessMessage"] = "Tu propuesta ha sido publicada correctamente.";
		}
		return this.RedirectToAction("Index", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Student,Guide")]
	public new async Task<IActionResult> View(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Where(m => m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Owner)
				.Include(m => m.Guide)
				.Include(m => m.WhoRejected)
				.Include(m => m.Assistants).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);
		} else if (this.User.IsInRole("Guide")) {
			memoir = await this._dbContext.IccMemoirs!
				.Where(m => m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.Include(m => m.Owner)
				.Include(m => m.Memorist)
				.Include(m => m.Assistants).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
			Description = memoir.Description,
			Phase = memoir.IccMemoir.MemoirPhase.ToString(),
			Assistants = memoir.Assistants!.Select(a => $"{a!.FirstName} {a.LastName}").ToList(),
			CreatedAt = memoir.CreatedAt,
			UpdatedAt = memoir.UpdatedAt,
		};
		if (this.User.IsInRole("Student")) {
			output.GuideId = memoir.Guide!.Id;
			output.GuideName = $"{memoir.Guide.FirstName} {memoir.Guide.LastName}";
			output.GuideEmail = memoir.Guide.Email;
			output.Office = memoir.Guide.Office;
			output.Schedule = memoir.Guide.Schedule;
			output.Specialization = memoir.Guide.Specialization;
			output.WhoRejected = memoir.WhoRejected is null && !memoir.WasTheCommittee ? string.Empty : $"{memoir.WhoRejected!.FirstName} {memoir.WhoRejected.LastName}";
			output.Reason = memoir.Reason;
			output.Requirements = memoir.Requirements;
		} else if (this.User.IsInRole("Guide")) {
			output.MemoristId = memoir.Memorist is null ? string.Empty : memoir.Memorist.Id;
			output.MemoristName = memoir.Memorist is null ? string.Empty : $"{memoir.Memorist.FirstName} {memoir.Memorist.LastName}";
			output.MemoristEmail = memoir.Memorist is null ? string.Empty : memoir.Memorist.Email;
			output.UniversityId = memoir.Memorist is null ? string.Empty : memoir.Memorist.UniversityId;
			output.RemainingCourses = memoir.Memorist is null ? string.Empty : memoir.Memorist.RemainingCourses;
			output.IsDoingThePractice = memoir.Memorist is not null && memoir.Memorist.IsDoingThePractice;
			output.IsWorking = memoir.Memorist is not null && memoir.Memorist.IsWorking;
		}
		return this.View(output);
	}

	[Authorize(Roles = "Student")]
	public async Task<IActionResult> Apply(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Where(m => m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(p => p.Guide).AsNoTracking()
			.FirstOrDefaultAsync(m => m.Id == id
				&& m.Phase == IccMemoir.MemoirPhase.PublishedByGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Applications", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
			GuideName = $"{memoir.Guide!.FirstName} {memoir.Guide.LastName}"
		};
		return this.View(output);
	}

	[Authorize(Roles = "Student"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Apply([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Where(m => m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(p => p.Guide)
			.FirstOrDefaultAsync(m => m.Id == input.Id
				&& m.Phase == IccMemoir.MemoirPhase.PublishedByGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("Applications", "Proposal", new { area = "University" });
		}
		memoir.Candidates!.Add(user);
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Has postulado a la propuesta correctamente.";
		return this.RedirectToAction("Applications", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Reject(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Include(m => m.Guide)
			.Where(m => m.Guide!.Id == user.Id
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Memorist).AsNoTracking()
			.FirstOrDefaultAsync(m => m.Id == id
				&& m.Phase == IccMemoir.MemoirPhase.SentToGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
			MemoristName = $"{memoir.Memorist!.FirstName} {memoir.Memorist.LastName}"
		};
		return this.View(output);
	}

	[Authorize(Roles = "Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Reject([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Include(m => m.Guide)
			.Where(m => m.Guide!.Id == user.Id
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Memorist)
			.FirstOrDefaultAsync(m => m.Id == input.Id
				&& m.Phase == IccMemoir.MemoirPhase.SentToGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		memoir.Phase = IccMemoir.MemoirPhase.RejectedByGuide;
		memoir.WhoRejected = user;
		memoir.Reason = input.Reason;
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "La propuesta ha sido rechazada correctamente.";
		return this.RedirectToAction("List", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Approve(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Include(m => m.Guide)
			.Where(m => m.Guide!.Id == user.Id
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Memorist).AsNoTracking()
			.FirstOrDefaultAsync(m => m.Id == id
				&& m.Phase == IccMemoir.MemoirPhase.SentToGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title,
			MemoristName = $"{memoir.Memorist!.FirstName} {memoir.Memorist.LastName}"
		};
		return this.View(output);
	}

	[Authorize(Roles = "Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Approve([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Include(m => m.Guide)
			.Where(m => m.Guide!.Id == user.Id
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Memorist)
			.FirstOrDefaultAsync(m => m.Id == input.Id
				&& m.Phase == IccMemoir.MemoirPhase.SentToGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		memoir.Phase = IccMemoir.MemoirPhase.ApprovedByGuide;
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "La propuesta ha sido aprobada correctamente.";
		return this.RedirectToAction("List", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Select(string memoirId, string memoristId) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var student = await this._userManager.FindByIdAsync(memoristId);
		if (user is null) {
			this.TempData["ErrorMessage"] = "Error al obtener al estudiante";
			return this.RedirectToAction("Students", "Proposal", new { area = "University" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Where(m => m.Owner == user
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Candidates)
			.Where(m => m.Candidates!.Any(s => s!.Id == memoristId)).AsNoTracking()
			.FirstOrDefaultAsync(m => m.Id == memoirId
				&& m.Phase == IccMemoir.MemoirPhase.PublishedByGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta.";
			return this.RedirectToAction("Students", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = memoirId,
			MemoristId = memoristId,
			MemoristName = $"{student!.FirstName} {student.LastName}",
			MemoristEmail = student.Email,
			UniversityId = student.UniversityId,
			RemainingCourses = student.RemainingCourses,
			IsDoingThePractice = student.IsDoingThePractice,
			IsWorking = student.IsWorking
		};
		return this.View(output);
	}

	[Authorize(Roles = "Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Select([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		var memoir = await this._dbContext.IccMemoirs!
			.Where(m => m.Owner == user
				&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
				&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
				&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
				&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
				&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
			.Include(m => m.Candidates)
			.Where(m => m.Candidates!.Any(s => s!.Id == input.MemoristId))
			.FirstOrDefaultAsync(m => m.Id == input.Id
				&& m.Phase == IccMemoir.MemoirPhase.PublishedByGuide);
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		if (!memoir.Candidates!.Any(s => s!.Id == input.MemoristId)) {
			this.TempData["ErrorMessage"] = "El estudiante no está en la lista de candidatos.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		var student = await this._userManager.FindByIdAsync(input!.MemoristId!);
		if (student is null) {
			this.TempData["ErrorMessage"] = "Error al obtener al estudiante.";
			return this.RedirectToAction("Index", "Proposal", new { area = "University" });
		}
		_ = memoir.Candidates!.Remove(student);
		memoir.Phase = IccMemoir.MemoirPhase.ReadyByGuide;
		memoir.Memorist = student;
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "El estudiante ha sido seleccionado a la propuesta correctamente.";
		return this.RedirectToAction("Index", "Proposal", new { area = "University" });
	}

	[Authorize(Roles = "Student,Guide")]
	public async Task<IActionResult> Convert(string id) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Memorist)
				.Where(m => m.Memorist!.Id == user.Id
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id
					&& m.Phase == IccMemoir.MemoirPhase.ApprovedByGuide);
		} else if (this.User.IsInRole("Teacher")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Guide)
				.Where(m => m.Guide!.Id == user.Id
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector).AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id
					&& m.Phase == IccMemoir.MemoirPhase.ReadyByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		var output = new IccMemoirViewModel {
			Id = id,
			Title = memoir.Title
		};
		return this.View(output);
	}

	[Authorize(Roles = "Student,Guide"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Convert([FromForm] IccMemoirViewModel input) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		IccMemoir? memoir = null!;
		if (this.User.IsInRole("Student")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Memorist)
				.Where(m => m.Memorist!.Id == user.Id
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.FirstOrDefaultAsync(m => m.Id == input.Id
					&& m.Phase == IccMemoir.MemoirPhase.ApprovedByGuide);
		} else if (this.User.IsInRole("Teacher")) {
			memoir = await this._dbContext.IccMemoirs!
				.Include(m => m.Guide)
				.Where(m => m.Guide!.Id == user.Id
					&& m.Phase != IccMemoir.MemoirPhase.SentToCommittee
					&& m.Phase != IccMemoir.MemoirPhase.RejectedByCommittee && m.Phase != IccMemoir.MemoirPhase.ApprovedByCommittee
					&& m.Phase != IccMemoir.MemoirPhase.InProgress && m.Phase != IccMemoir.MemoirPhase.Abandoned
					&& m.Phase != IccMemoir.MemoirPhase.Completed && m.Phase != IccMemoir.MemoirPhase.RejectedByDirector
					&& m.Phase != IccMemoir.MemoirPhase.ApprovedByDirector)
				.FirstOrDefaultAsync(m => m.Id == input.Id
					&& m.Phase == IccMemoir.MemoirPhase.ReadyByGuide);
		}
		if (memoir is null) {
			this.TempData["ErrorMessage"] = "Error al obtener la propuesta";
			return this.RedirectToAction("List", "Proposal", new { area = "University" });
		}
		memoir.Phase = IccMemoir.MemoirPhase.SentToCommittee;
		memoir.UpdatedAt = DateTimeOffset.Now;
		_ = this._dbContext.IccMemoirs!.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu propuesta ha sido enviada correctamente.";
		return this.RedirectToAction("Index", "Request", new { area = "University" });
	}
}