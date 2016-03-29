namespace SAHL.Tools.RestServiceRoutenator
{
    public interface IFileManagement
    {
        void Save(string location, string subFolder, string assemblyName, string type, string content);
    }
}