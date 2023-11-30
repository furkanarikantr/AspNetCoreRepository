using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Dtos.BuyOrderDtos
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderId { get; set; }

        public string StockSymbol { get; set; }

        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is not BuyOrderResponse)
            {
                return false;
            }

            BuyOrderResponse other = (BuyOrderResponse)obj;
            return BuyOrderId == other.BuyOrderId && StockSymbol == other.StockSymbol && StockName == other.StockName && DateAndTimeOfOrder == other.DateAndTimeOfOrder && Quantity == other.Quantity && Price == other.Price;
        }

        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderId}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }
    }

    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse() { BuyOrderId = buyOrder.BuyOrderId, StockSymbol = buyOrder.StockSymbol, StockName = buyOrder.StockName, Price = buyOrder.Price, DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder, Quantity = buyOrder.Quantity, TradeAmount = buyOrder.Price * buyOrder.Quantity };
        }
    }
}
