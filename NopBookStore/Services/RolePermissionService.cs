using Microsoft.EntityFrameworkCore;
using NopBookStore.Data;
using NopBookStore.IServices;
using NopBookStore.Middleware;
using NopBookStore.Models;

namespace NopBookStore.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly ModernBookShopDbContext _dbContext;
        private readonly ICurrentUser _currentUser;

        public RolePermissionService(ModernBookShopDbContext dbContext,
            ICurrentUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(int userId)
        {
            try
            {
                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);
                if (user == null)
                {
                    return Enumerable.Empty<string>(); // or throw an exception
                }

                var roles = await _dbContext.UserRoles.AsNoTracking().Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();
                if (!roles.Any())
                {
                    return Enumerable.Empty<string>(); // or throw an exception
                }
                var querye = _dbContext.RolePermissions
                .AsNoTracking()
                .Where(rp => roles.Contains(rp.RoleId))
                .Select(rp => rp.PermissionName)
                .ToQueryString();

                var currentUserId = _currentUser.GetUserId();

                var role = await _dbContext
                    .UserRoles
                    .Include(x => x.Role)
                    .Where(x => x.UserId == currentUserId)
                    .Select(x => x.Role)
                    .FirstOrDefaultAsync();

                if(role is null) 
                    return Enumerable.Empty<string>();

                var permissions = await _dbContext.RolePermissions
                    .Where(x => x.RoleId == role.RoleId)
                    .Select(x => x.PermissionName)
                    .ToListAsync();


                return permissions;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error in GetRolePermissionsAsync: {ex.Message}");
                throw; // rethrow the exception
            }
        }

    }
}
