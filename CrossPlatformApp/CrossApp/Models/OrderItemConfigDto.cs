using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossApp.Models
{
    public class OrderItemConfigDto
    {
        [JsonPropertyName("backup")]
        public bool Backup { get; set; }

        [JsonPropertyName("sharding")]
        public bool Sharding { get; set; }

        [JsonPropertyName("replicaSet")]
        public bool ReplicaSet { get; set; }
    }
}
