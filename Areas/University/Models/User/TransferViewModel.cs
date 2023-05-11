using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Areas.University.Models.User;

/// <summary>
/// Director teacher transfer view model.
/// </summary>
public class TransferViewModel {
	/// <summary>
	/// Current director teacher's ID.
	/// </summary>
	[Required(ErrorMessage = "ID del(la) director(a) de carrera actual requerido."), Display(Name = "ID del(la) director(a) de carrera actual")]
	public string CurrentDirectorTeacherId { get; set; } = string.Empty;
	/// <summary>
	/// New director teacher's ID.
	/// </summary>
	[Required(ErrorMessage = "ID del(la) nuevo(a) director(a) de carrera requerido."), Display(Name = "ID del(la) nuevo(a) director(a) de carrera")]
	public string NewDirectorTeacherId { get; set; } = string.Empty;
	/// <summary>
	/// New director teacher's name.
	/// </summary>
	[Required(ErrorMessage = "Nombre del(la) nuevo(a) director(a) de carrera requerido."), Display(Name = "Nombre del(la) nuevo(a) director(a) de carrera.")]
	public string NewDirectorTeacherName { get; set; } = string.Empty;
}