using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

#nullable disable
namespace Clothing_boutique_web.Areas.Admin.Models
{
    public partial class Account
    {
        public Account()
        {
            RoleAccounts = new HashSet<RoleAccount>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Addresss { get; set; }
        public string Phone { get; set; }

        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile ImageAvatar { get; set; }

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
