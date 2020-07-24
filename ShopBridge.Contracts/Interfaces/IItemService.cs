namespace ShopBridge.Contracts.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ShopBridge.Contracts.Common;
    using ShopBridge.Contracts.Dto;

    public interface IItemService
    {
        Task<WebResponseDto<List<ItemDto>>> GetItems();

        Task<WebResponseDto<ItemDto>> GetItem(Guid id);

        Task<WebResponseDto<ItemDto>> CreateItem(ItemDto item);

        Task<WebResponseDto<ItemDto>> UpdateItem(Guid id, ItemDto item);

        Task<WebResponseDto<bool>> DeleteItem(Guid id);
    }
}