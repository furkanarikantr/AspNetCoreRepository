﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderId { get; set; }

        [Required(ErrorMessage = "Stock Symbol can't be null or empty!")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty!")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000, ErrorMessage = "You can sell maximum of 1000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; }

        [Range(1, 1000, ErrorMessage = "The maximum price of stock is 1000. Minimum is 1")]
        public double Price { get; set; }
    }
}
