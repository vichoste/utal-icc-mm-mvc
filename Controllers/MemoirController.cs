using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;
using Utal.Icc.Mm.Mvc.Views.Shared;

using X.PagedList;

namespace Utal.Icc.Mm.Mvc.Controllers;

public class MemoirController : Controller {
	private readonly IccDbContext _dbContext;
	private readonly UserManager<IccUser> _userManager;

	public MemoirController(IccDbContext dbContext, UserManager<IccUser> userManager) {
		this._dbContext = dbContext;
		this._userManager = userManager;
	}

	[Authorize(Roles = "IccRegular,IccGuide")]
	public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		List<IccMemoir> memoirs = new();
		if (this.User.IsInRole("IccRegular")) {
			var memoirsQuery = this._dbContext.IccMemoirs.Include(m => m.Student).Where(m => (m.Phase == IccMemoir.Phases.Proposal || m.Phase == IccMemoir.Phases.Request) && m.Student!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
			var teacherMemoirsQuery = this._dbContext.IccTeacherMemoirs.Include(m => m.Candidates).Where(m => (m.Phase == IccMemoir.Phases.Proposal || m.Phase == IccMemoir.Phases.Request) && m.Candidates!.Any(c => c.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
			memoirs.AddRange(memoirsQuery);
			memoirs.AddRange(teacherMemoirsQuery);
			memoirs = memoirs.Distinct().ToList();
		} else if (this.User.IsInRole("IccGuide")) {
			memoirs = this._dbContext.IccMemoirs.Include(m => m.Guide).Where(m => (m.Phase == IccMemoir.Phases.Proposal || m.Phase == IccMemoir.Phases.Request) && m.Guide!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
		}
		var parameters = new[] { "Title" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					memoirs = memoirs.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					memoirs = memoirs.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccMemoir>();
			foreach (var parameter in parameters) {
				var partials = memoirs.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Memoirs = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Memoirs = memoirs.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)memoirs.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)memoirs.Count / 10)));
	}

	[Authorize(Roles = "IccRegular,IccGuide")]
	public async Task<IActionResult> Create() {
		if (this.User.IsInRole("IccRegular")) {
			this.ViewBag.Guides = (await this._userManager.GetUsersInRoleAsync("IccGuide")).ToList();
		}
		return this.View();
	}

