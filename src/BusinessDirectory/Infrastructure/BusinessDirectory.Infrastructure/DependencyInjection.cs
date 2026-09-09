using BusinessDirectory.Application.Interfaces;
using BusinessDirectory.Infrastructure.Data;
using BusinessDirectory.Infrastructure.Messaging;
using BusinessDirectory.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessDirectory.Infrastructure
{
    public static class DependencyInjection
    {
        // این یک Extension Method برای IServiceCollection است
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ۱. تنظیمات دیتابیس را به اینجا منتقل می‌کنیم
            services.AddDbContext<DirectoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<DirectoryDbContext>(o =>
                {
                    // بهش می‌گیم که دیتابیس ما SQL Server هست تا کوئری‌ها رو درست بسازه
                    o.UseSqlServer();

                    // این خط میگه: یک کارگر پس‌زمینه (Background Worker) بساز که خودش 
                    // پیام‌ها رو از جدول برداره و بفرسته به RabbitMQ
                    o.UseBusOutbox();
                });
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h => {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });
            // ۲. تزریق Repository را اینجا انجام می‌دهیم
            // چون این فایل داخل خود لایه Infrastructure است، کلاس internal را به راحتی می‌بیند!
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            // اضافه کردن آداپتور پیام‌رسان
            services.AddScoped<IMessageBus, EventBus>();
            return services;
        }
    }
}
