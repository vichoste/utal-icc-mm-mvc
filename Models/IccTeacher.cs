using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacher : IccUser {
	public enum IccTeacherRoles {
		[Display(Name = "Director(a) de carrera")]
		IccDirector,
		[Display(Name = "Integrante del comité")]
		IccCommittee,
		[Display(Name = "Profesor guía")]
		IccGuide
	}

	[Display(Name = "Invitado(a)")]
	public bool IsGuest { get; set; }

	[DataType(DataType.MultilineText), Display(Name = "Oficina")]
	public string Office { get; set; } = string.Empty;

	[DataType(DataType.MultilineText), Display(Name = "Horario")]
	public string Schedule { get; set; } = string.Empty;

	[DataType(DataType.MultilineText), Display(Name = "Especialización")]
	public string Specialization { get; set; } = string.Empty;

	[Display(Name = "Mis memorias"), InverseProperty("Guide")]
	public virtual ICollection<IccMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccMemoir>();
}