namespace Utal.Icc.Mm.Mvc.Views.Shared;
/// <summary>
/// Error view model.
/// </summary>
public class ErrorViewModel {
	/// <summary>
	/// Request ID.
	/// </summary>
	public string RequestId { get; set; } = string.Empty;

	/// <summary>
	/// Shows the request ID.
	/// </summary>
	public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}