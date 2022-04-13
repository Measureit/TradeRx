using TradeRx.Subscriptions;

namespace TradeRx.Extensions
{
    internal static class SubscriptionsExtension
    {
        public static Uri CombineSubscriptions(this Uri baseUrl, params ISubscription[] subscriptions)
        {
            var streams = subscriptions.Select(x => x.StreamName).ToArray();
            var urlPart = string.Join("/", streams);
            var urlPartFull = $"/stream?streams={urlPart}";

            var currentUrl = baseUrl.ToString().Trim();

            if (currentUrl.Contains("stream?"))
            {
                return baseUrl;
            }

            var newUrl = new Uri($"{currentUrl.TrimEnd('/')}{urlPartFull}");
            return newUrl;
        }
    }
}
