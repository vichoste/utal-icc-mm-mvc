using Microsoft.AspNetCore.Identity;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Seeders;

/// <summary>
/// Creates the roles.
/// </summary>
public static class RoleSeeder {
	/// <summary>
	/// Seed the database with the roles.
	/// </summary>
	/// <param name="services">Inject the application's services.</param>
	/// <exception cref="Exception"></exception>
	public static async Task SeedAsync(IServiceProvider services) {
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
		var teacherRoles = Enum.GetNames(typeof(IccTeacher.TeacherRole));
		var studentRoles = Enum.GetNames(typeof(IccStudent.StudentRole));
		foreach (var roleName in teacherRoles.Concat(studentRoles)) {
			var roleExists = await roleManager.RoleExistsAsync(roleName);
			if (!roleExists) {
				var role = new IdentityRole {
					Name = roleName
				};
				_ = await roleManager.CreateAsync(role);
			}
		}
	}
}