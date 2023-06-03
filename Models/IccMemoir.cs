using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccMemoir {
	public enum Phases {
		Proposal,
		Request,
		InProgress,
		Complete,
		Paused,
		Abandoned
	}

	public Phases Phase { get; set; } = Phases.Proposal;

	public string Id { get; set; } = string.Empty;

	public string Title { get; set; } = string.Empty;

	[DataType(DataType.MultilineText)]
	public string Description { get; set; } = string.Empty;

	public virtual IccStudent? Student { get; set; }

	public virtual IccTeacher? Guide { get; set; }
}