using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Model
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public required string Name { get; set; }
    }
}
