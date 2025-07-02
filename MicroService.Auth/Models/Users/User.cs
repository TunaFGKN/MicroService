using MicroService.Auth.Models.UserRoles;
using Shared.Abstract;

namespace MicroService.Auth.Models.Users;

public class User : IEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}