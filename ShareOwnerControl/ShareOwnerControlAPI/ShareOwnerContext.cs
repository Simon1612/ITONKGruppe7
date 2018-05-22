using Microsoft.EntityFrameworkCore;

namespace ShareOwnerControlAPI
{
    public class ShareOwnerContext : DbContext
    {
        public ShareOwnerContext(DbContextOptions<ShareOwnerContext> options) : base(options) { }
        public DbSet<ShareOwnerDataModel> TradeData { get; set; }
    }
}
