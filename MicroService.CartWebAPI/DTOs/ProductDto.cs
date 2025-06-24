﻿namespace MicroService.CartWebAPI.DTOs
{
    public sealed class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int Stock { get; set; }
    }
}
