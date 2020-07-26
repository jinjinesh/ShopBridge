namespace ShopBridge.Application.Converters
{
    using System;
    using ShopBridge.Contracts.Dto;
    using Shopbridge.Models;

    public static class BaseConverter
    {
        public static void ToDto(BaseDto dto, BaseModel model)
        {
            dto.Id = model.Id;
        }

        public static void ToModel(BaseModel model, BaseDto dto)
        {
            model.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
        }
    }
}
