namespace SAHL.Tools.Capitec.CSJsonifier
{
    public interface IFileManagement
    {
        void Save(string location, string subFolder, string assemblyName, string type, string content);
    }
}