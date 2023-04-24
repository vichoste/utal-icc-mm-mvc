using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Data;

/// <summary>
/// Database context.
/// </summary>
public class IccDbContext : IdentityDbContext<IccUser> {
	/// <summary>
	/// Students dataset.
	/// </summary>
	public DbSet<Student> Students { get; set; }
	/// <summary>
	/// Teachers dataset.
	/// </summary>
	public DbSet<Teacher> Teachers { get; set; }

	/// <summary>
	/// Creates the database context.
	/// </summary>
	/// <param name="options">Database options.</param>
	public IccDbContext(DbContextOptions<IccDbContext> options) : base(options) {
	}

	/// <summary>
	/// Overrides the database context's creation behavior.
	/// </summary>
	/// <param name="builder">Builder.</param>
	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);
		_ = builder.Entity<Student>().HasBaseType<IccUser>();
		_ = builder.Entity<Teacher>().HasBaseType<IccUser>();
	}
}