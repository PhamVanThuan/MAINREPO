using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Validation
{
    public class ValidatableCommand
    {
        public enum ValidationResultOrder
        {
            Unspecified,
            PropertyOrder,
        }

        public ValidationResultOrder ResultOrder { get; set; }
        private readonly ValidateStrategy validationStrategy;
        private readonly ITypeMetaDataLookup lookupTypes = new TypeMetaDataLookup();

        public ValidatableCommand(ValidationResultOrder validationResultOrder = ValidationResultOrder.Unspecified)
        {
            ResultOrder = validationResultOrder;
            validationStrategy =  new ValidateStrategy(lookupTypes);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return GetValidationResultsForValidatableObject(this);
        }

        private IEnumerable<ValidationResult> GetValidationResultsForValidatableObject(object validatableObject)
        {
            var results = GetValidationResults(validatableObject);
            var firstLevelChildren = GetCompositeChildren(validatableObject);

            var children = CreateGetChildrenQuery(firstLevelChildren).ToList();

            foreach (var item in children)
            {
                results.Add(item);
            }

            return results;
        }

        private IEnumerable<ValidationResult> CreateGetChildrenQuery(IEnumerable<object> children)
        {
            var query = children
                .SelectMany(GetValidationResultsForValidatableObject) //note: recursion
                .AsParallel()
                ;

            return SetResultOrder(query);
        }

        private IEnumerable<ValidationResult> SetResultOrder(ParallelQuery<ValidationResult> query)
        {
            switch (ResultOrder)
            {
                case ValidationResultOrder.PropertyOrder:
                    return query.AsOrdered();
                case ValidationResultOrder.Unspecified:
                    return query;
                default:
                    return query;
            }
        }

        private static ICollection<ValidationResult> GetValidationResults(object validatableObject)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(validatableObject, null, null);
            Validator.TryValidateObject(validatableObject, context, results, true);
            return results;
        }

        private IEnumerable<object> GetCompositeChildren(object validatableObject)
        {
            return GetPropertiesForObject(validatableObject)
                .Select(p => p.GetValue(validatableObject))
                .Select(x => validationStrategy.GetValidatableItems(x))
                .SelectMany(x => x);
        }

        private IEnumerable<PropertyInfo> GetPropertiesForObject(object validatableObject)
        {
            var typeOfGivenObject = validatableObject.GetType();
            return lookupTypes.GetPropertyInfo(typeOfGivenObject);
        }
    }
}