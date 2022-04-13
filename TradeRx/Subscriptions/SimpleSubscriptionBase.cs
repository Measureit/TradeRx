namespace TradeRx.Subscriptions
{
    public abstract class SimpleSubscriptionBase : ISubscription
    {
        protected SimpleSubscriptionBase(string symbol)
        {
            Symbol = (symbol ?? string.Empty).ToLower();
        }

        /// <summary>
        /// Target symbol (bnbbtc, ethbtc, etc)
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Target channel (trade, aggTrade, etc)
        /// </summary>
        public abstract string Channel { get; }

        /// <inheritdoc />
        public string StreamName => $"{Symbol}@{Channel}";
    }
}
