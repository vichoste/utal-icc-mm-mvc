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
	public DbSet<IccStudent> Students { get; set; }
	/// <summary>
	/// Teachers dataset.
	/// </summary>
	public DbSet<IccTeacher> Teachers { get; set; }
	/// <summary>
	/// Memoirs dataset.
	/// </summary>
	public DbSet<IccMemoir> Memoirs { get; set; }

	/// <summary>
	/// Creates the database context.
	/// </summary>
	/// <param name="options">Database options.</param>
	public IccDbContext(DbContextOptions<IccDbContext> options) : base(options) { }

	/// <summary>
	/// Overrides the database context's creation behavior.
	/// </summary>
	/// <param name="builder">Builder.</param>
	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);
		_ = builder.Entity<IccStudent>().HasBaseType<IccUser>();
		_ = builder.Entity<IccTeacher>().HasBaseType<IccUser>();
	}
}