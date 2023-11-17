using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidations.CustomValidators
{
    public class DateRangeValidatorAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; set; }

        public DateRangeValidatorAttribute(string _OtherPropertyName)
        {
            OtherPropertyName = _OtherPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                //Get to_date
                DateTime to_date = Convert.ToDateTime(value);

                //Get from_date
                PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);

                if (otherProperty != null)
                {
                    DateTime from_date = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));

                    if (from_date > to_date)
                    {
                        return new ValidationResult(ErrorMessage, new string[]
                        {
                        OtherPropertyName, validationContext.MemberName
                        });
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }

                return null;
            }

            return null;
        }
    }
}
