using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccStudent : IccUser {
	public enum IccStudentRoles {
		IccMemorist,
		IccRegular
	}

	public string UniversityId { get; set; } = string.Empty;

	[DataType(DataType.MultilineText)]
	public string RemainingCourses { get; set; } = string.Empty;

	public bool IsDoingThePractice { get; set; }

	public bool IsWorking { get; set; }

	[InverseProperty("Student")]
	public virtual ICollection<IccMemoir> MemoirsWhichIOwn { get; set; } = new HashSet<IccMemoir>();

	[InverseProperty("Candidates")]
	public virtual ICollection<IccTeacherMemoir> MemoirsWhichImCandidate { get; set; } = new HashSet<IccTeacherMemoir>();
}