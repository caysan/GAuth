using Core.Interfaces;
using Core.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestApiController : BaseController
    {
        private readonly IGenericService<Data.Models.Products, Core.Models.Dto.Product> _productService;

        public TestApiController(IGenericService<Data.Models.Products, Core.Models.Dto.Product> productService)
        {
            _productService = productService;
        }

        [HttpGet("GetListAnonym")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            return ActionResultInstance(await _productService.AddAsync(product));
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
        {
            return ActionResultInstance(await _productService.UpdateAsync(product.Id, product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return ActionResultInstance(await _productService.RemoveAsync(id));
        }
    }
}