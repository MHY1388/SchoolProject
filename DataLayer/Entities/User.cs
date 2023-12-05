using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class User:IdentityUser<int>
    {
        [MaxLength(200)]
        public string FirstName { get; set; }
        [MaxLength(200)]
        public string LastName { get; set; }
        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class? UserClass { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
