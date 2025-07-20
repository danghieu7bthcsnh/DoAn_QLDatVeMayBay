using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QLDatVeMayBay.Attributes
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _desiredValue;

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            _propertyName = propertyName;
            _desiredValue = desiredValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_propertyName);
            if (property == null)
                return new ValidationResult($"Không tìm thấy thuộc tính {_propertyName}");

            var propertyValue = property.GetValue(validationContext.ObjectInstance);

            if (propertyValue?.ToString() == _desiredValue.ToString())
            {
                if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)))
                    return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
