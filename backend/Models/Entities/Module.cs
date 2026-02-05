namespace backend.Models.Entities
{
    public class Module
    {
        public int ModuleID { get; set; }
        public required string ModuleName { get; set; }
        public required string ModuleCode { get; set; }
        public required string Credits { get; set; }

        // Foreign Key for Admin and Lecturer
        public Guid? AdminID { get; set; }
        public Guid? LecturerID { get; set; }
        public int? CourseID { get; set; }

        // Navigation property back to Lecturer
        public Admin? Admin { get; set; }
        public Lecturer? Lecturer { get; set; }
        public Course? Course { get; set; }

        // Has Many
        public List<Student> Students { get; set; } = new List<Student>();
        public List<ModuleTask> ModuleTasks { get; set; } = new List<ModuleTask>();
    }
}
