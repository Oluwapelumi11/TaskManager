using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Model;

namespace TaskManager.API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskItem>()
                .HasOne(t =>  t.AssignedToUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TaskItem>()
                .Property(t => t.Status)
                .HasConversion<string>();

            builder.Entity<Category>()
                .HasData(
                    new Category { Name="Work", CategoryID = 1 },
                    new Category { Name="Personal", CategoryID = 2 }
                );

            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Id="1", Name="Admin", NormalizedName="ADMIN" },
                    new IdentityRole { Id="2", Name="User", NormalizedName="USER" }
                );
        }

    }
}
