using BusinessDirectory.Application.Interfaces;
using BusinessDirectory.Infrastructure.Data;
using BusinessDirectory.Infrastructure.Repositories;
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

            // ۲. تزریق Repository را اینجا انجام می‌دهیم
            // چون این فایل داخل خود لایه Infrastructure است، کلاس internal را به راحتی می‌بیند!
            services.AddScoped<IBusinessRepository, BusinessRepository>();

            return services;
        }
    }
}
