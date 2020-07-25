namespace ShopBridge.Test
{
    using System;
    using NUnit.Framework;
    using ShopBridge.Application.Validators;
    using ShopBridge.Contracts.Dto;

    public class ItemValidationSpecs : ValidationSpecs<ItemDto>
    {
        [SetUp]
        public void Setup()
        {
            Validator = new ItemValidator();
        }
        [Test]
        public void Can_validate_item()
        {
            ShouldBeValid(Item());
        }

        [Test]
        public void Can_validate_name_in_item()
        {
            ShouldBeValid(Item(x => x.Name = Helper.RandomLettersString(100)));
            ShouldNotBeValid(Item(x => x.Name = Helper.RandomLettersString(101)));
            ShouldNotBeValid(Item(x => x.Name = string.Empty));
            ShouldNotBeValid(Item(x => x.Name = null));
        }

        [Test]
        public void Can_validate_description_in_item()
        {
            ShouldBeValid(Item(x => x.Description = Helper.RandomLettersString(500)));
            ShouldNotBeValid(Item(x => x.Description = Helper.RandomLettersString(501)));
            ShouldNotBeValid(Item(x => x.Description = string.Empty));
            ShouldNotBeValid(Item(x => x.Description = null));
        }

        [Test]
        public void Can_validate_price_in_item()
        {
            ShouldBeValid(Item(x => x.Price = Helper.RandomDecimal(100, 10)));
            ShouldNotBeValid(Item(x => x.Price = 0.0m));
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
    }
}
