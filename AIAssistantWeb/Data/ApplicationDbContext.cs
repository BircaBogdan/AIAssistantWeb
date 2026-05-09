using Microsoft.EntityFrameworkCore;

using AIAssistantWeb.Models.Auth;
using AIAssistant.Core.Models;

namespace AIAssistantWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<AssistantProfile> AssistantProfiles { get; set; }
    }
}