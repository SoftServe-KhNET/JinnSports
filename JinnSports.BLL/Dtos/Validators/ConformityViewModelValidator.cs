using System.ComponentModel.DataAnnotations;

namespace JinnSports.BLL.Dtos.Validators
{
    public class ConformityViewModelValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conformity = (ConformityViewModel)validationContext.ObjectInstance;

            if (conformity.ConformityId == 0 && conformity.ExistedName == null)
            {
                return new ValidationResult("You should chose something or write your variant :)");
            }            

            if (conformity.ConformityId != 0 && conformity.ExistedName != null)
            {
                return new ValidationResult("Chose only one option please!");
            }            

            return ValidationResult.Success;
        }
    }
}
