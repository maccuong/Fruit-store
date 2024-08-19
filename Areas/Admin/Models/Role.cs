using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

#nullable disable

namespace Clothing_boutique_web.Areas.Admin.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleAccounts = new HashSet<RoleAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
