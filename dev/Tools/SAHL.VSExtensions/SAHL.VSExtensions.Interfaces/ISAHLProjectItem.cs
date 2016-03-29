namespace SAHL.VSExtensions.Interfaces
{
    public interface ISAHLProjectItem
    {
        string Name { get; }

        string ItemPath { get; }

        string ProjectName { get; }

        string Namespace { get; }

        ISAHLProject CurrentProject { get; }

        ISAHLProjectItem GetOrAddFolder(string folderName);

        ISAHLProjectItem AddFile(string fileName, string fileExtension, string fileContent);
    }
}