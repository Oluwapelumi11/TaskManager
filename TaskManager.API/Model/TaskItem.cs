using Microsoft.VisualBasic;

namespace TaskManager.API.Model
{
    public class TaskItem
    {
        public int TaskID { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pending;

        public virtual Category? Category { get; set; }
        public int CategoryID { get; set; }

    }
}
