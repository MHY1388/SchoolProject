using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class UserRole:IdentityUserRole<int>
    {
        public virtual User ModelUser { get; set; }
        public virtual Role ModelRole { get; set; }
    }
}
