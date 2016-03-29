using System;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Core.Validation
{
    public interface ITypeMetaDataLookup
    {
        IEnumerable<PropertyInfo> GetPropertyInfo(Type givenType);

        ValidationActionType GetObjectTypeForGivenType(Type givenType);
    }
}