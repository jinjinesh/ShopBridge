namespace ShopBridge.Test
{
    using System.Linq;

    using FluentValidation;
    using NUnit.Framework;
    using ShopBridge.Application.Validators;

    public class ValidationSpecs<T>
    {
        protected AbstractValidator<T> Validator { get; set; }

        public void ShouldBeValid(T obj)
        {
            var result = Validator.Validate(obj);
            Assert.IsTrue(result.IsValid);
        }

        public void ShouldNotBeValid(T obj)
        {
            var result = Validator.Validate(obj);
            Assert.IsFalse(result.IsValid);
        }
    }
}
