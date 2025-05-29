using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Customer)},{nameof(RoleType.Shopkeeper)},{nameof(RoleType.Seller)}")]
    [Route("api/stores")]
    [ApiController]
    public class StoreController(IStoreRepository storeRepository, IBrandsRepository brandsRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<StoreResponse>>> GetAllStores()
        {
            // possibly add pagination
            var stores = await storeRepository.GetAllAsync();

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

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreResponse>> GetStoreById(Guid id)
        {
            var store = await storeRepository.GetByIdAsync(id);
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

        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Shopkeeper)}, {nameof(RoleType.Customer)}, {nameof(RoleType.Seller)}")]
        [HttpPost]
        public async Task<ActionResult> AddStore([FromBody] StoreRequest request)
        {
            var guidList = request.Brands.Select(b => Guid.Parse(b)).ToList();
            var brands = await brandsRepository.GetByIdsListAsync(guidList);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized("Usuário inválido.");

            var store = new Store
            {
                CreatedBy = userId,
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Website = request.Website,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PaymentConditions = request.PaymentConditions,
                Brands = brands
            };

            var validationResult = new StoreValidator().Validate(store);
            if (request == null || !validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            await storeRepository.AddStoreAsync(store);

            return Ok(new { id = store.Id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStore(Guid id, [FromBody] UpdateStoreRequest request)
        {
            var store = await storeRepository.GetByIdAsync(id);

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

            await storeRepository.UpdateStoreAsync(store);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStore(Guid id)
        {
            var existingStore = await storeRepository.GetByIdAsync(id);

            if (existingStore == null)
            {
                return NotFound("Store not found.");
            }

            await storeRepository.DeleteStoreAsync(id);
            return NoContent();
        }
    }
}
