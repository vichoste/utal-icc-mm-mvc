﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Data;

/// <summary>
/// Database context that supports identity for <see cref="IccUser">users</see>.
/// </summary>
public class IccDbContext : IdentityDbContext<IccUser> {
	/// <summary>
	/// <see cref="IccStudent">Students</see> dataset.
	/// </summary>
	public DbSet<IccStudent> IccStudents { get; set; }
	/// <summary>
	/// <see cref="IccTeacher">Teachers</see> dataset.
	/// </summary>
	public DbSet<IccTeacher> IccTeachers { get; set; }
	/// <summary>
	/// <see cref="IccStudentMemoir">Student's memoirs</see> dataset.
	/// </summary>
	public DbSet<IccStudentMemoir> IccStudentMemoirs { get; set; }
	/// <summary>
	/// <see cref="IccTeacherMemoir">Teacher's memoirs</see> dataset.
	/// </summary>
	public DbSet<IccTeacherMemoir> IccTeacherMemoirs { get; set; }
	/// <summary>
	/// <see cref="IccCommiteeRejection">Rejections made by the commitee</see> to any <see cref="IccMemoir">memoir</see>.
	/// </summary>
	public DbSet<IccCommiteeRejection> IccCommiteeRejections { get; set; }
	/// <summary>
	/// <see cref="IccTeacherRejection">Rejections made by teachers</see> to <see cref="IccStudentMemoir">student memoirs</see>.
	/// </summary>
	public DbSet<IccTeacherRejection> IccTeacherRejections { get; set; }

	/// <summary>
	/// Creates the database context.
	/// </summary>
	/// <param name="options">Database options injection.</param>
	public IccDbContext(DbContextOptions<IccDbContext> options) : base(options) { }

	/// <summary>
	/// Overrides the database context's creation behavior.
	/// </summary>
	/// <param name="modelBuilder">Builder.</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		_ = modelBuilder.Entity<IccStudent>().HasBaseType<IccUser>();
		_ = modelBuilder.Entity<IccTeacher>().HasBaseType<IccUser>();
		_ = modelBuilder.Entity<IccStudentMemoir>().HasBaseType<IccMemoir>();
		_ = modelBuilder.Entity<IccTeacherMemoir>().HasBaseType<IccMemoir>();
		_ = modelBuilder.Entity<IccCommiteeRejection>().HasBaseType<IccRejection>();
		_ = modelBuilder.Entity<IccTeacherRejection>().HasBaseType<IccRejection>();
	}
}