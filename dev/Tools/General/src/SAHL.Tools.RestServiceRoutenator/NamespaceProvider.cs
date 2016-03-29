namespace SAHL.Tools.RestServiceRoutenator
{
    public class NamespaceProvider : INamespaceProvider
    {
        public string Namespace
        {
            get;
            protected set;
        }

        public string Prefix
        {
            get;
            protected set;
        }

        public NamespaceProvider(string nameSpace,string prefix)
        {
            this.Namespace = nameSpace;
            this.Prefix = prefix;
        }
    }
}
