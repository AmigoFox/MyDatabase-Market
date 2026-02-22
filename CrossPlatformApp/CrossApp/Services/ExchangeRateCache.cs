using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Services
{
    public class ExchangeRateCache
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
        public bool IsLoaded => Rates.Count > 0;
    }
}
