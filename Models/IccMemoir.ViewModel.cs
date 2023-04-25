﻿using System.ComponentModel.DataAnnotations;
using static Utal.Icc.Mm.Mvc.Models.IccMemoir;

namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Represents a memoir.
/// </summary>
public class IccMemoirViewModel : IccViewModel {
	/// <summary>
	/// Memoir title.
	/// </summary>
	[Display(Name = "Título")]
	public string Title { get; set; } = string.Empty;
	/// <summary>
	/// Memoir description.
	/// </summary>
	[Display(Name = "Descripción")]
	public string Description { get; set; } = string.Empty;
	/// <inheritdoc cref="MemoirPhase"></inheritdoc>
	public MemoirPhase Phase { get; set; }
	/// <summary>
	/// <see cref="IccStudent">Student</see> of the <see cref="IccTeacher">teacher</see>'s <see cref="IccMemoir">memoir</see>. It may be selected by the teacher previously or with <see cref="IccTeacherMemoir.Candidates">a list of candidates</see>.
	/// </summary>
	public IccStudentViewModel? Student { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Guide teacher</see> of the memoir.
	/// </summary>
	public IccTeacherViewModel? GuideTeacher { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Assistant teachers</see> of the memoir.
	/// </summary>
	public ICollection<IccTeacherViewModel> AssistantTeachers { get; set; } = new HashSet<IccTeacherViewModel>();
	/// <summary>
	/// <see cref="IccCommiteeRejection">Rejections</see> made by the <see cref="IccTeacher">commitee</see>.
	/// </summary>
	public ICollection<IccCommiteeRejection> CommiteeRejections { get; set; } = new HashSet<IccCommiteeRejection>();
}