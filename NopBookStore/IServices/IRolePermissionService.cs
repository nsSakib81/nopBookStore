using NopBookStore.Models;

namespace NopBookStore.IServices
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<string>> GetRolePermissionsAsync(int RoleId);
    }
}
