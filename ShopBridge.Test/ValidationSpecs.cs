namespace ShopBridge.Test
{
    using FluentValidation;
    using NUnit.Framework;

    public class ValidationSpecs<T>
    {
        protected AbstractValidator<T> Validator { get; set; }

        protected void ShouldBeValid(T obj)
        {
            var result = Validator.Validate(obj);
            Assert.IsTrue(result.IsValid);
        }

        protected void ShouldNotBeValid(T obj)
        {
            var result = Validator.Validate(obj);
            Assert.IsFalse(result.IsValid);
        }
    }
}
