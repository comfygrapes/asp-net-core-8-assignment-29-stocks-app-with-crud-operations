using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders = new List<BuyOrder>();
        private readonly List<SellOrder> _sellOrders = new List<SellOrder>();

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));
            ValidationHelper.ValidateModel(buyOrderRequest);

            var buyOrder = buyOrderRequest.ToBuyOrder(Guid.NewGuid());

            _buyOrders.Add(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));
            ValidationHelper.ValidateModel(sellOrderRequest);

            var sellOrder = sellOrderRequest.ToSellOrder(Guid.NewGuid());

            _sellOrders.Add(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            var allBuyOrders = _buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();

            return allBuyOrders;
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            var allSellOrders = _sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();

            return allSellOrders;
        }
    }
}
