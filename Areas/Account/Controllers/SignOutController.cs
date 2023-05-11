using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.Account.Controllers;

/// <summary>
/// Controller for the sign-out.
/// </summary>
[Area("Account")]
public class SignOutController : Controller {
	private readonly SignInManager<IccUser> _signInManager;

	/// <summary>
	/// Creates a new instance of <see cref="SignInController"/>.
	/// </summary>
	public SignOutController(SignInManager<IccUser> signInManager) => this._signInManager = signInManager;

	/// <summary>
	/// Signs out the user.
	/// </summary>
	public async Task<IActionResult> Index() {
		if (this.User.Identity!.IsAuthenticated) {
			await this._signInManager.SignOutAsync();
			this.TempData["SuccessMessage"] = "Se ha cerrado tu sesión correctamente.";
		}
		return this.RedirectToAction("Index", "Home", new { area = string.Empty });
	}
}