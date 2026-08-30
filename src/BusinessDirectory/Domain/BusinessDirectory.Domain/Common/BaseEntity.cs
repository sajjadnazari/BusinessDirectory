namespace BusinessDirectory.Domain.Common
{
    public abstract class BaseEntity
    {
        // شناسه یکتا برای تمام موجودیت‌ها. 
        // (می‌توانستیم از نوع Generic <TId> استفاده کنیم تا بعضی‌ها int و بعضی‌ها Guid باشند، 
        // اما برای پروژه‌های بزرگ Guid استانداردتر است).
        public Guid Id { get; protected set; }

        // لیستی خصوصی برای نگهداری رویدادهایی که در این موجودیت اتفاق می‌افتد
        private readonly List<IDomainEvent> _domainEvents = new();

        // فقط خواندنی برای بیرون از کلاس، تا کسی نتواند از بیرون لیست را دستکاری کند
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        // متدی برای اضافه کردن یک رویداد جدید
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        // متدی برای پاک کردن رویدادها (بعد از اینکه آن‌ها را در دیتابیس ذخیره کردیم صدا زده می‌شود)
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
