using TaskManager.API.Model;

namespace TaskManager.API.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllUserTasksAsync(User user);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(TaskItem item, User user);
        Task<int> UpdateTaskAsync(TaskItem item);
        Task<int> DeleteTaskAsync(int id);
        Task AssignToUserAsync(User user, TaskItem item);
    }
}
