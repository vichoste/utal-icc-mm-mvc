namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// A <see cref="IccMemoir">memoir</see> created by a <see cref="IccStudent">student</see>.
/// </summary>
public class IccStudentMemoir : IccMemoir {
	/// <summary>
	/// <see cref="IccTeacherRejection">Rejections</see> made by the <see cref="IccTeacher">guide teachers</see>.
	/// </summary>
	public virtual ICollection<IccTeacherRejection> TeacherRejections { get; set; } = new HashSet<IccTeacherRejection>();
}