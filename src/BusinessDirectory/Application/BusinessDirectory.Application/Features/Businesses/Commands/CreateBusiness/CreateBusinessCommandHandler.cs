using BusinessDirectory.Application.Interfaces;
using BusinessDirectory.Domain.Entities;
using BusinessDirectory.Domain.ValueObjects;
using MediatR;

namespace BusinessDirectory.Application.Features.Businesses.Commands.CreateBusiness
{
    internal sealed class CreateBusinessCommandHandler(IBusinessRepository _repository): IRequestHandler<CreateBusinessCommand, Guid>
    {
        public async Task<Guid> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
        {
            // ۱. ساخت موجودیت Value Object آدرس
            // توجه: اگر ProvinceId نامعتبر باشد، خود کلاس Address در لایه Domain خطا می‌دهد
            var address = new Address(
                request.ProvinceId,
                request.CityId,
                request.StreetLine,
                request.PostalCode);

            // ۲. ساخت موجودیت Aggregate Root کسب‌وکار
            // متد Create را که قبلاً در لایه Domain نوشتیم صدا می‌زنیم
            var newBusiness = BusinessProfile.Create(
                request.OwnerId,
                request.Title,
                request.Description,
                address);

            // ۳. اضافه کردن به Repository (در حافظه EF Core قرار می‌گیرد)
            await _repository.AddAsync(newBusiness, cancellationToken);

            // ۴. ذخیره در دیتابیس اصلی
            // جادوی Domain Events که صحبتش رو کردیم، دقیقاً در دل همین SaveChanges اتفاق می‌افته!
            await _repository.SaveChangesAsync(cancellationToken);

            // ۵. برگرداندن شناسه کسب‌وکار جدید به کاربر
            return newBusiness.Id;
        }
    }
}
