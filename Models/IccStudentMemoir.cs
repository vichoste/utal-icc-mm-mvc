namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// A <see cref="IccMemoir">memoir</see> created by a <see cref="IccStudent">student</see>.
/// </summary>
public class IccStudentMemoir : IccMemoir {
	/// <summary>
	/// Guide mentor.
	/// </summary>
	public virtual IccTeacher? GuideMentor { get; set; }
	/// <summary>
	/// Assistant mentors.
	/// </summary>
	public virtual ICollection<IccTeacher> AssistantMentors { get; set; } = new HashSet<IccTeacher>();
	/// <summary>
	/// Rejections made by teachers.
	/// </summary>
	public virtual ICollection<IccTeacherRejection> TeacherRejections { get; set; } = new HashSet<IccTeacherRejection>();
}