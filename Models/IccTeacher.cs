using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacher : IccUser {
	public enum IccTeacherRole {
		[Display(Name = "Director(a) de carrera")]
		IccDirector,
		[Display(Name = "Integrante del comité")]
		IccCommittee,
		[Display(Name = "Profesor guía")]
		IccGuide
	}

	[Display(Name = "Invitado(a)")]
	public bool IsGuest { get; set; }

	[Display(Name = "Oficina")]
	public string Office { get; set; } = string.Empty;

	[Display(Name = "Horario")]
	public string Schedule { get; set; } = string.Empty;

	[Display(Name = "Especialización")]
	public string Specialization { get; set; } = string.Empty;

	[Display(Name = "Mis memorias"), InverseProperty("GuideTeacher")]
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccStudentMemoir>();
}