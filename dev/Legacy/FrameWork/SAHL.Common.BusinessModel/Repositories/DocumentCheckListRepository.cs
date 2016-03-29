using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Exceptions;
using System.Data;
using SAHL.Common.DataAccess;
using Castle.ActiveRecord.Queries;


namespace SAHL.Common.BusinessModel.Repositories{

    [FactoryType(typeof(IDocumentCheckListRepository))]
    public class DocumentCheckListRepository : AbstractRepositoryBase, IDocumentCheckListRepository
    {

        public void SaveApplicationDocument(IApplicationDocument applicationDocument)
        {
			base.Save<IApplicationDocument, ApplicationDocument_DAO>(applicationDocument);
			//IDAOObject dao = applicationDocument as IDAOObject;
			//ApplicationDocument_DAO appDoc = (ApplicationDocument_DAO)dao.GetDAOObject();
			//appDoc.SaveAndFlush();
			//if (ValidationHelper.PrincipalHasValidationErrors())
			//    throw new DomainValidationException();
        }

        public void DeleteApplicationDocument(IApplicationDocument applicationDocument)
        {
            IDAOObject dao = applicationDocument as IDAOObject;
            ApplicationDocument_DAO appDoc = (ApplicationDocument_DAO)dao.GetDAOObject();
            appDoc.DeleteAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        public IList<IApplicationDocument> GetApplicationDocumentsForApplication(int ApplicationKey)
        {
            IList<IApplicationDocument> appDocList = new List<IApplicationDocument>();
            string HQL = "select ap from ApplicationDocument_DAO ap where ap.Application.Key = ?";
            SimpleQuery<ApplicationDocument_DAO> q = new SimpleQuery<ApplicationDocument_DAO>(HQL, ApplicationKey);
            ApplicationDocument_DAO[] res = q.Execute();
            foreach (ApplicationDocument_DAO appDoc in res)
            {
                appDocList.Add(new ApplicationDocument(appDoc));
            }

            return appDocList;
        }

        public IDocumentSet GetDocumentSet(IApplication Application, IOriginationSourceProduct OriginationSourceProduct)
        {
            string HQL = "select ds from DocumentSet_DAO ds where ds.ApplicationType.Key = ? and ds.OriginationSourceProduct.Key = ?";
            SimpleQuery<DocumentSet_DAO> q = new SimpleQuery<DocumentSet_DAO>(HQL, Application.ApplicationType.Key, OriginationSourceProduct.Key);
            DocumentSet_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                return (new DocumentSet(res[0]));
            else
                return null;
        }

        public IList<IDocumentSetConfig> GetDocumentSetConfig(IDocumentSet DocumentSet)
        {
            IList<IDocumentSetConfig> DocumentSetConfigList = new List<IDocumentSetConfig>();
            string HQL = "select dsc from DocumentSetConfig_DAO dsc where dsc.DocumentSet.Key = ?";
            SimpleQuery<DocumentSetConfig_DAO> q = new SimpleQuery<DocumentSetConfig_DAO>(HQL, DocumentSet.Key);
            DocumentSetConfig_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
            {
                foreach (DocumentSetConfig_DAO dsc in res)
                {
                    DocumentSetConfigList.Add(new DocumentSetConfig(dsc));
                }
            }
            return DocumentSetConfigList;
        }
    }
}
