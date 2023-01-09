using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Models = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        IProductReadRepository _productReadRepository;
        IProductWriteRepository _productWriteRepository;
        readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, ILogger<UpdateProductCommandHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Models.Product product = await _productReadRepository.GetByIdAsync(request.ID);
            product.Stock = request.Stock;
            product.Name = request.Name;
            product.Price = request.Price;
            await _productWriteRepository.SaveAsync();
            _logger.LogInformation("Product Güncellendi...");
            return new();
        }
    }
}
