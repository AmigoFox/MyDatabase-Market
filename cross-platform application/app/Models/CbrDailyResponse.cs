using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Models
{


        public class CbrDailyResponse
        {
            public DateTime Date { get; set; }
            public Dictionary<string, ValuteItem> Valute { get; set; } = new();
        }

        public class ValuteItem
        {
            public string CharCode { get; set; } = "";
            public string Name { get; set; } = "";
            public int Nominal { get; set; }
            public decimal Value { get; set; }
        }

    
}
