using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SAHL.Testing.Common
{
    public static class ResourcesHelper
    {
        public static string resourcesDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\";

        public static byte[] GetResourceBytes(string fileName)
        {
            return File.ReadAllBytes(resourcesDirectory + fileName);
        }

        public static string GetResourceText(string fileName)
        {
            var text = File.ReadAllText(resourcesDirectory + fileName);
            return NormalizeCarriageReturn(text);
        }

        public static string NormalizeCarriageReturn(string text)
        {
            return Regex.Replace(text, @"\r\n|\n\r|\n|\r", "\n");
        }
    }
}
