namespace SAHL.X2Engine2.Providers
{
    public interface INugetRetriever
    {
        void InstallPackage(string packageName, string nugetBinariesPath, string packageVersion = null);
    }
}