using Microsoft.EntityFrameworkCore;
using NopBookStore.Data;
using NopBookStore.IServices;
using NopBookStore.Models;

namespace NopBookStore.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly ModernBookShopDbContext _dbContext;

        public RolePermissionService(ModernBookShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(int userId)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
            var roles = await _dbContext.UserRoles.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();
            var permissions = await _dbContext.RolePermissions
                                  .Where(rp => roles.Contains(rp.RoleId))
                                  .Select(rp => rp.PermissionName)
                                  .ToListAsync();

            return permissions;
        }

    }
}
