using SAHL.VSExtensions.Interfaces.Reflection;

namespace SAHL.VSExtensions.Core
{
    public class TypeInfo : ITypeInfo
    {
        public string Name
        {
            get;
            protected set;
        }

        public string FullName
        {
            get;
            protected set;
        }

        public TypeInfo(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
        }
    }
}