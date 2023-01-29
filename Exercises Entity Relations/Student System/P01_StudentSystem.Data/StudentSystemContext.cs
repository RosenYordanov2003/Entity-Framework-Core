namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Connection.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });

            modelBuilder.Entity<Student>().Property(s => s.PhoneNumber).IsUnicode(false);

            modelBuilder.Entity<Resource>().Property(r => r.Url).IsUnicode(false);

            modelBuilder.Entity<Homework>().Property(h => h.Content).IsUnicode(false);

        }
        DbSet<Student> Students { get; set; }

        DbSet<Resource> Resources { get; set; }

        DbSet<StudentCourse> StudentCourses { get; set; }

        DbSet<Homework> HomeworkSubmissions { get; set; }

        DbSet<Course> Courses { get; set;}
    }
}
