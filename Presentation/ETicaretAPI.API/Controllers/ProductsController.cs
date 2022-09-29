using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
       private readonly IProductWriteRepository _productWriteRepository;
       private readonly IProductReadRepository _productReadRepository;

       private readonly IOrderReadRepository _orderReadRepository;
       private readonly IOrderWriteRepository _orderWriteRepository; 
       private readonly ICustomerWriteRepository _customerWriteRepository; 
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        [HttpGet]
        public async Task Get()
        {
          var order = await  _orderReadRepository.GetByIdAsync("98961d84-56ec-4b5d-87ac-024c57534953");
            order.Address = "Ankara"; 
            await _orderWriteRepository.SaveAsync();
        }
       
    }
}
