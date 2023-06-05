using Microsoft.AspNetCore.Identity;
using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Seeders;

public static class IccRoleSeeder {
	public static async Task SeedAsync(IServiceProvider services) {
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
		var teacherRoles = Enum.GetNames(typeof(IccTeacher.IccTeacherRoles));
		var studentRoles = Enum.GetNames(typeof(IccStudent.IccStudentRoles));
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