namespace ShopBridge.Contracts.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public class ResponseDto
    {
        public Status StatusCode { get; set; }

        public string Message { get; set; }

        public List<Feedback> Feedback { get; set; } = new List<Feedback>();

        public ResponseDto()
        {
            
        }

        public ResponseDto(Status statusCode, string message, List<Feedback> feedback)
        {
            StatusCode = statusCode;
            Message = message;
            Feedback = feedback;
        }

        public ResponseDto(Status statusCode, string message, Feedback feedback)
        {
            StatusCode = statusCode;
            Message = message;
            Feedback = new List<Feedback> { feedback };
        }

        public ResponseDto(Status statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ResponseDto(Status statusCode)
        {
            StatusCode = statusCode;
            Feedback = new List<Feedback>();
        }

        public static ResponseDto Ok()
            => new ResponseDto(Status.Ok);

        public static ResponseDto Ok(string message)
            => new ResponseDto(Status.Ok, message, Common.Feedback.Info(message));

        public static ResponseDto Ok(string mainMessage, IEnumerable<Feedback> feedback)
            => new ResponseDto(Status.Ok, mainMessage, feedback.ToList());

        public static ResponseDto CheckMessage(string message)
            => new ResponseDto(Status.CheckMessage, message, Common.Feedback.Info(message));

        public static ResponseDto Failure(string mainMessage)
            => new ResponseDto(Status.Failure, mainMessage);

        public static ResponseDto Failure(string mainMessage, string feedback)
            => new ResponseDto(Status.Failure, mainMessage, Common.Feedback.Error(feedback));

        public static ResponseDto Failure(string mainMessage, IEnumerable<Feedback> feedback)
            => new ResponseDto(Status.Failure, mainMessage, feedback.ToList());
    }
}
