using System.Globalization;

using CsvHelper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Areas.University.Helpers;
using Utal.Icc.Mm.Mvc.Areas.University.Models.User;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.University.Controllers;

/// <summary>
/// Controller for the <see cref="IccUser">user</see>'s account.
/// </summary>
[Area("University"), Authorize]
public class UserController : Controller {
	private readonly UserManager<IccUser> _userManager;
	private readonly IUserStore<IccUser> _userStore;
	private readonly IUserEmailStore<IccUser> _emailStore;

	/// <summary>
	/// Creates a new instance of <see cref="UserController"/>.
	/// </summary>
	public UserController(UserManager<IccUser> userManager, IUserStore<IccUser> userStore) {
		this._userManager = userManager;
		this._userStore = userStore;
		this._emailStore = (IUserEmailStore<IccUser>)this._userStore;
	}

	#region Paginators
	/// <summary>
	/// Students' paginator.
	/// </summary>
	[Authorize(Roles = "Director")]
	public IActionResult Students(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var parameters = new[] { "FirstName", "LastName", "UniversityId", "Rut", "Email" };
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
		var users = this._userManager.Users
			.Where(u => u is IccStudent)
			.Cast<IccStudent>()
			.Select(u => new IccStudentViewModel {
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				UniversityId = u.UniversityId,
				Rut = u.Rut,
				Email = u.Email,
				IsDeactivated = u.IsDeactivated
			}).AsQueryable();
		var paginator = Paginator<IccStudentViewModel>.Create(users, pageNumber ?? 1, 10);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccStudentViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccStudentViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}

	/// <summary>
	/// Teachers' paginator.
	/// </summary>
	[Authorize(Roles = "Director")]
	public IActionResult Teachers(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		var parameters = new[] { "FirstName", "LastName", "Rut", "Email" };
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
		var users = this._userManager.Users
			.Where(u => u is IccTeacher)
			.Cast<IccStudent>()
			.Select(u => new IccTeacherViewModel {
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Rut = u.Rut,
				Email = u.Email,
				IsDeactivated = u.IsDeactivated
			}).AsQueryable();
		var paginator = Paginator<IccTeacherViewModel>.Create(users, pageNumber ?? 1, 10);
		if (!string.IsNullOrEmpty(sortOrder)) {
			paginator = Paginator<IccTeacherViewModel>.Sort(paginator.AsQueryable(), sortOrder, pageNumber ?? 1, 6, parameters);
		}
		if (!string.IsNullOrEmpty(searchString)) {
			paginator = Paginator<IccTeacherViewModel>.Filter(paginator.AsQueryable(), searchString, pageNumber ?? 1, 6, parameters);
		}
		return this.View(paginator);
	}
	#endregion

	#region CRUD
	/// <summary>
	/// Displays the view for creating new students.
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Director")]
	public IActionResult BatchCreateStudents() => this.View(new CsvFileViewModel());

	/// <summary>
	/// Creates new students.
	/// </summary>
	/// <param name="input">CSV containing the students' information.</param>
	[Authorize(Roles = "Director"), HttpPost]
	public async Task<IActionResult> BatchCreateStudents([FromForm] CsvFileViewModel input) {
		try {
			var warningMessages = new List<string>(); ;
			var successMessages = new List<string>();
			using var reader = new StreamReader(input.CsvFile!.OpenReadStream());
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
			var records = csv.GetRecords<CsvFileHelper>();
			foreach (var record in records) {
				var user = new IccStudent {
					FirstName = record.FirstName!,
					LastName = record.LastName!,
					UniversityId = record.UniversityId!,
					Rut = record.Rut!,
					CreatedAt = DateTimeOffset.Now,
					UpdatedAt = DateTimeOffset.Now
				};
				await this._userStore.SetUserNameAsync(user, record.Email, CancellationToken.None);
				await this._emailStore.SetEmailAsync(user, record.Email, CancellationToken.None);
				var result = await this._userManager.CreateAsync(user, record!.Password!);
				if (!result.Succeeded) {
					warningMessages.Add($"Estudiante con e-mail {record.Email} ya existe");
					continue;
				}
				_ = await this._userManager.AddToRoleAsync(user, "Student");
				successMessages.Add($"Estudiante con e-mail {record.Email} creado correctamente.");
			}
			if (warningMessages.Any()) {
				this.TempData["WarningMessages"] = warningMessages.AsEnumerable();
			}
			if (successMessages.Any()) {
				this.TempData["SuccessMessages"] = successMessages.AsEnumerable();
			}
			return this.RedirectToAction("Students", "User", new { area = "University" });
		} catch {
			this.TempData["ErrorMessage"] = "Error al importar el archivo CSV.";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		}
	}

