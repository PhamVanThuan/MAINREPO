using CsvHelper;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SAHL.Common.Service
{
    public class CsvFileParse : ITextFileParser
    {
        public CsvFileParse()
        {
        }

        public IEnumerable<T> Parse<T>(Stream fileStream) where T : class
        {
            try
            {
                var csv = new CsvReader(new StreamReader(fileStream));
                csv.Configuration.IsCaseSensitive = false;
                var leads = csv.GetRecords<T>();
                return leads.ToList();
            }
                
            catch (CsvHelperException ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException(MethodBase.GetCurrentMethod().Name, ex.Message, ex);

                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                spc.DomainMessages.Add(new Error(ex.Message, ex.Message));
                return null;
            }
        }
    }
}