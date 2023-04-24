using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Abstraction of a view model.
/// </summary>
public abstract class ViewModel {
	/// <summary>
	/// User (ID).
	/// </summary>
	[Display(Name = "ID")]
	public string Id { get; set; } = string.Empty;
	/// <summary>
	/// User creation timestamp.
	/// </summary>
	[Display(Name = "Creado(a)")]
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// User updated timestamp.
	/// </summary>
	[Display(Name = "Actualizado(a)")]
	public DateTimeOffset UpdatedAt { get; set; }
}