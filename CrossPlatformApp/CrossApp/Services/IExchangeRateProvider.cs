using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp.Services
{
        public interface IExchangeRateProvider
        {
            Task<decimal?> GetUsdRubAsync(CancellationToken ct = default);
        }

}