	[Authorize(Roles = "IccRegular"), HttpPost]
	public async Task<IActionResult> CreateStudentMemoir([FromForm] string memoirTitle, [FromForm] string description, [FromForm] string guide) {
		var @new = new IccStudentMemoir {
			Title = memoirTitle,
			Description = description,
			Student = await this._userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!) as IccStudent,
			Guide = await this._userManager.FindByIdAsync(guide) as IccTeacher
		};
		_ = this._dbContext.IccMemoirs.Add(@new);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido registrada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize]
	public IActionResult GetGuideDetails(string id) {
		var teacher = this._dbContext.IccTeachers.Find(id);
		return this.Json(new {
			office = teacher!.Office,
			schedule = teacher.Schedule,
			specialization = teacher.Specialization
		});
	}

	[Authorize(Roles = "IccGuide"), HttpPost]
	public async Task<IActionResult> CreateTeacherMemoir([FromForm] string memoirTitle, [FromForm] string description, [FromForm] string requirements) {
		var @new = new IccTeacherMemoir {
			Title = memoirTitle,
			Description = description,
			Guide = await this._userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!) as IccTeacher,
			Requirements = requirements
		};
		_ = this._dbContext.IccMemoirs.Add(@new);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido registrada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize]
	public IActionResult GetStudentDetails(string id) {
		var student = this._dbContext.IccStudents.Find(id);
		return this.Json(new {
			remainingCourses = student!.RemainingCourses,
			isDoingThePractice = student.IsDoingThePractice,
			isWorking = student.IsWorking
		});
	}

	[Authorize(Roles = "IccRegular,IccGuide"), System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "Code legibility.")]
	public async Task<IActionResult> Get(string id, bool isReadOnly, bool isApplying) {
		this.ViewBag.Id = id;
		this.ViewBag.IsReadOnly = isReadOnly;
		this.ViewBag.IsApplying = isApplying;
		IQueryable<IccMemoir> memoirQuery;
		if (this._dbContext.IccTeacherMemoirs.Any(m => m.Id == id)) {
			memoirQuery = this._dbContext.IccTeacherMemoirs.Include(m => m.Student).Include(m => m.Guide).Include(m => m.Candidates).Where(m => m.Id == id);
		} else {
			memoirQuery = this._dbContext.IccMemoirs.Include(m => m.Student).Include(m => m.Guide).Where(m => m.Id == id);
		}
		var memoir = memoirQuery.FirstOrDefault();
		this.ViewBag.MemoirTitle = memoir!.Title;
		this.ViewBag.Description = memoir.Description;
		var student = memoir.Student;
		var guide = memoir.Guide;
		this.ViewBag.Student = student is not null ? student.Id : string.Empty;
		this.ViewBag.Guide = guide is not null ? guide.Id : string.Empty;
		if (this.User.IsInRole("IccRegular")) {
			this.ViewBag.Guides = (await this._userManager.GetUsersInRoleAsync("IccGuide")).ToList();
		}
		if (memoir is IccTeacherMemoir teacherMemoir) {
			this.ViewBag.Requirements = teacherMemoir.Requirements;
			this.ViewBag.Candidates = teacherMemoir.Candidates;
			if (this.User.IsInRole("IccRegular")) {
				this.ViewBag.IsCandidate = teacherMemoir.Candidates.Any(c => c.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier));
			}
		}
		if (isReadOnly) {
			this.ViewBag.StudentFullName = student is not null ? student.FullName : string.Empty;
			this.ViewBag.GuideFullName = guide is not null ? guide.FullName : string.Empty;
		}
		this.ViewBag.IsProposal = memoir.Phase == IccMemoir.Phases.Proposal;
		return this.View();
	}

	[Authorize(Roles = "IccRegular"), HttpPost]
	public async Task<IActionResult> GetStudentMemoir([FromForm] string id, [FromForm] string memoirTitle, [FromForm] string description, [FromForm] string guide) {
		var studentMemoir = await this._dbContext.IccStudentMemoirs.FindAsync(id);
		studentMemoir!.Title = memoirTitle;
		studentMemoir.Description = description;
		studentMemoir.Guide = await this._userManager.FindByIdAsync(guide) as IccTeacher;
		_ = this._dbContext.IccMemoirs.Update(studentMemoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido editada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize(Roles = "IccGuide"), HttpPost]
	public async Task<IActionResult> GetTeacherMemoir([FromForm] string id, [FromForm] string memoirTitle, [FromForm] string description, [FromForm] string student, [FromForm] string requirements) {
		var teacherMemoir = await this._dbContext.IccTeacherMemoirs.FindAsync(id);
		teacherMemoir!.Title = memoirTitle;
		teacherMemoir.Description = description;
		teacherMemoir.Student = await this._userManager.FindByIdAsync(student) as IccStudent;
		teacherMemoir.Requirements = requirements;
		_ = this._dbContext.IccMemoirs.Update(teacherMemoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido editada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize(Roles = "IccRegular")]
	public IActionResult Application(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var memoirs = this._dbContext.IccTeacherMemoirs.Include(m => m.Candidates).Where(m => (m.Phase == IccMemoir.Phases.Proposal || m.Phase == IccMemoir.Phases.Request) && m.Phase == IccMemoir.Phases.Proposal && !m.Candidates!.Any(c => c.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
		var parameters = new[] { "Title" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					memoirs = memoirs.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					memoirs = memoirs.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccMemoir>();
			foreach (var parameter in parameters) {
				var partials = memoirs.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Memoirs = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Memoirs = memoirs.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)memoirs.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)memoirs.Count / 10)));
	}

	[Authorize(Roles = "IccRegular"), HttpPost]
	public async Task<IActionResult> Apply(string id) {
		var memoir = this._dbContext.IccTeacherMemoirs.Include(m => m.Candidates).FirstOrDefault(m => m.Id == id);
		memoir!.Candidates!.Add((await this._userManager.FindByIdAsync(this.User.FindFirstValue(ClaimTypes.NameIdentifier)!) as IccStudent)!);
		_ = this._dbContext.IccTeacherMemoirs.Update(memoir);
		_ = this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Te has postulado correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize(Roles = "IccRegular,IccGuide"), HttpPost]
	public async Task<IActionResult> Send(string id, [FromForm] string student) {
		var memoir = this._dbContext.IccMemoirs.FirstOrDefault(m => m.Id == id);
		memoir!.Phase = IccMemoir.Phases.Request;
		if (memoir is IccTeacherMemoir teacherMemoir && await this._userManager.FindByIdAsync(student) is IccStudent student1) {
			memoir!.Student = student1;
			_ = teacherMemoir!.Candidates!.Remove(student1);
			_ = this._dbContext.IccTeacherMemoirs.Update(teacherMemoir);
			_ = await this._dbContext.SaveChangesAsync();
		} else {
			_ = this._dbContext.IccMemoirs.Update(memoir);
			_ = await this._dbContext.SaveChangesAsync();
		}
		this.TempData["SuccessMessage"] = "Tu memoria ha sido enviada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize(Roles = "IccCommittee,IccDirector")]
	public IActionResult Admin(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var memoirs = this._dbContext.IccMemoirs.Where(m => m.Phase == IccMemoir.Phases.Request).ToList();
		var parameters = new[] { "Title" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					memoirs = memoirs.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					memoirs = memoirs.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccMemoir>();
			foreach (var parameter in parameters) {
				var partials = memoirs.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Memoirs = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Admin", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Memoirs = memoirs.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Admin", pageNumber ?? 1, (int)Math.Ceiling((decimal)memoirs.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)memoirs.Count / 10)));
	}

	[Authorize(Roles = "IccCommittee,IccDirector"), System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "Code legibility.")]
	public IActionResult Vote(string id) {
		this.ViewBag.Id = id;
		IQueryable<IccMemoir> memoirQuery;
		if (this._dbContext.IccTeacherMemoirs.Any(m => m.Id == id)) {
			memoirQuery = this._dbContext.IccTeacherMemoirs.Include(m => m.Student).Include(m => m.Guide).Where(m => m.Id == id);
		} else {
			memoirQuery = this._dbContext.IccMemoirs.Include(m => m.Student).Include(m => m.Guide).Where(m => m.Id == id);
		}
		var memoir = memoirQuery.FirstOrDefault();
		this.ViewBag.MemoirTitle = memoir!.Title;
		this.ViewBag.Description = memoir.Description;
		var student = memoir.Student;
		var guide = memoir.Guide;
		this.ViewBag.Student = student is not null ? student.Id : string.Empty;
		this.ViewBag.Guide = guide is not null ? guide.Id : string.Empty;
		if (memoir is IccTeacherMemoir teacherMemoir) {
			this.ViewBag.Requirements = teacherMemoir.Requirements;
		}
		this.ViewBag.StudentFullName = student is not null ? student.FullName : string.Empty;
		this.ViewBag.GuideFullName = guide is not null ? guide.FullName : string.Empty;
		return this.View();
	}

	[Authorize(Roles = "IccCommittee,IccDirector")]
	public async Task<IActionResult> Reject(string id) {
		var memoir = await this._dbContext.IccMemoirs.FindAsync(id);
		memoir!.Phase = IccMemoir.Phases.Proposal;
		_ = this._dbContext.IccMemoirs.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "La memoria ha sido rechazada correctamente.";
		return this.RedirectToAction("Admin", "Memoir");
	}

	[Authorize(Roles = "IccCommittee,IccDirector")]
	public async Task<IActionResult> Approve(string id) {
		var memoir = await this._dbContext.IccMemoirs.Include(m => m.Student).Where(m => m.Id == id).FirstOrDefaultAsync();
		memoir!.Phase = IccMemoir.Phases.InProgress;
		var student = await this._userManager.FindByIdAsync(memoir.Student!.Id);
		_ = await this._userManager.RemoveFromRoleAsync(student!, "IccRegular");
		_ = await this._userManager.AddToRoleAsync(student!, "IccMemorist");
		_ = this._dbContext.IccMemoirs.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "La memoria ha sido aprobada correctamente.";
		return this.RedirectToAction("Admin", "Memoir");
	}

	[Authorize(Roles = "IccMemorist,IccGuide,IccDirector")]
	public IActionResult My(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		List<IccMemoir> memoirs = new();
		if (this.User.IsInRole("IccMemorist")) {
			memoirs = this._dbContext.IccMemoirs.Include(m => m.Student).Where(m => m.Phase != IccMemoir.Phases.Proposal && m.Phase != IccMemoir.Phases.Request && m.Student!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
		} else if (this.User.IsInRole("IccGuide")) {
			memoirs = this._dbContext.IccMemoirs.Include(m => m.Guide).Where(m => m.Phase != IccMemoir.Phases.Proposal && m.Phase != IccMemoir.Phases.Request && m.Guide!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
		} else if (this.User.IsInRole("IccDirector")) {
			memoirs = this._dbContext.IccMemoirs.Where(m => m.Phase != IccMemoir.Phases.Proposal && m.Phase != IccMemoir.Phases.Request).ToList();
		}
		var parameters = new[] { "Title" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					memoirs = memoirs.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					memoirs = memoirs.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccMemoir>();
			foreach (var parameter in parameters) {
				var partials = memoirs.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Memoirs = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("My", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Memoirs = memoirs.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("My", pageNumber ?? 1, (int)Math.Ceiling((decimal)memoirs.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)memoirs.Count / 10)));
	}

	[Authorize(Roles = "IccMemorist,IccGuide,IccDirector")]
	public async Task<IActionResult> Status(string id) {
		var memoir = await this._dbContext.IccMemoirs.Include(m => m.Student).Include(m => m.Guide).Where(m => m.Id == id).FirstOrDefaultAsync();
		this.ViewBag.MemoirTitle = memoir!.Title;
		this.ViewBag.Description = memoir.Description;
		var student = memoir.Student;
		var guide = memoir.Guide;
		this.ViewBag.Student = student is not null ? student.Id : string.Empty;
		this.ViewBag.Guide = guide is not null ? guide.Id : string.Empty;
		this.ViewBag.StudentFullName = student is not null ? student.FullName : string.Empty;
		this.ViewBag.GuideFullName = guide is not null ? guide.FullName : string.Empty;
		this.ViewBag.Phase = memoir.Phase;
		return this.View();
	}

	[Authorize(Roles = "IccGuide,IccDirector")]
	public async Task<IActionResult> Set([FromForm] string id, [FromForm] string phase) {
		var memoir = await this._dbContext.IccMemoirs.FindAsync(id);
		memoir!.Phase = Enum.Parse<IccMemoir.Phases>(phase);
		_ = this._dbContext.IccMemoirs.Update(memoir);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "El estado de la memoria ha sido actualizado correctamente.";
		return this.RedirectToAction("My", "Memoir");
	}
}