using System;
using System.Reflection;

namespace SAHL.Common.Factories
{
    public class LoadArgs : EventArgs
    {
        public string TypeName;
        public string AsmName;
        public ReflectionTypeLoadException ReflectionException;

        public LoadArgs()
            : base()
        {
        }
    }
}