using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccStudent : IccUser {
	public enum IccStudentRoles {
		[Display(Name = "Alumno memorista")]
		IccMemorist,
		[Display(Name = "Alumno regular")]
		IccRegular
	}

	[Display(Name = "Número de matrícula")]
	public string UniversityId { get; set; } = string.Empty;

	[Display(Name = "Cursos restantes")]
	public string RemainingCourses { get; set; } = string.Empty;

	[Display(Name = "¿Está realizando práctica?")]
	public bool IsDoingThePractice { get; set; }

	[Display(Name = "¿Está trabajando?")]
	public bool IsWorking { get; set; }

	[Display(Name = "Mis memorias"), InverseProperty("Student")]
	public virtual ICollection<IccMemoir> MemoirsWhichIOwn { get; set; } = new HashSet<IccMemoir>();

	[Display(Name = "Mis postulaciones"), InverseProperty("Candidates")]
	public virtual ICollection<IccMemoir> MemoirsWhichImCandidate { get; set; } = new HashSet<IccMemoir>();
}