using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccUser : IdentityUser {
	[DataType(DataType.EmailAddress), EmailAddress]
	public override string? Email { get => base.Email; set => base.Email = value; }

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	[NotMapped]
	public string FullName => $"{this.FirstName} {this.LastName}";

	public string Rut { get; set; } = string.Empty;
}