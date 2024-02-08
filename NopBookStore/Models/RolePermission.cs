using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class RolePermission
    {
        [Key]
        public int RolePermissionId {  get; set; }

        public int RoleId { get; set; }
        public string PermissionName { get; set; }
    }
}
