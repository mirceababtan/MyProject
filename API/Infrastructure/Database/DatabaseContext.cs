using API.Resource.Authentication.Contract;
using API.Resource.Course.Bookmark.Contract;
using API.Resource.Course.Contract;
using API.Resource.Course.Lesson.Contract;
using API.Resource.Quiz.Contract;
using API.Resource.Quiz.Option;
using API.Resource.Quiz.Question.Contract;
using API.Resource.User.Contract;
using API.Resource.User.UserProgress.Contract;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<UserProgress> UserProgress { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<UserCompletedLessons> UserCompletedLessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany()
                .HasForeignKey(c => c.InstructorId);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany()
                .HasForeignKey(l => l.CourseId);

            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Lesson)
                .WithMany()
                .HasForeignKey(q => q.LessonId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany()
                .HasForeignKey(q => q.QuizId);

            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany()
                .HasForeignKey(o => o.QuestionId);

            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserProgress>()
                .HasOne(up => up.Lesson)
                .WithMany()
                .HasForeignKey(up => up.LessonId);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bookmark>()
                .HasOne(l => l.Lesson)
                .WithMany()
                .HasForeignKey(l => l.LessonId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Enrollment>()
                .HasKey(e => new { e.UserId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserCompletedLessons>()
            .HasKey(ucl => new { ucl.UserId, ucl.LessonId });

            modelBuilder.Entity<UserCompletedLessons>()
                .HasOne(ucl => ucl.User)
                .WithMany(u => u.CompletedLessons)
                .HasForeignKey(ucl => ucl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserCompletedLessons>()
                .HasOne(ucl => ucl.Lesson)
                .WithMany(l => l.CompletedByUsers)
                .HasForeignKey(ucl => ucl.LessonId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
