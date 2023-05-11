using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Shared;

/// <summary>
/// Partial viewmodel for the filter.
/// </summary>
public class FilterPartialViewModel {
	/// <summary>
	/// Target action.
	/// </summary>
	[Display(Name = "Acción")]
	public string Action { get; set; } = string.Empty;
	/// <summary>
	/// Target controller.
	/// </summary>
	[Display(Name = "Controlador")]
	public string Controller { get; set; } = string.Empty;
	/// <summary>
	/// Target area.
	/// </summary>
	[Display(Name = "Área")]
	public string Area { get; set; } = string.Empty;
	/// <summary>
	/// Search string (the filter).
	/// </summary>
	[Display(Name = "Filtro")]
	public string SearchString { get; set; } = string.Empty;

	/// <summary>
	/// Creates a filter.
	/// </summary>
	/// <param name="action">Target action.</param>
	/// <param name="controller">Controller action.</param>
	/// <param name="area">Area action.</param>
	public FilterPartialViewModel(string action, string controller, string area) {
		this.Action = action;
		this.Controller = controller;
		this.Area = area;
	}
}