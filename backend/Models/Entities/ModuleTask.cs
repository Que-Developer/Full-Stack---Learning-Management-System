namespace backend.Models.Entities
{
    public class ModuleTask
    {
        public int ModuleTaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public required DateTime DueDate { get; set; }

        // Foreign Key
        public int? ModuleId { get; set; }

        // Navigation Property
        public Module? Module { get; set; }

        // Has Many
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
