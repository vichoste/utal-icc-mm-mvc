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
		this._emailStore = (IUserEmailStore<IccUser>)this._userStore;
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

	public async Task<IActionResult> Logout() {
		if (this.User.Identity!.IsAuthenticated) {
			await this._signInManager.SignOutAsync();
			this.TempData["SuccessMessage"] = "Se ha cerrado tu sesión correctamente.";
		}
		return this.RedirectToAction("Index", "Home", new { area = string.Empty });
	}
}