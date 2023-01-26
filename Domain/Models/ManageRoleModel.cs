using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ManageRoleModel
    {
        public string Email { get; set; }
        public string NewRoleName { get; set; }
        public string OldRoleName { get; set; }
        public bool ChangeRole { get; set; }
    }
}
