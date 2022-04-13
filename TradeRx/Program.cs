// See https://aka.ms/new-console-template for more information
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TradeRx.Connectors;
using TradeRx.Extensions;
using TradeRx.Responses;
using TradeRx.Subscriptions;

Console.WriteLine("Start");
JsonSerializerOptions Settings = new JsonSerializerOptions
{
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
};
var client = new BinanceConnector();
var baseUrl = new Uri("wss://stream.binance.com:9443");
/* TRADE */
client.MessageReceived
    .Select(ToJson)
    .Select(ToObject)
    .GroupBy(x => x.Symbol)
    .SelectMany(x =>
        x
        .Window(TimeSpan.FromSeconds(10))
        .Select(y => y
            .Average(t => t.Price)
            .Subscribe(z =>
            {
                Console.WriteLine($"{x.Key}: {z}");
            }))
    ).Subscribe();

await client.StartAsync(baseUrl.CombineSubscriptions(
    new TradeSubscription("btcusdt"),
    new TradeSubscription("ethbtc"),
    new TradeSubscription("bnbusdt")), CancellationToken.None);
Console.ReadKey();

JsonElement ToJson(ResponseMessage msg) => JsonSerializer.Deserialize<JsonElement>(msg.Text);


Trade ToObject(JsonElement json)
{
    var stream = json.GetProperty("stream").GetString();
    if (stream != null && stream.ToLower().EndsWith("@trade"))
    {
        return json.GetProperty("data").Deserialize<Trade>(Settings);
    };
    return null;
}