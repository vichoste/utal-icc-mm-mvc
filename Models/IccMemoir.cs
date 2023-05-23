using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccMemoir {
	public enum MemoirPhase {
		Draft,
		SentToGuide,
		SentToCommittee,

	}

	[Display(Name = "ID")]
	public string Id { get; set; } = string.Empty;

	[Display(Name = "Título")]
	public string Title { get; set; } = string.Empty;

	[Display(Name = "Descripción")]
	public string Description { get; set; } = string.Empty;

	[Display(Name = "Fase")]
	public MemoirPhase Phase { get; set; }

	[Display(Name = "Creada")]
	public DateTimeOffset CreatedAt { get; set; }

	[Display(Name = "Actualizada")]
	public DateTimeOffset UpdatedAt { get; set; }

	[Display(Name = "Estudiante")]
	public virtual IccStudent? Student { get; set; }

	[Display(Name = "Profesor guía")]
	public virtual IccTeacher? GuideTeacher { get; set; }
}