using System.IO;
using System.Linq;

namespace SAHomeloans.SAHL_VS_WebJS.Core
{
    internal class RootDirectoryHelper
    {
        const string appDirectory = @"\app\";

        internal string GetSourceRoot(string projectFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(projectFolder);
            return ScanParent(dir,"");
        }

        private string ScanParent(DirectoryInfo dir,string path)
        {
            if (dir.Parent != null)
            {
                DirectoryInfo[] directories = dir.Parent.GetDirectories(".hg");
                path += "..\\";
                if (directories.Count() == 0)
                {
                    return ScanParent(dir.Parent,path);
                }
                return path;
            }
            return "";
        }

        internal static string GetAppRoot(string location)
        {
            location = location.ToLower();
            if (location.Contains(appDirectory))
            {
                int index = location.IndexOf(appDirectory);
                string result = location.Substring(index).Replace("\\","/");
                return string.Format(".{0}",result);
            }
            return "";
        }
    }
}