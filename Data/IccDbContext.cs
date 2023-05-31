using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Data;

public class IccDbContext : IdentityDbContext<IccUser> {
	public DbSet<IccStudent> IccStudents { get; set; }

	public DbSet<IccTeacher> IccTeachers { get; set; }

	public DbSet<IccMemoir> IccMemoirs { get; set; }

	public IccDbContext(DbContextOptions<IccDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);
		_ = modelBuilder.Entity<IccStudent>().HasBaseType<IccUser>();
		_ = modelBuilder.Entity<IccTeacher>().HasBaseType<IccUser>();
	}
}