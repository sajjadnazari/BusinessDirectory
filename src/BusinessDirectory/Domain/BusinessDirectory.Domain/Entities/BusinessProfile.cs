using BusinessDirectory.Domain.Common;
using BusinessDirectory.Domain.ValueObjects;

namespace BusinessDirectory.Domain.Entities
{
    public class BusinessProfile : BaseEntity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid OwnerId { get; private set; } // شناسه کاربری که صاحب این کسب‌وکار است
        public Address Location { get; private set; }
        public BusinessStatus Status { get; private set; } // Enum: Pending, Active, Suspended
        public DateTime CreatedAt { get; private set; }

        // ۱. سازنده بدون پارامتر (فقط برای استفاده EF Core در لایه زیرساخت)
        private BusinessProfile() { }

        // ۲. سازنده اصلی که کاملاً پرایوت است
        private BusinessProfile(Guid ownerId, string title, string description, Address location)
        {
            Id = Guid.NewGuid();
            OwnerId = ownerId;
            Title = title;
            Description = description;
            Location = location;
            Status = BusinessStatus.Pending; // کسب‌وکار جدید در انتظار تأیید است
            CreatedAt = DateTime.UtcNow;
        }

        // ۳. Factory Method: تنها راه ساخت یک کسب‌وکار جدید از بیرون
        public static BusinessProfile Create(Guid ownerId, string title, string description, Address location)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length < 3)
                throw new ArgumentException("عنوان کسب‌وکار باید حداقل ۳ کاراکتر باشد.");

            var business = new BusinessProfile(ownerId, title, description, location);

            // در اینجا می‌تونیم یک Domain Event مثل BusinessRegisteredEvent ثبت کنیم
            // business.AddDomainEvent(new BusinessRegisteredEvent(business.Id));

            return business;
        }

        // ۴. متدهای رفتاری (Behaviors): تغییر وضعیت موجودیت با منطق کنترل‌شده
        public void Verify()
        {
            if (Status == BusinessStatus.Active)
                throw new InvalidOperationException("این کسب‌وکار قبلاً فعال شده است.");

            Status = BusinessStatus.Active;
        }

        public void UpdateLocation(Address newAddress)
        {
            ArgumentNullException.ThrowIfNull(newAddress);
            Location = newAddress;
        }

        public enum BusinessStatus
        {
            Pending = 1,
            Active = 2,
            Suspended = 3
        }
    }
}
