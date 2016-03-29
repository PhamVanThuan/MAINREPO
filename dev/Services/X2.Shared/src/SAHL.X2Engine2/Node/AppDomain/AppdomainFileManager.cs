using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.X2Engine2.Node.AppDomain
{
    public class AppDomainFileManager : SAHL.X2Engine2.Node.AppDomain.IAppDomainFileManager
    {
        private List<string> validExtensions;
        private IFileSystem fileSystem;

        public AppDomainFileManager(IFileSystem fileSystem)
        {
            validExtensions = new List<string>(new string[] { ".dll", ".pdb", ".xml" });
            this.fileSystem = fileSystem;
        }

        public void CopyFile(string sourcePath, string destPath)
        {
            if (!fileSystem.File.Exists(destPath) && validExtensions.Contains(fileSystem.Path.GetExtension(destPath)))
            {
                fileSystem.File.Copy(sourcePath, destPath);
            }
        }

        public List<string> GetInstalledNuGetFiles(string nugetPackagePath)
        {
            List<string> Files = new List<string>();
            string[] directories = fileSystem.Directory.GetDirectories(nugetPackagePath, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < directories.Length; i++)
            {
                string folderToGetDllsFrom = directories[i];
                if (!fileSystem.Directory.Exists((directories[i] + "\\lib")))
                {
                    continue;
                }
                // look for a lib folder
                List<string> subFolders = new List<string>(fileSystem.Directory.GetDirectories(directories[i] + "\\lib"));
                var orderedSub = subFolders.Where(x => x.Contains("lib\\net") && !x.Contains("netcore")).OrderByDescending(y => y);
                // look for the higest version and only extract that
                if (orderedSub.Any())
                {
                    folderToGetDllsFrom = orderedSub.First();
                }
                // copy the files
                string[] files = fileSystem.Directory.GetFiles(folderToGetDllsFrom, "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    if (fileSystem.Path.GetExtension(file) == ".dll")
                    {
                        Files.Add(file);
                    }
                }
            }
            return Files;
        }

        public void WriteFile(byte[] data, string pathToWriteTo)
        {
            string folderPath = fileSystem.Path.GetDirectoryName(pathToWriteTo);
            if (!fileSystem.Directory.Exists(folderPath))
                fileSystem.Directory.CreateDirectory(folderPath);

            if (!fileSystem.File.Exists(pathToWriteTo))
            {
                using (Stream fs = fileSystem.File.Create(pathToWriteTo))// new FileStream(pathToWriteTo, FileMode.Create))
                {
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
            }
        }

        public void WriteConfigFile(string data, string pathToWriteTo)
        {
            string folderPath = fileSystem.Path.GetDirectoryName(pathToWriteTo);
            if (!fileSystem.Directory.Exists(folderPath))
                fileSystem.Directory.CreateDirectory(folderPath);

            var parsedDocument = XDocument.Parse(data);
            parsedDocument.Save(pathToWriteTo);
        }
    }
}