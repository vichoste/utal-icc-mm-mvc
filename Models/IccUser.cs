using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccUser : IdentityUser {
	[Display(Name = "ID")]
	public override string Id { get => base.Id; set => base.Id = value; }

	[DataType(DataType.EmailAddress), Display(Name = "E-mail"), EmailAddress]
	public override string? Email { get => base.Email; set => base.Email = value; }

	[Display(Name = "Nombre")]
	public string FirstName { get; set; } = string.Empty;

	[Display(Name = "Apellido")]
	public string LastName { get; set; } = string.Empty;

	[Display(Name = "RUT")]
	public string Rut { get; set; } = string.Empty;

	[Display(Name = "Desactivado(a)")]
	public bool IsDeactivated { get; set; }

	[DataType(DataType.DateTime), Display(Name = "Creado(a)")]
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

	[DataType(DataType.DateTime), Display(Name = "Actualizado(a)")]
	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}