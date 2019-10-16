using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Abstracts.ModelBase
{
    public class QueryResponse
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionStatus Status { get; set; }
    }
}
