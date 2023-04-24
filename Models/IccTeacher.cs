namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a teacher from the Computer Engineering carrer at University of Talca.
/// </summary>
public class IccTeacher : IccUser {
	#region Properties
	/// <summary>
	/// Roles a teacher can have.
	/// </summary>
	public enum TeacherRole {
		/// <summary>
		/// Director of the Computer Engineering carrer.
		/// </summary>
		CareerDirector,
		/// <summary>
		/// Member of the Computer Engineering's memoir commitee.
		/// </summary>
		CommiteeMember,
		/// <summary>
		/// Mentor of the Computer Engineering's memoir courses.
		/// </summary>
		CourseMentor,
		/// <summary>
		/// Mentor of a memorist ("Profesor guía").
		/// </summary>
		GuideMentor,
		/// <summary>
		/// Assistant mentor of a memorist ("Profesor co-guía").
		/// </summary>
		AssistantMentor,
		/// <summary>
		/// Guest teacher. They may be not part of the Computer Engineering carrer at University of Talca.
		/// </summary>
		Guest
	}
	/// <summary>
	/// Teacher's office location.
	/// </summary>
	public string Office { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's schedule.
	/// </summary>
	public string Schedule { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's specialization.
	/// </summary>
	public string Specialization { get; set; } = string.Empty;
	#endregion
	#region Memoirs
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccStudentMemoir>();
	#endregion
	#region Rejections
	/// <summary>
	/// Teacher rejections for memoir proposals.
	/// </summary>
	public virtual ICollection<IccTeacherRejection> TeacherRejections { get; set; } = new HashSet<IccTeacherRejection>();
	/// <summary>
	/// <see cref="CommiteeRejection">Rejectons as the commitee</see> that this teacher didn't support.
	/// </summary>
	public virtual ICollection<CommiteeRejection> CommiteeRejectionsWhichIDidNotSupport { get; set; } = new HashSet<CommiteeRejection>();
	/// <summary>
	/// <see cref="CommiteeRejection">Rejectons as the commitee</see> that this teacher supported.
	/// </summary>
	public virtual ICollection<CommiteeRejection> CommiteeRejectionsWhichIDidSupport { get; set; } = new HashSet<CommiteeRejection>();
	#endregion
}