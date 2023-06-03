using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacherMemoir : IccMemoir {
	[DataType(DataType.MultilineText), Display(Name = "Requisitos")]
	public string Requierments { get; set; } = string.Empty;

	[Display(Name = "Candidatos")]
	public virtual ICollection<IccStudent> Candidates { get; set; } = new HashSet<IccStudent>();
}