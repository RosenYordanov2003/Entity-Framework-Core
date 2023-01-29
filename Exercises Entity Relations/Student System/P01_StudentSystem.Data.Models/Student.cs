namespace P01_StudentSystem.Data.Models
{
    using P01_StudentSystem.Data.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Student
    {
        public Student()
        {
            Courses = new HashSet<StudentCourse>();
            HomeworkSubmissions = new HashSet<Homework>();
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        [MaxLength(GlobalConstants.PhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<StudentCourse> Courses { get; set; }

        public ICollection<Homework> HomeworkSubmissions { get; set; }
    }
}
