namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection reason for a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccRejection {
	/// <summary>
	/// Rejection ID.
	/// </summary>
	public Guid Id { get; set; } = Guid.NewGuid();
	/// <summary>
	/// Rejection reason.
	/// </summary>
	public string Reason { get; set; } = string.Empty;
	/// <summary>
	/// Linked <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public IccMemoir? Memoir { get; set; }
	/// <summary>
	/// Memoir creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Memoir updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}