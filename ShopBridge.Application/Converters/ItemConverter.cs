namespace ShopBridge.Application.Converters
{
    using System;

    using ShopBridge.Contracts.Dto;
    using Shopbridge.Models;

    public static class ItemConverter
    {
        public static ItemDto ToDto(Item model)
        {
            if (model == null)
                return null;

            var dto = new ItemDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageData = model.ImageData != null
                        ? Convert.ToBase64String(model.ImageData, 0, model.ImageData.Length) 
                        : default
            };

            BaseConverter.ToDto(dto, model);
            return dto;
        }

        public static Item ToModel(ItemDto dto)
        {
            if (dto == null)
                return null;

            var model = new Item
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageData = !string.IsNullOrWhiteSpace(dto.ImageData) 
                    ? Convert.FromBase64String(dto.ImageData)
                    : default
            };
            BaseConverter.ToModel(model, dto);
            return model;
        }
    }
}
