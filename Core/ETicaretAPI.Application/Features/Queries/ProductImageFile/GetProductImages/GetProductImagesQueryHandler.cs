using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.ProductImageFile;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        IProductReadRepository _productReadRepository;
        IConfiguration _configuration;

        public GetProductImagesQueryHandler(IConfiguration configuration, IProductReadRepository productReadRepository)
        {
            _configuration = configuration;
            _productReadRepository = productReadRepository;
        }

        public async Task<List<GetProductImagesQueryResponse>?> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            Models.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
               .FirstOrDefaultAsync(p => p.ID == Guid.Parse(request.ID));

            return product?.ProductImageFiles.Select(img => new GetProductImagesQueryResponse
            {
                Path = $"{_configuration["BaseStorageUrl:Azure"]}/{img.Path}",
                FileName = img.FileName,
                ID = img.ID
            }).ToList();
        }
    }
}
