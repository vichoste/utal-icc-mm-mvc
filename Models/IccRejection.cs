namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection reason for a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccRejection {
	/// <summary>
	/// Rejection (ID).
	/// </summary>
	public string? Id { get; set; }
	/// <summary>
	/// Rejection reason.
	/// </summary>
	public string Reason { get; set; } = string.Empty;
	/// <summary>
	/// Rejected <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public IccMemoir? Memoir { get; set; }
	/// <summary>
	/// Rejected <see cref="IccMemoir">memoir</see> (ID).
	/// </summary>
	public string? MemoirId { get; set; }
	/// <summary>
	/// Rejection creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Rejection updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}