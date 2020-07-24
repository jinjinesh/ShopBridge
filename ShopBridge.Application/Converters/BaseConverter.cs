namespace ShopBridge.Application.Converters
{
    using System;
    using ShopBridge.Contracts.Dto;
    using Shopbridge.Models;

    public static class BaseConverter
    {
        public static void ToDto(BaseDto dto, BaseModel model)
        {
            if (dto == null)
                return;

            if(model == null)
                return;

            dto.Id = model.Id;
        }

        public static void ToModel(BaseModel model, BaseDto dto)
        {
            if (dto == null)
                return;

            if (model == null)
                return;

            model.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
        }
    }
}
