﻿// <auto-generated />
using System;
using ECR.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ECR.Domain.Migrations
{
    [DbContext(typeof(MDRContext))]
    partial class MDRContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ECR.Domain.Models.Agency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Agency");
                });

            modelBuilder.Entity("ECR.Domain.Models.Audio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateRecorded")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecordId");

                    b.ToTable("Audio");
                });

            modelBuilder.Entity("ECR.Domain.Models.ContactDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgencyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.ToTable("ContactDetail");
                });

            modelBuilder.Entity("ECR.Domain.Models.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Login");
                });

            modelBuilder.Entity("ECR.Domain.Models.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AgencyId")
                        .HasColumnType("int");

                    b.Property<string>("CallType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTimeOfReport")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncidentLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriorityLevel")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.ToTable("Record");
                });

            modelBuilder.Entity("ECR.Domain.Models.Agency", b =>
                {
                    b.OwnsOne("ECR.Domain.Models.Name", "Name", b1 =>
                        {
                            b1.Property<int>("AgencyId")
                                .HasColumnType("int");

                            b1.Property<string>("Extension")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("First")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Last")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Middle")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("AgencyId");

                            b1.ToTable("Agency");

                            b1.WithOwner()
                                .HasForeignKey("AgencyId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("ECR.Domain.Models.Audio", b =>
                {
                    b.HasOne("ECR.Domain.Models.Record", "Record")
                        .WithMany("Audios")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Record");
                });

            modelBuilder.Entity("ECR.Domain.Models.ContactDetail", b =>
                {
                    b.HasOne("ECR.Domain.Models.Agency", "Agency")
                        .WithMany("ContactDetails")
                        .HasForeignKey("AgencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agency");
                });

            modelBuilder.Entity("ECR.Domain.Models.Login", b =>
                {
                    b.OwnsOne("ECR.Domain.Models.Name", "Name", b1 =>
                        {
                            b1.Property<int>("LoginId")
                                .HasColumnType("int");

                            b1.Property<string>("Extension")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("First")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Last")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Middle")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("LoginId");

                            b1.ToTable("Login");

                            b1.WithOwner()
                                .HasForeignKey("LoginId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("ECR.Domain.Models.Record", b =>
                {
                    b.HasOne("ECR.Domain.Models.Agency", "Agency")
                        .WithMany("Records")
                        .HasForeignKey("AgencyId");

                    b.OwnsOne("ECR.Domain.Models.Caller", "Call", b1 =>
                        {
                            b1.Property<int>("RecordId")
                                .HasColumnType("int");

                            b1.Property<string>("Address")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("ContactDetail")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("RecordId");

                            b1.ToTable("Record");

                            b1.WithOwner()
                                .HasForeignKey("RecordId");
                        });

                    b.Navigation("Agency");

                    b.Navigation("Call")
                        .IsRequired();
                });

            modelBuilder.Entity("ECR.Domain.Models.Agency", b =>
                {
                    b.Navigation("ContactDetails");

                    b.Navigation("Records");
                });

            modelBuilder.Entity("ECR.Domain.Models.Record", b =>
                {
                    b.Navigation("Audios");
                });
#pragma warning restore 612, 618
        }
    }
}
