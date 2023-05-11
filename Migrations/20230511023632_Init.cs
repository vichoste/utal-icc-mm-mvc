using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utal.Icc.Mm.Mvc.Migrations;
/// <inheritdoc />
public partial class Init : Migration {
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder) {
		_ = migrationBuilder.CreateTable(
			name: "AspNetRoles",
			columns: table => new {
				Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
			},
			constraints: table => _ = table.PrimaryKey("PK_AspNetRoles", x => x.Id));

		_ = migrationBuilder.CreateTable(
			name: "AspNetRoleClaims",
			columns: table => new {
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
				ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
					column: x => x.RoleId,
					principalTable: "AspNetRoles",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "AspNetUserClaims",
			columns: table => new {
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
				ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
			},
			constraints: table => _ = table.PrimaryKey("PK_AspNetUserClaims", x => x.Id));

		_ = migrationBuilder.CreateTable(
			name: "AspNetUserLogins",
			columns: table => new {
				LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
				ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
				ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
				UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table => _ = table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey }));

		_ = migrationBuilder.CreateTable(
			name: "AspNetUserRoles",
			columns: table => new {
				UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
				_ = table.ForeignKey(
					name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
					column: x => x.RoleId,
					principalTable: "AspNetRoles",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "AspNetUsers",
			columns: table => new {
				Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
				FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Rut = table.Column<string>(type: "nvarchar(max)", nullable: false),
				IsDeactivated = table.Column<bool>(type: "bit", nullable: false),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
				UniversityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
				RemainingCourses = table.Column<string>(type: "nvarchar(max)", nullable: true),
				IsDoingThePractice = table.Column<int>(type: "int", nullable: true),
				IsWorking = table.Column<bool>(type: "bit", nullable: true),
				IsGuest = table.Column<bool>(type: "bit", nullable: true),
				Office = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Schedule = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
				IccTeacherMemoirId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
				EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
				PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
				SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
				ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
				PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
				PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
				TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
				LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
				LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
				AccessFailedCount = table.Column<int>(type: "int", nullable: false)
			},
			constraints: table => _ = table.PrimaryKey("PK_AspNetUsers", x => x.Id));

		_ = migrationBuilder.CreateTable(
			name: "AspNetUserTokens",
			columns: table => new {
				UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
				_ = table.ForeignKey(
					name: "FK_AspNetUserTokens_AspNetUsers_UserId",
					column: x => x.UserId,
					principalTable: "AspNetUsers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "IccMemoir",
			columns: table => new {
				Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Phase = table.Column<int>(type: "int", nullable: false),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
				GuideTeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				IccTeacherMemoir_GuideTeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_IccMemoir", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_IccMemoir_AspNetUsers_GuideTeacherId",
					column: x => x.GuideTeacherId,
					principalTable: "AspNetUsers",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccMemoir_AspNetUsers_IccTeacherMemoir_GuideTeacherId",
					column: x => x.IccTeacherMemoir_GuideTeacherId,
					principalTable: "AspNetUsers",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccMemoir_AspNetUsers_StudentId",
					column: x => x.StudentId,
					principalTable: "AspNetUsers",
					principalColumn: "Id");
			});

		_ = migrationBuilder.CreateTable(
			name: "IccRejection",
			columns: table => new {
				Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
				MemoirId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				IccStudentMemoirId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				IccTeacherRejection_MemoirId = table.Column<string>(type: "nvarchar(450)", nullable: true)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_IccRejection", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_IccRejection_AspNetUsers_TeacherId",
					column: x => x.TeacherId,
					principalTable: "AspNetUsers",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccRejection_IccMemoir_IccStudentMemoirId",
					column: x => x.IccStudentMemoirId,
					principalTable: "IccMemoir",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccRejection_IccMemoir_IccTeacherRejection_MemoirId",
					column: x => x.IccTeacherRejection_MemoirId,
					principalTable: "IccMemoir",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccRejection_IccMemoir_MemoirId",
					column: x => x.MemoirId,
					principalTable: "IccMemoir",
					principalColumn: "Id");
			});

		_ = migrationBuilder.CreateTable(
			name: "IccStudentIccTeacherMemoir",
			columns: table => new {
				CandidatesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				MemoirsWhichImCandidateId = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_IccStudentIccTeacherMemoir", x => new { x.CandidatesId, x.MemoirsWhichImCandidateId });
				_ = table.ForeignKey(
					name: "FK_IccStudentIccTeacherMemoir_AspNetUsers_CandidatesId",
					column: x => x.CandidatesId,
					principalTable: "AspNetUsers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
				_ = table.ForeignKey(
					name: "FK_IccStudentIccTeacherMemoir_IccMemoir_MemoirsWhichImCandidateId",
					column: x => x.MemoirsWhichImCandidateId,
					principalTable: "IccMemoir",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "IccStudentMemoirIccTeacher",
			columns: table => new {
				AssistantTeachersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
				MemoirsWhichIAssistId = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_IccStudentMemoirIccTeacher", x => new { x.AssistantTeachersId, x.MemoirsWhichIAssistId });
				_ = table.ForeignKey(
					name: "FK_IccStudentMemoirIccTeacher_AspNetUsers_AssistantTeachersId",
					column: x => x.AssistantTeachersId,
					principalTable: "AspNetUsers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
				_ = table.ForeignKey(
					name: "FK_IccStudentMemoirIccTeacher_IccMemoir_MemoirsWhichIAssistId",
					column: x => x.MemoirsWhichIAssistId,
					principalTable: "IccMemoir",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		_ = migrationBuilder.CreateTable(
			name: "IccVote",
			columns: table => new {
				Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
				Type = table.Column<int>(type: "int", nullable: false),
				IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				MemoirId = table.Column<string>(type: "nvarchar(450)", nullable: true),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
				IccCommiteeRejectionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
			},
			constraints: table => {
				_ = table.PrimaryKey("PK_IccVote", x => x.Id);
				_ = table.ForeignKey(
					name: "FK_IccVote_AspNetUsers_IssuerId",
					column: x => x.IssuerId,
					principalTable: "AspNetUsers",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccVote_IccMemoir_MemoirId",
					column: x => x.MemoirId,
					principalTable: "IccMemoir",
					principalColumn: "Id");
				_ = table.ForeignKey(
					name: "FK_IccVote_IccRejection_IccCommiteeRejectionId",
					column: x => x.IccCommiteeRejectionId,
					principalTable: "IccRejection",
					principalColumn: "Id");
			});

		_ = migrationBuilder.CreateIndex(
			name: "IX_AspNetRoleClaims_RoleId",
			table: "AspNetRoleClaims",
			column: "RoleId");

		_ = migrationBuilder.CreateIndex(
			name: "RoleNameIndex",
			table: "AspNetRoles",
			column: "NormalizedName",
			unique: true,
			filter: "[NormalizedName] IS NOT NULL");

		_ = migrationBuilder.CreateIndex(
			name: "IX_AspNetUserClaims_UserId",
			table: "AspNetUserClaims",
			column: "UserId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_AspNetUserLogins_UserId",
			table: "AspNetUserLogins",
			column: "UserId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_AspNetUserRoles_RoleId",
			table: "AspNetUserRoles",
			column: "RoleId");

		_ = migrationBuilder.CreateIndex(
			name: "EmailIndex",
			table: "AspNetUsers",
			column: "NormalizedEmail");

		_ = migrationBuilder.CreateIndex(
			name: "IX_AspNetUsers_IccTeacherMemoirId",
			table: "AspNetUsers",
			column: "IccTeacherMemoirId");

		_ = migrationBuilder.CreateIndex(
			name: "UserNameIndex",
			table: "AspNetUsers",
			column: "NormalizedUserName",
			unique: true,
			filter: "[NormalizedUserName] IS NOT NULL");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccMemoir_GuideTeacherId",
			table: "IccMemoir",
			column: "GuideTeacherId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccMemoir_IccTeacherMemoir_GuideTeacherId",
			table: "IccMemoir",
			column: "IccTeacherMemoir_GuideTeacherId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccMemoir_StudentId",
			table: "IccMemoir",
			column: "StudentId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccRejection_IccStudentMemoirId",
			table: "IccRejection",
			column: "IccStudentMemoirId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccRejection_IccTeacherRejection_MemoirId",
			table: "IccRejection",
			column: "IccTeacherRejection_MemoirId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccRejection_MemoirId",
			table: "IccRejection",
			column: "MemoirId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccRejection_TeacherId",
			table: "IccRejection",
			column: "TeacherId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccStudentIccTeacherMemoir_MemoirsWhichImCandidateId",
			table: "IccStudentIccTeacherMemoir",
			column: "MemoirsWhichImCandidateId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccStudentMemoirIccTeacher_MemoirsWhichIAssistId",
			table: "IccStudentMemoirIccTeacher",
			column: "MemoirsWhichIAssistId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccVote_IccCommiteeRejectionId",
			table: "IccVote",
			column: "IccCommiteeRejectionId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccVote_IssuerId",
			table: "IccVote",
			column: "IssuerId");

		_ = migrationBuilder.CreateIndex(
			name: "IX_IccVote_MemoirId",
			table: "IccVote",
			column: "MemoirId");

		_ = migrationBuilder.AddForeignKey(
			name: "FK_AspNetUserClaims_AspNetUsers_UserId",
			table: "AspNetUserClaims",
			column: "UserId",
			principalTable: "AspNetUsers",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

		_ = migrationBuilder.AddForeignKey(
			name: "FK_AspNetUserLogins_AspNetUsers_UserId",
			table: "AspNetUserLogins",
			column: "UserId",
			principalTable: "AspNetUsers",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

		_ = migrationBuilder.AddForeignKey(
			name: "FK_AspNetUserRoles_AspNetUsers_UserId",
			table: "AspNetUserRoles",
			column: "UserId",
			principalTable: "AspNetUsers",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);

		_ = migrationBuilder.AddForeignKey(
			name: "FK_AspNetUsers_IccMemoir_IccTeacherMemoirId",
			table: "AspNetUsers",
			column: "IccTeacherMemoirId",
			principalTable: "IccMemoir",
			principalColumn: "Id");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder) {
		_ = migrationBuilder.DropForeignKey(
			name: "FK_IccMemoir_AspNetUsers_GuideTeacherId",
			table: "IccMemoir");

		_ = migrationBuilder.DropForeignKey(
			name: "FK_IccMemoir_AspNetUsers_IccTeacherMemoir_GuideTeacherId",
			table: "IccMemoir");

		_ = migrationBuilder.DropForeignKey(
			name: "FK_IccMemoir_AspNetUsers_StudentId",
			table: "IccMemoir");

		_ = migrationBuilder.DropTable(
			name: "AspNetRoleClaims");

		_ = migrationBuilder.DropTable(
			name: "AspNetUserClaims");

		_ = migrationBuilder.DropTable(
			name: "AspNetUserLogins");

		_ = migrationBuilder.DropTable(
			name: "AspNetUserRoles");

		_ = migrationBuilder.DropTable(
			name: "AspNetUserTokens");

		_ = migrationBuilder.DropTable(
			name: "IccStudentIccTeacherMemoir");

		_ = migrationBuilder.DropTable(
			name: "IccStudentMemoirIccTeacher");

		_ = migrationBuilder.DropTable(
			name: "IccVote");

		_ = migrationBuilder.DropTable(
			name: "AspNetRoles");

		_ = migrationBuilder.DropTable(
			name: "IccRejection");

		_ = migrationBuilder.DropTable(
			name: "AspNetUsers");

		_ = migrationBuilder.DropTable(
			name: "IccMemoir");
	}
}
