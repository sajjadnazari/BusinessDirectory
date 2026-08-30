using BusinessDirectory.Domain.Entities;

namespace BusinessDirectory.Application.Interfaces
{
    public interface IBusinessRepository
    {
        // متدی برای اضافه کردن کسب‌وکار به حافظه EF Core
        Task AddAsync(BusinessProfile business, CancellationToken cancellationToken = default);

        // متدی برای ذخیره نهایی در دیتابیس
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
