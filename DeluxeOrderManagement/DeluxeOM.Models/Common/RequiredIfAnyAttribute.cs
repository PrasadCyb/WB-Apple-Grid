﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeluxeOM.Models.Common
{
    public class RequiredIfAnyAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string[] _otherProperties;
        private const string _DefaultErrorMessage = "The {0} field is required";

        public RequiredIfAnyAttribute(params string[] otherProperties)
        {
            if (otherProperties.Length == 0) // would not make sense
            {
                throw new ArgumentException("At least one other property name must be provided");
            }
            _otherProperties = otherProperties;
            ErrorMessage = _DefaultErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) // no point checking if it has a value
            {
                int i = 0;
                foreach (string property in _otherProperties)
                {
                    i++;
                    var propertyName = validationContext.ObjectType.GetProperty(property);
                    var propertyValue = propertyName.GetValue(validationContext.ObjectInstance, null);
                    if (propertyValue != null)
                    {
                        break;
                    }
                    if (propertyValue == null && _otherProperties.Count()==i)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "requiredifany",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
            };
        //pass a comma separated list of the other propeties
            rule.ValidationParameters.Add("otherproperties", string.Join(",", _otherProperties));
            yield return rule;
        }
    }
}
