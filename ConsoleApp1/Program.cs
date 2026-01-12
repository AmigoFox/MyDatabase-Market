using System;
using System.Net.Http;
using System.Threading.Tasks;
using MihaZupan;

class Program
{
    static async Task Main()
    {
        // Полная строка прокси, как в файле
        string proxyLine =
            "SOCKS5://usrxvcxw41x:pwdghs2x0oj@air24.pro:10023|http://air24.pro:10023/api/reconnect?apiToken=7c3176eea4164289890450368b9aa51e";

        // ─────────────────────────────
        // 1. Разбор строки
        // ─────────────────────────────
        string[] parts = proxyLine.Split('|');
        string proxyAddress = parts[0];
        string reconnectUrl = parts[1];

        Uri proxyUri = new Uri(proxyAddress);

        string proxyHost = proxyUri.Host;
        int proxyPort = proxyUri.Port;

        string[] auth = proxyUri.UserInfo.Split(':');
        string proxyLogin = auth[0];
        string proxyPassword = auth[1];

        // ─────────────────────────────
        // 2. SOCKS5 proxy (air24)
        // ─────────────────────────────
        var socksProxy = new HttpToSocks5Proxy(
            proxyHost,
            proxyPort,
            proxyLogin,
            proxyPassword
        );

        var handler = new HttpClientHandler
        {
            Proxy = socksProxy,
            UseProxy = true
        };

        using var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        // ─────────────────────────────
        // 3. Вызов reconnect API ЧЕРЕЗ ПРОКСИ
        // ─────────────────────────────
        Console.WriteLine("Calling air24 reconnect API THROUGH SOCKS5 proxy");
        Console.WriteLine(reconnectUrl);
        Console.WriteLine();

        try
        {
            HttpResponseMessage response = await client.GetAsync(reconnectUrl);
            string body = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Status code: " + (int)response.StatusCode);
            Console.WriteLine("Response body:");
            Console.WriteLine(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Request error:");
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nDONE");
        Console.ReadKey();
    }
}
