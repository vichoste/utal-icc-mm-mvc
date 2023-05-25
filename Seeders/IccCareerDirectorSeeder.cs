using Microsoft.AspNetCore.Identity;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Seeders;

public static class IccCareerDirectorSeeder {
	public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, IWebHostEnvironment env) {
		var userManager = services.GetRequiredService<UserManager<IccUser>>();
		var userStore = services.GetRequiredService<IUserStore<IccUser>>();
		var emailStore = (IUserEmailStore<IccUser>)userStore;
		string email, password, firstName, lastName, rut;
		if (env.IsDevelopment()) {
			email = configuration["IccCareerDirectorEmail"] ?? throw new InvalidOperationException("Career director's email is not set");
			password = configuration["IccCareerDirectorPassword"] ?? throw new InvalidOperationException("Career director's password is not set");
			firstName = configuration["IccCareerDirectorFirstName"] ?? throw new InvalidOperationException("Career director's first name is not set");
			lastName = configuration["IccCareerDirectorLastName"] ?? throw new InvalidOperationException("Career director's last name is not set");
			rut = configuration["IccCareerDirectorRut"] ?? throw new InvalidOperationException("Career director's RUT is not set");
		} else {
			email = Environment.GetEnvironmentVariable("ICC_CAREER_DIRECTOR_EMAIL") ?? throw new InvalidOperationException("Career director's email is not set");
			password = Environment.GetEnvironmentVariable("ICC_CAREER_DIRECTOR_PASSWORD") ?? throw new InvalidOperationException("Career director's password is not set");
			firstName = Environment.GetEnvironmentVariable("ICC_CAREER_DIRECTOR_FIRST_NAME") ?? throw new InvalidOperationException("Career director's first name is not set");
			lastName = Environment.GetEnvironmentVariable("ICC_CAREER_DIRECTOR_LAST_NAME") ?? throw new InvalidOperationException("Career director's last name is not set");
			rut = Environment.GetEnvironmentVariable("ICC_CAREER_DIRECTOR_RUT") ?? throw new InvalidOperationException("Career director's RUT is not set");
		}
		var iccCareerDirector = await userManager.FindByEmailAsync(email);
		if (iccCareerDirector == null) {
			iccCareerDirector = new IccTeacher {
				UserName = email,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				Rut = rut,
				CreatedAt = DateTimeOffset.UtcNow,
				UpdatedAt = DateTimeOffset.UtcNow
			};
			await userStore.SetUserNameAsync(iccCareerDirector, email, CancellationToken.None);
			await emailStore.SetEmailAsync(iccCareerDirector, email, CancellationToken.None);
			_ = await userManager.CreateAsync(iccCareerDirector, password);
			_ = await userManager.AddToRoleAsync(iccCareerDirector, "IccDirector");
		}
	}
}