using NopBookStore.Models;

namespace NopBookStore.IServices
{
    public interface IUserService
    {
        User GetUserById(int UserId);

        Task<User?> GetUserByEmail(string userEmail);  
        IEnumerable<User> GetAllUsers();
        public void AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int UserId);


        Role GetRoleById(int RoleId);
        IEnumerable<Role> GetAllRoles();
        public void AddRole(Role role);
        public void UpdateRole(Role role);
        public void DeleteRole(int RoleId);


        UserRole GetUserRoleById(int RoleId);

       
        IEnumerable<UserRole> GetAllUserRoles();
        public void AddUserRole(UserRole role);
        public void UpdateUserRole(UserRole role);
        public void DeleteUserRole(UserRole role);


        Task<User> AuthenticateUserAsync(string userEmail, string userPassword);


        //IEnumerable<RolePermission> GetRolePermissionsByRoleId(int RoleId);
        //void AddRolePermission(RolePermission rolePermission);
        //public void DeleteRolePermission(int  RolePermissionId);
    }
}
