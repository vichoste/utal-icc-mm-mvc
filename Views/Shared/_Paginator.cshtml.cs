namespace Utal.Icc.Mm.Mvc.Views.Shared;

public class PaginatorPartialViewModel {

	public string Action { get; set; } = string.Empty;

	public int PageIndex { get; set; }

	public int TotalPages { get; set; }

	public bool HasPreviousPage { get; set; }

	public bool HasNextPage { get; set; }

	public PaginatorPartialViewModel(string action, int pageIndex, int totalPages, bool hasPreviousPage, bool hasNextPage) {
		this.Action = action;
		this.PageIndex = pageIndex;
		this.TotalPages = totalPages;
		this.HasPreviousPage = hasPreviousPage;
		this.HasNextPage = hasNextPage;
	}
}