using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Shared;

/// <summary>
/// Paginator partial viewmodel.
/// </summary>
public class PaginatorPartialViewModel {
	/// <summary>
	/// Target action of the paginator.
	/// </summary>
	[Display(Name = "Acción")]
	public string? Action { get; set; }
	/// <summary>
	/// Page index of the paginator.
	/// </summary>
	[Display(Name = "Índice de página")]
	public int? PageIndex { get; set; }
	/// <summary>
	/// Total pages of the paginator.
	/// </summary>
	[Display(Name = "Total de páginas")]
	public int? TotalPages { get; set; }
	/// <summary>
	/// Checks if the paginator has a previous page.
	/// </summary>
	[Display(Name = "Página anterior")]
	public bool HasPreviousPage { get; set; }
	/// <summary>
	/// Checks if the paginator has a next page.
	/// </summary>
	[Display(Name = "Página siguiente")]
	public bool HasNextPage { get; set; }

	/// <summary>
	/// Creates a paginator partial viewmodel.
	/// </summary>
	/// <param name="action"></param>
	/// <param name="pageIndex"></param>
	/// <param name="totalPages"></param>
	/// <param name="hasPreviousPage"></param>
	/// <param name="hasNextPage"></param>
	public PaginatorPartialViewModel(string action, int pageIndex, int totalPages, bool hasPreviousPage, bool hasNextPage) {
		this.Action = action;
		this.PageIndex = pageIndex;
		this.TotalPages = totalPages;
		this.HasPreviousPage = hasPreviousPage;
		this.HasNextPage = hasNextPage;
	}
}