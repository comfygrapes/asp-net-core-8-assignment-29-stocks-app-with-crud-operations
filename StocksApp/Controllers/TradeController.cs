using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Entities;
using StocksApp.Options;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly FinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;

        public TradeController(FinnhubService myService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnhubService = myService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            
            var stock = await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);
            var company = await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
                StockName = company?["name"].ToString(),
                Price = Convert.ToDouble(stock?["c"].ToString()),
                Quantity = 0,
            };

            ViewData["ApiKey"] = _configuration["FinnhubToken"];
            return View(stockTrade);
        }
    }
}
