namespace ShopBridge.Application.Validators
{
    using FluentValidation.Validators;

    public class NotNullNotEmptyValidator : PropertyValidator
    {
        public NotNullNotEmptyValidator() : base("'{PropertyName}' should not be empty.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null || context.PropertyValue.ToString().Trim() == string.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
