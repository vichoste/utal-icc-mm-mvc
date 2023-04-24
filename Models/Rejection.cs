namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection reason for a <see cref="Memoir">memoir</see>.
/// </summary>
public class Rejection {
	/// <summary>
	/// Rejection ID.
	/// </summary>
	public Guid Id { get; set; } = Guid.NewGuid();
	/// <summary>
	/// Rejection reason.
	/// </summary>
	public string Reason { get; set; } = string.Empty;
	/// <summary>
	/// Linked memoir.
	/// </summary>
	public Memoir Memoir { get; set; }
	/// <summary>
	/// Linked memoir ID.
	/// </summary>
	public int MemoirId { get; set; }
	/// <summary>
	/// Memoir creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Memoir updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}