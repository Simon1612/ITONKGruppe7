using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace StockShareProviderAPI
{
    public class AvailableSharesContext : DbContext
    {
        public AvailableSharesContext(DbContextOptions<AvailableSharesContext> options) : base(options) { }
        public DbSet<AvailableSharesDataModel> AvailableSharesDataModel { get; set; }
    }
}
