using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Areas.University.ViewModels.User;

public class CsvFileViewModel {
	[Display(Name = "Archivo CSV")]
	public IFormFile? CsvFile { get; set; }
}