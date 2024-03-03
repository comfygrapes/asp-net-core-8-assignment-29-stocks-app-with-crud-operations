using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Options;
using ServiceContracts;
using StocksApp.ViewModels;
using ServiceContracts.DTOs;

namespace StocksApp.Controllers
{
    [Route("[Controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;

        public TradeController(IFinnhubService myService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IStocksService stocksService)
        {
            _finnhubService = myService;
            _tradingOptions = tradingOptions;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [HttpGet]
        [Route("/")]
        [Route("[Action]")]
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
                Quantity = 100,
            };

            ViewData["ApiKey"] = _configuration["FinnhubToken"];
            return View(stockTrade);
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> Orders()
        {
            var buyOrders = await _stocksService.GetAllBuyOrders();
            var sellOrders = await _stocksService.GetAllSellOrders();

            var orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders,
            };

            return View(orders);
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            ModelState.Remove("DateAndTimeOfOrder");
            if (ModelState.IsValid)
            {
                buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

                await _stocksService.CreateBuyOrder(buyOrderRequest);

                return RedirectToAction("Orders", "Trade");
            }

            return RedirectToAction("Index", "Trade");
        }


        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            ModelState.Remove("DateAndTimeOfOrder");
            if (ModelState.IsValid)
            {
                sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

                await _stocksService.CreateSellOrder(sellOrderRequest);

                return RedirectToAction("Orders", "Trade"); 
            }

            return RedirectToAction("Index", "Trade");
        }
    }
}
