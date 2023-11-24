﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StockAppV2.Models;

namespace StockAppV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubService _finnhubService;

        public HomeController(IOptions<TradingOptions> tradingOptions,IConfiguration configuration ,IFinnhubService finnhubService)
        {
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _finnhubService = finnhubService;
        }

        [Route("/")]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
            {
                _tradingOptions.DefaultStockSymbol = "MSFT";
            }

            Dictionary<string, object>? companyProfileDictionary = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);
            Dictionary<string, object>? stockQuoteDictionary = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                StockSymbol = _tradingOptions.DefaultStockSymbol,
            };

            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]),
                    StockName = Convert.ToString(companyProfileDictionary["name"]),
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString())
                };
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}
