using System;

namespace SAHL.Core.Services.Attributes
{
    public class DecorationOrderAttribute : Attribute
    {
        public int Index { get; protected set; }

        public DecorationOrderAttribute(int index)
        {
            this.Index = index;
        }
    }
}