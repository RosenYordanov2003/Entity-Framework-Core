using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            StudentSystemContex studentSystemContext = new StudentSystemContex();
            studentSystemContext.Database.EnsureCreated();
            studentSystemContext.Add(new Student
            { Name = "Gosho", BirthDate = new DateTime(12, 12, 21), PhoneNumber = "+35983844", RegisteredOn = new DateTime(12, 12, 10) });
            studentSystemContext.SaveChanges();
        }
    }
}
