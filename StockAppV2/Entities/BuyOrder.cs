using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderId { get; set; }

        [Required(ErrorMessage = "Stock Symbol can't be null or empty!")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty!")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000, ErrorMessage = "You can buy maximum of 1000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; }

        [Range(1, 1000, ErrorMessage = "The maximum price of stock is 1000. Minimum is 1.")]
        public double Price { get; set; }
    }
}