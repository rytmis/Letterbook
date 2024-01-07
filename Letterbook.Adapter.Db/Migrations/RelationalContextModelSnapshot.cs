﻿// <auto-generated />
using System;
using Letterbook.Adapter.Db;
using Letterbook.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Letterbook.Adapter.Db.Migrations
{
    [DbContext(typeof(RelationalContext))]
    partial class RelationalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AudienceProfileMembers", b =>
                {
                    b.Property<string>("AudiencesId")
                        .HasColumnType("text");

                    b.Property<string>("MembersId")
                        .HasColumnType("text");

                    b.HasKey("AudiencesId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("AudienceProfileMembers");
                });

            modelBuilder.Entity("ImagesCreatedByProfile", b =>
                {
                    b.Property<string>("CreatedImagesId")
                        .HasColumnType("text");

                    b.Property<string>("CreatorsId")
                        .HasColumnType("text");

                    b.HasKey("CreatedImagesId", "CreatorsId");

                    b.HasIndex("CreatorsId");

                    b.ToTable("ImagesCreatedByProfile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Audience", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ImageId")
                        .HasColumnType("text");

                    b.Property<string>("NoteId")
                        .HasColumnType("text");

                    b.Property<string>("SourceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("NoteId");

                    b.HasIndex("SourceId");

                    b.ToTable("Audience");
                });

            modelBuilder.Entity("Letterbook.Core.Models.FollowerRelation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FollowerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FollowsId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Date");

                    b.HasIndex("FollowerId");

                    b.HasIndex("FollowsId");

                    b.ToTable("FollowerRelation");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LocalId")
                        .HasColumnType("uuid");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Letterbook.Core.Models.LinkedProfile", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProfileId")
                        .HasColumnType("text");

                    b.Property<decimal>("Permission")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("AccountId", "ProfileId");

                    b.HasIndex("ProfileId");

                    b.ToTable("LinkedProfile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Note", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Client")
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InReplyToId")
                        .HasColumnType("text");

                    b.Property<Guid?>("LocalId")
                        .HasColumnType("uuid");

                    b.Property<string>("Summary")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("InReplyToId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<CustomField[]>("CustomFields")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Followers")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Following")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Inbox")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("LocalId")
                        .HasColumnType("uuid");

                    b.Property<string>("Outbox")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnedById")
                        .HasColumnType("uuid");

                    b.Property<string>("SharedInbox")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("LocalId");

                    b.HasIndex("OwnedById");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Letterbook.Core.Models.SigningKey", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Expires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Family")
                        .HasColumnType("integer");

                    b.Property<int>("KeyOrder")
                        .HasColumnType("integer");

                    b.Property<string>("Label")
                        .HasColumnType("text");

                    b.Property<Guid?>("LocalId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("PrivateKey")
                        .HasColumnType("bytea");

                    b.Property<string>("ProfileId")
                        .HasColumnType("text");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("ProfileId");

                    b.ToTable("SigningKey");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", "AspnetIdentity");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", "AspnetIdentity");
                });

            modelBuilder.Entity("NotesBoostedByProfile", b =>
                {
                    b.Property<string>("BoostedById")
                        .HasColumnType("text");

                    b.Property<string>("BoostedNotesId")
                        .HasColumnType("text");

                    b.HasKey("BoostedById", "BoostedNotesId");

                    b.HasIndex("BoostedNotesId");

                    b.ToTable("NotesBoostedByProfile");
                });

            modelBuilder.Entity("NotesCreatedByProfile", b =>
                {
                    b.Property<string>("CreatedNotesId")
                        .HasColumnType("text");

                    b.Property<string>("CreatorsId")
                        .HasColumnType("text");

                    b.HasKey("CreatedNotesId", "CreatorsId");

                    b.HasIndex("CreatorsId");

                    b.ToTable("NotesCreatedByProfile");
                });

            modelBuilder.Entity("NotesLikedByProfile", b =>
                {
                    b.Property<string>("LikedById")
                        .HasColumnType("text");

                    b.Property<string>("LikedNotesId")
                        .HasColumnType("text");

                    b.HasKey("LikedById", "LikedNotesId");

                    b.HasIndex("LikedNotesId");

                    b.ToTable("NotesLikedByProfile");
                });

            modelBuilder.Entity("AudienceProfileMembers", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Audience", null)
                        .WithMany()
                        .HasForeignKey("AudiencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImagesCreatedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Image", null)
                        .WithMany()
                        .HasForeignKey("CreatedImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("CreatorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Letterbook.Core.Models.Audience", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Image", null)
                        .WithMany("Visibility")
                        .HasForeignKey("ImageId");

                    b.HasOne("Letterbook.Core.Models.Note", null)
                        .WithMany("Visibility")
                        .HasForeignKey("NoteId");

                    b.HasOne("Letterbook.Core.Models.Profile", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Letterbook.Core.Models.FollowerRelation", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", "Follower")
                        .WithMany("FollowingCollection")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", "Follows")
                        .WithMany("FollowersCollection")
                        .HasForeignKey("FollowsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Follows");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.OwnsMany("Letterbook.Core.Models.Mention", "Mentions", b1 =>
                        {
                            b1.Property<string>("ImageId")
                                .HasColumnType("text");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("SubjectId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Visibility")
                                .HasColumnType("integer");

                            b1.HasKey("ImageId", "Id");

                            b1.HasIndex("SubjectId");

                            b1.ToTable("Images_Mentions");

                            b1.WithOwner()
                                .HasForeignKey("ImageId");

                            b1.HasOne("Letterbook.Core.Models.Profile", "Subject")
                                .WithMany()
                                .HasForeignKey("SubjectId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Subject");
                        });

                    b.Navigation("Mentions");
                });

            modelBuilder.Entity("Letterbook.Core.Models.LinkedProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", "Account")
                        .WithMany("LinkedProfiles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", "Profile")
                        .WithMany("RelatedAccounts")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Note", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Note", "InReplyTo")
                        .WithMany("Replies")
                        .HasForeignKey("InReplyToId");

                    b.OwnsMany("Letterbook.Core.Models.Mention", "Mentions", b1 =>
                        {
                            b1.Property<string>("NoteId")
                                .HasColumnType("text");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uuid");

                            b1.Property<string>("SubjectId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Visibility")
                                .HasColumnType("integer");

                            b1.HasKey("NoteId", "Id");

                            b1.HasIndex("SubjectId");

                            b1.ToTable("Notes_Mentions");

                            b1.WithOwner()
                                .HasForeignKey("NoteId");

                            b1.HasOne("Letterbook.Core.Models.Profile", "Subject")
                                .WithMany()
                                .HasForeignKey("SubjectId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.Navigation("Subject");
                        });

                    b.Navigation("InReplyTo");

                    b.Navigation("Mentions");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", "OwnedBy")
                        .WithMany()
                        .HasForeignKey("OwnedById");

                    b.Navigation("OwnedBy");
                });

            modelBuilder.Entity("Letterbook.Core.Models.SigningKey", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany("Keys")
                        .HasForeignKey("ProfileId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotesBoostedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("BoostedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Note", null)
                        .WithMany()
                        .HasForeignKey("BoostedNotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotesCreatedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Note", null)
                        .WithMany()
                        .HasForeignKey("CreatedNotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("CreatorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotesLikedByProfile", b =>
                {
                    b.HasOne("Letterbook.Core.Models.Profile", null)
                        .WithMany()
                        .HasForeignKey("LikedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Letterbook.Core.Models.Note", null)
                        .WithMany()
                        .HasForeignKey("LikedNotesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Letterbook.Core.Models.Account", b =>
                {
                    b.Navigation("LinkedProfiles");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Image", b =>
                {
                    b.Navigation("Visibility");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Note", b =>
                {
                    b.Navigation("Replies");

                    b.Navigation("Visibility");
                });

            modelBuilder.Entity("Letterbook.Core.Models.Profile", b =>
                {
                    b.Navigation("FollowersCollection");

                    b.Navigation("FollowingCollection");

                    b.Navigation("Keys");

                    b.Navigation("RelatedAccounts");
                });
#pragma warning restore 612, 618
        }
    }
}
