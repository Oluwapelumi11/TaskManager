using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Interfaces;
using TaskManager.API.Model;

namespace TaskManager.API.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private AppDbContext _dbContext;
        private UserManager<User> _userManager;
        public TaskRepository(AppDbContext dbContext,UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task AssignToUserAsync(User user, TaskItem item)
        {
             await _dbContext.TaskItems.Where(t => t.TaskID == item.TaskID).ExecuteUpdateAsync(setter => setter
            .SetProperty(t=> t.AssignedToId, user.Id));
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem item, User user)
        {
            item.CreatedById = user.Id;
            await _dbContext.TaskItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            var deleted = await _dbContext.TaskItems.Where(t => t.TaskID == id).ExecuteDeleteAsync<TaskItem>();
            return deleted;
        }

        public async Task<IEnumerable<TaskItem>> GetAllUserTasksAsync(User user)
        {
            var tasks = await _dbContext.TaskItems.Where(t => t.CreatedById == user.Id).ToArrayAsync();
            return tasks;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            var task = await _dbContext.TaskItems.FirstOrDefaultAsync(t => t.TaskID == id);
            return task;
        }

        public async Task<int> UpdateTaskAsync(TaskItem item)
        {
            return await _dbContext.TaskItems.Where(t => t.TaskID == item.TaskID).ExecuteUpdateAsync(setters => setters
            .SetProperty(t => t.Title, t=> item.Title)
            .SetProperty(t => t.DueDate, t=> item.DueDate)
            .SetProperty(t => t.Status, t=> item.Status)
            .SetProperty(t => t.AssignedToId, t=> item.AssignedToId)
            .SetProperty(t => t.UpdatedAt,  DateTime.Now)
            .SetProperty(t => t.Description, t=> item.Description)
            .SetProperty(t => t.CategoryID, t=> item.CategoryID)
            );
        }
    }
}
