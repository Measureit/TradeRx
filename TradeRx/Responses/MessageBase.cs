using System.Text.Json.Serialization;
using TradeRx.Json;

namespace TradeRx.Responses
{
    public class MessageBase
    {
        [JsonPropertyName("e")]
        public string Event { get; set; }
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonPropertyName("E"), ]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime EventTime { get; set; }
    }
}
