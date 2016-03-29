using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Validation
{
    public class ValidateCommand : IValidateCommand
    {
        private readonly IValidateStrategy validationStrategy;
        private readonly ITypeMetaDataLookup lookupTypes;

        public ValidateCommand(ITypeMetaDataLookup lookupsForTypes, IValidateStrategy strategy)
        {
            lookupTypes = lookupsForTypes;
            validationStrategy = strategy;
        }

        public virtual IEnumerable<ValidationResult> Validate(object objectToValidate)
        {
            return GetValidationResultsForValidatableObject(objectToValidate, null);
        }

        protected virtual IEnumerable<ValidationResult> CreateGetMembersQuery(IEnumerable members, PropertyInfo containingPropertyInfo)
        {
            return members
                .Cast<object>()
                .SelectMany(a => GetValidationResultsForValidatableObject(a, containingPropertyInfo)) //note: recursion
                .AsParallel();
        }

        protected virtual IList<ValidationResult> GetValidationResultsForValidatableObject(object validatableObject, PropertyInfo containingPropertyInfo)
        {
            var rawResults = GetValidationResults(validatableObject);
            var results = IncludeContainingPropertyNameInErrorMessage(rawResults, containingPropertyInfo);

            var members = GetCompositeMembers(validatableObject);
            foreach (var member in members)
            {
                var memberChildrenResults = CreateGetMembersQuery(member.Members, member.ContainingPropertyInfo); //get query for getting (child) members of this member
                foreach (var result in memberChildrenResults)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        protected virtual IList<ValidationResult> IncludeContainingPropertyNameInErrorMessage(IList<ValidationResult> results, PropertyInfo containingPropertyInfo)
        {
            foreach (var item in results)
            {
                item.ErrorMessage = string.Format("{0} {{in: {1}}}", item.ErrorMessage, containingPropertyInfo == null ? "root" : containingPropertyInfo.Name);
            }
            return results;
        }

        protected virtual IList<ValidationResult> GetValidationResults(object validatableObject)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(validatableObject, null, null);
            Validator.TryValidateObject(validatableObject, context, results, true);
            return results;
        }

        internal virtual IEnumerable<MemberInfo> GetCompositeMembers(object validatableObject)
        {
            foreach (PropertyInfo a in this.lookupTypes.GetPropertyInfo(validatableObject.GetType()))
            {
                if (a.GetIndexParameters().Any())
                {
                    //we ignore indexer properties because we cannot determine the key values and their types to retrieve them
                    continue;
                }
                var objectToCheck = a.GetValue(validatableObject);
                var validatableItems = this.validationStrategy.GetValidatableItems(objectToCheck);
                yield return new MemberInfo(a, validatableItems);
            }
        }
    }
}