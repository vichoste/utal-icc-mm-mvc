using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccMemoir {
	public enum Phases {
		[Display(Name = "Propuesta")]
		Proposal,
		[Display(Name = "Solicitud")]
		Request,
		[Display(Name = "En curso")]
		InProgress,
		[Display(Name = "Completa")]
		Complete,
		[Display(Name = "Pausada")]
		Paused,
		[Display(Name = "Abandonada")]
		Abandoned
	}

	[Display(Name = "Fase")]
	public Phases Phase { get; set; } = Phases.Proposal;

	[Display(Name = "ID")]
	public string Id { get; set; } = string.Empty;

	[Display(Name = "Título")]
	public string Title { get; set; } = string.Empty;

	[Display(Name = "Descripción")]
	public string Description { get; set; } = string.Empty;

	[Display(Name = "Estudiante")]
	public virtual IccStudent? Student { get; set; }

	[Display(Name = "Profesor guía")]
	public virtual IccTeacher? GuideTeacher { get; set; }

	[Display(Name = "Requisitos")]
	public string Requierments { get; set; } = string.Empty;

	[Display(Name = "Candidatos")]
	public virtual ICollection<IccStudent> Candidates { get; set; } = new HashSet<IccStudent>();
}