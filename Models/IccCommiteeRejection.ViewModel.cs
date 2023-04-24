namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by the Commitee to a <see cref="IccMemoir">memoir</see>.
/// </summary>
public class IccCommiteeRejectionViewModel : IccRejectionViewModel {
	/// <summary>
	/// <see cref="IccVote">Votes</see> to consider in this rejection.
	/// </summary>
	public virtual ICollection<IccVoteViewModel> IccVotes { get; set; } = new HashSet<IccVoteViewModel>();
}