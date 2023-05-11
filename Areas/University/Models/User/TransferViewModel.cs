using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Areas.University.Models.User;

/// <summary>
/// Director teacher transfer view model.
/// </summary>
public class TransferViewModel {
	/// <summary>
	/// Current director teacher's ID.
	/// </summary>
	[Display(Name = "ID del director de carrera actual")]
	public string? CurrentDirectorTeacherId { get; set; }
	/// <summary>
	/// New director teacher's ID.
	/// </summary>
	[Display(Name = "ID del nuevo director de carrera")]
	public string? NewDirectorTeacherId { get; set; }
	/// <summary>
	/// New director teacher's name.
	/// </summary>
	[Display(Name = "Nombre del nuevo director de carrera")]
	public string? NewDirectorTeacherName { get; set; }
}