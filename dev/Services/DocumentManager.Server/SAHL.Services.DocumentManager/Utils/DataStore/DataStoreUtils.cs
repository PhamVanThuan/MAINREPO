using SAHL.Services.Interfaces.DocumentManager.Models;
using System;

namespace SAHL.Services.DocumentManager.Utils.DataStore
{
    public class DataStoreUtils : IDataStoreUtils
    {
        public string GenerateDataStoreGuid()
        {
            return Guid.NewGuid().ToString("B").ToLower();
        }

        public string GetFileNameForClientFileDocument(ClientFileDocumentModel document, DateTime date)
        {
            var dateString = date.ToString("yyyy-MM-dd HHmmss").Trim();
            return String.Format("{0} - {1} - {2}", document.IdentityNumber, document.Category, dateString);
        }

    }
}