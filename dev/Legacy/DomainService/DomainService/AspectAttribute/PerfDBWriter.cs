using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AspectAttribute
{
    public class PerfDBWriter
    {
        static Queue<string> ToInsert = new Queue<string>();
        static ManualResetEvent mre = new ManualResetEvent(false);
        static bool ShouldWrite = true;

        static PerfDBWriter()
        {
#warning go read this from config file
            ShouldWrite = true;
            Thread t = new Thread(new ThreadStart(ProcessEvents));
            t.Name = "Method_Perf_Writer";
            t.Start();
        }

        public static void Dispose()
        {
            mre.Set();
            ToInsert.Clear();
        }

        public static void WriteToDB(string Query)
        {
            lock (ToInsert)
            {
                ToInsert.Enqueue(Query);
            }
        }

        private static void ProcessEvents()
        {
            while (!mre.WaitOne(10, false))
            {
                try
                {
                    string Query = string.Empty;
                    while (ToInsert.Count > 0)
                    {
                        lock (ToInsert)
                        {
                            Query = ToInsert.Dequeue();
                            ToInsert.TrimExcess();
                        }
                        if (ShouldWrite)
                            DBMan.ExecuteNonQuery(Query);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}