using FluentValidation;
using MicroService.ProductWebAPI.Context;
using MicroService.ProductWebAPI.Models;

namespace MicroService.ProductWebAPI.Validation;

public class ProductValidator: AbstractValidator<Product>
{
    private readonly ProductDbContext _context;
    public ProductValidator(ProductDbContext context)
    {
        _context = context;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.").Must(UniqueName).WithMessage("A product with this name already exists!");
        
        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative number.");
        
        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative number.");
    }

    public bool UniqueName(Product product, string name)
    {
        return !_context.Products.Any(x => x.Name.ToLower() == name.ToLower());
    }
}