	/// <summary>
	/// Displays the teacher creation view.
	/// </summary>
	[Authorize(Roles = "Director")]
	public IActionResult CreateTeacher() => this.View(new IccUserViewModel());

	/// <summary>
	/// Creates a new teacher.
	/// </summary>
	/// <param name="input">Teacher data.</param>
	[Authorize(Roles = "Director"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateTeacher([FromForm] IccTeacherViewModel input) {
		var roles = new List<string>();
		if (input.IsGuide) {
			roles.Add("Guide");
		}
		if (input.IsAssistant) {
			roles.Add("Assistant");
		}
		if (input.IsCommittee) {
			roles.Add("Committee");
		}
		var user = new IccTeacher {
			FirstName = input.FirstName,
			LastName = input.LastName,
			Rut = input.Rut,
			CreatedAt = DateTimeOffset.Now,
			UpdatedAt = DateTimeOffset.Now
		};
		await this._userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
		await this._emailStore.SetEmailAsync(user, input.Email, CancellationToken.None);
		_ = await this._userManager.CreateAsync(user, input.Password!);
		_ = await this._userManager.AddToRolesAsync(user, roles);
		this.TempData["SuccessMessage"] = "Profesor(a) creado(a) exitosamente.";
		return this.RedirectToAction("Teachers", "User", new { area = "University" });
	}

	/// <summary>
	/// Displays the student's edit view.
	/// </summary>
	[Authorize(Roles = "Director")]
	public async Task<IActionResult> EditStudent(string id) {
		var user = await this._userManager.FindByIdAsync(id);
		if (user is null || user is not IccStudent student) {
			this.TempData["ErrorMessage"] = "El(la) usuario(a) no es un estudiante.";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		}
		var output = new IccStudentViewModel {
			Id = student.Id,
			FirstName = student.FirstName,
			LastName = student.LastName,
			Rut = student.Rut,
			Email = student.Email,
			CreatedAt = student.CreatedAt,
			UpdatedAt = student.UpdatedAt,
			UniversityId = student.UniversityId
		};
		return this.View(output);
	}

	/// <summary>
	/// Displays the teacher's edit view.
	/// </summary>
	[Authorize(Roles = "Director")]
	public async Task<IActionResult> EditTeacher(string id) {
		var user = await this._userManager.FindByIdAsync(id);
		if (user is null || user is not IccTeacher teacher) {
			this.TempData["ErrorMessage"] = "El(la) usuario(a) no es un(a) profesor(a).";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		var output = new IccTeacherViewModel {
			Id = teacher.Id,
			FirstName = teacher.FirstName,
			LastName = teacher.LastName,
			Rut = teacher.Rut,
			Email = teacher.Email,
			CreatedAt = teacher.CreatedAt,
			UpdatedAt = teacher.UpdatedAt,
			IsAssistant = await this._userManager.IsInRoleAsync(user, "Assistant"),
			IsCommittee = await this._userManager.IsInRoleAsync(user, "Committee"),
			IsGuide = await this._userManager.IsInRoleAsync(user, "Guide")
		};
		return this.View(output);
	}

	/// <summary>
	/// Edits a student.
	/// </summary>
	/// <param name="input">Updated student's data.</param>
	[Authorize(Roles = "Director"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditStudent([FromForm] IccStudentViewModel input) {
		var user = await this._userManager.FindByIdAsync(input.Id!);
		if (user is null || user is not IccStudent student) {
			this.TempData["ErrorMessage"] = "El(la) usuario(a) no es un(a) estudiante.";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		}
		await this._userStore.SetUserNameAsync(student, input.Email, CancellationToken.None);
		await this._emailStore.SetEmailAsync(student, input.Email, CancellationToken.None);
		student.FirstName = input.FirstName;
		student.LastName = input.LastName;
		student.Rut = input.Rut;
		student.UniversityId = input.UniversityId;
		student.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(student);
		var output = new IccStudentViewModel {
			Id = student.Id,
			FirstName = student.FirstName,
			LastName = student.LastName,
			Rut = student.Rut,
			Email = student.Email,
			CreatedAt = student.CreatedAt,
			UpdatedAt = student.UpdatedAt,
			UniversityId = student.UniversityId
		};
		this.ViewBag.SuccessMessage = "Estudiante editado(a) correctamente.";
		return this.View(output);
	}

	/// <summary>
	/// Edits a teacher.
	/// </summary>
	/// <param name="input">Updated teacher's data.</param>
	[Authorize(Roles = "Director"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> EditTeacher([FromForm] IccTeacherViewModel input) {
		var user = await this._userManager.FindByIdAsync(input.Id!);
		if (user is null || user is not IccTeacher teacher) {
			this.TempData["ErrorMessage"] = "El profesor no es un estudiante.";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		await this._userStore.SetUserNameAsync(teacher, input.Email, CancellationToken.None);
		await this._emailStore.SetEmailAsync(teacher, input.Email, CancellationToken.None);
		teacher.FirstName = input.FirstName;
		teacher.LastName = input.LastName;
		teacher.Rut = input.Rut;
		var roles = (await this._userManager.GetRolesAsync(teacher)).ToList();
		if (roles.Contains("Teacher")) {
			_ = roles.Remove("Teacher");
		}
		if (roles.Contains("Director")) {
			_ = roles.Remove("Director");
		}
		var rankRoles = new List<string>();
		if (input.IsGuide) {
			rankRoles.Add("Guide");
		}
		if (input.IsAssistant) {
			rankRoles.Add("Assistant");
		}
		if (input.IsCommittee) {
			rankRoles.Add("Committee");
		}
		_ = await this._userManager.AddToRolesAsync(teacher, rankRoles);
		teacher.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(teacher);
		var output = new IccTeacherViewModel {
			Id = teacher.Id,
			FirstName = teacher.FirstName,
			LastName = teacher.LastName,
			Rut = teacher.Rut,
			Email = teacher.Email,
			CreatedAt = teacher.CreatedAt,
			UpdatedAt = teacher.UpdatedAt,
			IsAssistant = await this._userManager.IsInRoleAsync(teacher, "Assistant"),
			IsCommittee = await this._userManager.IsInRoleAsync(teacher, "Committee"),
			IsGuide = await this._userManager.IsInRoleAsync(teacher, "Guide")
		};
		this.ViewBag.SuccessMessage = "Profesor(a) editado(a) correctamente.";
		return this.View(output);
	}
	#endregion

	#region Utilities
	/// <summary>
	/// Displays a confirmation view for user activation/deactivation.
	/// </summary>
	[Authorize(Roles = "Director")]
	public async Task<IActionResult> Toggle(string id) {
		var user = await this._userManager.FindByIdAsync(id);
		if (user is null) {
			this.TempData["ErrorMessage"] = "Error al obtener al(a) usuario(a).";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		}
		if (user.Id == this._userManager.GetUserId(this.User)) {
			this.TempData["ErrorMessage"] = "No te puedes desactivar a tí mismo.";
			if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(id))!, "Student")) {
				return this.RedirectToAction("Students", "User", new { area = "University" });
			} else if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(id))!, "Teacher")) {
				return this.RedirectToAction("Teachers", "User", new { area = "University" });
			}
			return this.RedirectToAction("Students", "User", new { area = "University" });
		} else if ((await this._userManager.GetRolesAsync(user)).Contains("Director")) {
			this.TempData["ErrorMessage"] = "No puedes desactivar al(a) director(a) de carrera actual.";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		var output = new IccUserViewModel {
			Id = user.Id,
			Email = user.Email,
			IsDeactivated = user.IsDeactivated
		};
		return this.View(output);
	}

	/// <summary>
	/// Toggles the activation status for an user.
	/// </summary>
	/// <param name="input"></param>
	[Authorize(Roles = "Director"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Toggle([FromForm] IccUserViewModel input) {
		var user = await this._userManager.FindByIdAsync(input.Id!);
		if (user is null) {
			this.TempData["ErrorMessage"] = "Error al obtener al(la) usuario.";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		}
		if (user.Id == this._userManager.GetUserId(this.User)) {
			this.TempData["ErrorMessage"] = "No te puedes desactivar a tí mismo.";
			if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(input.Id!))!, "Student")) {
				return this.RedirectToAction("Students", "User", new { area = "University" });
			} else if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(input.Id!))!, "Teacher")) {
				return this.RedirectToAction("Teachers", "User", new { area = "University" });
			}
			return this.RedirectToAction("Students", "User", new { area = "University" });
		} else if ((await this._userManager.GetRolesAsync(user)).Contains("Director")) {
			this.TempData["ErrorMessage"] = "No puedes desactivar al(la) director de carrera actual.";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		user.IsDeactivated = !user.IsDeactivated;
		user.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(user);
		if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(input.Id!))!, "Student")) {
			this.TempData["SuccessMessage"] = user.IsDeactivated ? "Estudiante desactivado(a) correctamente." : "Estudiante activado(a) correctamente.";
			return this.RedirectToAction("Students", "User", new { area = "University" });
		} else if (await this._userManager.IsInRoleAsync((await this._userManager.FindByIdAsync(input.Id!))!, "Teacher")) {
			this.TempData["SuccessMessage"] = user.IsDeactivated ? "Profesor(a) desactivado(a) correctamente." : "Profesor(a) activado(a) correctamente.";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		this.TempData["SuccessMessage"] = user.IsDeactivated ? "Estudiante desactivado(a) correctamente." : "Estudiante activado(a) correctamente.";
		return this.RedirectToAction("Students", "User", new { area = "University" });
	}

	/// <summary>
	/// Displays the director teacher's transfer confirmation view.
	/// </summary>
	[Authorize(Roles = "Director")]
	public async Task<IActionResult> Transfer(string currentDirectorTeacherId, string newDirectorTeacherId) {
		var current = await this._userManager.FindByIdAsync(currentDirectorTeacherId);
		var @new = await this._userManager.FindByIdAsync(newDirectorTeacherId);
		var check = current is not null && @new is not null;
		check = check && (await this._userManager.GetRolesAsync(current!)).Contains("Director");
		check = check && (await this._userManager.GetRolesAsync(@new!)).Contains("Teacher");
		check = check && current!.Id != @new!.Id;
		if (!check) {
			this.TempData["ErrorMessage"] = "Revisa los(as) profesores fuente y objetivo antes de hacer la transferencia.";
			return this.RedirectToAction("Users", "User", new { area = "University" });
		}
		var transferViewModel = new TransferViewModel {
			CurrentDirectorTeacherId = current!.Id,
			NewDirectorTeacherId = @new!.Id,
			NewDirectorTeacherName = $"{@new.FirstName} {@new.LastName}"
		};
		return this.View(transferViewModel);
	}

	/// <summary>
	/// Transfers the director teacher role from one teacher to another.
	/// </summary>
	/// <param name="model">Teachers' information.</param>
	[Authorize(Roles = "Director"), HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Transfer([FromForm] TransferViewModel model) {
		var current = await this._userManager.FindByIdAsync(model.CurrentDirectorTeacherId!);
		var @new = await this._userManager.FindByIdAsync(model.NewDirectorTeacherId!);
		var check = current is not null && @new is not null;
		check = check && (await this._userManager.GetRolesAsync(current!)).Contains("Director");
		check = check && (await this._userManager.GetRolesAsync(@new!)).Contains("Teacher");
		check = check && current!.Id != @new!.Id;
		if (!check) {
			this.TempData["ErrorMessage"] = "Revisa los(as) profesores fuente y objetivo antes de hacer la transferencia.";
			return this.RedirectToAction("Teachers", "User", new { area = "University" });
		}
		_ = await this._userManager.RemoveFromRoleAsync(current!, "Director");
		_ = await this._userManager.AddToRoleAsync(@new!, "Director");
		current!.UpdatedAt = DateTimeOffset.Now;
		@new!.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(current);
		this.TempData["SuccessMessage"] = "Transferencia realizada correctamente.";
		return this.RedirectToAction("Teachers", "User", new { area = "University" });
	}
	#endregion
}