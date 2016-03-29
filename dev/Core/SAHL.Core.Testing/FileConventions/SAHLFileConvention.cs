using System;
using System.IO;

namespace SAHL.Core.Testing.FileConventions
{
    public abstract class SAHLFileConvention
    {
        public bool Run(FileInfo file, Func<bool> finalCheck)
        {
            if (file.Extension != ".dll" || !file.Name.Contains("SAHL"))
            {
                return false;
            }
            return finalCheck.Invoke();
        }
    }
}