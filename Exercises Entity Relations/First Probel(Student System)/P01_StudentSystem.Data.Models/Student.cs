namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Student
    {
        public Student()
        {
            StudentCourses = new HashSet<StudentCourse>();
            HomeWork = new HashSet<HomeWork>();
        }
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? BirthDate { get; set; }

        ICollection<StudentCourse> StudentCourses { get; set; }
        ICollection<HomeWork> HomeWork { get; set; }
    }
}
