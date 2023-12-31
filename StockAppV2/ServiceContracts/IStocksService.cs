﻿using ServiceContracts.Dtos.BuyOrderDtos;
using ServiceContracts.Dtos.SellOrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IStocksService
    {
        BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest);
        List<BuyOrderResponse> GetBuyOrders();

        SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest);
        List<SellOrderResponse> GetSellOrders();
    }
}
