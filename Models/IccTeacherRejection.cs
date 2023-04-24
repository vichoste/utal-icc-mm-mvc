namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by a <see cref="IccTeacher">teacher</see>.
/// </summary>
public class IccTeacherRejection : IccRejection {
	/// <summary>
	/// <see cref="IccTeacher">Teacher</see> who made the <see cref="IccRejection">rejection</see>.
	/// </summary>
	public virtual IccTeacher? Teacher { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Teacher</see> who made the <see cref="IccRejection">rejection</see> (ID).
	/// </summary>
	public virtual string? TeacherId { get; set; }
}