using MicroService.Auth.Models.Roles;
using MicroService.Auth.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace MicroService.Auth.Models.UserRoles;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}
