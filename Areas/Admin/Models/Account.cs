using System;
using System.Collections.Generic;

#nullable disable

namespace Clothing_boutique_web.Models
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

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
