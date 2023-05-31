using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Utal.Icc.Mm.Mvc.Data;
using Utal.Icc.Mm.Mvc.Models;
using Utal.Icc.Mm.Mvc.Views.Shared;

using X.PagedList;

namespace Utal.Icc.Mm.Mvc.Controllers;

public class MemoirController : Controller {
	private readonly IccDbContext _dbContext;

	public MemoirController(IccDbContext dbContext) => this._dbContext = dbContext;

	[Authorize(Roles = "IccRegular,IccGuide")]
	public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? pageNumber) {
		List<IccMemoir> memoirs = new();
		if (this.User.IsInRole("IccRegular")) {
			memoirs = this._dbContext.IccMemoirs
				.Include(m => m.Student)
				.Where(m => m.Student!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
				.ToList();
		} else if (this.User.IsInRole("IccGuide")) {
			memoirs = this._dbContext.IccMemoirs
				.Include(m => m.GuideTeacher)
				.Where(m => m.GuideTeacher!.Id == this.User.FindFirstValue(ClaimTypes.NameIdentifier))
				.ToList();
		}
		var parameters = new[] { "Title" };
		foreach (var parameter in parameters) {
			this.ViewData[$"{parameter}SortParam"] = sortOrder == parameter ? $"{parameter}Desc" : parameter;
		}
		this.ViewData["CurrentSort"] = sortOrder;
		if (searchString is not null) {
			pageNumber = 1;
			this.ViewData["CurrentFilter"] = searchString;
			var filtered = new List<IccMemoir>();
			foreach (var parameter in parameters) {
				var partials = memoirs
					.Where(vm => !(vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.IsNullOrEmpty()
						&& (vm.GetType().GetProperty(parameter)!.GetValue(vm, null) as string)!.Contains(searchString));
				foreach (var partial in partials) {
					if (!filtered.Any(vm => vm.Id == partial.Id)) {
						filtered.Add(partial);
					}
				}
			}
			this.ViewBag.Memoirs = filtered.ToPagedList(pageNumber ?? 1, 10);
			return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)filtered.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)filtered.Count / 10)));
		}
		searchString = currentFilter;
		this.ViewBag.Memoirs = memoirs.ToPagedList(pageNumber ?? 1, 10);
		return this.View(new PaginatorPartialViewModel("Index", pageNumber ?? 1, (int)Math.Ceiling((decimal)memoirs.Count / 10), pageNumber > 1, pageNumber < (int)Math.Ceiling((decimal)memoirs.Count / 10)));
	}
}