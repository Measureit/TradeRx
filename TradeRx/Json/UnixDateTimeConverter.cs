using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TradeRx.Json
{
    public static class BinanceTime
    {
        /// <summary>
        /// Base Unix time (1.1.1970)
        /// </summary>
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Return current total Unix milliseconds
        /// </summary>
        /// <returns></returns>
        public static long NowMs()
        {
            var subtracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long)subtracted.TotalMilliseconds;
        }

        /// <summary>
        /// Convert Unix milliseconds into DateTime
        /// </summary>
        public static DateTime ConvertToTime(long timeInMs)
        {
            return UnixBase.AddMilliseconds(timeInMs);
        }
    }
    internal class UnixDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
           ref Utf8JsonReader reader,
           Type typeToConvert,
           JsonSerializerOptions options)
        {
            return BinanceTime.ConvertToTime(reader.GetInt64());

        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options)
        {
            var subtracted = dateTimeValue.Subtract(BinanceTime.UnixBase);
            writer.WriteRawValue(subtracted.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }
    }
}
