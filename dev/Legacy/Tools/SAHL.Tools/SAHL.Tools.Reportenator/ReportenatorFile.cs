namespace SAHL.Tools.Reportenator
{
    public class ReportenatorFile
    {
        public ReportenatorFile(string directory, string fileName)
        {
            this.Directory = directory;
            this.FileName = fileName;
        }

        public string Directory { get; private set; }

        public string FileName { get; private set; }
    }
}