using MicroService.Auth.Models.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace MicroService.Auth.Models.Roles;

public class Role
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
