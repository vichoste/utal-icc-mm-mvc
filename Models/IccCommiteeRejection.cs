namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by the Commitee to a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccCommiteeRejection : IccRejection {
	/// <summary>
	/// <see cref="IccTeacher">Teachers</see> from the commitee who didn't reject the <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public virtual ICollection<IccTeacher> WhoDidntReject { get; set; } = new HashSet<IccTeacher>();
	/// <summary>
	/// <see cref="IccTeacher">Teachers</see> from the commitee who rejected the <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public virtual ICollection<IccTeacher> WhoRejected { get; set; } = new HashSet<IccTeacher>();
}