using System.ComponentModel.DataAnnotations;

namespace ModelExample.CustomValidators
{
    public class MinimumDateValidator : ValidationAttribute
    {
        public string DefaultErrorMessage { get; set; } = "Order date should be greater than or equal to {0}";
        public DateTime _minimumDate { get; set; }

        public MinimumDateValidator(string minimumDate)
        {
            _minimumDate = Convert.ToDateTime(minimumDate);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime orderDate = (DateTime)value;

                if (orderDate > _minimumDate)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, _minimumDate.ToString("yyyy-MM-dd")), new string[] { nameof(validationContext.MemberName) });
                }

                return ValidationResult.Success;
            }
            return null;
        }
    }
}
