using API_DatabaseMarket.DTOs.Orders;
using System.Diagnostics.Metrics;

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

        private static readonly Dictionary<string, decimal> CountryMultipliers =
        new()
        {
            ["RU"] = 1.0m,
            ["BY"] = 1.2m,
            ["KZ"] = 1.5m,
            ["UZ"] = 1.2m,
            ["CN"] = 1.8m,
            ["US"] = 2.0m,
            ["DE"] = 1.9m,
            ["GB"] = 2.1m,
            ["FR"] = 1.8m,
            ["JP"] = 2.2m,
            ["SG"] = 2.3m
        };

        private decimal GetCountriesMultiplier(List<string> countries)
        {
            if (countries == null || countries.Count == 0)
                return 1m;

            decimal max = 1m;

            foreach (var country in countries)
            {
                var code = country.ToUpperInvariant();

                var multiplier = CountryMultipliers.TryGetValue(code, out var value)
                    ? value
                    : 1.0m;

                if (multiplier > max)
                    max = multiplier;
            }

            if (countries.Count > 1)
                max *= 1.2m;

            return max;
        }
    }
}