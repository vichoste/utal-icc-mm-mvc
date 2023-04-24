namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a teacher from the Computer Engineering carrer at University of Talca.
/// </summary>
public class Teacher : IccUser {
	/// <summary>
	/// Roles a teacher can have.
	/// </summary>
	public enum TeacherRole {
		/// <summary>
		/// Director of the Computer Engineering carrer.
		/// </summary>
		CareerDirector,
		/// <summary>
		/// Member of the Computer Engineering's memoir commitee.
		/// </summary>
		CommiteeMember,
		/// <summary>
		/// Mentor of the Computer Engineering's memoir courses.
		/// </summary>
		CourseMentor,
		/// <summary>
		/// Mentor of a memorist ("Profesor guía").
		/// </summary>
		GuideMentor,
		/// <summary>
		/// Assistant mentor of a memorist ("Profesor co-guía").
		/// </summary>
		AssistantMentor
	}
	/// <summary>
	/// Teacher's office location.
	/// </summary>
	public string Office { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's schedule.
	/// </summary>
	public string Schedule { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's specialization.
	/// </summary>
	public string Specialization { get; set; } = string.Empty;
}