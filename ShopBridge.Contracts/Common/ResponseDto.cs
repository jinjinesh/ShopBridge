namespace ShopBridge.Contracts.Common
{
    using System.Collections.Generic;

    public class ResponseDto
    {
        public Status StatusCode { get; set; }

        public List<Feedback> Feedback { get; set; } = new List<Feedback>();

        public ResponseDto()
        {
            
        }
    }
}
