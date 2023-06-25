using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Shared;

public class FilterPartialViewModel {
	[Display(Name = "Acción")]
	public string Action { get; set; } = string.Empty;

	[Display(Name = "Controlador")]
	public string Controller { get; set; } = string.Empty;

	[Display(Name = "Filtro")]
	public string SearchString { get; set; } = string.Empty;

	public bool IsMemoir { get; set; }

	[Display(Name = "Estado de memoria")]
	public string Phase { get; set; } = string.Empty;

	public FilterPartialViewModel(string action, string controller, bool isMemoir = false, string phase = "") {
		this.Action = action;
		this.Controller = controller;
		this.IsMemoir = isMemoir;
		this.Phase = phase;
	}
}