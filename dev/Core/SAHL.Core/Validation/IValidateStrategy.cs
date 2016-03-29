using System.Collections.Generic;

namespace SAHL.Core.Validation
{
    public interface IValidateStrategy
    {
        IEnumerable<object> GetValidatableItems(object objectToCheck);
    }
}