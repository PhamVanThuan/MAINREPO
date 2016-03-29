namespace SAHL.Tools.ObjectModelGenerator.Lib.Templates
{
    public class BusinessModelDescriptionProperty
    {
        public string PropertyName { get; protected set; }

        public string TypeName { get; protected set; }

        public bool IsPrimaryKey { get; protected set; }

        public bool IsIdentitySeed { get; protected set; }

        public bool IsComputed { get; protected set; }
    }
}