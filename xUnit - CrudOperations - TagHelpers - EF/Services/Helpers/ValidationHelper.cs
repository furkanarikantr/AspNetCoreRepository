using ServiceContracts.DTOs.PersonDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        //Model Validation
        internal static void ModelValidation(object obj) //obj nesnesi validate işlemi yapılacak model/entity.
        {
            //Validation işlemini gerçekleştirecek sınıf oluşturuluyor ve model/entity içine ekleniyor.
            ValidationContext validationContext = new ValidationContext(obj);
            //Validation sonuçlarını tutulacak liste oluşturuluyor.
            List<ValidationResult> validationResults = new List<ValidationResult>();
            //Validation işlemi gerçekleştirilmeye başlanıyor. Model/entity - gerçekleştirecek sınıf - sonuçları tutacak liste - true değeri ?
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            //İşlem sonucu true/false değerlerine göre başarılı ve başarısız olduğu anlaşılıp hata fırlattırılıyor.
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
