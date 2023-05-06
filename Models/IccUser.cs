using Microsoft.AspNetCore.Identity;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a user from the Computer Engineering career at University of Talca.
/// </summary>
public class IccUser : IdentityUser {
	/// <summary>
	/// User's first name.
	/// </summary>
	public string FirstName { get; set; } = string.Empty;
	/// <summary>
	/// User's last name.
	/// </summary>
	public string LastName { get; set; } = string.Empty;
	/// <summary>
	/// User's chlean RUT.
	/// </summary>
	public string Rut { get; set; } = string.Empty;
	/// <summary>
	/// Checks if the user is deactivated.
	/// </summary>
	public bool IsDeactivated { get; set; }
	/// <summary>
	/// User creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// User updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
}