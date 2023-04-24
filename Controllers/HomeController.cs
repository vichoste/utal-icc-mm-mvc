using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Views.Shared;

namespace Utal.Icc.Mm.Mvc.Controllers;

/// <summary>
/// Home controller.
/// </summary>
public class HomeController : Controller {
	/// <summary>
	/// Creates a home controller.
	/// </summary>
	public HomeController() { }

	/// <summary>
	/// Returns the index view.
	/// </summary>
	/// <returns>Index view.</returns>
	public IActionResult Index() => this.View();

	/// <summary>
	/// Returns the error view.
	/// </summary>
	/// <returns>Error view.</returns>
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}