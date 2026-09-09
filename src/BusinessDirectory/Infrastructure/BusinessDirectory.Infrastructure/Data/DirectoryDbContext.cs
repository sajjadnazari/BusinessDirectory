using BusinessDirectory.Domain.Common;
using BusinessDirectory.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace BusinessDirectory.Infrastructure.Data
{
    public class DirectoryDbContext : DbContext
    {
        private readonly IPublisher _publisher; // واسط MediatR برای پخش رویدادها

        public DbSet<BusinessProfile> BusinessProfiles => Set<BusinessProfile>();

        // تزریق DbContextOptions و IPublisher
        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // این خط به صورت خودکار تمام کلاس‌های IEntityTypeConfiguration (مثل بالایی) رو پیدا و اعمال می‌کنه
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }

        // بازنویسی متد SaveChangesAsync برای اجرای جادوی Domain Events
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ۱. پیدا کردن تمام موجودیت‌هایی که تغییر کرده‌اند و رویدادی در جیب خود دارند
            var domainEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            // ۲. استخراج تمام رویدادها از موجودیت‌ها
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // ۳. پاک کردن رویدادها از موجودیت‌ها (تا اگر دوباره Save شد، دو بار اجرا نشوند)
            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            // ۴. ذخیره تغییرات در دیتابیس اصلی (PostgreSQL)
            var result = await base.SaveChangesAsync(cancellationToken);

            // ۵. پخش کردن رویدادها در کل سیستم تا Handlerهای دیگر (مثل ارسال ایمیل) اجرا شوند
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result; // تعداد ردیف‌های تغییر یافته رو برمی‌گردونه
        }
    }


}
