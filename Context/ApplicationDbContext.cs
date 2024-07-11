using Microsoft.EntityFrameworkCore;
using ocp_blog.Model;

namespace ocp_blog.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}