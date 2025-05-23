using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController(IStoreRepository repository) : ControllerBase
    {

        [HttpGet("stores")]
        public async Task<ActionResult<List<StoreResponse>>> GetAllStores()
        {
            // possibly add pagination
            var stores = await repository.GetAllAsync();

            var response = stores.Select(stores => new StoreResponse
            {
                Name = stores.Name,
                Address = stores.Address,
                Email = stores.Email,
                Website = stores.Website,
                Latitude = stores.Latitude,
                Longitude = stores.Longitude,
                Brands = [.. stores.Brands.Select(b => new BrandResponse
                {
                    Id = b.Id,
                    Name = b.Name
                })]
            }).ToList();

            return Ok(response);
        }

        [HttpGet("store/{id}")]
        public async Task<ActionResult<StoreResponse>> GetStoreById(Guid id)
        {
            var store = await repository.GetByIdAsync(id);
            if (store == null)
            {
                return NotFound("Store not found.");
            }

            var response = new StoreResponse
            {
                Name = store.Name,
                Address = store.Address,
                Email = store.Email,
                Website = store.Website,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                Brands = [.. store.Brands.Select(b => new BrandResponse
                {
                    Id = b.Id,
                    Name = b.Name
                })]
            };

            return Ok(response);
        }

        [HttpPost("store")]
        public async Task<ActionResult> AddStore([FromBody] StoreRequest request)
        {
            var store = new Store
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Website = request.Website,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Brands = [.. request!.Brands.Select(b => new Brand
                {
                    Id = b.Id,
                    Name = b.Name
                })]
            };

            var validationResult = new StoreValidator().Validate(store);
            if (request == null || !validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            await repository.AddStoreAsync(store);

            return Ok(new { id = store.Id });
        }

        [HttpPut("store/{id}")]
        public async Task<ActionResult> UpdateStore(Guid id, [FromBody] UpdateStoreRequest request)
        {
            var store = await repository.GetByIdAsync(id);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            store.UpdatedAt = DateTime.UtcNow;
            store.Name = request.Name;
            store.Email = request.Email;
            store.Website = request.Website;

            var validationResult = new StoreValidator().Validate(store);
            if (request == null || !validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            await repository.UpdateStoreAsync(store);
            return NoContent();
        }

        [HttpDelete("store/{id}")]
        public async Task<ActionResult> DeleteStore(Guid id)
        {
            var existingStore = await repository.GetByIdAsync(id);

            if (existingStore == null)
            {
                return NotFound("Store not found.");
            }

            await repository.DeleteStoreAsync(id);
            return NoContent();
        }
    }
}
