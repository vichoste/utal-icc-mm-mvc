using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Controllers;

[Authorize(Roles = "IccStudent,IccGuide,IccCommittee,IccDirector")]
public class MemoirController : Controller {
	private readonly IccDbContext _dbContext;

	public MemoirController(IccDbContext dbContext, UserManager<IccUser> userManager) => this._dbContext = dbContext;

	public async Task<IActionResult> Proposals(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		if (this.User.IsInRole("IccStudent")) {
			return this.View();
		}
		return this.NotFound();
	}
}