using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.University.Controllers;

/// <summary>
/// Controller for the memoir proposals.
/// </summary>
[Area("University"), Authorize]
public class ProposalController : Controller {
	private readonly IccDbContext _dbContext;
	private readonly UserManager<IccUser> _userManager;

	/// <summary>
	/// Creates a new instance of <see cref="ProposalController"/>.
	/// </summary>
	/// <param name="dbContext">Database context.</param>
	/// <param name="userManager">User manager.</param>
	public ProposalController(IccDbContext dbContext, UserManager<IccUser> userManager) {
		this._dbContext = dbContext;
		this._userManager = userManager;
	}

	#region Helpers
	protected async Task PopulateAssistants(IccTeacher guide) {
		var assistants = (
			await this._userManager.GetUsersInRoleAsync("Assistant"))
				.Where(u => u != guide && !u.IsDeactivated)
				.OrderBy(u => u.LastName)
				.ToList();
		this.ViewData[$"Assistants"] = assistants.Select(u => new SelectListItem {
			Text = $"{u.FirstName} {u.LastName}",
			Value = u.Id
		});
	}
	#endregion

	#region Paginators

	#endregion

	#region CRUD
	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	[Authorize(Roles = "Guide")]
	public async Task<IActionResult> Create() {
		var user = await this._userManager.GetUserAsync(this.User) as IccTeacher;
		if (user!.IsDeactivated) {
			this.TempData["ErrorMessage"] = "Tu cuenta está desactivada.";
			return this.RedirectToAction("Index", "Home", new { area = "" });
		}
		await this.PopulateAssistants(user!);
		return this.View(new IccMemoirViewModel());
	}

	#endregion
}