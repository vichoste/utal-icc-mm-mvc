﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Areas.Account.Models.SignIn;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.Account.Controllers;

/// <summary>
/// Controller for the sign-in.
/// </summary>
[Area("Account")]
public class SignInController : Controller {
	private readonly SignInManager<IccUser> _signInManager;

	/// <summary>
	/// Creates a new instance of <see cref="SignInController"/>.
	/// </summary>
	public SignInController(SignInManager<IccUser> signInManager) => this._signInManager = signInManager;

	/// <summary>
	/// Displays the sign-in page.
	/// </summary>
	public IActionResult Index() => this.User.Identity!.IsAuthenticated ? this.RedirectToAction("Index", "Home", new { area = string.Empty }) : this.View();

	/// <summary>
	/// Signs in the user.
	/// </summary>
	/// <param name="model">Login form.</param>
	[HttpPost, ValidateAntiForgeryToken]
	public async Task<IActionResult> Index([FromForm] IndexViewModel model) {
		if (!this.ModelState.IsValid) {
			return this.View(model);
		}
		if (this.User.Identity!.IsAuthenticated) {
			return this.RedirectToAction("Index", "Home", new { area = string.Empty });
		}
		var result = await this._signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
		if (!result.Succeeded) {
			this.ViewBag.DangerMessage = "Credenciales incorrectas.";
			return this.View(new IndexViewModel());
		}
		return this.RedirectToAction("Index", "Home", new { area = string.Empty });
	}
}