using Microsoft.EntityFrameworkCore;
using ShareOwnerControlAPI.Models;

namespace ShareOwnerControlAPI
{
    public class ShareOwnerContext : DbContext
    {
        public ShareOwnerContext(DbContextOptions<ShareOwnerContext> options) : base(options) { }
        public DbSet<ShareOwnerDataModel> ShareOwnerDataModel { get; set; }
        public DbSet<OwnerDataModel> OwnerDataModel { get; set; }
        public DbSet<ShareDataModel> ShareDataModel { get; set; }
    }
}
