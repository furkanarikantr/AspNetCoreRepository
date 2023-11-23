﻿namespace StocksApplication.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}
