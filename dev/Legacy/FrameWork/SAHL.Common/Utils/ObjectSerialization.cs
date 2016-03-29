using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SAHL.Common.Utils
{
    public class ObjectSerialization
    {
        /// <summary>
        /// Converts an object to a string of base 64 for serialization. The objects class have to be Serializable for it to work
        /// </summary>
        /// <returns>String</returns>
        public static string SerializeToBase64(object ObjectToConvert)
        {
            IFormatter x = new BinaryFormatter();
            System.IO.MemoryStream oMS = new System.IO.MemoryStream();
            x.Serialize(oMS, ObjectToConvert);
            byte[] oBA = oMS.ToArray();
            return Convert.ToBase64String(oBA);
        }

        /// <summary>
        /// Converts a base64 string representation of an object to the Object representation
        /// </summary>
        public static object DeserializeFromBase64(string StringToConvert)
        {
            IFormatter x = new BinaryFormatter();
            byte[] oBA = Convert.FromBase64String(StringToConvert);
            System.IO.MemoryStream oMS = new System.IO.MemoryStream();
            oMS.Write(oBA, 0, oBA.Length);
            oMS.Position = 0;
            return x.Deserialize(oMS);
        }
    }
}