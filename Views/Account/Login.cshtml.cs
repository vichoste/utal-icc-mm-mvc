using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Account;

public class Login {
	[Display(Name = "E-mail")]
	public string Email { get; set; } = string.Empty;

	[DataType(DataType.Password), Display(Name = "Contraseña")]
	public string Password { get; set; } = string.Empty;

	[Display(Name = "Recuérdame")]
	public bool RememberMe { get; set; }
}