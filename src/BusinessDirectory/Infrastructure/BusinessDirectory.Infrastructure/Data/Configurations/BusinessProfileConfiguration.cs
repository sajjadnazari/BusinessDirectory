using BusinessDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessDirectory.Infrastructure.Data.Configurations
{
    public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
    {
        public void Configure(EntityTypeBuilder<BusinessProfile> builder)
        {
            // نام جدول در پایگاه داده
            builder.ToTable("Businesses");

            // تنظیم کلید اصلی
            builder.HasKey(x => x.Id);

            // تنظیمات فیلدهای معمولی
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(2000);

            // جادوی Value Object (Owned Types)
            // اینجا به EF می‌گیم که Address یک جدول جدا نیست، 
            // بلکه فیلدهای آن باید در همون جدول Businesses قرار بگیرن.
            builder.OwnsOne(x => x.Location, address =>
            {
                address.Property(a => a.ProvinceId).HasColumnName("ProvinceId").IsRequired();
                address.Property(a => a.CityId).HasColumnName("CityId").IsRequired();
                address.Property(a => a.StreetLine).HasColumnName("StreetLine").HasMaxLength(500);
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(10);
            });
        }
    }
}
