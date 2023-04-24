namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a student from the Computer Engineering carrer at University of Talca.
/// </summary>
public class Student : IccUser {
	/// <summary>
	/// Roles a student can have.
	/// </summary>
	public enum StudentRole {
		/// <summary>
		/// If the student has completed all the memoir courses.
		/// </summary>
		Complete,
		/// <summary>
		/// Memorist student.
		/// </summary>
		Memorist,
		/// <summary>
		/// Regular student.
		/// </summary>
		Regular
	}
	/// <summary>
	/// Student's university ID (known as "Número de matrícula").
	/// </summary>
	public string UniversityId { get; set; } = string.Empty;
	/// <summary>
	/// Student's remaining courses.
	/// </summary>
	public string RemainingCourses { get; set; } = string.Empty;
	/// <summary>
	/// Checks if the student is doing a professional practice.
	/// Values are:
	/// 0: Student is not doing the professional practice.
	/// 1: Student is doing the first professional practice.
	/// 2: Student is doing the second professional practice.
	/// </summary>
	public int IsDoingThePractice { get; set; }
	/// <summary>
	/// Checks if the student is working.
	/// </summary>
	public bool IsWorking { get; set; }
}