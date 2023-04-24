namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by the Commitee to a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccCommiteeRejection : IccRejection {
	/// <summary>
	/// <see cref="IccVote">Votes</see> to consider in this rejection.
	/// </summary>
	public virtual ICollection<IccVote> IccVotes { get; set; } = new HashSet<IccVote>();
}