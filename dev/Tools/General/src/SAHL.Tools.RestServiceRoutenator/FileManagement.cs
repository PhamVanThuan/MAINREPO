using System.IO;

namespace SAHL.Tools.RestServiceRoutenator
{
    public class FileManagement : IFileManagement
    {
        public void Save(string location, string subFolder, string assemblyName, string type, string content)
        {
            string fileName = string.Format("{0}.{1}.js", assemblyName, type);
            string path = Path.Combine(location, subFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, fileName);
            File.WriteAllText(filePath, content);
        }
    }
}