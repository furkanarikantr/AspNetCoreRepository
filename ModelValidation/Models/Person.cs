using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidations.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidations.Models
{
    public class Person : IValidatableObject
    {
        [Display(Name = "Person Name")]
        [Required(ErrorMessage ="{0} can't be empty!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long!")]
        [RegularExpression("^[A-Za-z .]*$", ErrorMessage = "{0} should contain only alphabets, space and dot (.)!")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        [EmailAddress(ErrorMessage = "{0} should be a proper email address!")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "{0} should contain 10 digits!")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "{0} can't be empty!")]
        public string? Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} can't be empty!")]
        [Compare("Password", ErrorMessage = "{0} and {1} don't match!")]
        public string? ConfirmPassword { get; set; }

        [Range(0, 999, ErrorMessage = "{0} should be between ${1} and ${2} ")]
        public double? Price { get; set; }

        //[MinYearValidator(ErrorMessage = "Date of Birth should not be newer than Jan 01, 2000!")]
        //[MinYearValidator(2005, ErrorMessage = "Date of Birth should not be never than Jan 01, {0}!")]
        [MinYearValidator(2002)]
        //[BindNever]
        public DateTime? DateOfBirth { get; set; }

        public DateTime? FromDate { get; set; }

        [DateRangeValidator("FromDate", ErrorMessage = "'From Date' should be older than or equel to 'To Date'")]
        public DateTime? ToDate { get; set; }

        public int? Age { get; set; }

        public override string ToString()
        {
            return $"Person Object => Person Name : {PersonName}, Email : {Email}, Password : {Password}, Confirm Password : {ConfirmPassword}, Price : {Price}, Date of Birth : {DateOfBirth}";
        }

        //IValidatableObject implement
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth.HasValue == false && Age.HasValue == false)
            {
                yield return new ValidationResult("Either of Date of Birth or Age must be supplied!", new[] {nameof(Age)});
            }
        }
    }
}
