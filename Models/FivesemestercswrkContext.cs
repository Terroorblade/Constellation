using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace КурсоваяБД5.Models;

public partial class FivesemestercswrkContext : DbContext
{
    public FivesemestercswrkContext()
    {
    }

    public FivesemestercswrkContext(DbContextOptions<FivesemestercswrkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DailySchedule> DailySchedules { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<Habit> Habits { get; set; }

    public virtual DbSet<HabitOfTheDay> HabitOfTheDays { get; set; }

    public virtual DbSet<SpheresOfLife> SpheresOfLives { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSphereSatisfaction> UserSphereSatisfactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5598;Database=fivesemestercswrk;Username=terroor;Password=5598;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailySchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("daily_schedule_pkey");

            entity.ToTable("daily_schedule", "fivesembd");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ScheduleData).HasColumnName("schedule_data");
            entity.Property(e => e.UserSchedule).HasColumnName("user_schedule");

            entity.HasOne(d => d.UserScheduleNavigation).WithMany(p => p.DailySchedules)
                .HasForeignKey(d => d.UserSchedule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("daily_schedule_user_schedule_fkey");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("event_pkey");

            entity.ToTable("event", "fivesembd");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.EventDate).HasColumnName("event_date") .HasColumnType("timestamp without time zone");
            entity.Property(e => e.EventSchedule).HasColumnName("event_schedule");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Priority)
                .HasMaxLength(6)
                .HasDefaultValueSql("'low'::character varying")
                .HasColumnName("priority");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.EventScheduleNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventSchedule)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("event_event_schedule_fkey");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.GoalId).HasName("goal_pkey");

            entity.ToTable("goal", "fivesembd");

            entity.Property(e => e.GoalId).HasColumnName("goal_id");
            entity.Property(e => e.CreateDate).HasColumnName("create_date") .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Deadline).HasColumnName("deadline") .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.GoalSphere).HasColumnName("goal_sphere");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.GoalSphereNavigation).WithMany(p => p.Goals)
                .HasForeignKey(d => d.GoalSphere)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("goal_goal_sphere_fkey");
        });

        modelBuilder.Entity<Habit>(entity =>
        {
            entity.HasKey(e => e.HabitId).HasName("habit_pkey");

            entity.ToTable("habit", "fivesembd");

            entity.Property(e => e.HabitId).HasColumnName("habit_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Frequency).HasColumnName("frequency");
            entity.Property(e => e.GoalHabit).HasColumnName("goal_habit");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Reminder)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reminder");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.GoalHabitNavigation).WithMany(p => p.Habits)
                .HasForeignKey(d => d.GoalHabit)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("habit_goal_habit_fkey");
        });

        modelBuilder.Entity<HabitOfTheDay>(entity =>
        {
            entity.HasKey(e => e.HabitDayId).HasName("habit_of_the_day_pkey");

            entity.ToTable("habit_of_the_day", "fivesembd");

            entity.Property(e => e.HabitDayId).HasColumnName("habit_day_id");
            entity.Property(e => e.HabitDay).HasColumnName("habit_day").HasColumnType("timestamp without time zone");
            entity.Property(e => e.ScheduleDay).HasColumnName("schedule_day");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.HabitDayNavigation).WithMany(p => p.HabitOfTheDays)
                .HasForeignKey(d => d.HabitDay)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("habit_of_the_day_habit_day_fkey");

            entity.HasOne(d => d.ScheduleDayNavigation).WithMany(p => p.HabitOfTheDays)
                .HasForeignKey(d => d.ScheduleDay)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("habit_of_the_day_schedule_day_fkey");
        });

        modelBuilder.Entity<SpheresOfLife>(entity =>
        {
            entity.HasKey(e => e.SphereId).HasName("spheres_of_life_pkey");

            entity.ToTable("spheres_of_life", "fivesembd");

            entity.Property(e => e.SphereId).HasColumnName("sphere_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");
       
            entity.ToTable("users", "fivesembd");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IdentityUserId).HasColumnName("identity_user_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday") .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserSphereSatisfaction>(entity =>
        {
            entity.HasKey(e => e.SatisfactionId).HasName("user_sphere_satisfaction_pkey");

            entity.ToTable("user_sphere_satisfaction", "fivesembd");

            entity.Property(e => e.SatisfactionId).HasColumnName("satisfaction_id");
            entity.Property(e => e.SatisfactionLevel).HasColumnName("satisfaction_level");
            entity.Property(e => e.SphereIds).HasColumnName("sphere_ids");
            entity.Property(e => e.UserSpheres).HasColumnName("user_spheres");

            entity.HasOne(d => d.SphereIdsNavigation).WithMany(p => p.UserSphereSatisfactions)
                .HasForeignKey(d => d.SphereIds)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_sphere_satisfaction_sphere_ids_fkey");

            entity.HasOne(d => d.UserSpheresNavigation).WithMany(p => p.UserSphereSatisfactions)
                .HasForeignKey(d => d.UserSpheres)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_sphere_satisfaction_user_spheres_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
