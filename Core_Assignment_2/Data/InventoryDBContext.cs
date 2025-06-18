using Core_Assignment_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Core_Assignment_2.Data
{
    public class InventoryDBContext:DbContext
    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> options) : base(options) { }

        public DbSet<Inventory> Inventories { get; set; }
    }
}
