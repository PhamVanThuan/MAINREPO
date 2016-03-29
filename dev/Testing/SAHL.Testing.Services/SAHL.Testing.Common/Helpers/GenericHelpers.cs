using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Testing.Common.Helpers
{
    public static class GenericHelpers
    {
        public static T DeepCopy<T>(T input)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, input);
            m.Position = 0;
            return (T)b.Deserialize(m);
        }
    }
}
