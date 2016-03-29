using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Diagnostics;
using PDFDocumentWriter.Logging;
using System.Configuration;
using PDFUtils.PDFWriting;

namespace PDFDocumentWriter
{
    public class Controller
    {
        internal class LegalDocumentWorkObj
        {
            int _ID;

            public int ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            Dictionary<string, object> _Params = new Dictionary<string, object>();
            public Dictionary<string, object> Params { get { return _Params; } }
            private int _ReportStatementKey;

            internal int ReportStatementKey
            {
                get { return _ReportStatementKey; }
            }
            internal LegalDocumentWorkObj(DataRow dr)
            {
                _ID = Convert.ToInt32(dr["ID"]);
                Params.Add("PurposeNumber", Convert.ToInt32(dr["LoanPurposeKey"]));
                Params.Add("AccountKey", Convert.ToInt32(dr["GenericKey"]));
                _ReportStatementKey = Convert.ToInt32(dr["ReportStatementKey"]);
            }
        }

        ManualResetEvent mre = new ManualResetEvent(false);
        bool Started = false;
        public event EventHandler OnError;
        string ServiceName = "LEGALDOCUMENTPROCESSOR";

        static Controller()
        {
            try
            {
                LogSettingsClass lsl = new LogSettingsClass();

                LogPlugin.SeedLogSettings(lsl);
                LogPlugin.LogError("Logging Setup");
                lsl.AppName = string.Format("Document Engine");
                LogSection section = ConfigurationManager.GetSection("LogSection") as LogSection;

                if (section != null)
                {
                    lsl.ConsoleLevel = Convert.ToInt32(section.Logging["Console"].level);
                    lsl.ConsoleLevelLock = Convert.ToBoolean(section.Logging["Console"].Lock);
                    lsl.FileLevel = Convert.ToInt32(section.Logging["File"].level);
                    lsl.FileLevelLock = Convert.ToBoolean(section.Logging["File"].Lock);
                    lsl.FilePath = section.Logging["File"].path;
                }

                lsl.NumDaysToStore = 14;
                lsl.RollOverSizeInKB = 4096;
                LogPlugin.SeedLogSettings(lsl);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Starts the thread that will look in the DB for docs to be generated.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (Started) return true;
            Thread t = new Thread(new ThreadStart(Do));
            t.Name = "WorkerThread";
            t.Start();
            Started = true;
            //ThreadPool.SetMaxThreads(10, 10);
            return Started;
        }

        /// <summary>
        /// Signals the Thread to stop processing docs from teh DB.
        /// </summary>
        public void Stop()
        {
            mre.Set();
        }

        void Do()
        {
            try
            {
                while (!mre.WaitOne(0, false))
                {
                    int GenericKey = 0;
                    int DocumentType = 0;
                    string OutPath = string.Empty;
                    try
                    {
                        DataTable dt = new DataTable();
                        DataAccess.DataAccess.GetDocumentsToProcess(dt, ServiceName);
                        bool FoundDocs = false;
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                DocumentType = Convert.ToInt32(dr["DocumentType"]);
                                GenericKey = Convert.ToInt32(dr["GenericKey"]);
                                OutPath = dr["OutputPath"].ToString();
                                switch (DocumentType)
                                {
                                    case 1:
                                        {
                                            LegalDocumentWorkObj obj = new LegalDocumentWorkObj(dr);
                                            //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessLegalDocument), obj);
                                            ProcessLegalDocument(obj);
                                            break;
                                        }
                                    default:
                                        {
                                            // log this as an unknown type of doc to process.
                                            break;
                                        }
                                }
                                FoundDocs = true;
                            }
                            catch (Exception ex)
                            {
                                // Carry on Processing and Log this Error;
                                LogPlugin.LogError("Unable to process document {0} type {1} output {2}{3}{4}", GenericKey, DocumentType, OutPath,
                                    Environment.NewLine, ex.ToString());
                            }
                        }
                        if (FoundDocs)
                            Thread.Sleep(5);
                        else
                            Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        LogPlugin.LogError("Unable to get documents for processing{0}{1}", Environment.NewLine, ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                // Things are really poked. This will stop all doc processing, we need to raise a critical error here
                // for someone to restart the service.
                if (null != OnError)
                {
                    OnError("Controller", new ErrorEventArgs(ex));
                }
            }
        }


        void ProcessLegalDocument(object state)
        {
            try
            {
                LegalDocumentWorkObj doc = state as LegalDocumentWorkObj;
                // check we got passed the correct work obj
                if (null == doc)
                {
                    LogPlugin.LogError("Incorrect Type Passed to ProcessLegalDocument");
                    return;
                }
                try
                {

                    // generate the document
                    PDFGenerator generator = new PDFGenerator();
                    Dictionary<string, object> Params = new Dictionary<string, object>();

                    generator.GenerateDocument(new PDFGenerationObject(doc.Params, doc.ReportStatementKey));

                    // if success remove from DB
                    DataAccess.DataAccess.RemoveProcessedDocument(doc.ID);
                }
                catch (Exception ex)
                {
                    LogPlugin.LogError("Unable to process document:ReportStatementKey:{2}, {0}{1}", 
                        Environment.NewLine, ex.ToString(), doc.ReportStatementKey);
                }
            }
            catch (Exception ex)
            {
                LogPlugin.LogError("Unable to process document:{0}{1}", Environment.NewLine, ex.ToString());
            }
        }
    }
}
