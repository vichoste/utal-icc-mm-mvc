using CsvHelper.Configuration.Attributes;

namespace Utal.Icc.Mm.Mvc.Areas.University.Helpers;

/// <summary>
/// Helper for handling CSV files.
/// </summary>
public class CsvFileHelper {
	/// <summary>
	/// Student's first name.
	/// </summary>
	[Index(0)]
	public string? FirstName { get; set; }
	/// <summary>
	/// Student's last name.
	/// </summary>
	[Index(1)]
	public string? LastName { get; set; }
	/// <summary>
	/// Student's university ID.
	/// </summary>
	[Index(2)]
	public string? UniversityId { get; set; }
	/// <summary>
	/// Student's RUT.
	/// </summary>
	[Index(3)]
	public string? Rut { get; set; }
	/// <summary>
	/// Student's email.
	/// </summary>
	[Index(4)]
	public string? Email { get; set; }
	/// <summary>
	/// Student's password.
	/// </summary>
	[Index(5)]
	public string? Password { get; set; }
}