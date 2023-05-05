namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// A <see cref="IccMemoir">memoir</see> created by a <see cref="IccStudent">teacher</see>.
/// </summary>
public class IccTeacherMemoirViewModel : IccMemoirViewModel {
	/// <summary>
	/// <see cref="IccStudent">Candidates</see> of the <see cref="IccTeacher">teacher</see>'s <see cref="IccMemoir">memoir</see>. One of them will become the <see cref="IccStudent">chosen one</see>.
	/// </summary>
	public virtual ICollection<IccStudentViewModel> Candidates { get; set; } = new HashSet<IccStudentViewModel>();
}