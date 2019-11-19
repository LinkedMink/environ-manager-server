using System.ComponentModel.DataAnnotations;
using System.Reflection;
using LinkedMink.Web.Infastructure.Resources;

namespace LinkedMink.Web.Infastructure.ValidationAttributes
{
    public class RequiredIfNewAttribute : ValidationAttribute
    {
        public RequiredIfNewAttribute(string idAttribute)
        {
            _idAttribute = idAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo property = validationContext.ObjectType.GetProperty(_idAttribute);
            MethodInfo getMethod = property?.GetGetMethod();

            int? id = (int?) getMethod?.Invoke(validationContext.ObjectInstance, null);
            
            if (id > 0)
            {
                return ValidationResult.Success;
            }

            if (value == null)
            {
                return new ValidationResult(string.Format(Errors.Required, validationContext.MemberName));
            }

            string stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(string.Format(Errors.Required, validationContext.MemberName));
            }

            return ValidationResult.Success;
        }

        private readonly string _idAttribute;
    }
}
