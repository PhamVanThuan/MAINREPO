using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Validation
{
    public class ValidateStrategy : IValidateStrategy
    {
        protected readonly Dictionary<ValidationActionType, Func<object, IEnumerable>> validationFunctions = new Dictionary<ValidationActionType, Func<object, IEnumerable>>();
        protected readonly ITypeMetaDataLookup typeMetaDataLookup;

        public ValidateStrategy(ITypeMetaDataLookup lookUpData)
        {
            typeMetaDataLookup = lookUpData;
            SetupStrategyDictionary();
        }

        private void SetupStrategyDictionary()
        {
            validationFunctions.Add(ValidationActionType.IsEnumerable, GetCompositeList);
            validationFunctions.Add(ValidationActionType.IsValidatable, GetCompositeObject);
            validationFunctions.Add(ValidationActionType.IsNotValidatable, DoNothing);
            validationFunctions.Add(ValidationActionType.IsEnumerableOfKeyValuePair, GetValueFromPair);
            validationFunctions.Add(ValidationActionType.Unspecified, ThrowNotImplemented);
        }

        public virtual IEnumerable<object> GetValidatableItems(object objectToCheck)
        {
            if (objectToCheck == null)
            {
                yield break;
            }

            var validtionActionType = typeMetaDataLookup.GetObjectTypeForGivenType(objectToCheck.GetType());

            var validationFunction = validationFunctions[validtionActionType];
            foreach (var item in validationFunction(objectToCheck))
            {
                yield return item;
            }
        }

        protected virtual IEnumerable<object> GetValueFromPair(object objectToGetValueFrom)
        {
            var keyValuePairs = (IEnumerable)objectToGetValueFrom;
            foreach (var keyValuePair in keyValuePairs)
            {
                yield return keyValuePair.GetType().GetProperty("Value").GetValue(keyValuePair);
            }
        }

        protected virtual IEnumerable<object> ThrowNotImplemented(object failingObject)
        {
            throw new NotImplementedException(string.Format("An error has occured where the datatype was unknown in context \n type : {0}", failingObject.GetType()));
        }

        protected virtual IEnumerable GetCompositeList(object collectionObject)
        {
            return (IEnumerable)collectionObject;
        }

        protected virtual IEnumerable<object> GetCompositeObject(object compositeObject)
        {
            yield return compositeObject;
        }

        protected virtual IEnumerable<object> DoNothing(object compositeObject)
        {
            return Enumerable.Empty<object>();
        }
    }
}