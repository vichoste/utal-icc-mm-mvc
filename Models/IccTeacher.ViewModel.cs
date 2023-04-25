using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a teacher from the Computer Engineering carrer at University of Talca.
/// </summary>
public class IccTeacherViewModel : IccUserViewModel {
	/// <summary>
	/// Indicates if this teacher is not necessarily a teacher from the Computer Engineering carrer.
	/// </summary>
	[Display(Name = "Invitado(a)")]
	public bool IsGuest { get; set; }
	/// <summary>
	/// Teacher's office location.
	/// </summary>
	[Display(Name = "Oficina")]
	public string Office { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's schedule.
	/// </summary>
	[Display(Name = "Horario")]
	public string Schedule { get; set; } = string.Empty;
	/// <summary>
	/// Teacher's specialization.
	/// </summary>
	[Display(Name = "Especialización")]
	public string Specialization { get; set; } = string.Empty;
	/// <summary>
	/// Check if this teacher is a guide teacher.
	/// </summary>
	public bool IsGuide { get; set; }
	/// <summary>
	/// Check if this teacher is an assistant teacher.
	/// </summary>
	public bool IsAssistant { get; set; }
	/// <summary>
	/// Check if this teacher is a member of the memoir commitee.
	/// </summary>
	public bool IsCommittee { get; set; }
	/// <summary>
	/// Check if this teacher is the director of the Computer Engineering carrer.
	/// </summary>
	public bool IsDirector { get; set; }
	/// <summary>
	/// <see cref="IccStudentMemoir">Student memoirs</see> which they guide.
	/// </summary>
	public virtual ICollection<IccStudentMemoirViewModel> MemoirsWhichIGuide { get; set; } = new HashSet<IccStudentMemoirViewModel>();
	/// <summary>
	/// <see cref="IccStudentMemoir">Student memoirs</see> which they assist.
	/// </summary>
	public virtual ICollection<IccStudentMemoirViewModel> MemoirsWhichIAssist { get; set; } = new HashSet<IccStudentMemoirViewModel>();
	/// <summary>
	/// Teacher rejections for memoir proposals.
	/// </summary>
	[InverseProperty("Teacher")]
	public virtual ICollection<IccTeacherRejectionViewModel> MemoirsWhichIRejected { get; set; } = new HashSet<IccTeacherRejectionViewModel>();
}