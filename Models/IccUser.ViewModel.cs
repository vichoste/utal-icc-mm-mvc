using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a user from the Computer Engineering carrer at University of Talca.
/// </summary>
public class IccUserViewModel {
	/// <summary>
	/// User (ID).
	/// </summary>
	[Display(Name = "ID")]
	public string ID { get; set; } = string.Empty;
	/// <summary>
	/// User's first name.
	/// </summary>
	[Display(Name = "Nombre")]
	public string FirstName { get; set; } = string.Empty;
	/// <summary>
	/// User's last name.
	/// </summary>
	[Display(Name = "Apellido")]
	public string LastName { get; set; } = string.Empty;
	/// <summary>
	/// User's email.
	/// </summary>
	[DataType(DataType.EmailAddress), Display(Name = "E-mail"), EmailAddress]
	public string? Email { get; set; }
	/// <summary>
	/// User's chlean RUT.
	/// </summary>
	[Display(Name = "RUT")]
	public string Rut { get; set; } = string.Empty;
	/// <summary>
	/// Checks if the user is deactivated.
	/// </summary>
	[Display(Name = "Desactivado(a)")]
	public bool IsDeactivated { get; set; }
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