using System;

namespace SAHL.Core.Identity
{
    public class CombGuid : SAHL.Core.Identity.ICombGuid
    {
        // C# Comb Guid generation
        // Found at http://stackoverflow.com/questions/665417/sequential-guid-in-linq-to-sql/2187898#2187898

        private static ICombGuid instance;
        private static readonly object lockObject = new object();

        public Guid Generate()
        {
            byte[] destinationArray = Guid.NewGuid().ToByteArray();
            DateTime time = new DateTime(0x76c, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan span = new TimeSpan(now.Ticks - time.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes = BitConverter.GetBytes(span.Days);
            byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes);
            Array.Reverse(array);
            Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
            Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);
            return new Guid(destinationArray);
        }

        public string GenerateString()
        {
            return this.Generate().ToString();
        }

        public static ICombGuid Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new CombGuid();
                    }
                    return instance;
                }
            }
            set
            {
                instance = value;
            }
        }
    }
}