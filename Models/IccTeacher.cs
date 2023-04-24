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
		CourseTeacher,
		/// <summary>
		/// They are able to be mentor of a group of <see cref="IccStudent">memorists</see> ("Profesor guía").
		/// </summary>
		GuideTeacher,
		/// <summary>
		/// They are able to be co-mentor of a group of <see cref="IccStudent">memorists</see> ("Profesor co-guía").
		/// </summary>
		AssistantTeacher
	}
	/// <summary>
	/// Indicates if this teacher is not necessarily a teacher from the Computer Engineering carrer.
	/// </summary>
	public bool IsGuest { get; set; }
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
	#region Student memoirs
	/// <summary>
	/// <see cref="IccStudentMemoir">Student memoirs</see> which they guide.
	/// </summary>
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIGuide { get; set; } = new HashSet<IccStudentMemoir>();
	/// <summary>
	/// <see cref="IccStudentMemoir">Student memoirs</see> which they assist.
	/// </summary>
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIAssist { get; set; } = new HashSet<IccStudentMemoir>();
	#endregion
	#region Rejections
	/// <summary>
	/// Teacher rejections for memoir proposals.
	/// </summary>
	public virtual ICollection<IccTeacherRejection> TeacherRejections { get; set; } = new HashSet<IccTeacherRejection>();
	/// <summary>
	/// <see cref="IccCommiteeRejection">Rejectons as the commitee</see> that this teacher didn't support.
	/// </summary>
	public virtual ICollection<IccCommiteeRejection> CommiteeRejectionsWhichIDidNotSupport { get; set; } = new HashSet<IccCommiteeRejection>();
	/// <summary>
	/// <see cref="IccCommiteeRejection">Rejectons as the commitee</see> that this teacher supported.
	/// </summary>
	public virtual ICollection<IccCommiteeRejection> CommiteeRejectionsWhichIDidSupport { get; set; } = new HashSet<IccCommiteeRejection>();
	#endregion
}