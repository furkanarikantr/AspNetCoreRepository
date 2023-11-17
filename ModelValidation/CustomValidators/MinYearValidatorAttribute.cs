using System.ComponentModel.DataAnnotations;

namespace ModelValidations.CustomValidators
{
    //ValidationAttribute, .Net içinde bulunan, doğrulama yapılandırmasına sahip temel sınıftır. Bu sınıf ile özel doğrulama yapılır.
    public class MinYearValidatorAttribute : ValidationAttribute
    {
        public int MinYear { get; set; } = 2000;
        public string DefaultErrorMessage { get; set; } = "Year should not be less than {0}!"; 
        public MinYearValidatorAttribute(int _MinYear)
        {
            MinYear = _MinYear;  
        }

        //IsValid, doğrulama mantığını içerir, doğrulama işlemi için çağırıldığında çalışır.
        //value, doğrulama işlemine tabi tutulan özelliği temsil eder.
        //ValidationContext, doğrulama bağlamını temsil eder ve bu bağlam içinde kullanıcı tarafından sağlanan diğer verilere erişim sağlar.
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null) 
            {
                DateTime date = (DateTime)value;
                if (date.Year >= MinYear)
                {
                    //return new ValidationResult("Minimum year allowed is 2000!");
                    //return new ValidationResult(string.Format(ErrorMessage, MinYear));
                    return new ValidationResult(string.Format(DefaultErrorMessage, MinYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return null;
        }
    }
}
