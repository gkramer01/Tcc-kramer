using Domain.Enums;
using Domain.Interfaces;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/brands")]
    [ApiController]
    public class BrandsController(IBrandsRepository brandsRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<BrandsResponse> GetAllBrands()
        {
            var brands = await brandsRepository.GetAllBrandsAsync();
            var response = new BrandsResponse
            {
                Brands = [.. brands.Select(b => new BrandResponse
                {
                    Id = b.Id,
                    Name = b.Name
                })]
            };
            return response;
        }

    }
}
