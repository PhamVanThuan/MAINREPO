using System;
using System.IO;

namespace SAHL.Core.Testing.FileConventions
{
    public interface IFileConvention
    {
        bool Process(FileInfo file);
    }
}
