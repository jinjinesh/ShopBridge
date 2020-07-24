namespace ShopBridge.Application.Validators
{
    using ShopBridge.Contracts.Dto;

    using FluentValidation;

    public class ItemValidator : AbstractValidator<ItemDto>
    {
        public ItemValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .NotNull()
                .Length(1, 100)
                .WithMessage("Name is mandatory");

            RuleFor(r => r.Description)
                .NotEmpty()
                .NotNull()
                .Length(1, 500)
                .WithMessage("Description is mandatory");

            RuleFor(r => r.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Price is mandatory and should be greater than zero");
        }
    }
}
