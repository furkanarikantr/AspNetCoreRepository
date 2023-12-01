using ServiceContracts;
using ServiceContracts.Dtos.BuyOrderDtos;
using ServiceContracts.Dtos.SellOrderDtos;
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
        public void CreateBuyOrder_NullBuyOrder()
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
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest() 
            { 
                StockSymbol = "MSFT", 
                StockName = "Microsoft", 
                DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"), 
                Price = 1, 
                Quantity = 1 
            };

            BuyOrderResponse buyOrderResponseFromCreate = _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderId);
        }

        #endregion

        #region CreateSellOrder
        [Fact]
        public void CreateSellOrder_NullSellOrder()
        {
            SellOrderRequest? buyOrderRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _stocksService.CreateSellOrder(buyOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        public void CreateSellOrder_QuantityIsLessThanMinimum(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Quantity = sellOrderQuantity,
                Price = 1,
                StockName = "Microsoft",
                StockSymbol = "MSFT",
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(1001)]
        public void CreateSellOrder_QuantityIsGreaterThanMaximum(uint sellOrderQuantity)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Quantity = sellOrderQuantity,
                Price = 1,
                StockName = "Microsoft",
                StockSymbol = "MSFT",
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(0)]
        public void CreateSellOrder_PriceIsLessThanMinimum(double sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Quantity = 1,
                Price = sellOrderPrice,
                StockName = "Microsoft",
                StockSymbol = "MSFT",
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Theory]
        [InlineData(1001)]
        public void CreateSellOrder_PriceIsGreaterThanMaximum(double sellOrderPrice)
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Quantity = 1,
                Price = sellOrderPrice,
                StockName = "Microsoft",
                StockSymbol = "MSFT",
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_StockSymbolIsNull()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                Quantity = 1,
                Price = 1,
                StockName = "Microsoft",
                StockSymbol = null,
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_DateOfOrderIsLessThanYear200()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest() 
            { 
                StockSymbol = "MSFT", 
                StockName = "Microsoft", 
                DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"), 
                Price = 1, 
                Quantity = 1 
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_ValidData_ToBeSuccessful()
        {
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2004-12-31"),
                Price = 1,
                Quantity = 1
            };

            SellOrderResponse sellOrderResponseFromCreate = _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotEqual(Guid.Empty, sellOrderResponseFromCreate.SellOrderId);
        }
        #endregion

        #region GetBuyOrders
        [Fact]
        public void GetBuyOrders_DefaultList()
        {
            List<BuyOrderResponse> buyOrderResponseFromGet = _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponseFromGet);
        }

        [Fact]
        public void GetBuyOrders_WithFewBuyOrders()
        {
            BuyOrderRequest? buyOrderRequest1 = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
                Price = 1,
                Quantity = 1
            };
            BuyOrderRequest? buyOrderRequest2 = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
                Price = 1,
                Quantity = 12
            };

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>() { buyOrderRequest1, buyOrderRequest2 };
            List<BuyOrderResponse> buyOrderResponsesFromAdd = new List<BuyOrderResponse>();

            foreach (BuyOrderRequest buyOrderRequest in buyOrderRequests)
            {
                buyOrderResponsesFromAdd.Add(_stocksService.CreateBuyOrder(buyOrderRequest));
            }

            List<BuyOrderResponse> buyOrderResponsesFromGet = _stocksService.GetBuyOrders();

            foreach (BuyOrderResponse buyOrderResponse in buyOrderResponsesFromAdd)
            {
                Assert.Contains(buyOrderResponse, buyOrderResponsesFromGet);
            }
        }
        #endregion

        #region GetSellOrders
        [Fact]
        public void GetSellOrders_DefaultList()
        {
            List<SellOrderResponse> sellOrderResponseFromGet = _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponseFromGet);
        }

        [Fact]
        public void GetSellOrders_WithFewBuyOrders()
        {
            SellOrderRequest? sellOrderRequest1 = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
                Price = 1,
                Quantity = 1
            };
            SellOrderRequest? sellOrderRequest2 = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = Convert.ToDateTime("2024-12-31"),
                Price = 1,
                Quantity = 1
            };

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>() { sellOrderRequest1, sellOrderRequest2 };
            List<SellOrderResponse> sellOrderResponsesFromAdd = new List<SellOrderResponse>();

            foreach(SellOrderRequest sellOrderRequest in sellOrderRequests)
            {
                sellOrderResponsesFromAdd.Add(_stocksService.CreateSellOrder(sellOrderRequest));
            }

            List<SellOrderResponse> sellOrderResponsesFromGet = _stocksService.GetSellOrders();

            foreach(SellOrderResponse sellOrderResponse in sellOrderResponsesFromAdd)
            {
                Assert.Contains(sellOrderResponse, sellOrderResponsesFromGet);
            }
        }
        #endregion
    }
}