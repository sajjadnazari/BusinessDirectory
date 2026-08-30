namespace BusinessDirectory.Domain.ValueObjects
{
    public record Address
    {
        public int ProvinceId { get; init; }
        public int CityId { get; init; }
        public string StreetLine { get; init; }
        public string PostalCode { get; init; }

        // سازنده خصوصی برای جلوگیری از ساخت آدرس خالی
        private Address() { }

        public Address(int provinceId, int cityId, string streetLine, string postalCode)
        {
            // اینجا می‌تونی ولیدیشن‌های پایه رو قرار بدی
            if (string.IsNullOrWhiteSpace(streetLine))
                throw new ArgumentException("آدرس خیابان نمی‌تواند خالی باشد.");

            ProvinceId = provinceId;
            CityId = cityId;
            StreetLine = streetLine;
            PostalCode = postalCode;
        }
    }
}
