namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a student from the Computer Engineering carrer at University of Talca.
/// </summary>
public class IccStudent : IccUser {
	#region Properties
	/// <summary>
	/// Roles a student can have.
	/// </summary>
	public enum StudentRole {
		/// <summary>
		/// If the student has completed all the memoir courses.
		/// </summary>
		Complete,
		/// <summary>
		/// <see cref="IccMemoir">Memorist</see> student.
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
	#endregion
	#region Memoirs
	/// <summary>
	/// <see cref="IccStudentMemoir">Student memoirs</see> which they own.
	/// </summary>
	public virtual ICollection<IccStudentMemoir> MemoirsWhichIOwn { get; set; } = new HashSet<IccStudentMemoir>();
	/// <summary>
	/// <see cref="IccTeacherMemoir">Teacher memoirs</see> which they are candidates of.
	/// </summary>
	public virtual ICollection<IccTeacherMemoir> MemoirsWhichImCandidate { get; set; } = new HashSet<IccTeacherMemoir>();
	/// <summary>
	/// <see cref="IccTeacherMemoir">Teacher memoirs</see> which they are part of.
	/// </summary>
	public virtual ICollection<IccStudentMemoir> MemoirsWhichImPartOf { get; set; } = new HashSet<IccStudentMemoir>();
	#endregion
}