using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccUser : IdentityUser {
	[Display(Name = "ID"), Required]
	public override string Id { get => base.Id; set => base.Id = value; }

	[DataType(DataType.EmailAddress), Display(Name = "E-mail"), EmailAddress, Required]
	public override string? Email { get => base.Email; set => base.Email = value; }

	[Display(Name = "Nombre"), Required]
	public string FirstName { get; set; } = string.Empty;

	[Display(Name = "Apellido"), Required]
	public string LastName { get; set; } = string.Empty;

	[Display(Name = "RUT"), Required]
	public string Rut { get; set; } = string.Empty;

	[Display(Name = "Desactivado(a)"), Required]
	public bool IsDeactivated { get; set; }

	[DataType(DataType.DateTime), Display(Name = "Creado(a)"), Required]
	public DateTimeOffset CreatedAt { get; set; }

	[DataType(DataType.DateTime), Display(Name = "Actualizado(a)"), Required]
	public DateTimeOffset UpdatedAt { get; set; }
}