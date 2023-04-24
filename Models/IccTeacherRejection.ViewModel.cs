namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by a <see cref="IccTeacher">teacher</see>.
/// </summary>
public class IccTeacherRejectionViewModel : IccRejectionViewModel {
	/// <summary>
	/// <see cref="IccTeacher">Teacher</see> who made the <see cref="IccRejection">rejection</see>.
	/// </summary>
	public virtual IccTeacherViewModel? Teacher { get; set; }
}