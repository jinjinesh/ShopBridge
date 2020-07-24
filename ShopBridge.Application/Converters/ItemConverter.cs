namespace ShopBridge.Application.Converters
{
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
                ImageData = model.ImageData,
                Price = model.Price
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
                ImageData = dto.ImageData
            };
            BaseConverter.ToModel(model, dto);
            return model;
        }
    }
}
