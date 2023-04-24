using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a student from the Computer Engineering carrer at University of Talca.
/// </summary>
public class IccStudentViewModel : IccUserViewModel {
	/// <summary>
	/// Student's university ID (known as "Número de matrícula").
	/// </summary>
	/// </summary>
	[Display(Name = "Número de matrícula")]
	public string UniversityId { get; set; } = string.Empty;
	/// <summary>
	/// Student's remaining courses.
	/// </summary>
	/// </summary>
	[Display(Name = "Cursos restantes")]
	public string RemainingCourses { get; set; } = string.Empty;
	/// <summary>
	/// Checks if the student is doing a professional practice.
	/// Values are:
	/// 0: Student is not doing the professional practice.
	/// 1: Student is doing the first professional practice.
	/// 2: Student is doing the second professional practice.
	/// </summary>
	/// </summary>
	[Display(Name = "En práctica")]
	public int CurrentPractice { get; set; }
	/// <summary>
	/// Checks if the student is working.
	/// </summary>
	/// </summary>
	[Display(Name = "Trabajando")]
	public bool IsWorking { get; set; }
	/// <summary>
	/// <see cref="IccMemoir">Student memoirs</see> which they own.
	/// </summary>
	public ICollection<IccMemoirViewModel> MemoirsWhichIOwn { get; set; } = new HashSet<IccMemoirViewModel>();
	/// <summary>
	/// <see cref="IccTeacherMemoir">Teacher memoirs</see> which they are candidates of.
	/// </summary>
	public ICollection<IccTeacherMemoirViewModel> MemoirsWhichImCandidate { get; set; } = new HashSet<IccTeacherMemoirViewModel>();
}