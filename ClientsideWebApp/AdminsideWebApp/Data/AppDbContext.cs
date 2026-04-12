using AdminsideWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminsideWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<ProjectModel> Projects { get; set; }
    }
}
