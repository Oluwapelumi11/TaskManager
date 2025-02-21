namespace TaskManager.API.Model
{
    public class TaskItem
    {
        public int TaskId { get; set; }
         public required string Title { get; set; }
         public required string Description { get; set; }
         public Status Status { get; set; } = Status.UnCompleted;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsCompleted { get; set; }
        public Category Category { get; set; }
    }
}
