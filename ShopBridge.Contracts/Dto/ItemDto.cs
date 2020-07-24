namespace ShopBridge.Contracts.Dto
{
    public class ItemDto : BaseDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public byte[] ImageData { get; set; }
    }
}
