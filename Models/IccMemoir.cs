namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a memoir.
/// </summary>
public class IccMemoir {
	#region Properties
	/// <summary>
	/// Memoir phase.
	/// </summary>
	public enum MemoirPhase {
		Draft,
		Visible,
		Accepted,
		Rejected
	}
	/// <summary>
	/// Memoir (ID).
	/// </summary>
	public string? Id { get; set; }
	/// <summary>
	/// Memoir title.
	/// </summary>
	public string Title { get; set; } = string.Empty;
	/// <summary>
	/// Memoir description.
	/// </summary>
	public string Description { get; set; } = string.Empty;
	/// <inheritdoc cref="MemoirPhase"></inheritdoc>
	public MemoirPhase Phase { get; set; }
	/// <summary>
	/// Memoir creation timestamp.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }
	/// <summary>
	/// Memoir updated timestamp.
	/// </summary>
	public DateTimeOffset UpdatedAt { get; set; }
	#endregion
	#region Student
	/// <summary>
	/// <see cref="IccStudent">Student</see> of the <see cref="IccTeacher">teacher</see>'s <see cref="IccMemoir">memoir</see>. It may be selected by the teacher previously or with <see cref="IccTeacherMemoir.Candidates">a list of candidates</see>.
	/// </summary>
	public virtual IccStudent? Student { get; set; }
	#endregion
	#region Teacher
	/// <summary>
	/// <see cref="IccTeacher">Guide teacher</see> of the memoir.
	/// </summary>
	public virtual IccTeacher? GuideTeacher { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Assistant teachers</see> of the memoir.
	/// </summary>
	public virtual ICollection<IccTeacher> AssistantTeachers { get; set; } = new HashSet<IccTeacher>();
	#endregion
	#region Commitee
	/// <summary>
	/// <see cref="IccCommiteeRejection">Rejections</see> made by the <see cref="IccTeacher">commitee</see>.
	/// </summary>
	public virtual ICollection<IccCommiteeRejection> CommiteeRejections { get; set; } = new HashSet<IccCommiteeRejection>();
	#endregion
}