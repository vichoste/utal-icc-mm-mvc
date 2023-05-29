using System.Globalization;

using CsvHelper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Utal.Icc.Mm.Mvc.Models;
using Utal.Icc.Mm.Mvc.Views.Account;
using Utal.Icc.Mm.Mvc.Views.Shared;

using X.PagedList;

namespace Utal.Icc.Mm.Mvc.Controllers;

public class AccountController : Controller {
	private readonly IUserEmailStore<IccUser> _emailStore;
	private readonly SignInManager<IccUser> _signInManager;
	private readonly UserManager<IccUser> _userManager;
	private readonly IUserStore<IccUser> _userStore;

	public AccountController(UserManager<IccUser> userManager, SignInManager<IccUser> signInManager, IUserStore<IccUser> userStore) {
		this._userStore = userStore;
		this._emailStore = (IUserEmailStore<IccUser>)this._userStore!;
		this._signInManager = signInManager;
		this._userManager = userManager;
	}

	public IActionResult Login() => this.User.Identity!.IsAuthenticated ? this.RedirectToAction("Index", "Home") : this.View();

	[HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Login([FromForm] Login model) {
		if (this.User.Identity!.IsAuthenticated) {
			return this.RedirectToAction("Index", "Home");
		}
		var result = await this._signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Credenciales incorrectas.";
			return this.View(model);
		}
		return this.RedirectToAction("Index", "Home");
	}

	[Authorize]
	public async Task<IActionResult> Logout() {
		await this._signInManager.SignOutAsync();
		this.TempData["SuccessMessage"] = "Se ha cerrado tu sesión correctamente.";
		return this.RedirectToAction("Index", "Home");
	}

