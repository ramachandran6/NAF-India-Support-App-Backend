﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NISA.DataAccessLayer;

#nullable disable

namespace NISA.DataAccessLayer.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NISA.Model.AttachmentDetails", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("fileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<int>("ticketId")
                        .HasColumnType("int");

                    b.Property<string>("uploadedDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("attachmentDetails");
                });

            modelBuilder.Entity("NISA.Model.EmployeeRole", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("id"));

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("employeeRoles");
                });

            modelBuilder.Entity("NISA.Model.ImageEntity", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<bool?>("isActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ticketId")
                        .HasColumnType("int");

                    b.Property<string>("uploadedDate")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ImageId");

                    b.ToTable("imageEntities");
                });

            modelBuilder.Entity("NISA.Model.LookUpTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lookUpTables");
                });

            modelBuilder.Entity("NISA.Model.TicketComments", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("id"));

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("commentedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("commentedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ticketRefnum")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("ticketComments");
                });

            modelBuilder.Entity("NISA.Model.TicketDetails", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int?>("age")
                        .HasColumnType("int");

                    b.Property<int>("assignedTo")
                        .HasColumnType("int");

                    b.Property<string>("attachments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("createdBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("departmentLookUpId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("endDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("isDeleted")
                        .HasColumnType("bit");

                    b.Property<bool?>("isReopened")
                        .HasColumnType("bit");

                    b.Property<string>("owner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("priority")
                        .HasColumnType("int");

                    b.Property<int?>("severity")
                        .HasColumnType("int");

                    b.Property<string>("startDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ticketRefnum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("departmentLookUpId");

                    b.HasIndex("userId");

                    b.ToTable("ticketDetails");
                });

            modelBuilder.Entity("NISA.Model.TicketHistoryTable", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("attachments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("departmentLookUpRefId")
                        .HasColumnType("int");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("endDate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("priority")
                        .HasColumnType("int");

                    b.Property<int?>("severity")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ticketRefNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("updatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("updatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("departmentLookUpRefId");

                    b.HasIndex("updatedBy");

                    b.ToTable("ticketHistoryTables");
                });

            modelBuilder.Entity("NISA.Model.UserDetails", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("departmentLookupRefId")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool?>("isLoggedIn")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("roleId")
                        .HasColumnType("int");

                    b.Property<string>("userName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("departmentLookupRefId");

                    b.HasIndex("roleId");

                    b.ToTable("userDetails");
                });

            modelBuilder.Entity("NISA.Model.TicketDetails", b =>
                {
                    b.HasOne("NISA.Model.LookUpTable", "LookUpTable")
                        .WithMany()
                        .HasForeignKey("departmentLookUpId");

                    b.HasOne("NISA.Model.UserDetails", "UserDetails")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("LookUpTable");

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("NISA.Model.TicketHistoryTable", b =>
                {
                    b.HasOne("NISA.Model.LookUpTable", "LookUpTable")
                        .WithMany()
                        .HasForeignKey("departmentLookUpRefId");

                    b.HasOne("NISA.Model.UserDetails", "UserDetails")
                        .WithMany()
                        .HasForeignKey("updatedBy");

                    b.Navigation("LookUpTable");

                    b.Navigation("UserDetails");
                });

            modelBuilder.Entity("NISA.Model.UserDetails", b =>
                {
                    b.HasOne("NISA.Model.LookUpTable", "LookUpTables")
                        .WithMany()
                        .HasForeignKey("departmentLookupRefId");

                    b.HasOne("NISA.Model.EmployeeRole", "employeeRole")
                        .WithMany()
                        .HasForeignKey("roleId");

                    b.Navigation("LookUpTables");

                    b.Navigation("employeeRole");
                });
#pragma warning restore 612, 618
        }
    }
}
