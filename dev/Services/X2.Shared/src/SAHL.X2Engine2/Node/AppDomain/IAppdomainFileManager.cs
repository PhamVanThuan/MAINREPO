using System.Collections.Generic;

namespace SAHL.X2Engine2.Node.AppDomain
{
    public interface IAppDomainFileManager
    {
        void CopyFile(string sourcePath, string destPath);

        List<string> GetInstalledNuGetFiles(string nugetPackagePath);

        void WriteFile(byte[] data, string pathToWriteTo);

        void WriteConfigFile(string data, string pathToWriteTo);
    }
}