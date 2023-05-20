using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Models;
using Utal.Icc.Mm.Mvc.Models.Dtos;

namespace Utal.Icc.Mm.Mvc.Controllers;

public class AccountController : Controller {
	private readonly IUserEmailStore<IccUser> _emailStore;
	private readonly SignInManager<IccUser> _signInManager;
	private readonly UserManager<IccUser> _userManager;
	private readonly IUserStore<IccUser> _userStore;

	public AccountController(UserManager<IccUser> userManager, SignInManager<IccUser> signInManager, IUserStore<IccUser> userStore) {
		this._emailStore = (IUserEmailStore<IccUser>)this._userStore!;
		this._signInManager = signInManager;
		this._userManager = userManager;
		this._userStore = userStore;
	}

	public IActionResult Login() => this.User.Identity!.IsAuthenticated ? this.RedirectToAction("Index", "Home") : this.View();

	[HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Login([FromForm] LoginDto dto) {
		if (!this.ModelState.IsValid) {
			return this.View(dto);
		}
		if (this.User.Identity!.IsAuthenticated) {
			return this.RedirectToAction("Index", "Home");
		}
		var result = await this._signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, false);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Credenciales incorrectas.";
			return this.View(new LoginDto());
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
	public async Task<IActionResult> Password([FromForm] PasswordDto dto) {
		if (!this.ModelState.IsValid) {
			return this.View(dto);
		}
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home");
		}
		var result = await this._userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Contraseña incorrecta.";
			this.ViewBag.DangerMessages = result.Errors.Select(e => e.Description).ToList();
			return this.View(dto);
		}
		user.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(user);
		this.TempData["SuccessMessage"] = "Has cambiado tu contraseña correctamente.";
		return this.RedirectToAction("Index", "Home");
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Code legibility")]
	public async Task<IActionResult> Profile() {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home");
		}
		if (user is IccStudent student) {
			return this.View("StudentProfile", student);
		}
		if (user is IccTeacher teacher) {
			return this.View("TeacherProfile", teacher);
		}
		return this.NotFound();
	}

	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Profile([FromForm] IccUser model) {
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home");
		}
		if (user is IccStudent student && model is IccStudent studentModel) {
			if (!this.ModelState.IsValid) {
				return this.View("StudentProfile", student);
			}
			student.RemainingCourses = studentModel.RemainingCourses;
			student.IsDoingThePractice = studentModel.IsDoingThePractice;
			student.UpdatedAt = DateTimeOffset.Now;
			_ = await this._userManager.UpdateAsync(student);
			this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
			return this.View();
		} else if (user is IccTeacher teacher && model is IccTeacher teacherModel) {
			if (!this.ModelState.IsValid) {
				return this.View("TeacherProfile", teacher);
			}
			teacher.Office = teacherModel.Office;
			teacher.Schedule = teacherModel.Schedule;
			teacher.Specialization = teacherModel.Specialization;
			teacher.UpdatedAt = DateTimeOffset.Now;
			_ = await this._userManager.UpdateAsync(teacher);
			this.TempData["SuccessMessage"] = "Has actualizado tu perfil correctamente.";
			return this.View();
		}
		return this.NotFound();
	}
}