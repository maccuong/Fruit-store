using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;

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
        [Required(ErrorMessage = "UserName is required")]
        [MinLength(6, ErrorMessage = "The min lenght of username is 6 charater")]
        [MaxLength(25, ErrorMessage = "The max lenght of username is 25 charater")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Fullname is required")]
        [MinLength(6)]
        [MaxLength(25)]
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool Status { get; set; }
        public string Addresss { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string AvatarName { get; set; }
        [NotMapped]
        [DisplayName("Avatar")]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageAvatar { get; set; }
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
