namespace SAHL.Tools.RestServiceRoutenator
{
    public interface INamespaceProvider
    {
        string Namespace { get;  }
        string Prefix { get; }
    }
}
