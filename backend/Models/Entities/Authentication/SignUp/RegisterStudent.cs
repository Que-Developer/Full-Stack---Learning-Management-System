using Microsoft.Identity.Client;

namespace backend.Models.Entities.Authentication.SignUp
{
    public class RegisterStudent
    {
        public int StudentID { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Gender { get; set; }
        public required string DateOfBirth { get; set; }
        public required string HomeAddress { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool isProcessed { get; set; } = false;
        
        // Foreign Key for Course
        public int CourseID { get; set; }
        
        // Navigation property
        public Course Course { get; set; }
    }
}
