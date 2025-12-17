using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace app.ViewModels;

public partial class DatabaseCalculatorViewModel : ObservableObject
{
    private const decimal FIXED_USD_TO_RUB_RATE = 95m;
    private const string RUB_SYMBOL = "₽";
    private const string USD_SYMBOL = "$";

    [ObservableProperty] private string _selectedType = "MySQL";
    [ObservableProperty] private int _sizeGB = 10;
    [ObservableProperty] private string _selectedIOPS = "Низкая (100)";
    [ObservableProperty] private string _storageType = "SSD";
    [ObservableProperty] private string _scalability = "None";

    [ObservableProperty] private bool _russiaSelected = true;
    [ObservableProperty] private bool _kazakhstanSelected;
    [ObservableProperty] private bool _chinaSelected;
    [ObservableProperty] private bool _uzbekistanSelected;
    [ObservableProperty] private bool _belarusSelected;

    [ObservableProperty] private decimal _priceInRub;
    [ObservableProperty] private decimal _priceInUsd;

    public string PriceInRubFormatted => $"{PriceInRub:N2} {RUB_SYMBOL}";
    public string PriceInUsdFormatted => $"{PriceInUsd:N2} {USD_SYMBOL}";
    public int SelectedCountriesCount => GetSelectedCountries().Count();

    public string SelectedCountriesText => string.Join(", ", GetSelectedCountries());

    public List<string> GetSelectedCountries()
    {
        var list = new List<string>();
        if (RussiaSelected) list.Add("Россия");
        if (KazakhstanSelected) list.Add("Казахстан");
        if (ChinaSelected) list.Add("Китай");
        if (UzbekistanSelected) list.Add("Узбекистан");
        if (BelarusSelected) list.Add("Беларусь");
        return list;

    }

    private decimal GetCountriesMultiplier()
    {
        var count = GetSelectedCountries().Count();
        if (count < 1) return 1m;
        if (count > 3) return 3.3m;
        return count * 1.1m;
    }

    [RelayCommand]
    public void OnSelectionChanged()
    {
        ValidateAndUpdate();
    }

    [RelayCommand]
    public void ValidateAndUpdate()
    {
        var countries = GetSelectedCountries();
        if (countries.Count == 0)
        {
            RussiaSelected = true;
            countries = new List<string> { "Россия" };
        }

        if (countries.Count > 3)
        {
            var limited = countries.Take(3).ToList();
            RussiaSelected = limited.Contains("Россия");
            KazakhstanSelected = limited.Contains("Казахстан");
            ChinaSelected = limited.Contains("Китай");
            UzbekistanSelected = limited.Contains("Узбекистан");
            BelarusSelected = limited.Contains("Беларусь");
        }

        if (StorageType == "HDD" &&
            (SelectedIOPS == "Высокая (5000)" || SelectedIOPS == "Очень высокая (10000)"))
        {
            SelectedIOPS = "Средняя (1000)";
        }

        UpdatePrice((byte)countries.Count);
        OnPropertyChanged(nameof(SelectedCountriesCount));
        OnPropertyChanged(nameof(SelectedCountriesText));
    }

    private void UpdatePrice(byte countries)
    {
        decimal basePrice = SelectedType switch
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

        decimal iopsMultiplier = SelectedIOPS switch
        {
            "Низкая (100)" => 1m,
            "Средняя (1000)" => 1.5m,
            "Высокая (5000)" => 2m,
            "Очень высокая (10000)" => 3m,
            _ => 1m
        };

        decimal storageMultiplier = StorageType switch
        {
            "HDD" => 0.8m,
            "SSD" => 1.2m,
            "NVMe" => 1.8m,
            _ => 1m
        };

        decimal scalabilityMultiplier = Scalability switch
        {
            "Replication" => 1.3m,
            "Autoscaling" => 1.5m,
            _ => 1m
        };

        decimal countriesMultiplier = GetCountriesMultiplier();
        var price = (basePrice + (SizeGB * 0.10m)) * iopsMultiplier * storageMultiplier * scalabilityMultiplier * countriesMultiplier;

        PriceInRub = Math.Round(price, 2);
        PriceInUsd = Math.Round(price / FIXED_USD_TO_RUB_RATE, 2);


        OnPropertyChanged(nameof(PriceInRubFormatted));
        OnPropertyChanged(nameof(PriceInUsdFormatted));
        OnPropertyChanged(nameof(SelectedCountriesText));
        OnPropertyChanged(nameof(SelectedCountriesCount));
    }
}
