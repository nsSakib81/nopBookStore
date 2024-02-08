using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
    }
}
