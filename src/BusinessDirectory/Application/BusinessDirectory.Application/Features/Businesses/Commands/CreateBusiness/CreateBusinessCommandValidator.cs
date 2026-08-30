using FluentValidation;

namespace BusinessDirectory.Application.Features.Businesses.Commands.CreateBusiness
{
    public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
    {
        public CreateBusinessCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان کسب‌وکار الزامی است.")
                .MinimumLength(3).WithMessage("عنوان باید حداقل ۳ کاراکتر باشد.")
                .MaximumLength(100).WithMessage("عنوان نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

            RuleFor(x => x.ProvinceId)
                .GreaterThan(0).WithMessage("استان نامعتبر است.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("کد پستی الزامی است.")
                .Matches(@"^\d{10}$").WithMessage("کد پستی باید دقیقاً ۱۰ رقم باشد."); // Regex برای کد پستی ایران
        }
    }
}
