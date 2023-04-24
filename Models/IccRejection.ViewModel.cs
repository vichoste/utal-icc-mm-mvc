using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection reason for a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccRejectionViewModel : ViewModel {
	/// <summary>
	/// Rejection reason.
	/// </summary>
	[Display(Name = "Razón")]
	public string Reason { get; set; } = string.Empty;
	/// <summary>
	/// Rejected <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public IccMemoirViewModel? Memoir { get; set; }
}