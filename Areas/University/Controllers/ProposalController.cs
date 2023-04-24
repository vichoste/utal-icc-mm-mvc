using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Areas.University.Controllers;

[Area("University"), Authorize]
public class ProposalController : Controller {
	private readonly IccDbContext _dbContext;
	private readonly UserManager<IccUser> _userManager;

	public ProposalController(IccDbContext dbContext, UserManager<IccUser> userManager) {
		this._dbContext = dbContext;
		this._userManager = userManager;
	}

	#region Helpers
	private async Task PopulateAssistants(IccTeacher guide) {
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

	#region CRUD

	#endregion
}