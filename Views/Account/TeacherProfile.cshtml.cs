using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Account;

public class TeacherProfile {
	[DataType(DataType.EmailAddress), Display(Name = "E-mail"), EmailAddress]
	public string? Email { get; set; }

	[Display(Name = "Nombre")]
	public string FirstName { get; set; } = string.Empty;

	[Display(Name = "Apellido")]
	public string LastName { get; set; } = string.Empty;

	[Display(Name = "RUT")]
	public string Rut { get; set; } = string.Empty;

	public Password Password { get; set; } = new();

	[Display(Name = "Oficina")]
	public string Office { get; set; } = string.Empty;

	[Display(Name = "Horario")]
	public string Schedule { get; set; } = string.Empty;

	[Display(Name = "Especialización")]
	public string Specialization { get; set; } = string.Empty;

	[Display(Name = "Profesor guía")]
	public bool IsGuide { get; set; }

	[Display(Name = "Profesor de comité")]
	public bool IsCommittee { get; set; }

	[Display(Name = "Director de carrera")]
	public bool IsDirector { get; set; }
}