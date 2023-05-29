using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Account;

public class StudentProfile {
	[DataType(DataType.EmailAddress), Display(Name = "E-mail"), EmailAddress]
	public string? Email { get; set; }

	[Display(Name = "Nombre")]
	public string FirstName { get; set; } = string.Empty;

	[Display(Name = "Apellido")]
	public string LastName { get; set; } = string.Empty;

	[Display(Name = "RUT")]
	public string Rut { get; set; } = string.Empty;

	[Display(Name = "Número de matrícula")]
	public string? UniversityId { get; set; }

	[Display(Name = "Cursos restantes")]
	public string? RemainingCourses { get; set; }

	[Display(Name = "¿Está realizando práctica?")]
	public bool IsDoingThePractice { get; set; }

	[Display(Name = "¿Está trabajando?")]
	public bool IsWorking { get; set; }
}