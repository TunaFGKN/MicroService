using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Abstract;

namespace MicroService.OrderAPI.Models;

public class Order: IEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public Guid? CartId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; } = decimal.Zero;

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty; // JSON string

    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; }
}