	[Authorize]
	public IActionResult Password() => this.View();

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Password([FromForm] Password model) {
		var user = await this._userManager.GetUserAsync(this.User);
		var result = await this._userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Error al cambiar tu contraseña.";
			this.ViewBag.DangerMessages = result.Errors.Select(e => e.Description).ToList();
			return this.View(model);
		}
		_ = await this._userManager.UpdateAsync(user!);
		this.TempData["SuccessMessage"] = "Has cambiado tu contraseña correctamente.";
		return this.RedirectToAction("Index", "Home");
	}

	[Authorize, System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Code legibility")]
	public async Task<IActionResult> Profile() {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user is IccStudent student) {
			return this.View("StudentProfile", new StudentProfile {
				Email = student.Email,
				FirstName = student.FirstName,
				LastName = student.LastName,
				Rut = student.Rut,
				UniversityId = student.UniversityId,
				RemainingCourses = student.RemainingCourses,
				IsDoingThePractice = student.IsDoingThePractice,
				IsWorking = student.IsWorking,
			});
		}
		if (user is IccTeacher teacher) {
			return this.View("TeacherProfile", new TeacherProfile {
				Email = teacher.Email,
				FirstName = teacher.FirstName,
				LastName = teacher.LastName,
				Rut = teacher.Rut,
				Office = teacher.Office,
				Schedule = teacher.Schedule,
				Specialization = teacher.Specialization
			});
		}
		return this.NotFound();
	}

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> StudentProfile([FromForm] IccStudent model) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user is IccStudent student) {
			student.RemainingCourses = model.RemainingCourses;
			student.IsDoingThePractice = model.IsDoingThePractice;
			_ = await this._userManager.UpdateAsync(student);
			this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
			return this.RedirectToAction("Profile", "Account");
		}
		return this.NotFound();
	}

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> TeacherProfile([FromForm] IccTeacher model) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user is IccTeacher teacher) {
			teacher.Office = model.Office;
			teacher.Schedule = model.Schedule;
			teacher.Specialization = model.Specialization;
			_ = await this._userManager.UpdateAsync(teacher);
			this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
			return this.RedirectToAction("Profile", "Account");
		}
		return this.NotFound();
	}

	[Authorize(Roles = "IccDirector")]
	public async Task<IActionResult> Students(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		var students = this._userManager.Users.OfType<IccStudent>().ToList();
		var parameters = new[] { "FirstName", "LastName", "UniversityId", "Rut", "Email" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccStudent>();
			foreach (var parameter in parameters) {
				var partials = students
					.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty()
						&& (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Students = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Students", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Students = students.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Students", pageNumber ?? 1, (int)Math.Ceiling((decimal)students.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)students.Count / 10)));
	}

	[Authorize(Roles = "IccDirector")]
	public async Task<IActionResult> Teachers(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var user = await this._userManager.GetUserAsync(this.User);
		var teachers = this._userManager.Users.OfType<IccTeacher>().ToList();
		var parameters = new[] { "FirstName", "LastName", "Rut", "Email" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccTeacher>();
			foreach (var parameter in parameters) {
				var partials = teachers
					.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty()
						&& (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Teachers = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Teachers", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Teachers = teachers.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Teachers", pageNumber ?? 1, (int)Math.Ceiling((decimal)teachers.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)teachers.Count / 10)));
	}

	[Authorize(Roles = "IccDirector")]
	public IActionResult BatchCreateStudents() => this.View();

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> BatchCreateStudents([FromForm] BatchCreateStudents model) {
		try {
			var warningMessages = new List<string>(); ;
			var successMessages = new List<string>();
			using var reader = new StreamReader(model.CsvFile!.OpenReadStream());
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			var records = csv.GetRecords<Helpers.BatchCreateStudents>();
			foreach (var record in records) {
				var user = new IccStudent {
					FirstName = record.FirstName!,
					LastName = record.LastName!,
					UniversityId = record.UniversityId!,
					Rut = record.Rut!
				};
				await this._userStore.SetUserNameAsync(user, record.Email, CancellationToken.None);
				await this._emailStore.SetEmailAsync(user, record.Email, CancellationToken.None);
				var result = await this._userManager.CreateAsync(user, record!.Password!);
				if (!result.Succeeded) {
					warningMessages.Add($"Estudiante con e-mail {record.Email} ya existe");
					continue;
				}
				_ = await this._userManager.AddToRoleAsync(user, "Regular");
				successMessages.Add($"Estudiante con e-mail {record.Email} creado correctamente.");
			}
			if (warningMessages.Any()) {
				this.TempData["WarningMessages"] = warningMessages.AsEnumerable();
			}
			if (successMessages.Any()) {
				this.TempData["SuccessMessages"] = successMessages.AsEnumerable();
			}
			return this.RedirectToAction("Students", "Account", new { area = string.Empty });
		} catch {
			this.TempData["ErrorMessage"] = "Error al importar el archivo CSV.";
			return this.RedirectToAction("Students", "Account", new { area = string.Empty });
		}
	}

	[Authorize(Roles = "IccDirector")]
	public IActionResult CreateTeacher() => this.View();

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateTeacher(EditTeacher model) {
		var teacher = new IccTeacher {
			FirstName = model.FirstName,
			LastName = model.LastName,
			Rut = model.Rut
		};
		var roles = new List<string>();
		if (model.IsDirector) {
			roles.Add("IccDirector");
		}
		if (model.IsCommittee) {
			roles.Add("IccCommittee");
		}
		if (model.IsGuide) {
			roles.Add("IccGuide");
		}
		await this._userStore.SetUserNameAsync(teacher, model.Email, CancellationToken.None);
		await this._emailStore.SetEmailAsync(teacher, model.Email, CancellationToken.None);
		_ =  await this._userManager.CreateAsync(teacher, model.Password.NewPassword);
		_ = await this._userManager.AddToRolesAsync(teacher, roles);
		this.TempData["SuccessMessage"] = "Profesor(a) creado(a) exitosamente.";
		return this.RedirectToAction("Teachers", "Account", new { area = string.Empty });
	}

	[Authorize(Roles = "IccDirector")]
	public async Task<IActionResult> Edit(string id) {
		var target = await this._userManager.FindByIdAsync(id);
		if (target is IccStudent student) {
			return this.View("EditStudent", new EditStudent {
				Email = student.Email,
				FirstName = student.FirstName,
				LastName = student.LastName,
				Rut = student.Rut,
				UniversityId = student.UniversityId,
				RemainingCourses = student.RemainingCourses,
				IsDoingThePractice = student.IsDoingThePractice,
				IsWorking = student.IsWorking,
				Password = new()
			});
		} else if (target is IccTeacher teacher) {
			return this.View("EditTeacher", new EditTeacher {
				Email = teacher.Email,
				FirstName = teacher.FirstName,
				LastName = teacher.LastName,
				Rut = teacher.Rut,
				Office = teacher.Office,
				Schedule = teacher.Schedule,
				Specialization = teacher.Specialization,
				IsCommittee = await this._userManager.IsInRoleAsync(teacher, "IccCommittee"),
				IsGuide = await this._userManager.IsInRoleAsync(teacher, "IccGuide"),
				Password = new()
			});
		}
		return this.NotFound();
	}

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditStudent(EditStudent model) {
		var target = await this._userManager.FindByEmailAsync(model.Email!);
		if (target is IccStudent student) {
			student.FirstName = model.FirstName;
			student.LastName = model.LastName;
			student.Rut = model.Rut;
			student.UniversityId = model.UniversityId!;
			student.RemainingCourses = model.RemainingCourses!;
			student.IsDoingThePractice = model.IsDoingThePractice;
			student.IsWorking = model.IsWorking;
			await this._userManager.UpdateAsync(student);
			this.TempData["SuccessMessage"] = "Estudiante actualizado exitosamente.";
			return this.RedirectToAction("Students", "Account", new { area = string.Empty });
		}
		return this.NotFound();
	}

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditTeacher(EditTeacher model) {
		var target = await this._userManager.FindByEmailAsync(model.Email!);
		if (target is IccTeacher teacher) {
			teacher.FirstName = model.FirstName;
			teacher.LastName = model.LastName;
			teacher.Rut = model.Rut;
			teacher.Office = model.Office;
			teacher.Schedule = model.Schedule;
			teacher.Specialization = model.Specialization;
			await this._userManager.UpdateAsync(teacher);
			var roles = new List<string>();
			if (model.IsCommittee) {
				roles.Add("IccCommittee");
			}
			if (model.IsGuide) {
				roles.Add("IccGuide");
			}
			var currentRoles = (await this._userManager.GetRolesAsync(teacher)).ToList();
			var rolesToRemove = currentRoles.Except(roles).ToList();
			if (rolesToRemove.Contains("IccDirector")) {
				_ = rolesToRemove.Remove("IccDirector");
			}
			var rolesToAdd = roles.Except(currentRoles);
			await this._userManager.RemoveFromRolesAsync(teacher, rolesToRemove);
			await this._userManager.AddToRolesAsync(teacher, rolesToAdd);
			this.TempData["SuccessMessage"] = "Profesor(a) actualizado(a) exitosamente.";
			return this.RedirectToAction("Teachers", "Account", new { area = string.Empty });
		}
		return this.NotFound();
	}
}