namespace ShopBridge.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    

    using ShopBridge.Contracts.Common;
    using ShopBridge.Contracts.Dto;
    using ShopBridge.Contracts.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService ItemService;

        public ItemController(IItemService itemService)
        {
            ItemService = itemService;
        }


        // GET: api/<ItemController>
        [HttpGet]
        public async Task<WebResponseDto<List<ItemDto>>> GetItems()
        {
            return await ItemService.GetItems();
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public async Task<WebResponseDto<ItemDto>> GetItem(Guid id)
        {
            return await ItemService.GetItem(id);
        }

        // POST api/<ItemController>
        [HttpPost]
        public async Task<WebResponseDto<ItemDto>> CreateItem(ItemDto item)
        {
            return await ItemService.CreateItem(item);
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public async Task<WebResponseDto<ItemDto>> UpdateItem(Guid id, ItemDto item)
        {
            return await ItemService.UpdateItem(id, item);
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public async Task<WebResponseDto<bool>> DeleteItem(Guid id)
        {
            return await ItemService.DeleteItem(id);
        }
    }
}
