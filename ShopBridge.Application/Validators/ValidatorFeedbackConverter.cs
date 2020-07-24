namespace ShopBridge.Application.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using ShopBridge.Contracts.Common;

    using FluentValidation.Results;

    public static class ValidatorFeedbackConverter
    {
        private const string PropertySeparator = " > ";
        private const string MessageSeparator = " , ";
        private const string RegexToRemoveSquareBracket = @"[\[\]']+";
        private const string RegexToFindSquareBracketWithIndex = "\\[(\\d+)\\]";
        private const string LineNumber = " - line {0}";

        private static readonly Dictionary<string, string> CustomMessageDictionary = new Dictionary<string, string>();

        public static List<Feedback> ToFeedback(IList<ValidationFailure> failures, string sourceName = null)
        {
            if (failures == null || !failures.Any())
            {
                return null;
            }

            var outcomeFeedback = new List<Feedback>();
            foreach (var failure in failures)
            {
                var formattedErrorMessage = FormatErrorMessage(failure);
                formattedErrorMessage = string.IsNullOrEmpty(formattedErrorMessage) ? failure.ErrorMessage : formattedErrorMessage;

                outcomeFeedback.Add(new Feedback
                {
                    Severity = GetSeverity(failure.Severity),
                    Message = formattedErrorMessage,
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

        private static string FormatErrorMessage(ValidationFailure failure)
        {
            if (string.IsNullOrEmpty(failure?.PropertyName))
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            var fullPropertyName = failure.PropertyName;
            if (CustomMessageDictionary.Keys.Contains(fullPropertyName))
            {
                fullPropertyName = CustomMessageDictionary[fullPropertyName];
            }

            var allProperties = fullPropertyName.Split('.');
            if (allProperties.Length > 1)
            {
                for (var loopIndex = 0; loopIndex < allProperties.Length - 1; loopIndex++)
                {
                    var propertyName = allProperties[loopIndex];

                    if (propertyName.Contains('['))
                    {
                        var strItemIndexWithBracket = Regex.Match(propertyName, RegexToFindSquareBracketWithIndex).Value;
                        var strItemIndex = Regex.Replace(strItemIndexWithBracket, RegexToRemoveSquareBracket, string.Empty);
                        var key = propertyName.Replace(strItemIndexWithBracket, string.Empty);
                        if (!string.IsNullOrEmpty(key) && CustomMessageDictionary.Keys.Contains(key))
                        {
                            propertyName = propertyName.Replace(key, CustomMessageDictionary[key]);
                        }

                        if (int.TryParse(strItemIndex, out int index))
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append(PropertySeparator);
                            }
                            var lineNumberMessage = string.Format(LineNumber, ++index);
                            var message = propertyName.Replace(strItemIndexWithBracket, lineNumberMessage);
                            builder.Append(message);
                        }
                    }
                }

                if (builder.Length > 0)
                {
                    builder.Append(MessageSeparator);
                    builder.Append(failure.ErrorMessage);
                }
                else
                {
                    AppendDefaultMessage(failure, builder);
                }
            }
            else
            {
                AppendDefaultMessage(failure, builder);
            }
            return builder.ToString();
        }

        private static void AppendDefaultMessage(ValidationFailure failure, StringBuilder builder)
        {
            builder.Append(failure.ErrorMessage);
        }
    }
}
