using MicroService.Auth.Models.Roles;
using MicroService.Auth.Models.Users;
using Shared.Abstract;

namespace MicroService.Auth.Models.UserRoles;

public class UserRole: IEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User User { get; set; } = default!;
    public Role Role { get; set; } = default!;
}