namespace ShopBridge.Application.Items
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    using ShopBridge.Application.Converters;
    using ShopBridge.Application.Validators;
    using ShopBridge.Contracts.Common;
    using ShopBridge.Contracts.Dto;
    using ShopBridge.Contracts.Interfaces;
    using Shopbridge.Database.Repository;
    using Shopbridge.Database.UnitOfWork;
    using Shopbridge.Models;

    public class ItemService : IItemService
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IRepository<Item> ItemRepository;
        private readonly ILogger<ItemService> Logger;

        public ItemService(
            IUnitOfWork unitOfWork,
            IRepository<Item> itemRepository,
            ILogger<ItemService> logger
        )
        {
            UnitOfWork = unitOfWork;
            ItemRepository = itemRepository;
            Logger = logger;
        }

        public async Task<WebResponseDto<List<ItemDto>>> GetItems()
        {
            var items = await ItemRepository.GetAll().ToListAsync();
            return WebResponseDto.For(
                items.Select(ItemConverter.ToDto).ToList(),
                Status.Ok
            );
        }

        public async Task<WebResponseDto<ItemDto>> GetItem(Guid id)
        {
            var item = await ItemRepository.FindAsync(id);
            if (item == null)
            {
                return WebResponseDto.For<ItemDto>(Severity.Error, $"Item not found for id {id}");
            }
            return WebResponseDto.For(
                ItemConverter.ToDto(item),
                Status.Ok
            );
        }

        public async Task<WebResponseDto<ItemDto>> CreateItem(ItemDto item)
        {
            if (item == null)
            {
                return WebResponseDto.For<ItemDto>(Severity.Error, "Item is null");
            }

            var validationResult = await new ItemValidator().ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                var outcomeFeedback = ValidatorFeedbackConverter.ToFeedback(validationResult.Errors);
                return WebResponseDto.For(item, Status.Failure, outcomeFeedback);
            }

            var itemData = ItemConverter.ToModel(item);

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                await ItemRepository.InsertAsync(itemData).ConfigureAwait(false);
                await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
            }

            return WebResponseDto.For(ItemConverter.ToDto(itemData), Status.Ok);
        }

        public async Task<WebResponseDto<ItemDto>> UpdateItem(Guid id, ItemDto item)
        {
            if (id == Guid.Empty)
            {
                return WebResponseDto.For<ItemDto>(Severity.Error, "id is null or empty");
            }

            if (item == null)
            {
                return WebResponseDto.For<ItemDto>(Severity.Error, "Item is null");
            }

            if (id != item.Id)
            {
                return WebResponseDto.For<ItemDto>(Severity.Error, "Id is mismatch");
            }

            var validationResult = await new ItemValidator().ValidateAsync(item);

            if (!validationResult.IsValid)
            {
                var outcomeFeedback = ValidatorFeedbackConverter.ToFeedback(validationResult.Errors);
                return WebResponseDto.For(item, Status.Failure, outcomeFeedback);
            }

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var fetchItem = await ItemRepository.FindAsync(id);

                if (fetchItem == null)
                {
                    return WebResponseDto.For<ItemDto>(Severity.Error, $"Item not found for id {id}");
                }

                fetchItem.Name = item.Name;
                fetchItem.Description = item.Description;
                fetchItem.Price = item.Price;
                fetchItem.ImageData = item.ImageData;

                ItemRepository.Update(fetchItem);
                await UnitOfWork.SaveChangesAsync().ConfigureAwait(false);
                transaction.Commit();
                return WebResponseDto.For(ItemConverter.ToDto(fetchItem), Status.Ok);
            }
        }

        public async Task<WebResponseDto<bool>> DeleteItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                return WebResponseDto.For<bool>(Severity.Error, "id is null or empty");
            }

            var fetchItem = await ItemRepository.FindAsync(id);

            if (fetchItem == null)
            {
                return WebResponseDto.For<bool>(Severity.Error, $"Item not found for id {id}");
            }

            ItemRepository.Delete(fetchItem);
            await ItemRepository.SaveChangesAsync().ConfigureAwait(false);
            return WebResponseDto.For(true, Status.Ok);
        }
    }
}
