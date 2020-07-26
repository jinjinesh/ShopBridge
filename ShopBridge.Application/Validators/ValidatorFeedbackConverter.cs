namespace ShopBridge.Application.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using ShopBridge.Contracts.Common;

    using FluentValidation.Results;

    public static class ValidatorFeedbackConverter
    {
        public static List<Feedback> ToFeedback(IList<ValidationFailure> failures)
        {
            if (failures == null || !failures.Any())
            {
                return new List<Feedback>();
            }

            var outcomeFeedback = new List<Feedback>();
            foreach (var failure in failures)
            {
                outcomeFeedback.Add(new Feedback
                {
                    Severity = GetSeverity(failure.Severity),
                    Message = failure.ErrorMessage,
                });
            }

            return outcomeFeedback;
        }

        private static Severity GetSeverity(FluentValidation.Severity severity)
        {
            switch (severity)
            {
                case FluentValidation.Severity.Error: return Severity.Error;
                case FluentValidation.Severity.Warning: return Severity.Warning;
                case FluentValidation.Severity.Info: return Severity.Info;
                default: return Severity.Error;
            }
        }
    }
}
