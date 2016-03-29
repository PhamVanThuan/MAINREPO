
namespace SAHL.Services.CATS
{
    public interface IFileWriter
    {

        void WriteFileLine(string text, string fileNam);

        string CreateFile(string fileName);
    }
}
