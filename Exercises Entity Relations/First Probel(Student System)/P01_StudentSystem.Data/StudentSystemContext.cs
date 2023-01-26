using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using StudentSystemContext;
using System;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContex : DbContext
    {
        public StudentSystemContex()
        {

        }
        public StudentSystemContex(DbContextOptions<StudentSystemContex> options)
            : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resourse> Resources { get; set; }
        public DbSet<HomeWork> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuartion.ConfigurationString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Property(x => x.PhoneNumber).IsUnicode(false);

            modelBuilder.Entity<Resourse>().Property(x => x.Url).IsUnicode(false);

            modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });
        }
    }
}
