using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SAHL.Core.Validation
{
    internal class MemberInfo
    {
        internal PropertyInfo ContainingPropertyInfo { get; private set; }

        internal IEnumerable Members { get; private set; }

        internal MemberInfo(PropertyInfo containingPropertyInfo, IEnumerable members)
        {
            ContainingPropertyInfo = containingPropertyInfo;
            Members = members;
        }
    }
}