using backend.Models.Entities.Authentication.SignUp;

namespace backend.Models.Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public required string CourseName { get; set; }
        public required string Faculty { get; set; }
        public string? Description { get; set; }

        // Foreign Key
        public int? AdminID { get; set; }

        //Navigation property
        public Admin? Admin { get; set; }

        // Has Many
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public ICollection<RegisterStudent> RegisterStudents { get; set; } = new List<RegisterStudent>();
    }
}
