using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Data;

public class IccDbContext : IdentityDbContext<IccUser> {
	public virtual DbSet<IccUser> IccUsers { get; set; }

	public virtual DbSet<IccStudent> IccStudents { get; set; }

	public virtual DbSet<IccTeacher> IccTeachers { get; set; }

	public virtual DbSet<IccMemoir> IccMemoirs { get; set; }

	public virtual DbSet<IccStudentMemoir> IccStudentMemoirs { get; set; }

	public virtual DbSet<IccTeacherMemoir> IccTeacherMemoirs { get; set; }

	public IccDbContext(DbContextOptions<IccDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		_ = modelBuilder.Entity<IccStudent>().HasBaseType<IccUser>();
		_ = modelBuilder.Entity<IccTeacher>().HasBaseType<IccUser>();
		_ = modelBuilder.Entity<IccStudentMemoir>().HasBaseType<IccMemoir>();
		_ = modelBuilder.Entity<IccTeacherMemoir>().HasBaseType<IccMemoir>();
	}
}