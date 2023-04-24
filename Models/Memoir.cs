namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a memoir.
/// </summary>
public class Memoir {
	/// <summary>
	/// Memoir ID.
	/// </summary>
	public Guid Id { get; set; } = Guid.NewGuid();
	/// <summary>
	/// Memoir title.
	/// </summary>
	public string Title { get; set; } = string.Empty;
	/// <summary>
	/// Memoir description.
	/// </summary>
	public string Description { get; set; } = string.Empty;
	/// <summary>
	/// Memoir creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Memoir updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
	/// <summary>
	/// Rejections for this memoir (if they have one or more).
	/// </summary>
	public ICollection<Rejection> Rejections { get; set; } = new HashSet<Rejection>();
}