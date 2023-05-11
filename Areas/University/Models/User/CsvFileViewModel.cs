using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Areas.University.Models.User;

/// <summary>
/// CSV file view model.
/// </summary>
public class CsvFileViewModel {
	/// <summary>
	/// CSV file.
	/// </summary>
	[Required(ErrorMessage = "El archivo CSV de estudiantes es requerido."), Display(Name = "Archivo CSV")]
	public IFormFile CsvFile { get; set; } = null!;
}