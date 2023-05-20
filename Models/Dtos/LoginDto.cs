using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Models.Dtos;

public class LoginDto {
	[Required(ErrorMessage = "El correo electrónico es requerido."), EmailAddress(ErrorMessage = "El correo electrónico no es válido."), Display(Name = "E-mail")]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "La contraseña es requerida."), DataType(DataType.Password), StringLength(64, ErrorMessage = "La contraseña debe tener un mínimo de 6 caracteres.", MinimumLength = 6), Display(Name = "Contraseña")]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "Recuérdame")]
	public bool RememberMe { get; set; }
}