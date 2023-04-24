﻿namespace Utal.Icc.Mm.Mvc.Models;

/// <summary>
/// Rejection made by a <see cref="IccTeacher">teacher</see>.
/// </summary>
public class IccTeacherRejection : IccRejection {
	/// <summary>
	/// <see cref="IccTeacher">Teacher</see> who made the rejection.
	/// </summary>
	public virtual IccTeacher? Teacher { get; set; }
}