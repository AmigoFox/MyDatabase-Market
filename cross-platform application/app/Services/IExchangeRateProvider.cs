using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Services
{
        public interface IExchangeRateProvider
        {
            Task<decimal?> GetUsdRubAsync(CancellationToken ct = default);
        }

}
