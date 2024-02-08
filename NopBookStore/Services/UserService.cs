using Microsoft.EntityFrameworkCore;
using NopBookStore.Data;
using NopBookStore.IServices;
using NopBookStore.Models;

namespace NopBookStore.Services
{
    public class UserService : IUserService
    {
        private readonly ModernBookShopDbContext _context;

        public UserService(ModernBookShopDbContext context)
        {
            _context = context;
        }
        public User GetUserById(int UserId)
        {
            return _context.Users.Find(UserId);
        }
        public async Task<User?> GetUserByEmail(string UserEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserEmail == UserEmail);
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(User user) { }
        public void DeleteUser(int UserId) { }
        

        public Role GetRoleById(int RoleId) {  return _context.Roles.Find(RoleId); }

        public IEnumerable<Role>GetAllRoles() { return _context.Roles.ToList();}
        public void AddRole(Role role) { }
        public void UpdateRole(Role role) { }
        public void DeleteRole(int role) { }

        public UserRole GetUserRoleById(int UserRoleId) {  return _context.UserRoles.Find(UserRoleId); }
        public IEnumerable<UserRole> GetAllUserRoles() { return _context.UserRoles.ToList(); }
        public void AddUserRole(UserRole role) { }
        public void UpdateUserRole(UserRole role) { }
        public void DeleteUserRole(UserRole role) { }

        public async Task<User> AuthenticateUserAsync(string userEmail, string userPassword)
        {
            // Implement your logic to authenticate the user based on email and password
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == userEmail && u.UserPassword == userPassword);
        }

    }
}
