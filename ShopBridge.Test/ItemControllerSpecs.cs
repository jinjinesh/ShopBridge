namespace ShopBridge.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using ShopBridge.Application.Items;
    using ShopBridge.Controllers;
    using ShopBridge.Contracts.Common;
    using ShopBridge.Contracts.Dto;
    using Shopbridge.Database.Repository;
    using Shopbridge.Database.UnitOfWork;
    using Shopbridge.Models;

    using NUnit.Framework;

    public class ItemControllerSpecs : BaseSpecs
    {
        private ItemController itemController;

        #region Setup

        [SetUp]
        public void Setup()
        {
            SetupDbAndLogger();
            var itemControllerLogger = loggerFactory.CreateLogger<ItemController>();
            var itemServiceLogger = loggerFactory.CreateLogger<ItemService>();
            var unitOfWork = new UnitOfWork(shopBridgeDbContext);
            var itemRepository = new Repository<Item>(shopBridgeDbContext);
            var itemService = new ItemService(unitOfWork, itemRepository, itemServiceLogger);
            itemController = new ItemController(itemService, itemControllerLogger);
        }

        [TearDown]
        public void TearDown()
        {
            shopBridgeDbContext.Dispose();
        }

        #endregion


        #region Test cases

        [Test]
        public async Task Can_create_item()
        {
            var item = Item();
            var createdItem = await CreateItem(item);
            Assert.NotNull(createdItem);
            Assert.NotNull(createdItem.Data);
            Assert.AreEqual(Status.Ok, createdItem.Response.StatusCode);
            Assert.AreNotEqual(Guid.Empty, createdItem.Data.Id);
            AssertItem(item, createdItem.Data);
        }

        [Test]
        public async Task Cannot_create_item_if_item_is_null()
        {
            var createdItem = await CreateItem(null);
            Assert.NotNull(createdItem);
            Assert.Null(createdItem.Data);
            Assert.AreEqual(Status.Failure, createdItem.Response.StatusCode);
            Assert.IsTrue(createdItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_create_item_if_validation_fail()
        {
            var createdItem = await CreateItem(Item(x => x.Name = null));
            Assert.NotNull(createdItem);
            Assert.NotNull(createdItem.Data);
            Assert.AreEqual(Status.Failure, createdItem.Response.StatusCode);
            Assert.IsTrue(createdItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Can_get_item()
        {
            var createdItem = await CreateItem(Item());
            var fetchItem = await GetItem(createdItem.Data.Id);
            Assert.NotNull(fetchItem);
            Assert.NotNull(fetchItem.Data);
            Assert.AreEqual(Status.Ok, fetchItem.Response.StatusCode);
            Assert.AreEqual(createdItem.Data.Id, fetchItem.Data.Id);
            AssertItem(createdItem.Data, fetchItem.Data);
        }

        [Test]
        public async Task Shall_get_error_while_fetching_item_if_id_not_present()
        {
            var fetchItem = await GetItem(Guid.NewGuid());
            Assert.NotNull(fetchItem);
            Assert.Null(fetchItem.Data);
            Assert.AreEqual(Status.Failure, fetchItem.Response.StatusCode);
            Assert.IsTrue(fetchItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Can_get_all_items()
        {
            var createdItem1 = await CreateItem(Item());
            var createdItem2 = await CreateItem(Item());
            var fetchItems = await GetAllItems();
            Assert.NotNull(fetchItems);
            Assert.NotNull(fetchItems.Data);
            Assert.AreEqual(Status.Ok, fetchItems.Response.StatusCode);
            AssertItem(createdItem1.Data, fetchItems.Data.First(x => x.Id == createdItem1.Data.Id));
            AssertItem(createdItem2.Data, fetchItems.Data.First(x => x.Id == createdItem2.Data.Id));
        }

        [Test]
        public async Task Can_update_item()
        {
            var createdItem = await CreateItem(Item());
            createdItem.Data.Name = Helper.RandomLettersString(100);
            var updateItem = await UpdateItem(createdItem.Data.Id, createdItem.Data);
            Assert.NotNull(updateItem);
            Assert.NotNull(updateItem.Data);
            Assert.AreEqual(Status.Ok, updateItem.Response.StatusCode);
            var fetchItem = await GetItem(createdItem.Data.Id);
            AssertItem(updateItem.Data, fetchItem.Data);
        }

        [Test]
        public async Task Cannot_update_item_if_id_is_empty()
        {
            var updateItem = await UpdateItem(Guid.Empty, Item());
            Assert.NotNull(updateItem);
            Assert.Null(updateItem.Data);
            Assert.AreEqual(Status.Failure, updateItem.Response.StatusCode);
            Assert.IsTrue(updateItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_update_item_if_item_is_null()
        {
            var updateItem = await UpdateItem(Guid.NewGuid(), null);
            Assert.NotNull(updateItem);
            Assert.Null(updateItem.Data);
            Assert.AreEqual(Status.Failure, updateItem.Response.StatusCode);
            Assert.IsTrue(updateItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_update_item_if_id_and_item_id_mismatch()
        {
            var updateItem = await UpdateItem(Guid.NewGuid(), Item());
            Assert.NotNull(updateItem);
            Assert.Null(updateItem.Data);
            Assert.AreEqual(Status.Failure, updateItem.Response.StatusCode);
            Assert.IsTrue(updateItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_update_item_if_validation_fail()
        {
            var id = Guid.NewGuid();
            var updateItem = await UpdateItem(id, Item(x =>
            {
                x.Id = id;
                x.Name = string.Empty;
            }));
            Assert.NotNull(updateItem);
            Assert.NotNull(updateItem.Data);
            Assert.AreEqual(Status.Failure, updateItem.Response.StatusCode);
            Assert.IsTrue(updateItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_update_item_if_item_not_found()
        {
            var id = Guid.NewGuid();
            var updateItem = await UpdateItem(id, Item(x => x.Id = id));
            Assert.NotNull(updateItem);
            Assert.Null(updateItem.Data);
            Assert.AreEqual(Status.Failure, updateItem.Response.StatusCode);
            Assert.IsTrue(updateItem.Response.Feedback.Any());
        }

        [Test]
        public async Task Can_delete_item()
        {
            var createdItem = await CreateItem(Item());
            var deleteResponse = await DeleteItem(createdItem.Data.Id);
            Assert.IsTrue(deleteResponse.Data);
            Assert.AreEqual(Status.Ok, deleteResponse.Response.StatusCode);
            var fetchItem = await GetItem(createdItem.Data.Id);
            Assert.AreEqual(Status.Failure, fetchItem.Response.StatusCode);
        }

        [Test]
        public async Task Cannot_delete_item_if_item_not_found()
        {
            var deleteResponse = await DeleteItem(Guid.NewGuid());
            Assert.NotNull(deleteResponse);
            Assert.AreEqual(Status.Failure, deleteResponse.Response.StatusCode);
            Assert.IsFalse(deleteResponse.Data);
            Assert.IsTrue(deleteResponse.Response.Feedback.Any());
        }

        [Test]
        public async Task Cannot_delete_item_if_item_id_is_empty()
        {
            var deleteResponse = await DeleteItem(Guid.Empty);
            Assert.NotNull(deleteResponse);
            Assert.AreEqual(Status.Failure, deleteResponse.Response.StatusCode);
            Assert.IsFalse(deleteResponse.Data);
            Assert.IsTrue(deleteResponse.Response.Feedback.Any());
        }

        #endregion

        #region Private methods

        private async Task<WebResponseDto<ItemDto>> CreateItem(ItemDto item)
        {
            return await itemController.CreateItem(item);
        }

        private async Task<WebResponseDto<ItemDto>> GetItem(Guid id)
        {
            return await itemController.GetItem(id);
        }

        private async Task<WebResponseDto<List<ItemDto>>> GetAllItems()
        {
            return await itemController.GetItems();
        }

        private async Task<WebResponseDto<ItemDto>> UpdateItem(Guid id, ItemDto item)
        {
            return await itemController.UpdateItem(id, item);
        }

        private async Task<WebResponseDto<bool>> DeleteItem(Guid id)
        {
            return await itemController.DeleteItem(id);
        }

        private ItemDto Item(Action<ItemDto> action = null)
        {
            var item = new ItemDto
            {
                Name = Helper.RandomLettersString(100),
                Description = Helper.RandomString(500),
                Price = Helper.RandomDecimal(100, 10)
            };
            action?.Invoke(item);
            return item;
        }

        private static void AssertItem(ItemDto expectedItem, ItemDto actualItem)
        {
            Assert.AreEqual(expectedItem.Name, actualItem.Name);
            Assert.AreEqual(expectedItem.Description, actualItem.Description);
            Assert.AreEqual(expectedItem.Price, actualItem.Price);
            Assert.AreEqual(expectedItem.ImageData, actualItem.ImageData);
        }

        #endregion
    }
}
