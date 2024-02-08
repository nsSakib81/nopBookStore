using Microsoft.EntityFrameworkCore;
using NopBookStore.Models;

namespace NopBookStore.Data
{
    public class ModernBookShopDbContext : DbContext
    {
        public ModernBookShopDbContext(DbContextOptions<ModernBookShopDbContext>options):base(options)
        {
                
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}
