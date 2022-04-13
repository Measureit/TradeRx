using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TradeRx.Json;

namespace TradeRx.Responses
{
    public class Trade : MessageBase
    {
        /// <summary>
        /// The symbol the trade was for
        /// </summary>
        [JsonPropertyName("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// The id of this aggregated trade
        /// </summary>
        [JsonPropertyName("t")]
        public long TradeId { get; set; }

        /// <summary>
        /// The price of the trades
        /// </summary>
        [JsonPropertyName("p")]
        public double Price { get; set; }

        /// <summary>
        /// The combined quantity of the trades
        /// </summary>
        [JsonPropertyName("q")]
        public double Quantity { get; set; }

        /// <summary>
        /// The first trade id in this aggregation
        /// </summary>
        [JsonPropertyName("b")]
        public long BuyerOrderId { get; set; }

        /// <summary>
        /// The last trade id in this aggregation
        /// </summary>
        [JsonPropertyName("a")]
        public long SellerOrderId { get; set; }

        /// <summary>
        /// The time of the trades
        /// </summary>
        [JsonPropertyName("T")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// Whether the buyer was the maker
        /// </summary>
        [JsonPropertyName("m")]
        public bool IsBuyerMaker { get; set; }

        /// <summary>
        /// Was the trade the best price match?
        /// </summary>
        [JsonPropertyName("M")]
        public bool IsMatch { get; set; }

        /// <summary>
        /// Side of the trade
        /// </summary>
        [JsonIgnore]
        public TradeSide Side => IsBuyerMaker ? TradeSide.Sell : TradeSide.Buy;
    }

    public enum TradeSide
    {
        /// <summary>
        /// Somebody bought something
        /// </summary>
        Buy,

        /// <summary>
        /// Somebody sold something
        /// </summary>
        Sell
    }
}
