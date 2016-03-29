using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Globals;
using SAHL.Common.DomainMessages;
using SAHL.Common.Collections;
using SAHL.Common.Exceptions;
using SAHL.Common.BusinessModel.TypeMapper;
using SAHL.Common.CacheData;
using SAHL.Common.Security;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IDocumentCheckListService))]
    public class DocumentCheckListService : IDocumentCheckListService
    {
        private IList<IApplicationDocument> _applicationDocumentList;
        private List<string> _missingDocuments;
        private bool _hasError = false;
        private string _errorMessage = string.Empty;
		private readonly IDocumentCheckListRepository documentCheckListRepository;
		private readonly IApplicationRepository applicationRepository;
		public DocumentCheckListService()
			: this(RepositoryFactory.GetRepository<IDocumentCheckListRepository>(), RepositoryFactory.GetRepository<IApplicationRepository>())
		{

		}
		public DocumentCheckListService(IDocumentCheckListRepository documentCheckListRepository, IApplicationRepository applicationRepository)
		{
			this.documentCheckListRepository = documentCheckListRepository;
			this.applicationRepository = applicationRepository;
		}

        /// <summary>
        /// First checks the application for the matching DocumentSet. 
        /// A list of DocumentTypes is generated that is required by the application at is current state.
        /// If any documents are no longer required it will be removed, any new items will be added.
        /// There are many instance where a document will appear more than once e.g. ID Document for Main Applicant and Suretor.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        public IList<IApplicationDocument> GetList(int ApplicationKey)
        {
            UpdateList(ApplicationKey);
            if (!_hasError)
            {
                IList<IApplicationDocument> retrievedAppDocList = documentCheckListRepository.GetApplicationDocumentsForApplication(ApplicationKey);
                return retrievedAppDocList;
            }
            else
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                IDomainMessageCollection dmc = spc.DomainMessages;
                dmc.Add(new Error(_errorMessage, _errorMessage));
                return (new List<IApplicationDocument>());
            }
        }

        /// <summary>
        /// Enumerates through the list of documents stored for the given application checking for any unchecked items 
        /// (not recieved i.e. RecievedDate == NULL)
        /// If there are any unchecked items false will be returned or if all documents have been recieved, true will be returned.
        /// </summary>
        /// <param name="applicationDocumentList"></param>
        public void SaveList(IList<IApplicationDocument> applicationDocumentList)
        {
            foreach (IApplicationDocument appDoc in applicationDocumentList)
            {
				documentCheckListRepository.SaveApplicationDocument(appDoc);
            }
        }

        /// <summary>
        /// Removes documents no longer required
        /// </summary>
        /// <param name="applicationDocumentList"></param>
        public void DeleteList(IList<IApplicationDocument> applicationDocumentList)
        {
            foreach (IApplicationDocument appDoc in applicationDocumentList)
            {
                documentCheckListRepository.DeleteApplicationDocument(appDoc);
            }
        }

        /// <summary>
        /// Enumerates through the list of documents stored for the given application checking for any unchecked items 
        /// (not recieved i.e. RecievedDate == NULL)
        /// If there are any unchecked items false will be returned or if all documents have been recieved, true will be returned.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        public bool ValidateList(int ApplicationKey)
        {
            bool pass = true;
			IList<IApplicationDocument> applicationDocumentList = documentCheckListRepository.GetApplicationDocumentsForApplication(ApplicationKey);
            
            foreach (IApplicationDocument appDoc in applicationDocumentList)
            {
                if (appDoc.DocumentReceivedDate == null)
                {
                    pass = false;
                    break;
                }
            }
            return pass;
        }

        /// <summary>
        /// Enumerates through the list of documents stored for the given application checking for any unchecked items 
        /// (not recieved i.e. RecievedDate == NULL)
        /// If there are any unchecked items false will be returned or if all documents have been recieved, true will be returned.
        /// This method also will display which documents have not been recieved.
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <returns></returns>
        public IList<string> ValidateListWithMessages(int ApplicationKey)
        {
            _missingDocuments = new List<string>();
			IList<IApplicationDocument> applicationDocumentList = documentCheckListRepository.GetApplicationDocumentsForApplication(ApplicationKey);

            foreach (IApplicationDocument appDoc in applicationDocumentList)
            {
                if (appDoc.DocumentReceivedDate == null)
                {
                    _missingDocuments.Add(string.Format("{0} - Still to be received ", (appDoc.Description != null ? appDoc.Description : appDoc.DocumentType.Description)));
                }
            }
            return _missingDocuments;
        }

        /// <summary>
        /// Updates the list of documents required for a particular applicationso we always have an updated list 
        /// of what is complete - this is needed for reporting as the backend doesn't have access to the Rules. 
        /// </summary>
        /// <param name="ApplicationKey"></param>
        private void UpdateList(int ApplicationKey)
        {
            IApplication application = applicationRepository.GetApplicationByKey(ApplicationKey);
			IOriginationSourceProduct originationSourceProduct = applicationRepository.GetOriginationSourceProduct(application);
            IList<IApplicationDocument> refreshedAppDocList = BuildDocumentTypeList(application, originationSourceProduct);
            IList<IApplicationDocument> appDocsForDeletion = GetApplicationDocumentsToBeDeleted(refreshedAppDocList);
            DeleteList(appDocsForDeletion);
            SaveList(refreshedAppDocList);
        }

        #region Helper Methods

        IList<IApplicationDocument> GetApplicationDocumentsToBeDeleted(IList<IApplicationDocument> updatedAppDocList)
        {
            IList<IApplicationDocument> appDocsToBeDeleted = new List<IApplicationDocument>();

            foreach (IApplicationDocument appDoc in _applicationDocumentList)
            {
                bool _docFound = false;
                foreach (IApplicationDocument _appDoc in updatedAppDocList)
                {
                    if (appDoc.DocumentType.GenericKeyType != null && (_appDoc.GenericKey == appDoc.GenericKey && _appDoc.DocumentType.Key == appDoc.DocumentType.Key))
                    {
                        _docFound = true;
                        break;
                    }
                    else if (appDoc.DocumentType.GenericKeyType == null && _appDoc.DocumentType.Key == appDoc.DocumentType.Key)
                    {
                        _docFound = true;
                        break;
                    }
                }
                if (!_docFound)
                    appDocsToBeDeleted.Add(appDoc);
            }

            return appDocsToBeDeleted;
        }

        IList<IApplicationDocument> BuildDocumentTypeList(IApplication application, IOriginationSourceProduct originationSourceProduct)
        {
			IRuleService ruleService = ServiceFactory.GetService<IRuleService>();
			_applicationDocumentList = documentCheckListRepository.GetApplicationDocumentsForApplication(application.Key);
			IDocumentSet DocumentSet = documentCheckListRepository.GetDocumentSet(application, originationSourceProduct);
            IList<IApplicationDocument> applicationDocumentCheckList = new List<IApplicationDocument>();

            if (DocumentSet != null)
            {
				IList<IDocumentSetConfig> DocumentSetConfigList = documentCheckListRepository.GetDocumentSetConfig(DocumentSet);
                foreach (IDocumentSetConfig docSetConfig in DocumentSetConfigList)
                {
					ParseRuleForDocSetConfig(ruleService, docSetConfig, application, applicationDocumentCheckList);
                }
            }
            else
            {
                _hasError = true;
                _errorMessage = string.Format("DocumentSet has not been configured for OriginationSource ({0}), Product ({1}) & ApplicationType ({2}).", 
                    originationSourceProduct.OriginationSource.Description, originationSourceProduct.Product.Description, application.ApplicationType.Description);
            }

            return applicationDocumentCheckList;
        }

        void ParseRuleForDocSetConfig(IRuleService ruleService, IDocumentSetConfig documentSetConfig, IApplication application, IList<IApplicationDocument> applicationDocumentCheckList)
        {
            IDomainMessageCollection Messages = new DomainMessageCollection();
            Dictionary<int, string> genericList = new Dictionary<int, string>();
            int addDocumentType = ruleService.ExecuteRule(Messages, documentSetConfig.RuleItem.Name, documentSetConfig, genericList, application);
            if (addDocumentType != 0)
            {
                if (documentSetConfig.DocumentType.GenericKeyType != null)
                {
                    foreach (KeyValuePair<int, string> kv in genericList)
                    {
                        IApplicationDocument appDocument = ApplicationDocumentExists(kv.Key, documentSetConfig.DocumentType.Key);
                        if (appDocument == null)
                        {
                            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                            appDocument = bmtm.GetMappedType<IApplicationDocument, ApplicationDocument_DAO>(new ApplicationDocument_DAO());
                            appDocument.GenericKey = kv.Key;
                            appDocument.Application = application;
                            appDocument.DocumentType = documentSetConfig.DocumentType;
                        }
                        appDocument.Description = kv.Value;
						applicationDocumentCheckList.Add(appDocument);
                    }
                }
                else
                {
                    IApplicationDocument appDocument = ApplicationDocumentExists(documentSetConfig.DocumentType.Key);
                    if (appDocument == null)
                    {
                        IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                        appDocument = bmtm.GetMappedType<IApplicationDocument, ApplicationDocument_DAO>(new ApplicationDocument_DAO());
                        appDocument.Application = application;
                        appDocument.DocumentType = documentSetConfig.DocumentType;
                    }
                    appDocument.Description = documentSetConfig.DocumentType.Description;
					applicationDocumentCheckList.Add(appDocument);
                }
            }
        }

        IApplicationDocument ApplicationDocumentExists(int genericKey, int documentTypeKey)
        {
            foreach (IApplicationDocument appDoc in _applicationDocumentList)
            {
                if (appDoc.GenericKey == genericKey && appDoc.DocumentType.Key == documentTypeKey)
                {
                    return appDoc;
                }
            }
            return null;
        }

        IApplicationDocument ApplicationDocumentExists(int documentTypeKey)
        {
            foreach (IApplicationDocument appDoc in _applicationDocumentList)
            {
                if (appDoc.DocumentType.Key == documentTypeKey)
                {
                    return appDoc;
                }
            }
            return null;
        }

        #endregion
    }
}