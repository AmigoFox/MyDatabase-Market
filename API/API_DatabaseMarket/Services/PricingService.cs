using API_DatabaseMarket.DTOs.Orders;

namespace API_DatabaseMarket.Services
{
    public class PricingService : IPricingService
    {
        public decimal Calculate(CreateOrderItemDto dto)
        {
            decimal basePrice = dto.DatabaseType switch
            {
                "MySQL" => 10m,
                "PostgreSQL" => 15m,
                "MongoDB" => 20m,
                "Redis" => 12m,
                "SQLite" => 5m,
                "Microsoft SQL Server" => 25m,
                "Cassandra" => 18m,
                "Oracle Database" => 30m,
                "MariaDB" => 10m,
                _ => 8m
            };

            decimal iopsMultiplier = dto.Iops switch
            {
                "Низкая (100)" => 1m,
                "Средняя (1000)" => 1.5m,
                "Высокая (5000)" => 2m,
                "Очень высокая (10000)" => 3m,
                _ => 1m
            };

            decimal storageMultiplier = dto.StorageType switch
            {
                "HDD" => 0.8m,
                "SSD" => 1.2m,
                "NVMe" => 1.8m,
                _ => 1m
            };

            decimal scalabilityMultiplier = dto.Scalability switch
            {
                "Replication" => 1.3m,
                "Autoscaling" => 1.5m,
                _ => 1m
            };

            decimal countriesMultiplier = GetCountriesMultiplier(dto.Countries);

            var price = (basePrice + (dto.SizeGB * 0.10m))
                        * iopsMultiplier
                        * storageMultiplier
                        * scalabilityMultiplier
                        * countriesMultiplier;

            return Math.Round(price, 2);
        }

        private decimal GetCountriesMultiplier(List<string> countries)
        {
            if (countries == null || countries.Count == 0)
                return 1m;

            decimal total = 0m;

            foreach (var country in countries)
            {
                total += country switch
                {
                    "Россия" => 1.0m,
                    "Беларусь" => 1.2m,
                    "Казахстан" => 1.5m,
                    "Узбекистан" => 1.2m,
                    "Китай" => 1.8m,
                    _ => 1.0m
                };
            }

            if (countries.Count > 1)
                total *= 1.2m;

            return total;
        }
    }
}