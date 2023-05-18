using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacher : IccUser {
	public enum TeacherRole {
		Director,
		Commitee,
		Guide
	}

	[Display(Name = "Invitado(a)"), Required]
	public bool IsGuest { get; set; }

	[Display(Name = "Oficina"), Required]
	public string Office { get; set; } = string.Empty;

	[Display(Name = "Horario"), Required]
	public string Schedule { get; set; } = string.Empty;

	[Display(Name = "Especialización"), Required]
	public string Specialization { get; set; } = string.Empty;

	[InverseProperty("GuideTeacher")]
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccStudentMemoir>();
}