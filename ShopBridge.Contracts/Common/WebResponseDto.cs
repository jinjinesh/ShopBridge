namespace ShopBridge.Contracts.Common
{
    using System.Collections.Generic;

    public class WebResponseDto
    {
        public ResponseDto Response { get; set; }

        public WebResponseDto()
        {
            Response = new ResponseDto()
            {
                Feedback = new List<Feedback>()
            };
        }

        public void AddFeedback(Feedback feedback)
        {
            Response.Feedback.Add(feedback);
        }

        public void AddFeedback(Severity severity, string message)
        {
            AddFeedback(new Feedback
            {
                Severity = severity,
                Message = message
            });
        }

        public void SetStatus(Status status)
        {
            Response.StatusCode = status;
        }

        public static WebResponseDto<T> For<T>()
        {
            var response = new WebResponseDto<T>();
            response.SetStatus(Status.Ok);

            return response;
        }

        public static WebResponseDto<T> For<T>(T data, Status status, List<Feedback> feedback = null)
        {
            return new WebResponseDto<T>(data, new ResponseDto
            {
                StatusCode = status,
                Feedback = feedback
            });
        }

        public static WebResponseDto<T> For<T>(Severity severity, string message)
        {
            var response = For<T>();
            response.SetStatusAndAddFeedback(severity, message);
            return response;
        }

        public void SetStatusAndAddFeedback(Severity severity, string message)
        {
            if (severity == Severity.Warning)
            {
                SetStatus(Status.CheckMessage);
            }
            else if (severity >= Severity.Error)
            {
                SetStatus(Status.Failure);
            }
            AddFeedback(severity, message);
        }

    }


    public class WebResponseDto<T> : WebResponseDto
    {
        public T Data { get; set; }

        public WebResponseDto()
        {
            
        }

        public WebResponseDto(T data, ResponseDto response)
        {
            Data = data;
            Response = response;
        }
    }
}
