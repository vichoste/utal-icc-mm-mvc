using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccStudent : IccUser {
	public enum StudentRole {
		Memorist,
		Regular
	}

	[Display(Name = "Número de matrícula")]
	public string UniversityId { get; set; } = string.Empty;

	[Display(Name = "Cursos restantes")]
	public string RemainingCourses { get; set; } = string.Empty;

	[Display(Name = "En práctica")]
	public int IsDoingThePractice { get; set; }

	[Display(Name = "Trabajando")]
	public bool IsWorking { get; set; }

	[Display(Name = "Mis memorias"), InverseProperty("Student")]
	public virtual ICollection<IccMemoir> MemoirsWhichIOwn { get; set; } = new HashSet<IccMemoir>();

	[Display(Name = "Mis postulaciones"), InverseProperty("Candidates")]
	public virtual ICollection<IccTeacherMemoir> MemoirsWhichImCandidate { get; set; } = new HashSet<IccTeacherMemoir>();
}