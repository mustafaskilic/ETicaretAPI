using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {

            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ürün adını boş geçmeyiniz.")
                    .MaximumLength(150)
                    .MinimumLength(2)
                    .WithMessage($"Lütfen ürün adını 2 ile 150 arasında giriniz");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stok bilgisini boş girmeyin")
                    .Must(s => s >= 0)
                        .WithMessage("Stok bilgisi negatif olamaz!");

            RuleFor(p => p.Price)
              .NotEmpty()
              .NotNull()
                  .WithMessage("Lütfen fiyat bilgisini boş girmeyin")
                  .Must(s => s >= 0)
                      .WithMessage("Fiyat bilgisi negatif olamaz!");
        }
    }
}
