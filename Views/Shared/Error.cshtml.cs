namespace Utal.Icc.Mm.Mvc.Views.Shared;

public class ErrorViewModel {
	public string RequestId { get; set; } = string.Empty;

	public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}