using System.ComponentModel.DataAnnotations;

namespace Utal.Icc.Mm.Mvc.Views.Account;

public class BatchCreateStudents {
	[Display(Name = "Archivo CSV")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IFormFile CsvFile { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}