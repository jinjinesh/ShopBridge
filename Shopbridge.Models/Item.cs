namespace Shopbridge.Models
{
    public class Item : BaseModel
    {
        public virtual string Name { get; set; }
 
        public virtual decimal Price { get; set; }
 
        public virtual string Description { get; set; }
 
        public virtual byte[] ImageData { get; set; }
    }
}
