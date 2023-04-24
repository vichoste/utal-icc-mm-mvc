namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// A <see cref="IccMemoir">memoir</see> created by a <see cref="IccStudent">student</see>.
/// </summary>
public class IccStudentMemoirViewModel : IccMemoirViewModel {
	/// <summary>
	/// <see cref="IccTeacherRejection">Rejections</see> made by the <see cref="IccTeacher">guide teachers</see>.
	/// </summary>
	public virtual ICollection<IccTeacherRejectionViewModel> TeacherRejections { get; set; } = new HashSet<IccTeacherRejectionViewModel>();
}