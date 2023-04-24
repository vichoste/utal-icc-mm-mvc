using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a vote from a <see cref="IccTeacher">teacher</see> against a <see cref="IccMemoir"/>
/// </summary>
public class IccVoteViewModel {
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
	[Display(Name = "ID")]
	public string? Id { get; set; }
	/// <inheritdoc cref="VoteType"></inheritdoc>
	[Display(Name = "Voto")]
	public VoteType Type { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Teacher from the commitee</see> who did this vote.
	/// </summary>
	[Display(Name = "Votante")]
	public IccTeacherViewModel? Issuer { get; set; }
	/// <summary>
	/// <see cref="IccMemoir">Memoir</see> that was voted.
	/// </summary>
	[Display(Name = "Memoria")]
	public IccMemoirViewModel? Memoir { get; set; }
}