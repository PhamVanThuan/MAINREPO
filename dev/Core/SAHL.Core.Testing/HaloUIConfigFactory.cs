using Newtonsoft.Json;
using SAHL.Core.Testing.Config.UI;
using System;
using System.IO;
namespace SAHL.Core.Testing.Factories
{
    public sealed class HaloUIConfigFactory
    {
        public HaloUIConfig Create(string filePath)
        {
            var jsonSerializer = new JsonSerializer();
            if (!File.Exists(filePath))
            {
                throw new Exception(String.Format("File does not exist {0}",filePath));
            }
            using(var reader = new StreamReader(filePath))
            {
                return (HaloUIConfig)jsonSerializer.Deserialize(reader, typeof(HaloUIConfig));
            }
        }
    }
}