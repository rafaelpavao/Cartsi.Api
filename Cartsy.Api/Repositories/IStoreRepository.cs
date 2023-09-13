using Cartsy.Api.Entities;
using Cartsy.Api.Models;

namespace Cartsy.Api.Repositories;

public interface IStoreRepository
{
    Task<bool> AddStoreAsync(StoreForCreationDto store);
    Task<StoreWithServicesDto?> GetStoreWithServicesByIdAsync(int storeId);
    Task<StoreWithAddressDto?> GetStoreWithAddressByIdAsync(int storeId);
    Task<StoreWithOrdersDto?> GetStoreWithOrdersByIdAsync(int storeId);
    Task<StoreWithItemsDto?> GetStoreWithItemsByIdAsync(int storeId);

    Task<bool> AddItemsToStoreById(int storeId, List<ItemDto> items);
    Task<bool> AddOrderToStoreById(int storeId, OrderDto order);
    Task<bool> AddServicesToStoreById(int storeId, List<AdditionalServiceDto> services);
    Task<bool> SaveChangesAsync();
}