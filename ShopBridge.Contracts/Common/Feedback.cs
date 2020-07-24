namespace ShopBridge.Contracts.Common
{
    public class Feedback
    {
        public Severity Severity { get; set; }

        public string Message { get; set; }

        public Feedback() { }

        public Feedback(Severity severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public override string ToString()
        {
            return Message;
        }

        public static Feedback Info(string message) =>
            new Feedback(Severity.Info, message);

        public static Feedback Error(string message) =>
            new Feedback(Severity.Error, message);
    }
}
