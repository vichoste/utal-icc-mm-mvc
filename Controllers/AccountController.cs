using System.Globalization;

using CsvHelper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using Utal.Icc.Mm.Mvc.Models;
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
	public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password, [FromForm] bool rememberMe) {
		var result = await this._signInManager.PasswordSignInAsync(email, password, rememberMe, false);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Credenciales incorrectas.";
			return this.View();
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
	public async Task<IActionResult> Password([FromForm] string currentPassword, [FromForm] string newPassword) {
		var user = await this._userManager.GetUserAsync(this.User);
		var result = await this._userManager.ChangePasswordAsync(user!, currentPassword, newPassword);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Error al cambiar tu contraseña.";
			this.ViewBag.DangerMessages = result.Errors.Select(e => e.Description).ToList();
			return this.View();
		}
		_ = await this._userManager.UpdateAsync(user!);
		this.TempData["SuccessMessage"] = "Has cambiado tu contraseña correctamente.";
		return this.RedirectToAction("Index", "Home");
	}

	[Authorize]
	public async Task<IActionResult> Profile() {
		var user = await this._userManager.GetUserAsync(this.User);
		this.ViewBag.Email = user!.Email;
		this.ViewBag.FirstName = user.FirstName;
		this.ViewBag.LastName = user.LastName;
		this.ViewBag.Rut = user.Rut;
		if (user is IccStudent student) {
			this.ViewBag.UniversityId = student.UniversityId;
			this.ViewBag.RemainingCourses = student.RemainingCourses;
			this.ViewBag.IsDoingThePractice = student.IsDoingThePractice;
			this.ViewBag.IsWorking = student.IsWorking;
			return this.View("StudentProfile");
		}
		if (user is IccTeacher teacher) {
			this.ViewBag.Office = teacher.Office;
			this.ViewBag.Schedule = teacher.Schedule;
			this.ViewBag.Specialization = teacher.Specialization;
			return this.View("TeacherProfile");
		}
		return this.NotFound();
	}

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> StudentProfile([FromForm] string remainingCourses, [FromForm] bool isDoingThePractice, [FromForm] bool isWorking) {
		var student = await this._userManager.GetUserAsync(this.User) as IccStudent;
		student!.RemainingCourses = remainingCourses;
		student.IsDoingThePractice = isDoingThePractice;
		student.IsWorking = isWorking;
		_ = await this._userManager.UpdateAsync(student);
		this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
		return this.RedirectToAction("Profile", "Account");
	}

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> TeacherProfile([FromForm] string office, [FromForm] string schedule, [FromForm] string specialization) {
		var teacher = await this._userManager.GetUserAsync(this.User) as IccTeacher;
		teacher!.Office = office;
		teacher.Schedule = schedule;
		teacher.Specialization = specialization;
		_ = await this._userManager.UpdateAsync(teacher);
		this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
		return this.RedirectToAction("Profile", "Account");
	}

	[Authorize(Roles = "IccDirector")]
	public IActionResult Students(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var students = this._userManager.Users.OfType<IccStudent>().ToList();
		var parameters = new[] { "FirstName", "LastName", "UniversityId", "Rut", "Email" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					students = students.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					students = students.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccStudent>();
			foreach (var parameter in parameters) {
				var partials = students.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
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
	public IActionResult Teachers(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var teachers = this._userManager.Users.OfType<IccTeacher>().ToList();
		var parameters = new[] { "FirstName", "LastName", "Rut", "Email" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (!string.IsNullOrEmpty(sortOrder)) {
			foreach (var parameter in parameters) {
				if (parameter == sortOrder) {
					teachers = teachers.OrderBy(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				} else if ($"{parameter}Desc" == sortOrder) {
					teachers = teachers.OrderByDescending(e => e.GetType().GetProperty(parameter)!.GetValue(e, null)).ToList();
					break;
				}
			}
		}
		if (!string.IsNullOrEmpty(searchString)) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccTeacher>();
			foreach (var parameter in parameters) {
				var partials = teachers.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty() && (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
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
	public async Task<IActionResult> BatchCreateStudents([FromForm] IFormFile csv) {
		try {
			var warningMessages = new List<string>();
			var successMessages = new List<string>();
			using var reader = new StreamReader(csv.OpenReadStream());
			using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
			var records = csvReader.GetRecords<Helpers.BatchCreateStudents>();
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
				_ = await this._userManager.AddToRoleAsync(user, "IccRegular");
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
	public async Task<IActionResult> CreateTeacher([FromForm] string email, [FromForm] string password, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string rut, [FromForm] bool isCommittee, [FromForm] bool isGuide) {
		var teacher = new IccTeacher {
			FirstName = firstName,
			LastName = lastName,
			Rut = rut
		};
		var roles = new List<string>();
		if (isCommittee) {
			roles.Add("IccCommittee");
		}
		if (isGuide) {
			roles.Add("IccGuide");
		}
		await this._userStore.SetUserNameAsync(teacher, email, CancellationToken.None);
		await this._emailStore.SetEmailAsync(teacher, email, CancellationToken.None);
		_ =  await this._userManager.CreateAsync(teacher, password);
		_ = await this._userManager.AddToRolesAsync(teacher, roles);
		this.TempData["SuccessMessage"] = "Profesor(a) creado(a) exitosamente.";
		return this.RedirectToAction("Teachers", "Account", new { area = string.Empty });
	}

	[Authorize(Roles = "IccDirector")]
	public async Task<IActionResult> Edit(string id) {
		var target = await this._userManager.FindByIdAsync(id);
		this.ViewBag.Id = id;
		this.ViewBag.Email = target!.Email;
		this.ViewBag.FirstName = target.FirstName;
		this.ViewBag.LastName = target.LastName;
		this.ViewBag.Rut = target.Rut;
		if (target is IccStudent student) {
			this.ViewBag.UniversityId = student.UniversityId;
			this.ViewBag.RemainingCourses = student.RemainingCourses;
			this.ViewBag.IsDoingThePractice = student.IsDoingThePractice;
			this.ViewBag.IsWorking = student.IsWorking;
			return this.View("EditStudent");
		} else if (target is IccTeacher teacher) {
			this.ViewBag.Office = teacher.Office;
			this.ViewBag.Schedule = teacher.Schedule;
			this.ViewBag.Specialization = teacher.Specialization;
			this.ViewBag.IsCommittee = await this._userManager.IsInRoleAsync(teacher, "IccCommittee");
			this.ViewBag.IsGuide = await this._userManager.IsInRoleAsync(teacher, "IccGuide");
			return this.View("EditTeacher");
		}
		return this.NotFound();
	}

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditStudent([FromForm] string id, [FromForm] string email, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string rut, [FromForm] string universityId, [FromForm] string remainingCourses, [FromForm] bool isDoingThePractice, [FromForm] bool isWorking) {
		var student = await this._userManager.FindByIdAsync(id) as IccStudent;
		student!.Email = email;
		student.FirstName = firstName;
		student.LastName = lastName;
		student.Rut = rut;
		student.UniversityId = universityId;
		student.RemainingCourses = remainingCourses;
		student.IsDoingThePractice = isDoingThePractice;
		student.IsWorking = isWorking;
		_ = await this._userManager.UpdateAsync(student);
		this.TempData["SuccessMessage"] = "Estudiante actualizado exitosamente.";
		return this.RedirectToAction("Students", "Account", new { area = string.Empty });
	}

	[Authorize(Roles = "IccDirector"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditTeacher([FromForm] string id, [FromForm] string email, [FromForm] string firstName, [FromForm] string lastName, [FromForm] string rut, [FromForm] string office, [FromForm] string schedule, [FromForm] string specialization, [FromForm] bool isCommittee, [FromForm] bool isGuide) {
		var teacher = await this._userManager.FindByIdAsync(id) as IccTeacher;
		teacher!.Email = email;
		teacher.FirstName = firstName;
		teacher.LastName = lastName;
		teacher.Rut = rut;
		teacher.Office = office;
		teacher.Schedule = schedule;
		teacher.Specialization = specialization;
		_ = await this._userManager.UpdateAsync(teacher);
		var roles = new List<string>();
		if (isCommittee) {
			roles.Add("IccCommittee");
		}
		if (isGuide) {
			roles.Add("IccGuide");
		}
		var currentRoles = (await this._userManager.GetRolesAsync(teacher)).ToList();
		var rolesToRemove = currentRoles.Except(roles).ToList();
		if (rolesToRemove.Contains("IccDirector")) {
			_ = rolesToRemove.Remove("IccDirector");
		}
		var rolesToAdd = roles.Except(currentRoles);
		_ = await this._userManager.RemoveFromRolesAsync(teacher, rolesToRemove);
		_ = await this._userManager.AddToRolesAsync(teacher, rolesToAdd);
		this.TempData["SuccessMessage"] = "Profesor(a) actualizado(a) exitosamente.";
		return this.RedirectToAction("Teachers", "Account", new { area = string.Empty });
	}
}