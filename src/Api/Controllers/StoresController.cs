using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests.Stores;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Customer)},{nameof(RoleType.Shopkeeper)},{nameof(RoleType.Seller)}")]
    [Route("api/stores")]
    [ApiController]
    public class StoresController(IStoreRepository storeRepository, IBrandsRepository brandsRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<StoreResponse>>> GetAllStores()
        {
            // possibly add pagination
            var stores = await storeRepository.GetAllAsync();

            var response = stores.Select(stores => new StoreResponse
            {
                Id = stores.Id,
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

        [HttpGet("search")]
        public async Task<ActionResult<List<StoreResponse>>> SearchStoresByName([FromQuery] string storeName)
        {
            var stores = await storeRepository.GetByNameAsync(storeName);

            var response = stores.Select(stores => new StoreResponse
            {
                Id = stores.Id,
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
                Id = store.Id,
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

        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Shopkeeper)},{nameof(RoleType.Customer)},{nameof(RoleType.Seller)}")]
        [HttpPost]
        public async Task<ActionResult> AddStore([FromBody] CreateStoreRequest request)
        {
            var guidList = request.Brands.Select(b => Guid.Parse(b)).ToList();
            var brands = await brandsRepository.GetByIdsListAsync(guidList);

            var userIdClaim = User.FindFirst("id")?.Value;

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

            var requestedBrandIds = request.Brands.Select(b => b.Id).ToHashSet();

            var currentBrandIds = store.Brands.Select(b => b.Id).ToList();

            var brandsToRemove = store.Brands.Where(b => !requestedBrandIds.Contains(b.Id)).ToList();
            foreach (var brand in brandsToRemove)
            {
                store.Brands.Remove(brand);
            }

            var brandIdsToAdd = requestedBrandIds.Except(currentBrandIds).ToList();
            var brandsToAdd = await brandsRepository.GetByIdsListAsync(brandIdsToAdd);
            foreach (var brand in brandsToAdd)
            {
                store.Brands.Add(brand);
            }

            store.UpdatedAt = DateTime.UtcNow;
            store.Name = request.Name;
            store.Email = request.Email;
            store.Address = request.Address;
            store.Website = request.Website;
            store.PaymentConditions = request.PaymentConditions;

            var validationResult = new StoreValidator().Validate(store);
            if (request == null || !validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            await storeRepository.UpdateStoreAsync(store);
            return Ok(id);
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
