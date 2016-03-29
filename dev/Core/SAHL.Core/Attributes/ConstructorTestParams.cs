using System;

namespace SAHL.Core.Attributes
{
    public class ConstructorTestParams : Attribute
    {
        public ConstructorTestParams(string constructorArgs)
        {
            this.ConstructorArgs = constructorArgs;
        }

        public string ConstructorArgs { get; private set; }
    }
}