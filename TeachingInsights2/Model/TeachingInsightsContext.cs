namespace TeachingInsights2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TeachingInsightsContext : DbContext
    {
        public TeachingInsightsContext()
            : base("name=TeachingInsightsContext")
        {
        }

        public virtual DbSet<CalibrationSetting> CalibrationSettings { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Lecture> Lectures { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserHasCourse> UserHasCourses { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalibrationSetting>()
                .Property(e => e.settingName)
                .IsUnicode(false);

            modelBuilder.Entity<CalibrationSetting>()
                .HasMany(e => e.Users)
                .WithMany(e => e.CalibrationSettings)
                .Map(m => m.ToTable("UserHasCalibrationSetting", "TeachingInsights").MapLeftKey("settingId").MapRightKey("username"));

            modelBuilder.Entity<Course>()
                .Property(e => e.courseName)
                .IsUnicode(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Lectures)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.UserHasCourses)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.emailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.currentIpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserHasCourses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserHasCourse>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<UserType>()
                .Property(e => e.userTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserType)
                .WillCascadeOnDelete(false);
        }
    }
}
