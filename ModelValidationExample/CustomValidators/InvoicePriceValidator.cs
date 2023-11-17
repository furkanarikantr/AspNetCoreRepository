using ModelExample.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelExample.CustomValidators
{
    public class InvoicePriceValidator : ValidationAttribute
    {
        public string DefaultErrorMessage { get; set; } = "Invoice Price should be equal to the total cost of all products (i.e. {0}) in the order.";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //Adım 1 : "InvoicePrice" değeri var mı yok mu diye kontrol et!
            if (value != null)
            {
                //Adım 2 : Order'daki Products'ların bilgilerini al!
                PropertyInfo? OtherProperty = validationContext.ObjectType.GetProperty(nameof(Order.Products));

                if (OtherProperty != null)
                {
                    List<Product> products = (List<Product>)OtherProperty.GetValue(validationContext.ObjectInstance)!;

                    //Adım 3 : Product'lardaki totalPrice'ı hesapla!
                    double? totalPrice = 0;
                    foreach (Product product in products)
                    {
                        totalPrice += product.Price * product.Quantity;
                    }

                    //Adım 4 : InvoicePrice bilgisini tut.
                    double actualPrice = (double)value;

                    if (totalPrice > 0)
                    {
                        if (totalPrice != actualPrice)
                        {
                            return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, totalPrice), new string[] { nameof(validationContext.MemberName) });
                        }
                    }
                    else
                    {
                        return new ValidationResult("No products found to validate invoice price", new string[] { nameof(validationContext.MemberName) });
                    }

                    return ValidationResult.Success;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
