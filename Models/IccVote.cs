namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a vote from a <see cref="IccTeacher">teacher</see> against a <see cref="IccMemoir"/>
/// </summary>
public class IccVote {
	/// <summary>
	/// Vote type.
	/// </summary>
	public enum VoteType {
		/// <summary>
		/// Vote against the memoir.
		/// </summary>
		Against,
		/// <summary>
		/// Vote in favor of the memoir.
		/// </summary>
		For
	}
	/// <summary>
	/// Vote ID.
	/// </summary>
	public string? Id { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Teacher from the commitee</see> who did this vote.
	/// </summary>
	public IccTeacher? Issuer { get; set; }
	/// <summary>
	/// <see cref="IccMemoir">Memoir</see> that was voted.
	/// </summary>
	public IccMemoir? Memoir { get; set; }
	/// <inheritdoc cref="VoteType"></inheritdoc>
	public VoteType Type { get; set; }
}