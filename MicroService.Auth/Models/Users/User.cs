using MicroService.Auth.Models.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace MicroService.Auth.Models.Users;

public class User 
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
