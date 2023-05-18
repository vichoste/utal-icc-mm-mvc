using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacherMemoir : IccMemoir {
	[Display(Name = "Candidatos")]
	public virtual ICollection<IccStudent> Candidates { get; set; } = new HashSet<IccStudent>();
}