using System;
using System.Xml.Linq;

namespace SAHL.Core.Events
{
    public interface ITypeInstantiator
    {
        object Create(Type typeOfValue, XElement valueForType);
    }
}