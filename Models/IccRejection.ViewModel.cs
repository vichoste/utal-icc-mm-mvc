using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection reason for a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccRejectionViewModel {
	/// <summary>
	/// Rejection (ID).
	/// </summary>
	[Display(Name = "ID")]
	public string? Id { get; set; }
	/// <summary>
	/// Rejection reason.
	/// </summary>
	[Display(Name = "Razón")]
	public string Reason { get; set; } = string.Empty;
	/// <summary>
	/// Rejected <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public IccMemoirViewModel? Memoir { get; set; }
	/// <summary>
	/// Rejection creation timestamp.
	/// </summary>
	[Display(Name = "Creado")]
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Rejection updated timestamp.
	/// </summary>
	[Display(Name = "Actualizado")]
	public DateTimeOffset UpdatedAt { get; set; }
}