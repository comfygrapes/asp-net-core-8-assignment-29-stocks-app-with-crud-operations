using ServiceContracts;
using ServiceContracts.DTOs;
using Services;

namespace StocksAppTests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;

        public StocksServiceTests()
        {
            _stocksService = new StocksService();
        }

        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullRequest()
        {
            BuyOrderRequest? buy_order_request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_BelowMinimumQuantity()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                Quantity = 0,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_AboveMaximumQuantity()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                Quantity = 100001,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                // Act
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_BelowMinimumPrice()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                Price = 0,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_AboveMaximumPrice()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                Price = 10001,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                StockSymbol = null,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => 
            {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_DateTimeOutOfRange()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buy_order_request);
            });
        }

        [Fact]
        public async Task CreateBuyOrder_DetailsValid()
        {
            BuyOrderRequest? buy_order_request = new()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 200,
                Quantity = 50,
                DateAndTimeOfOrder = DateTime.Now
            };

            var buy_order_response_from_create = await _stocksService.CreateBuyOrder(buy_order_request);

            Assert.NotEqual(buy_order_response_from_create.BuyOrderId, Guid.Empty);
        }

        #endregion

        #region CreateSellOrder

        [Fact]
        public async Task CreateSellOrder_NullRequest()
        {
            SellOrderRequest? sell_order_request = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_BelowMinimumQuantity()
        {
            SellOrderRequest? sell_order_request = new()
            {
                Quantity = 0,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_AboveMaximumQuantity()
        {
            SellOrderRequest? sell_order_request = new()
            {
                Quantity = 100001,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                // Act
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_BelowMinimumPrice()
        {
            SellOrderRequest? sell_order_request = new()
            {
                Price = 0,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_AboveMaximumPrice()
        {
            SellOrderRequest? sell_order_request = new()
            {
                Price = 10001,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () => {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest? sell_order_request = new()
            {
                StockSymbol = null,
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_DateTimeOutOfRange()
        {
            SellOrderRequest? sell_order_request = new()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sell_order_request);
            });
        }

        [Fact]
        public async Task CreateSellOrder_DetailsValid()
        {
            SellOrderRequest? sell_order_request = new()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 200,
                Quantity = 50,
                DateAndTimeOfOrder = DateTime.Now
            };

            var sell_order_response_from_create = await _stocksService.CreateSellOrder(sell_order_request);

            Assert.NotEqual(sell_order_response_from_create.SellOrderId, Guid.Empty);
        }

        #endregion

        #region GetAllBuyOrders

        [Fact]
        public async Task GetAllBuyOrders_EmptyList()
        {
            var buy_order_responses_from_get_all = await _stocksService.GetAllBuyOrders();

            Assert.Empty(buy_order_responses_from_get_all);
        }

        [Fact]
        public async Task GetAllBuyOrders_CorrectOrders()
        {
            BuyOrderRequest? buy_order_request_1 = new()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 413.64,
                Quantity = 50,
                DateAndTimeOfOrder = DateTime.Now
            };

            BuyOrderRequest? buy_order_request_2 = new()
            {
                StockSymbol = "NVDA",
                StockName = "Nvidia",
                Price = 791.12,
                Quantity = 250,
                DateAndTimeOfOrder = DateTime.Now
            };

            var buy_order_requests = new List<BuyOrderRequest>() { buy_order_request_1, buy_order_request_2 };
            var buy_order_responses = new List<BuyOrderResponse>();

            foreach (var buy_order_request in buy_order_requests)
            {
                buy_order_responses.Add(await _stocksService.CreateBuyOrder(buy_order_request));
            }

            var buy_order_responses_from_get_all = await _stocksService.GetAllBuyOrders();

            Assert.Equal(buy_order_responses.Count, buy_order_responses_from_get_all.Count);

            for (int i = 0; i < buy_order_responses_from_get_all.Count; i++)
            {
                Assert.Equal(buy_order_responses[i], buy_order_responses_from_get_all[i]);
            }
        }

        #endregion

        #region GetAllSellOrders

        [Fact]
        public async Task GetAllSellOrders_EmptyList()
        {
            var sell_order_responses_from_get_all = await _stocksService.GetAllSellOrders();

            Assert.Empty(sell_order_responses_from_get_all);
        }

        [Fact]
        public async Task GetAllSellOrders_CorrectOrders()
        {
            SellOrderRequest? sell_order_request_1 = new()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 413.64,
                Quantity = 50,
                DateAndTimeOfOrder = DateTime.Now
            };

            SellOrderRequest? sell_order_request_2 = new()
            {
                StockSymbol = "NVDA",
                StockName = "Nvidia",
                Price = 791.12,
                Quantity = 250,
                DateAndTimeOfOrder = DateTime.Now
            };

            var sell_order_requests = new List<SellOrderRequest>() { sell_order_request_1, sell_order_request_2 };
            var sell_order_responses = new List<SellOrderResponse>();

            foreach (var sell_order_request in sell_order_requests)
            {
                sell_order_responses.Add(await _stocksService.CreateSellOrder(sell_order_request));
            }

            var sell_order_responses_from_get_all = await _stocksService.GetAllSellOrders();

            Assert.Equal(sell_order_responses.Count, sell_order_responses_from_get_all.Count);

            for (int i = 0; i < sell_order_responses_from_get_all.Count; i++)
            {
                Assert.Equal(sell_order_responses[i], sell_order_responses_from_get_all[i]);
            }
        }                                                                                                                                                                                                                                                                                                                                                     

        #endregion
    }
}