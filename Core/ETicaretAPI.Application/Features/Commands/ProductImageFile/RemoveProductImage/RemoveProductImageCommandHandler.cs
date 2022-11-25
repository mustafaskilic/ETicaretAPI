using ETicaretAPI.Application.Repositories.ProductImageFile;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Models = ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Models.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                 .FirstOrDefaultAsync(p => p.ID == Guid.Parse(request.ID));

            Models.ProductImageFile? productImageFile = product?.ProductImageFiles.FirstOrDefault(p => p.ID == Guid.Parse(request.ImageID));
            if (productImageFile is not null)
                product?.ProductImageFiles.Remove(productImageFile);

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
