using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models.Dtos;

public class PasswordDto {
	[Required(ErrorMessage = "Debe ingresar su contraseña actual."), DataType(DataType.Password), StringLength(64, ErrorMessage = "La contraseña debe tener un mínimo de 6 caracteres.", MinimumLength = 6), Display(Name = "Contraseña actual")]
	public string CurrentPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Debe ingresar su nueva contraseña."), DataType(DataType.Password), StringLength(64, ErrorMessage = "La contraseña debe tener un mínimo de 6 caracteres.", MinimumLength = 6), Display(Name = "Nueva contraseña")]
	public string NewPassword { get; set; } = string.Empty;

	[DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden."), Display(Name = "Confirmar nueva contraseña")]
	public string ConfirmNewPassword { get; set; } = string.Empty;
}