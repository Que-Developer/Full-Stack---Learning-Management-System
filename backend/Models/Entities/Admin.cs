namespace backend.Models.Entities
{
    public class Admin
    {
        public Guid AdminID { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        // Navigation Properties (The "Many" side)
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Lecturer> Lecturers { get; set; } = new List<Lecturer>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
