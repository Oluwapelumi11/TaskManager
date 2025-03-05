using TaskManager.API.Model;

namespace TaskManager.API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCatgeryById(int categoryId);
        Task<Category> GetUserCategories(User user);
        Task<Category> CreateCategory(Category category);
        Task<Category> UpdateCategory(Category category);
        Task<Category> DeleteCategory(int categoryId);

    }
}
