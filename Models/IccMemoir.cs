using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccMemoir {
	public enum MemoirPhase {
		Draft,
		SentToGuide,
		SentToCommittee,

	}

	[Display(Name = "ID"), Required]
	public string Id { get; set; } = string.Empty;

	[Display(Name = "Título"), Required]
	public string Title { get; set; } = string.Empty;

	[Display(Name = "Descripción"), Required]
	public string Description { get; set; } = string.Empty;

	[Display(Name = "Fase"), Required]
	public MemoirPhase Phase { get; set; }

	[Display(Name = "Creada"), Required]
	public DateTimeOffset CreatedAt { get; set; }

	[Display(Name = "Actualizada"), Required]
	public DateTimeOffset UpdatedAt { get; set; }

	[Display(Name = "Estudiante")]
	public virtual IccStudent? Student { get; set; }

	[Display(Name = "Profesor guía")]
	public virtual IccTeacher? GuideTeacher { get; set; }
}