using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using app.Services; // важно: здесь лежит ExchangeRateCache

namespace app.ViewModels;

public partial class DatabaseCalculatorViewModel : ObservableObject
{
    private readonly ExchangeRateCache _cache;

    public DatabaseCalculatorViewModel(ExchangeRateCache cache)
    {
        _cache = cache;
    }

    [ObservableProperty] private string _selectedType = "MySQL";
    [ObservableProperty] private int _sizeGB = 10;
    [ObservableProperty] private string _selectedIOPS = "Низкая (100)";
    [ObservableProperty] private string _storageType = "SSD";
    [ObservableProperty] private string _scalability = "None";

    [ObservableProperty] private bool _russiaSelected;
    [ObservableProperty] private bool _kazakhstanSelected;
    [ObservableProperty] private bool _chinaSelected;
    [ObservableProperty] private bool _uzbekistanSelected;
    [ObservableProperty] private bool _belarusSelected;

    [ObservableProperty] private decimal _priceInRub;
    [ObservableProperty] private decimal _priceInUsd;
    [ObservableProperty] private decimal _priceInCny;
    [ObservableProperty] private decimal _priceInInr;
    [ObservableProperty] private decimal _priceInTry;
    [ObservableProperty] private decimal _priceInByn;
    [ObservableProperty] private decimal _priceInKzt;
    [ObservableProperty] private decimal _priceInKrw;
    [ObservableProperty] private decimal _priceInEur;
    [ObservableProperty] private decimal _priceInAmd;
    [ObservableProperty] private decimal _priceInUzs;


    public string UsdRate => $"{_cache.Rates.GetValueOrDefault("USD"):N2}";
    public string CnyRate => $"{_cache.Rates.GetValueOrDefault("CNY"):N2}";
    public string EurRate => $"{_cache.Rates.GetValueOrDefault("EUR"):N2}";
    public string KztRate => $"{_cache.Rates.GetValueOrDefault("KZT"):N2}";
    public string BynRate => $"{_cache.Rates.GetValueOrDefault("BYN"):N2}";
    public string InrRate => $"{_cache.Rates.GetValueOrDefault("INR"):N2}";
    public string TryRate => $"{_cache.Rates.GetValueOrDefault("TRY"):N2}";
    public string KrwRate => $"{_cache.Rates.GetValueOrDefault("KRW"):N2}";
    public string AmdRate => $"{_cache.Rates.GetValueOrDefault("AMD"):N2}";
    public string UzsRate => $"{_cache.Rates.GetValueOrDefault("UZS"):N2}";



    public string PriceInRubFormatted => $"{PriceInRub:N2} ₽";
    public string PriceInUsdFormatted => $"{PriceInUsd:N2} $";
    public string PriceInCnyFormatted => $"{PriceInCny:N2} ¥";
    public string PriceInInrFormatted => $"{PriceInInr:N2} ₹";
    public string PriceInTryFormatted => $"{PriceInTry:N2} ₺";
    public string PriceInBynFormatted => $"{PriceInByn:N2} Br";
    public string PriceInKztFormatted => $"{PriceInKzt:N2} ₸";
    public string PriceInKrwFormatted => $"{PriceInKrw:N2} ₩";
    public string PriceInEurFormatted => $"{PriceInEur:N2} €";
    public string PriceInAmdFormatted => $"{PriceInAmd:N2} ֏";
    public string PriceInUzsFormatted => $"{PriceInUzs:N2} сум";

    partial void OnRussiaSelectedChanged(bool value) => ValidateAndUpdate();
    partial void OnKazakhstanSelectedChanged(bool value) => ValidateAndUpdate();
    partial void OnChinaSelectedChanged(bool value) => ValidateAndUpdate();
    partial void OnUzbekistanSelectedChanged(bool value) => ValidateAndUpdate();
    partial void OnBelarusSelectedChanged(bool value) => ValidateAndUpdate();

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
        var countries = GetSelectedCountries();
        if (countries.Count == 0) return 1m;

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
            total *= 1.1m;

        return total;
    }

    [RelayCommand]
    public void OnSelectionChanged() => ValidateAndUpdate();

    [RelayCommand]
    public void ValidateAndUpdate()
    {
        var countries = GetSelectedCountries();
        if (countries.Count == 0)
        {
            RussiaSelected = true;
            countries = new List<string> { "Россия" };
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

    private decimal GetRate(string code)
    {
        if (_cache.Rates.TryGetValue(code, out var rate))
            return rate;

        throw new Exception($"Курс {code} не загружен");
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

        try
        {
            PriceInUsd = Math.Round(PriceInRub / GetRate("USD"), 2);
            PriceInCny = Math.Round(PriceInRub / GetRate("CNY"), 2);
            PriceInInr = Math.Round(PriceInRub / GetRate("INR"), 2);
            PriceInTry = Math.Round(PriceInRub / GetRate("TRY"), 2);
            PriceInByn = Math.Round(PriceInRub / GetRate("BYN"), 2);
            PriceInKzt = Math.Round(PriceInRub / GetRate("KZT"), 2);
            PriceInKrw = Math.Round(PriceInRub / GetRate("KRW"), 2);
            PriceInEur = Math.Round(PriceInRub / GetRate("EUR"), 2);
            PriceInAmd = Math.Round(PriceInRub / GetRate("AMD"), 2);
            PriceInUzs = Math.Round(PriceInRub / GetRate("UZS"), 2);
        }
        catch (Exception)
        {
            PriceInUsd = PriceInCny = PriceInInr = PriceInTry = PriceInByn =
                PriceInKzt = PriceInKrw = PriceInEur = PriceInAmd = PriceInUzs = 0m;
        }

        OnPropertyChanged(nameof(PriceInRubFormatted));
        OnPropertyChanged(nameof(PriceInUsdFormatted));
        OnPropertyChanged(nameof(PriceInCnyFormatted));
        OnPropertyChanged(nameof(PriceInInrFormatted));
        OnPropertyChanged(nameof(PriceInTryFormatted));
        OnPropertyChanged(nameof(PriceInBynFormatted));
        OnPropertyChanged(nameof(PriceInKztFormatted));
        OnPropertyChanged(nameof(PriceInKrwFormatted));
        OnPropertyChanged(nameof(PriceInEurFormatted));
        OnPropertyChanged(nameof(PriceInAmdFormatted));
        OnPropertyChanged(nameof(PriceInUzsFormatted));
        OnPropertyChanged(nameof(SelectedCountriesText));
        OnPropertyChanged(nameof(SelectedCountriesCount));
    }
}
