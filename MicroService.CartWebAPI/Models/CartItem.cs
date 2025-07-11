﻿using System.Text.Json.Serialization;
using Shared.Abstract;

namespace MicroService.CartWebAPI.Models;

public class CartItem: IEntity
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; } = default!;
    public DateTime? UpdatedDate { get; set; }
    
    [JsonIgnore]
    public Cart Cart { get; set; } = default!;
}