﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Utal.Icc.Mm.Mvc.Data;

#nullable disable

namespace Utal.Icc.Mm.Mvc.Migrations
{
    [DbContext(typeof(IccDbContext))]
    [Migration("20230425014157_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IccStudentIccTeacherMemoir", b =>
                {
                    b.Property<string>("CandidatesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MemoirsWhichImCandidateId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CandidatesId", "MemoirsWhichImCandidateId");

                    b.HasIndex("MemoirsWhichImCandidateId");

                    b.ToTable("IccStudentIccTeacherMemoir");
                });

            modelBuilder.Entity("IccStudentMemoirIccTeacher", b =>
                {
                    b.Property<string>("AssistantTeachersId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MemoirsWhichIAssistId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AssistantTeachersId", "MemoirsWhichIAssistId");

                    b.HasIndex("MemoirsWhichIAssistId");

                    b.ToTable("IccStudentMemoirIccTeacher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccMemoir", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phase")
                        .HasColumnType("int");

                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("IccMemoir");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IccMemoir");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccRejection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("IccRejection");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IccRejection");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeactivated")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Rut")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IccUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccVote", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("IccCommiteeRejectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IssuerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MemoirId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IccCommiteeRejectionId");

                    b.HasIndex("IssuerId");

                    b.HasIndex("MemoirId");

                    b.ToTable("IccVote");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccStudentMemoir", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccMemoir");

                    b.Property<string>("GuideTeacherId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("GuideTeacherId");

                    b.HasDiscriminator().HasValue("IccStudentMemoir");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacherMemoir", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccMemoir");

                    b.Property<string>("GuideTeacherId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("GuideTeacherId");

                    b.ToTable("IccMemoir", t =>
                        {
                            t.Property("GuideTeacherId")
                                .HasColumnName("IccTeacherMemoir_GuideTeacherId");
                        });

                    b.HasDiscriminator().HasValue("IccTeacherMemoir");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccCommiteeRejection", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccRejection");

                    b.Property<string>("MemoirId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("MemoirId");

                    b.HasDiscriminator().HasValue("IccCommiteeRejection");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacherRejection", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccRejection");

                    b.Property<string>("IccStudentMemoirId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MemoirId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TeacherId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("IccStudentMemoirId");

                    b.HasIndex("MemoirId");

                    b.HasIndex("TeacherId");

                    b.ToTable("IccRejection", t =>
                        {
                            t.Property("MemoirId")
                                .HasColumnName("IccTeacherRejection_MemoirId");
                        });

                    b.HasDiscriminator().HasValue("IccTeacherRejection");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccStudent", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccUser");

                    b.Property<int>("IsDoingThePractice")
                        .HasColumnType("int");

                    b.Property<bool>("IsWorking")
                        .HasColumnType("bit");

                    b.Property<string>("RemainingCourses")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniversityId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("IccStudent");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacher", b =>
                {
                    b.HasBaseType("Utal.Icc.Mm.Mvc.Models.IccUser");

                    b.Property<string>("IccTeacherMemoirId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsGuest")
                        .HasColumnType("bit");

                    b.Property<string>("Office")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Schedule")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("IccTeacherMemoirId");

                    b.HasDiscriminator().HasValue("IccTeacher");
                });

            modelBuilder.Entity("IccStudentIccTeacherMemoir", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccStudent", null)
                        .WithMany()
                        .HasForeignKey("CandidatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacherMemoir", null)
                        .WithMany()
                        .HasForeignKey("MemoirsWhichImCandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IccStudentMemoirIccTeacher", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacher", null)
                        .WithMany()
                        .HasForeignKey("AssistantTeachersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccStudentMemoir", null)
                        .WithMany()
                        .HasForeignKey("MemoirsWhichIAssistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccMemoir", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccStudent", "Student")
                        .WithMany("MemoirsWhichIOwn")
                        .HasForeignKey("StudentId");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccVote", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccCommiteeRejection", null)
                        .WithMany("IccVotes")
                        .HasForeignKey("IccCommiteeRejectionId");

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacher", "Issuer")
                        .WithMany()
                        .HasForeignKey("IssuerId");

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccMemoir", "Memoir")
                        .WithMany()
                        .HasForeignKey("MemoirId");

                    b.Navigation("Issuer");

                    b.Navigation("Memoir");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccStudentMemoir", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacher", "GuideTeacher")
                        .WithMany("MemoirsWhichIGuide")
                        .HasForeignKey("GuideTeacherId");

                    b.Navigation("GuideTeacher");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacherMemoir", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacher", "GuideTeacher")
                        .WithMany()
                        .HasForeignKey("GuideTeacherId");

                    b.Navigation("GuideTeacher");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccCommiteeRejection", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccMemoir", "Memoir")
                        .WithMany("CommiteeRejections")
                        .HasForeignKey("MemoirId");

                    b.Navigation("Memoir");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacherRejection", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccStudentMemoir", null)
                        .WithMany("TeacherRejections")
                        .HasForeignKey("IccStudentMemoirId");

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccMemoir", "Memoir")
                        .WithMany()
                        .HasForeignKey("MemoirId");

                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacher", "Teacher")
                        .WithMany("MemoirsWhichIRejected")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Memoir");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacher", b =>
                {
                    b.HasOne("Utal.Icc.Mm.Mvc.Models.IccTeacherMemoir", null)
                        .WithMany("AssistantTeachers")
                        .HasForeignKey("IccTeacherMemoirId");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccMemoir", b =>
                {
                    b.Navigation("CommiteeRejections");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccStudentMemoir", b =>
                {
                    b.Navigation("TeacherRejections");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacherMemoir", b =>
                {
                    b.Navigation("AssistantTeachers");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccCommiteeRejection", b =>
                {
                    b.Navigation("IccVotes");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccStudent", b =>
                {
                    b.Navigation("MemoirsWhichIOwn");
                });

            modelBuilder.Entity("Utal.Icc.Mm.Mvc.Models.IccTeacher", b =>
                {
                    b.Navigation("MemoirsWhichIGuide");

                    b.Navigation("MemoirsWhichIRejected");
                });
#pragma warning restore 612, 618
        }
    }
}
