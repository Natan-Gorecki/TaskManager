﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManager.Service.Database.Sqlite;

#nullable disable

namespace TaskManager.Service.Database.Sqlite.Migrations
{
    [DbContext(typeof(SqliteTaskManagerContext))]
    partial class SqliteTaskManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.18");

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbLabel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SpaceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SpaceId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbSpace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Spaces");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTask", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTask2Label", b =>
                {
                    b.Property<string>("TaskId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LabelId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("TaskId", "LabelId");

                    b.HasIndex("LabelId");

                    b.ToTable("Task2Labels");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTask2Task", b =>
                {
                    b.Property<string>("ParentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChildId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChildTaskId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentTaskId")
                        .HasColumnType("TEXT");

                    b.HasKey("ParentId", "ChildId");

                    b.HasIndex("ChildTaskId");

                    b.HasIndex("ParentTaskId");

                    b.ToTable("Task2Tasks");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTimeEntry", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TimeEntries");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbLabel", b =>
                {
                    b.HasOne("TaskManager.Service.Database.Models.DbSpace", "Space")
                        .WithMany()
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Space");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTask2Label", b =>
                {
                    b.HasOne("TaskManager.Service.Database.Models.DbLabel", "Label")
                        .WithMany()
                        .HasForeignKey("LabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManager.Service.Database.Models.DbTask", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Label");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTask2Task", b =>
                {
                    b.HasOne("TaskManager.Service.Database.Models.DbTask", "ChildTask")
                        .WithMany()
                        .HasForeignKey("ChildTaskId");

                    b.HasOne("TaskManager.Service.Database.Models.DbTask", "ParentTask")
                        .WithMany()
                        .HasForeignKey("ParentTaskId");

                    b.Navigation("ChildTask");

                    b.Navigation("ParentTask");
                });

            modelBuilder.Entity("TaskManager.Service.Database.Models.DbTimeEntry", b =>
                {
                    b.HasOne("TaskManager.Service.Database.Models.DbTask", "Task")
                        .WithMany()
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });
#pragma warning restore 612, 618
        }
    }
}
