using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacher : IccUser {
	public enum IccTeacherRoles {
		IccDirector,
		IccCommittee,
		IccGuide
	}

	public bool IsGuest { get; set; }

	[DataType(DataType.MultilineText)]
	public string Office { get; set; } = string.Empty;

	[DataType(DataType.MultilineText)]
	public string Schedule { get; set; } = string.Empty;

	[DataType(DataType.MultilineText)]
	public string Specialization { get; set; } = string.Empty;

	[InverseProperty("Guide")]
	public virtual ICollection<IccMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccMemoir>();
}