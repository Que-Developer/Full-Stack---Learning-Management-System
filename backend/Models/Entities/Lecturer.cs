namespace backend.Models.Entities
{
    public class Lecturer
    {
        public Guid LecturerID { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        // Foreign Key
        public Guid? AdminID { get; set; }
        public int? CourseID { get; set; }

        // Navigation property back to Admin
        public Admin? Admin { get; set; }
        public Course? Course { get; set; }

        // Has Many Modules
        public ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}
