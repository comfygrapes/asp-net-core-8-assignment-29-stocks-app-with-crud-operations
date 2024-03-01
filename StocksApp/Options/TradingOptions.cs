using Microsoft.Extensions.Options;

namespace StocksApp.Options
{
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
    }
}
