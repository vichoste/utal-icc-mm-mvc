﻿using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;

using Utal.Icc.Mm.Mvc.Views.Shared;

namespace Utal.Icc.Mm.Mvc.Controllers;

public class HomeController : Controller {
	public HomeController() { }

	public IActionResult Index() => this.View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}