using ModelExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelExample.Models
{
    public class Order
    {
        [Display(Name = "Order No")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        public int? OrderNo { get; set; }

        [Display(Name = "Order Date")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [MinimumDateValidator("2000-01-01", ErrorMessage = "Order Date should be greater than or equal to 2000!")] //CustomValidator
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Invoice Price")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [Range(1, double.MaxValue, ErrorMessage = "{0} should be between a valid number!")]
        [InvoicePriceValidator] //CustomValidator
        public double? InvoicePrice { get; set; }

        [ProductsListValidator] //CustomValidator
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
