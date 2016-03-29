using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IDocumentCheckListRepository
    {
        void SaveApplicationDocument(IApplicationDocument applicationDocument);

        void DeleteApplicationDocument(IApplicationDocument applicationDocument);

        IList<IApplicationDocument> GetApplicationDocumentsForApplication(int ApplicationKey);

        IDocumentSet GetDocumentSet(IApplication Application, IOriginationSourceProduct OriginationSourceProduct);

        IList<IDocumentSetConfig> GetDocumentSetConfig(IDocumentSet DocumentSet);
    }
}