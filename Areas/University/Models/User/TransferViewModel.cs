using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Areas.University.Models.User;

public class TransferViewModel {
	[Display(Name = "ID del director de carrera actual")]
	public string? CurrentDirectorTeacherId { get; set; }
	[Display(Name = "ID del nuevo director de carrera")]
	public string? NewDirectorTeacherId { get; set; }
	[Display(Name = "Nombre del nuevo director de carrera")]
	public string? NewDirectorTeacherName { get; set; }
}