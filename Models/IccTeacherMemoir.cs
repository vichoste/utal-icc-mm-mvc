using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models;

public class IccTeacherMemoir : IccMemoir {
	[DataType(DataType.MultilineText)]
	public string Requirements { get; set; } = string.Empty;

	public virtual ICollection<IccStudent> Candidates { get; set; } = new HashSet<IccStudent>();
}