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
			memoirs = this._dbContext.IccMemoirs
				.Include(m => m.Student)
				.Where(m => m.Student!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
				.ToList();
		} else if (this.User.IsInRole("IccGuide")) {
			memoirs = this._dbContext.IccMemoirs
				.Include(m => m.Guide)
				.Where(m => m.Guide!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
				.ToList();
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
				var partials = memoirs
					.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty()
						&& (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
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

	[Authorize(Roles = "IccRegular")]
	public IActionResult GetTeacherDetails(string id) {
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

	[Authorize(Roles = "IccRegular,IccGuide")]
	public async Task<IActionResult> Get(string id, bool isReadOnly) {
		this.ViewBag.Id = id;
		this.ViewBag.IsReadOnly = isReadOnly;
		if (this.User.IsInRole("IccRegular")) {
			this.ViewBag.Guides = (await this._userManager.GetUsersInRoleAsync("IccGuide")).ToList();
			var studentMemoir = await this._dbContext.IccStudentMemoirs.FindAsync(id);
			this.ViewBag.MemoirTitle = studentMemoir!.Title;
			this.ViewBag.Description = studentMemoir.Description;
			var guide = studentMemoir.Guide;
			this.ViewBag.Guide = guide is not null ? guide.Id : string.Empty;
		} else if (this.User.IsInRole("IccGuide")) {
			var teacherMemoir = await this._dbContext.IccTeacherMemoirs.FindAsync(id);
			this.ViewBag.Id = id;
			this.ViewBag.MemoirTitle = teacherMemoir!.Title;
			this.ViewBag.Description = teacherMemoir.Description;
			var student = teacherMemoir.Student;
			this.ViewBag.Student = student is not null ? student.Id : string.Empty;
			this.ViewBag.Requirements = teacherMemoir.Requirements;
			this.ViewBag.Candidates = teacherMemoir.Candidates;
		}
		return this.View();
	}

	[Authorize(Roles = "IccRegular"), HttpPost]
	public async Task<IActionResult> GetStudentMemoir([FromForm] string id, [FromForm] string memoirTitle, [FromForm] string description, [FromForm] string guide) {
		var target = await this._dbContext.IccStudentMemoirs.FindAsync(id);
		target!.Title = memoirTitle;
		target.Description = description;
		target.Guide = await this._userManager.FindByIdAsync(guide) as IccTeacher;
		_ = this._dbContext.IccMemoirs.Update(target);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido editada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}

	[Authorize(Roles = "IccGuide")]
	public IActionResult GetStudentDetails(string id) {
		var student = this._dbContext.IccStudents.Find(id);
		return this.Json(new {
			remainingCourses = student!.RemainingCourses,
			isDoingThePractice = student.IsDoingThePractice,
			isWorking = student.IsWorking
		});
	}

	[Authorize(Roles = "IccGuide"), HttpPost]
	public async Task<IActionResult> GetTeacherMemoir([FromForm] string id, [FromForm] string memoirTitle, [FromForm] string description, [FromForm] string student, [FromForm] string requirements) {
		var target = await this._dbContext.IccTeacherMemoirs.FindAsync(id);
		target!.Title = memoirTitle;
		target.Description = description;
		target.Student = await this._userManager.FindByIdAsync(student) as IccStudent;
		target.Requirements = requirements;
		_ = this._dbContext.IccMemoirs.Update(target);
		_ = await this._dbContext.SaveChangesAsync();
		this.TempData["SuccessMessage"] = "Tu memoria ha sido editada correctamente.";
		return this.RedirectToAction("Index", "Memoir");
	}
}