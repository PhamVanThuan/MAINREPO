using System;
using SAHL.Services.Interfaces.DocumentManager.Models;

namespace SAHL.Services.DocumentManager.Utils.DataStore
{
    public interface IDataStoreUtils
    {
        string GenerateDataStoreGuid();
        string GetFileNameForClientFileDocument(ClientFileDocumentModel document, DateTime date);
    }
}