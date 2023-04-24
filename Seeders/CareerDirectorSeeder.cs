using Microsoft.AspNetCore.Identity;

using Utal.Icc.Mm.Mvc.Models;

namespace Utal.Icc.Mm.Mvc.Seeders;

/// <summary>
/// Creates a default career director..
/// </summary>
public static class CareerDirectorSeeder {
	/// <summary>
	/// Seeds the database with a default career director..
	/// </summary>
	/// <param name="services">Inject the application's services.</param>
	/// <param name="configuration">Inject the application's configuration.</param>
	/// <param name="env">Inject the application's environment.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, IWebHostEnvironment env) {
		var userManager = services.GetRequiredService<UserManager<IccUser>>();
		string email, password, firstName, lastName, rut;
		if (env.IsDevelopment()) {
			email = configuration["CareerDirectorEmail"] ?? throw new InvalidOperationException("Career director's email is not set");
			password = configuration["CareerDirectorPassword"] ?? throw new InvalidOperationException("Career director's password is not set");
			firstName = configuration["CareerDirectorFirstName"] ?? throw new InvalidOperationException("Career director's first name is not set");
			lastName = configuration["CareerDirectorLastName"] ?? throw new InvalidOperationException("Career director's last name is not set");
			rut = configuration["CareerDirectorRut"] ?? throw new InvalidOperationException("Career director's RUT is not set");
		} else {
			email = Environment.GetEnvironmentVariable("CAREER_DIRECTOR_EMAIL") ?? throw new InvalidOperationException("Career director's email is not set");
			password = Environment.GetEnvironmentVariable("CAREER_DIRECTOR_PASSWORD") ?? throw new InvalidOperationException("Career director's password is not set");
			firstName = Environment.GetEnvironmentVariable("CAREER_DIRECTOR_FIRST_NAME") ?? throw new InvalidOperationException("Career director's first name is not set");
			lastName = Environment.GetEnvironmentVariable("CAREER_DIRECTOR_LAST_NAME") ?? throw new InvalidOperationException("Career director's last name is not set");
			rut = Environment.GetEnvironmentVariable("CAREER_DIRECTOR_RUT") ?? throw new InvalidOperationException("Career director's RUT is not set");
		}
		var careerDirector = await userManager.FindByEmailAsync(email);
		if (careerDirector == null) {
			careerDirector = new IccUser {
				UserName = email,
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				Rut = rut,
				CreatedAt = DateTimeOffset.UtcNow,
				UpdatedAt = DateTimeOffset.UtcNow
			};
			var result = await userManager.CreateAsync(careerDirector, password);
			if (!result.Succeeded) {
				var errors = result.Errors.Select(e => e.Description);
				var errorsString = string.Join(", ", errors);
				throw new Exception($"Error while creating the Career Director \"{email}\": {errorsString}");
			}
		}
	}
}