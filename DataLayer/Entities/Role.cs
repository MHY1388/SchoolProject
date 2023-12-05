using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities;

public class Role:IdentityRole<int>
{
    public virtual ICollection<UserRole> UserRoles { get; set; }
}