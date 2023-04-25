using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Areas.Account.Models.Profile;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.Account.Controllers;

/// <summary>
/// Controller for the user's profile.
/// </summary>
[Area("Account")]
public class ProfileController : Controller {
	private readonly UserManager<IccUser> _userManager;
	private readonly IUserStore<IccUser> _userStore;
	private readonly IUserEmailStore<IccUser> _emailStore;

	/// <summary>
	/// Creates a new instance of <see cref="ProfileController"/>.
	/// </summary>
	/// <param name="userManager">User manager injection.</param>
	/// <param name="userStore">User store injection.</param>
	public ProfileController(UserManager<IccUser> userManager, IUserStore<IccUser> userStore) {
		this._userManager = userManager;
		this._userStore = userStore;
		this._emailStore = (IUserEmailStore<IccUser>)this._userStore;
	}

	/// <summary>
	/// Displays the user's profile.
	/// </summary>
	public async Task<IActionResult> Index() {
		if (await this._userManager.GetUserAsync(this.User) is not IccUser user) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		var output = new IccUserViewModel {
			FirstName = user!.FirstName,
			LastName = user.LastName,
			Rut = user.Rut,
			Email = await this._emailStore.GetEmailAsync(user, CancellationToken.None),
			CreatedAt = user.CreatedAt,
			UpdatedAt = user.UpdatedAt
		};
		return this.View(output);
	}

	/// <summary>
	/// Displays the password changer.
	/// </summary>
	[Authorize]
	public IActionResult ChangePassword() => this.View();

	/// <summary>
	/// Changes the password of the user.
	/// </summary>
	/// <param name="model">Old, new and confirmation passwords.</param>
	/// <returns>Returns to <see cref="Index"/> if successful. Otherwise, return <see cref="ChangePassword()"/> with the warning or error messages.</returns>
	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordViewModel model) {
		if (!this.ModelState.IsValid) {
			this.ViewBag.WarningMessage = "Revisa que los campos estén correctos.";
			return this.View(model);
		}
		var user = await this._userManager.GetUserAsync(this.User);
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		var result = await this._userManager.ChangePasswordAsync(user!, model.CurrentPassword!, model.NewPassword!);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Contraseña incorrecta.";
			this.ViewBag.DangerMessages = result.Errors.Select(e => e.Description).ToList();
			return this.View(model);
		}
		user.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(user);
		this.TempData["SuccessMessage"] = "Has cambiado tu contraseña correctamente.";
		return this.RedirectToAction("Index", "Profile", new { area = "Account" });
	}

	/// <summary>
	/// Displays the student's profile.
	/// </summary>
	[Authorize]
	public async Task<IActionResult> Student() {
		var user = await this._userManager.GetUserAsync(this.User) as IccStudent;
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		var output = new IccStudentViewModel {
			UniversityId = user.UniversityId,
			RemainingCourses = user.RemainingCourses,
			CurrentPractice = user.IsDoingThePractice,
			IsWorking = user.IsWorking
		};
		return this.View(output);
	}

	/// <summary>
	/// Updates the student's profile.
	/// </summary>
	/// <param name="model">Updated information of the student.</param>
	/// <returns>Same view with success message.</returns>
	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Student([FromForm] IccStudentViewModel model) {
		if (!this.ModelState.IsValid) {
			this.ViewBag.WarningMessage = "Revisa que los campos estén correctos.";
			return this.View(model);
		}
		var user = await this._userManager.GetUserAsync(this.User) as IccStudent;
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		user.RemainingCourses = model.RemainingCourses;
		user.IsDoingThePractice = model.CurrentPractice;
		user.IsWorking = model.IsWorking;
		user.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(user);
		var output = new IccStudentViewModel {
			UniversityId = user.UniversityId,
			RemainingCourses = user.RemainingCourses,
			CurrentPractice = user.IsDoingThePractice,
			IsWorking = user.IsWorking
		};
		this.ViewBag.SuccessMessage = "Has actualizado tu perfil correctamente.";
		return this.View(output);
	}

	/// <summary>
	/// Displays the teacher's profile.
	/// </summary>
	[Authorize]
	public async Task<IActionResult> Teacher() {
		var user = await this._userManager.GetUserAsync(this.User) as IccTeacher;
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		var output = new IccTeacherViewModel {
			Office = user.Office,
			Schedule = user.Schedule,
			Specialization = user.Specialization
		};
		return this.View(output);
	}

	/// <summary>
	/// Updates the teacher's profile.
	/// </summary>
	/// <param name="model">Updated information of the teacher.</param>
	/// <returns>Same view with success message.</returns>
	[Authorize, HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Teacher([FromForm] IccTeacherViewModel model) {
		if (!this.ModelState.IsValid) {
			this.ViewBag.WarningMessage = "Revisa que los campos estén correctos.";
			return this.View(model);
		}
		var user = await this._userManager.GetUserAsync(this.User) as IccTeacher;
		if (user!.IsDeactivated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		user.Office = model.Office;
		user.Schedule = model.Schedule;
		user.Specialization = model.Specialization;
		user.UpdatedAt = DateTimeOffset.Now;
		_ = await this._userManager.UpdateAsync(user);
		var output = new IccTeacherViewModel {
			Office = model.Office,
			Schedule = model.Schedule,
			Specialization = model.Specialization
		};
		this.ViewBag.SuccessMessage = "Has actualizado tu perfil correctamente.";
		return this.View(output);
	}
}