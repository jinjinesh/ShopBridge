namespace Shopbridge.Models
{
    using System;

    public class BaseModel : IEntityType
    {
        public virtual Guid Id { get; set; }
    }
}
