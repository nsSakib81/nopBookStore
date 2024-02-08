using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail {  get; set; }
        public string UserPassword { get; set; }
    }
}
