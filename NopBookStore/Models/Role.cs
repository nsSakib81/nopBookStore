using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public RolePermission RolePermission { get; set; }
    }
}
