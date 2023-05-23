using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Account;

public class Password {
	[DataType(DataType.Password), Display(Name = "Contraseña actual")]
	public string CurrentPassword { get; set; } = string.Empty;

	[DataType(DataType.Password), Display(Name = "Nueva contraseña")]
	public string NewPassword { get; set; } = string.Empty;

	[DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden."), Display(Name = "Confirmar nueva contraseña")]
	public string ConfirmNewPassword { get; set; } = string.Empty;
}