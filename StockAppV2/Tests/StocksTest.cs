using ServiceContracts;
using ServiceContracts.Dtos.BuyOrderDtos;
using Services;

namespace Tests
{
    public class StocksTest
    {
        private readonly IStocksService _stocksService;

        public StocksTest()
        {
            _stocksService = new StocksService();
        }

        #region CreateBuyOrder
        [Fact]
        public void CreateBuyOrder_NullByOrderRequest()
        {
            BuyOrderRequest? buyOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_QuantityIsLessThenMinimum()
        {
            uint lessQuantity = 0;
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() 
            { 
                StockSymbol = "MSFT", 
                StockName = "Microsoft", 
                Price = 1,
                Quantity = lessQuantity
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_QuantityIsGreaterThenMaximum()
        {
            uint lessQuantity = 1001;
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = lessQuantity
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory] 
        [InlineData(0)]
        public void CreateBuyOrder_PriceIsLessThanMinimum(uint buyOrderPrice)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = buyOrderPrice, Quantity = 1 };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(10001)]
        public void CreateBuyOrder_PriceIsGreaterThanMaximum(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = buyOrderQuantity };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = null, Price = 1, Quantity = 1 };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_DateOfOrderIsLessThanYear2000()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("1999-10-10"), Price = 1, Quantity = 1 };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_ValidData()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), Price = 1, Quantity = 1 };

            BuyOrderResponse buyOrderResponseFromCreate = _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderId);
        }

        #endregion
    }
}