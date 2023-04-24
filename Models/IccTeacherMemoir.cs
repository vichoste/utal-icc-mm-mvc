namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// A <see cref="IccMemoir">memoir</see> created by a <see cref="IccStudent">teacher</see>.
/// </summary>
public class IccTeacherMemoir : IccMemoir {
	/// <summary>
	/// <see cref="IccStudent">Student</see> of the <see cref="IccTeacher">teacher</see>'s <see cref="IccMemoir">memoir</see>. It may be selected by the teacher previously or with <see cref="Candidates">a list of candidates</see>.
	/// </summary>
	public virtual IccStudent? Student { get; set; }
	/// <summary>
	/// <see cref="IccStudent">Candidates</see> of the <see cref="IccTeacher">teacher</see>'s <see cref="IccMemoir">memoir</see>. One of them will become the <see cref="Student">chosen one</see>.
	/// </summary>
	public virtual ICollection<IccStudent> Candidates { get; set; } = new HashSet<IccStudent>();
}