namespace backend.Models.Entities
{
    public class Student
    {
        public int StudentID { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Gender { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string HomeAddress { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int? AdminID { get; set; }
        public int? CourseID { get; set; }

        //Navigation property back to Admin
        public Admin? Admin { get; set; }
        public Course? Course { get; set; }

        // Has Many 
        public List<Module> Modules { get; set; } = new List<Module>();
        public List<ModuleTask> ModuleTasks { get; set; } = new List<ModuleTask>();

    }
}
