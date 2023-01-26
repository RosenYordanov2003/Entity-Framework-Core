namespace P01_StudentSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Course
    {
        [Key]
        public int CoursrId { get; set; }
        [MaxLength(80)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }

        public ICollection<Resourse> Resourses { get; set; }

        public ICollection<HomeWork> HomeWork { get; set; }
    }
}
