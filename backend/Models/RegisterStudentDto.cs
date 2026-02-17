using backend.Models.Entities;

namespace backend.Models
{
    public class RegisterStudentDto
    {
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string HomeAddress { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public required bool IsProcessed { get; set; } = false;
        public required string Result { get; set; } = "Pending";
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }

        // Foreign Key for Course
        public int? CourseID { get; set; }
    }
}
