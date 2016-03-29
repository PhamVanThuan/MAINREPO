using System;
using System.Linq;

namespace SAHL.Services.JsonDocument.Managers
{
    public interface IJsonDocumentDataManager
    {
        void CreateJsonDocument(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data);

        void SaveJsonDocument(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data);

        bool DoesDocumentExist(Guid id);
    }
}