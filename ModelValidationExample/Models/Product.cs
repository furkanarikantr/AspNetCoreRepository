using System.ComponentModel.DataAnnotations;

namespace ModelExample.Models
{
    public class Product
    {
        [Display(Name = "Product Code")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} should be between a valid number!")]
        public int? ProductCode { get; set; }

        [Display(Name = "Product Price")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [Range(1, double.MaxValue, ErrorMessage = "{0} should be between a valid number!")]
        public double? Price { get; set; }

        [Display(Name = "Product Quantity")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} should be between a valid number!")]
        public int? Quantity { get; set; }
    }
}
