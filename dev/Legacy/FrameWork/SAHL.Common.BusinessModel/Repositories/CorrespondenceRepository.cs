using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.CorrespondenceGeneration;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Logging;
using System.Reflection;
using SAHL.Common.Globals;
using System.Data;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(ICorrespondenceRepository))]
    public class CorrespondenceRepository : AbstractRepositoryBase, ICorrespondenceRepository
    {
        #region ICorrespondenceRepository Members

        /// <summary>
        /// Creates an Empty Correspondence object
        /// </summary>
        /// <returns>ICorrespondence</returns>
        public ICorrespondence CreateEmptyCorrespondence()
        {
            return base.CreateEmpty<ICorrespondence, Correspondence_DAO>();
            //return new SAHL.Common.BusinessModel.Correspondence(new Correspondence_DAO());
        }

        /// <summary>
        /// Creates an Empty CorrespondenceParameter object
        /// </summary>
        /// <returns>ICorrespondenceParameters</returns>
        public ICorrespondenceParameters CreateEmptyCorrespondenceParameter()
        {
            return base.CreateEmpty<ICorrespondenceParameters, CorrespondenceParameters_DAO>();
            //return new SAHL.Common.BusinessModel.CorrespondenceParameters(new CorrespondenceParameters_DAO());
        }

        /// <summary>
        /// Returns the correspondence record for a specified CorrespondenceKey
        /// </summary>
        /// <param name="Key">The CorrespondenceKey</param>
        public ICorrespondence GetCorrespondenceByKey(int Key)
        {
            return base.GetByKey<ICorrespondence, Correspondence_DAO>(Key);

            //string HQL = "from Correspondence_DAO c where c.Key = ?";
            //SimpleQuery query = new SimpleQuery(typeof(Correspondence_DAO), HQL, CorrespondenceKey);
            //Correspondence_DAO[] o = Correspondence_DAO.ExecuteQuery(query) as Correspondence_DAO[];
            //if (o.Length == 0)
            //    return null;

            //return new SAHL.Common.BusinessModel.Correspondence(o[0]);
        }

        /// <summary>
        /// Returns all correspondence records for a specified ReportStatementKey and GenericKey
        /// </summary>
        /// <param name="ReportStatementKey">The int ReportStatementKey</param>
        /// <param name="GenericKey">The int GenericKey</param>
        /// <param name="GenericKeyTypeKey">The int GenericKeyTypeKey</param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to return only unprocessed correspondence records</param>
        public IEventList<ICorrespondence> GetCorrespondenceByReportStatementAndGenericKey(int ReportStatementKey, int GenericKey, int GenericKeyTypeKey, bool UnprocessedOnly)
        {
            string HQL = "from Correspondence_DAO c where c.GenericKey = ? AND c.GenericKeyType.Key = ? AND c.ReportStatement.Key = ?";
            if (UnprocessedOnly)
                HQL += " AND CompletedDate is null";

            SimpleQuery<Correspondence_DAO> q = new SimpleQuery<Correspondence_DAO>(HQL, GenericKey, GenericKeyTypeKey, ReportStatementKey);

            Correspondence_DAO[] res = q.Execute();

            IEventList<ICorrespondence> list = new DAOEventList<Correspondence_DAO, ICorrespondence, SAHL.Common.BusinessModel.Correspondence>(res);
            return list;
        }

        /// <summary>
        /// Returns all correspondence records for a specified GenericKey
        /// </summary>
        /// <param name="genericKeyValue">The int GenericKey</param>
        /// <param name="genericKeyTypeValue">The int GenericKeyType</param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to return only unprocessed correspondence records</param>
        /// <returns></returns>
        public IEventList<ICorrespondence> GetCorrespondenceByGenericKey(int genericKeyValue, int genericKeyTypeValue, bool UnprocessedOnly)
        {
            Dictionary<int, int> genericKeyValues = new Dictionary<int, int>();
            genericKeyValues.Add(genericKeyValue, genericKeyTypeValue);
            return GetCorrespondenceByGenericKeys(genericKeyValues, UnprocessedOnly);
        }

        /// <summary>
        /// Returns all correspondence records for a specified GenericKeys
        /// </summary>
        /// <param name="genericKeyValues">The int GenericKeys</param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to return only unprocessed correspondence records</param>
        public IEventList<ICorrespondence> GetCorrespondenceByGenericKeys(Dictionary<int, int> genericKeyValues, bool UnprocessedOnly)
        {
            if (genericKeyValues.Count <= 0)
                return null;

            IEventList<ICorrespondence> list = new EventList<ICorrespondence>();
            string sql = string.Empty;
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            if (UnprocessedOnly)
                sql = @"SELECT c.* FROM [2am].[dbo].[Correspondence] c where c.GenericKey = ? and c.GenericKeyTypeKey = ? and c.CompletedDate is null";
            else
                sql = @"SELECT c.* FROM [2am].[dbo].[Correspondence] c where c.GenericKey = ? and c.GenericKeyTypeKey = ?";

            foreach (KeyValuePair<int, int> kv in genericKeyValues)
            {
                SimpleQuery<Correspondence_DAO> q = new SimpleQuery<Correspondence_DAO>(QueryLanguage.Sql, sql, kv.Key, kv.Value);
                q.AddSqlReturnDefinition(typeof(Correspondence_DAO), "c");
                Correspondence_DAO[] res = q.Execute();

                if (res != null && res.Length > 0)
                {
                    foreach (Correspondence_DAO _correspondenceRes in res)
                    {
                        list.Add(null, BMTM.GetMappedType<ICorrespondence, Correspondence_DAO>(_correspondenceRes));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Returns all correspondence parameter records for a specified CorrespondenceKey
        /// </summary>
        /// <param name="CorrespondenceKey">The int CorrespondenceKey</param>
        public IEventList<ICorrespondenceParameters> GetCorrespondenceParametersByCorrespondenceKey(int CorrespondenceKey)
        {
            string HQL = "from CorrespondenceParameters_DAO p where p.Correspondence.Key = ?";

            SimpleQuery<CorrespondenceParameters_DAO> q = new SimpleQuery<CorrespondenceParameters_DAO>(HQL, CorrespondenceKey);

            CorrespondenceParameters_DAO[] res = q.Execute();

            IEventList<ICorrespondenceParameters> list = new DAOEventList<CorrespondenceParameters_DAO, ICorrespondenceParameters, SAHL.Common.BusinessModel.CorrespondenceParameters>(res);
            return list;
        }

        /// <summary>
        /// Adds/Updates Correspondence object
        /// </summary>
        /// <param name="correspondence">The Correspondence entity.</param>
        public void SaveCorrespondence(ICorrespondence correspondence)
        {
            base.Save<ICorrespondence, Correspondence_DAO>(correspondence);
            //IDAOObject IDAO = correspondence as IDAOObject;
            //Correspondence_DAO dao = (Correspondence_DAO)IDAO.GetDAOObject();
            //dao.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        /// <summary>
        /// Adds/Updates Correspondence object
        /// </summary>
        /// <param name="correspondence">The Correspondence entity.</param>
        public void SaveCorrespondenceList(List<ICorrespondence> correspondence)
        {
            foreach (ICorrespondence _correspondence in correspondence)
            {
                IDAOObject IDAO = _correspondence as IDAOObject;
                Correspondence_DAO dao = (Correspondence_DAO)IDAO.GetDAOObject();
                dao.SaveAndFlush();
            }
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Deletes all correspondence records for a specified CorrespondenceKey
        /// The domain will delete the correspondenceparameter records aswell
        /// </summary>
        /// <param name="CorrespondenceKey">The int ReportStatementKey</param>
        public void RemoveCorrespondenceByKey(int CorrespondenceKey)
        {
            string query = string.Format("CorrespondenceKey = {0}", CorrespondenceKey);

            // remove the correspondence records
            Correspondence_DAO.DeleteAll(query);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Deletes all correspondence and correspondenceparameter records for a specified ReportStatementKey and GenericKey
        /// </summary>
        /// <param name="ReportStatementKey">The int ReportStatementKey</param>
        /// <param name="GenericKey">The int GenericKey of the reocrds to delete</param>
        /// <param name="GenericKeyTypeKey">The int GenericKeyTypeKey of the reocrds to delete</param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to only remove unprocessed correspondence records or not</param>
        public void RemoveCorrespondenceByReportStatementAndGenericKey(int ReportStatementKey, int GenericKey, int GenericKeyTypeKey, bool UnprocessedOnly)
        {
            // Identify the Correspondence records to be deleted
            IEventList<ICorrespondence> correspondence = GetCorrespondenceByReportStatementAndGenericKey(ReportStatementKey, GenericKey, GenericKeyTypeKey, UnprocessedOnly);
            if (correspondence == null || correspondence.Count < 1)
                return;

            // build a list of the CorrespondenceKeys
            string keyList = "";
            for (int i = 0; i < correspondence.Count; i++)
            {
                if (i == 0)
                    keyList = correspondence[i].Key.ToString();
                else
                    keyList += "," + correspondence[i].Key;
            }

            string query = string.Format("CorrespondenceKey in {0}", "(" + keyList + ")");

            // remove the correspondence records
            Correspondence_DAO.DeleteAll(query);
        }

        /// <summary>
        /// Deletes all correspondence records for a specified GenericKey
        /// </summary>
        /// <param name="GenericKey">The int GenericKey</param>
        /// <param name="GenericKeyType">The GenericKeyType.</param>
        /// <param name="UnprocessedOnly">A bool value to determine whether to only remove unprocessed correspondence records or not</param>
        /// <param name="KeepReportStatementKey">If this ReportStatementKey is specified, correspondence for this report type will not be deleted</param>
        public void RemoveCorrespondenceByGenericKey(int GenericKey, int GenericKeyType, bool UnprocessedOnly, int KeepReportStatementKey)
        {
            // Identify the Correspondence records to be deleted
            IEventList<ICorrespondence> correspondence = GetCorrespondenceByGenericKey(GenericKey, GenericKeyType, UnprocessedOnly);
            if (correspondence == null || correspondence.Count < 1)
                return;

            // build a list of the CorrespondenceKeys
            string keyList = "";
            int i = 0;
            foreach (ICorrespondence corr in correspondence)
            {
                if (corr.ReportStatement.Key != KeepReportStatementKey)
                {
                    if (i == 0)
                        keyList = corr.Key.ToString();
                    else
                        keyList += "," + corr.Key;

                    i++;
                }
            }

            string query = string.Format("CorrespondenceKey in {0}", "(" + keyList + ")");

            // remove the correspondence records
            Correspondence_DAO.DeleteAll(query);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        #endregion ICorrespondenceRepository Members

        #region CorrespondenceDetails

        /// <summary>
        /// Create and empty correspondence detail object
        /// </summary>
        /// <returns></returns>
        public ICorrespondenceDetail CreateEmptyCorrespondenceDetail()
        {
            return base.CreateEmpty<ICorrespondenceDetail, CorrespondenceDetail_DAO>();
        }

        /// <summary>
        /// Get Correspondence Detail Record by Key
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public ICorrespondenceDetail GetCorrespondenceDetailByKey(int Key)
        {
            return base.GetByKey<ICorrespondenceDetail, CorrespondenceDetail_DAO>(Key);
        }

        /// <summary>
        /// Adds/Updates Correspondence Detail object
        /// </summary>
        /// <param name="correspondencedetail">The Correspondence entity.</param>
        public void SaveCorrespondenceDetail(ICorrespondenceDetail correspondencedetail)
        {
            base.Save<ICorrespondenceDetail, CorrespondenceDetail_DAO>(correspondencedetail);
        }

        #endregion CorrespondenceDetails

        #region Correspondence Generation

        public void SendCorrespondenceReportToLegalEntity(ISendCorrespondenceRequest sendCorrespondenceRequest)
        {
            ILegalEntityRepository legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILegalEntity legalEntity = legalEntityRepository.GetLegalEntityByKey(sendCorrespondenceRequest.LegalEntityKey);
            DefaultParameterBuilder defaultParameterBuilder = new DefaultParameterBuilder();

            SAHL.Common.Configuration.CorrespondenceReportElement correspondenceReportElement = new Common.Configuration.CorrespondenceReportElement();
            ReportData reportData = new ReportData(sendCorrespondenceRequest.ReportName, sendCorrespondenceRequest.OriginationSourceProductKey, correspondenceReportElement, sendCorrespondenceRequest.BatchPrint, sendCorrespondenceRequest.AllowPreview, sendCorrespondenceRequest.DataStorName, sendCorrespondenceRequest.UpdateConditions, sendCorrespondenceRequest.SendUserConfirmationEmail, sendCorrespondenceRequest.EmailProcessedPDFtoConsultant, sendCorrespondenceRequest.CorrespondenceTemplate, sendCorrespondenceRequest.CombineDocumentsIfEmailing);
            defaultParameterBuilder.GetReportParameters(reportData);

            List<CorrespondenceMediumInfo> CorrespondenceMediumInfos = new List<CorrespondenceMediumInfo>();
            CorrespondenceMediumInfo correspondenceMediumInfo = new CorrespondenceMediumInfo();
            correspondenceMediumInfo.LegalEntityKey = sendCorrespondenceRequest.LegalEntityKey;
            correspondenceMediumInfo.EmailAddress = string.IsNullOrEmpty(legalEntity.EmailAddress) ? " " : legalEntity.EmailAddress;
            correspondenceMediumInfo.FaxCode = string.IsNullOrEmpty(legalEntity.FaxCode) ? " " : legalEntity.FaxCode;
            correspondenceMediumInfo.FaxNumber = string.IsNullOrEmpty(legalEntity.FaxNumber) ? " " : legalEntity.FaxNumber;
            correspondenceMediumInfo.CellPhoneNumber = string.IsNullOrEmpty(legalEntity.CellPhoneNumber) ? " " : legalEntity.CellPhoneNumber;
            correspondenceMediumInfo.CorrespondenceMediumsSelected.Add(sendCorrespondenceRequest.SelectedCorrespondenceMedium);
            CorrespondenceMediumInfos.Add(correspondenceMediumInfo);
            GenerateCorrespondenceReport(sendCorrespondenceRequest.GenericKey, sendCorrespondenceRequest.GenericKeyTypeKey, sendCorrespondenceRequest.LoanNumber, sendCorrespondenceRequest.ADUserName , sendCorrespondenceRequest.ReportGenericValue, reportData, CorrespondenceMediumInfos);
        }

        public void GenerateCorrespondenceReport(int genericKey, int genericKeyTypeKey, int loanNumber, string adusername, string ReportGenericValue, ReportData rd, List<CorrespondenceMediumInfo> correspondenceMediumInfos)
        {
            // All documents that are emailed or faxed will be processed straight away
            // Documents that are printed will either be processed no or queued for batch printing depending on their setup
            string renderedMessage = "";
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection messages = spc.DomainMessages;

            // Build a list of report data for each report and its parameter values
            ILegalEntityRepository _legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IReportRepository _reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            DefaultParameterBuilder defaultParameterBuilder = new DefaultParameterBuilder();

            foreach (var correspondenceMediumInfo in correspondenceMediumInfos)
            {
                Dictionary<CorrespondenceExt, byte[]> dicCorrespondence = new Dictionary<CorrespondenceExt, byte[]>();
                ILegalEntity selectedLegalEntity = _legalEntityRepo.GetLegalEntityByKey(correspondenceMediumInfo.LegalEntityKey);

                foreach (var correspondenceMedium in correspondenceMediumInfo.CorrespondenceMediumsSelected)
                {
                    int correspondenceMediumKey = (int)correspondenceMedium;

                    // Populate the Report Parameters
                    // create our new reportdata object for saving
                    ReportData saveReportData = new ReportData(rd.ReportName, rd.OriginationSourceProductKey, rd.CorrespondenceReportElement, rd.BatchPrint, rd.AllowPreview, rd.DataStorName, rd.UpdateConditions, rd.SendUserConfirmationEmail, rd.EmailProcessedPDFtoConsultant, rd.CorrespondenceTemplate, rd.CombineDocumentsIfEmailing);
                    foreach (ReportDataParameter parm in rd.ReportParameters)
                    {
                        ReportDataParameter saveParm = new ReportDataParameter(parm.ReportParameterKey, parm.ParameterName, parm.ParameterValue);
                        saveReportData.ReportParameters.Add(saveParm);
                    }

                    // We make the assumption since the report will only accept one generic parameter
                    // the first item in the reportParameters will be the item to be set up
                    foreach (var param in saveReportData.ReportParameters)
                    {
                        param.ParameterValue = ReportGenericValue;
                        break;
                    }

                    IReportStatement reportStatement = _reportRepo.GetReportStatementByKey(rd.ReportStatementKey);
                    ICorrespondence correspondence = CreateEmptyCorrespondence();
                    correspondence.GenericKey = genericKey;
                    correspondence.GenericKeyType = _lookupRepo.GenericKeyType.ObjectDictionary[Convert.ToString(genericKeyTypeKey)];
                    correspondence.ReportStatement = reportStatement;
                    correspondence.CorrespondenceMedium = _lookupRepo.CorrespondenceMediums.ObjectDictionary[Convert.ToString(correspondenceMediumKey)];
                    switch (correspondenceMedium)
                    {
                        case SAHL.Common.Globals.CorrespondenceMediums.Email:
                            correspondence.DestinationValue = correspondenceMediumInfo.EmailAddress;
                            break;
                        case SAHL.Common.Globals.CorrespondenceMediums.Fax:
                            correspondence.DestinationValue = correspondenceMediumInfo.FaxCode + correspondenceMediumInfo.FaxNumber.TrimStart('0');
                            break;
                        case SAHL.Common.Globals.CorrespondenceMediums.SMS:
                            correspondence.DestinationValue = correspondenceMediumInfo.CellPhoneNumber;
                            break;
                        default:
                            break;
                    }

                    correspondence.DueDate = DateTime.Now;
                    correspondence.ChangeDate = DateTime.Now;
                    //correspondence.UserID = _view.CurrentPrincipal.Identity.Name;
                    correspondence.UserID = adusername;
                    // We will only ever do this for one legal entity
                    //TODO
                    correspondence.LegalEntity = selectedLegalEntity;

                    // setup correspondence parameters
                    // only insert into correspondenceparameters if :-
                    // 1. report is configured for "batch printing"
                    // 2. "print" has been selected
                    // 3. reportparameters exist in the reportparameter table
                    string dataStorParameters = "";
                    foreach (ReportDataParameter parm in saveReportData.ReportParameters)
                    {
                        // build this string of params here for use later in datastor
                        dataStorParameters += parm.ParameterName + ":" + parm.ParameterValue + " | ";

                        if (correspondenceMedium == SAHL.Common.Globals.CorrespondenceMediums.Post && saveReportData.BatchPrint && parm.ReportParameterKey > 0)
                        {
                            ICorrespondenceParameters correspondenceParm = CreateEmptyCorrespondenceParameter();
                            correspondenceParm.Correspondence = correspondence;
                            correspondenceParm.ReportParameter = _reportRepo.GetReportParameterByKey(parm.ReportParameterKey);
                            correspondenceParm.ReportParameterValue = parm.ParameterValue.ToString();
                            // add the correspondenceparameter object to the correspondence object
                            correspondence.CorrespondenceParameters.Add(messages, correspondenceParm);
                        }
                    }

                    // add the string of params temporarily to the correspondence record here for use later in datastor
                    // this will get overwritten when we save the correspondence record a little later
                    correspondence.OutputFile = dataStorParameters;

                    // render the report : only render if user has selected :-
                    // 1. Email
                    // 2. Fax
                    // 3. Print (and the report is not configured for batch print)
                    renderedMessage = "";
                    byte[] renderedReport = null;
                    if (correspondenceMedium != SAHL.Common.Globals.CorrespondenceMediums.Post || saveReportData.BatchPrint == false)
                    {
                        IDictionary<string, string> reportParameters = new Dictionary<string, string>();
                        foreach (ReportDataParameter parm in saveReportData.ReportParameters)
                        {
                            reportParameters.Add(parm.ParameterName, parm.ParameterValue.ToString());
                        }

                        switch (reportStatement.ReportType.Key)
                        {
                            case (int)SAHL.Common.Globals.ReportTypes.ReportingServicesReport:
                                renderedReport = _reportRepo.RenderSQLReport(rd.StatementName, reportParameters, out renderedMessage);
                                break;
                            case (int)SAHL.Common.Globals.ReportTypes.StaticPDF:
                            case (int)SAHL.Common.Globals.ReportTypes.PDFReport:
                                string renderedPDF = String.Empty;
                                if (reportStatement.ReportType.Key == (int)SAHL.Common.Globals.ReportTypes.StaticPDF)
                                    renderedPDF = reportStatement.StatementName;
                                else
                                    renderedPDF = _reportRepo.GeneratePDFReport(reportStatement.Key, reportParameters, out renderedMessage);

                                ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();
                                WindowsImpersonationContext wic = securityService.BeginImpersonation();

                                try
                                {
                                    // get the pdf report int a byte stream for use later
                                    FileStream fs = new FileStream(renderedPDF, FileMode.Open, FileAccess.Read);
                                    renderedReport = new byte[fs.Length];

                                    //Read block of bytes from stream into the byte array
                                    fs.Read(renderedReport, 0, System.Convert.ToInt32(fs.Length));

                                    //Close the File Stream
                                    fs.Close();
                                }
                                catch (Exception e)
                                {
                                    Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                                    methodParameters.Add("genericKey", genericKey);
                                    methodParameters.Add("genericKeyTypeKey", genericKeyTypeKey);
                                    methodParameters.Add("loanNumber", loanNumber);
                                    methodParameters.Add("adusername", adusername);
                                    methodParameters.Add("ReportGenericValue", ReportGenericValue);

                                    LogPlugin.Logger.LogErrorMessageWithException("GenerateCorrespondenceReport", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });

                                    throw;
                                }
                                finally
                                {
                                    // end impersonation
                                    securityService.EndImpersonation(wic);
                                }

                                break;
                            default:
                                break;
                        }
                    }

                    if (String.IsNullOrEmpty(renderedMessage))
                    {
                        // add the Correspondence object and its rendered report to the collection
                        CorrespondenceExt correspondenceExt = new CorrespondenceExt(correspondence, saveReportData.ExcludeFromDataSTOR);
                        dicCorrespondence.Add(correspondenceExt, renderedReport);
                    }
                }

                if (dicCorrespondence.Count == 0)
                {
                    messages.Add(new Error("Error Rendering Reports - No reports processed./r/n" + renderedMessage, "Error Rendering Reports - No reports processed./r/n" + renderedMessage));
                    return;
                }

                // save the correspondence
                try
                {
                    SaveCorrespondenceReports(dicCorrespondence, rd, adusername, loanNumber, genericKey, correspondenceMediumInfo);
                }
                catch (Exception e)
                {
                    Dictionary<string, object> methodParameters = new Dictionary<string, object>();
                    methodParameters.Add("genericKey", genericKey);
                    methodParameters.Add("genericKeyTypeKey", genericKeyTypeKey);
                    methodParameters.Add("loanNumber", loanNumber);
                    methodParameters.Add("adusername", adusername);
                    methodParameters.Add("ReportGenericValue", ReportGenericValue);

                    LogPlugin.Logger.LogErrorMessageWithException("SAHL.Common.BusinessModel.Repositories.GenerateCorrespondenceReport", e.Message, e, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodParameters } });
                    throw;
                }
            }
        }

        private bool SaveCorrespondenceReports(Dictionary<CorrespondenceExt, byte[]> dicCorrespondence, ReportData reportData, string adusername, int loanNumber, int genericKey, CorrespondenceMediumInfo correspondenceMediumInfo)
        {
            bool success = true;

            WindowsImpersonationContext wic = null;
            IOrganisationStructureRepository orgStructRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplicationRepository _applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILegalEntityRepository legalEntityRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection messages = spc.DomainMessages;
            ILegalEntity selectedLegalEntity = legalEntityRepo.GetLegalEntityByKey(correspondenceMediumInfo.LegalEntityKey);

            string msg = "";
            if (dicCorrespondence.Count <= 0)
            {
                msg = "Correspondence List Empty - No reports processed.";
                messages.Add(new Error(msg, msg));
                return false;
            }

            IDataStorRepository dataStorRepository = RepositoryFactory.GetRepository<IDataStorRepository>();

            List<string> emailAttachments = new List<string>();

            // get the STOR object
            string dataStorName = reportData.DataStorName;
            ISTOR stor = dataStorRepository.GetSTORByName(dataStorName);

            IADUser adUser = orgStructRepo.GetAdUserForAdUserName(adusername);
            bool sendUserConfirmationEmail = reportData.SendUserConfirmationEmail;
            bool emailProcessedPDFtoConsultant = reportData.EmailProcessedPDFtoConsultant;

            if (adUser.LegalEntity == null)
                throw new Exception("ADUser record for " + adUser.ADUserName + " has no LegalEntity record - ADUserKey: " + adUser.Key);
            else if (adUser.LegalEntity.EmailAddress.Trim().Length <= 0)
                throw new Exception("LegalEntity record for ADUser " + adUser.ADUserName + " has no email address - LegalEntityKey: " + adUser.LegalEntity.Key.ToString());

            string userEmailAddress = adUser.LegalEntity.EmailAddress;
            string subject = "";
            string body = "";
            string destinationDesc = "";
            string clientEmailAddress = "";

            CorrespondenceTemplates correspondenceTemplate = (reportData.CorrespondenceTemplate == null) ? CorrespondenceTemplates.EmailCorrespondenceGeneric : reportData.CorrespondenceTemplate;

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            ISecurityService securityService = ServiceFactory.GetService<ISecurityService>();

            List<string> confirmationEmailDocuments = new List<string>();

            foreach (KeyValuePair<CorrespondenceExt, byte[]> CorrespondenceKeyValuePair in dicCorrespondence)
            {
                CorrespondenceExt correspondenceExt = CorrespondenceKeyValuePair.Key;
                ICorrespondence correspondence = correspondenceExt.Correspondence;

                byte[] renderedReport = CorrespondenceKeyValuePair.Value;

                string outputFileDataSTOR = "", dataSTORDirectory = "";
                string guid = "";
                string outputFileClientEmail = "";
                string dataStageDirectory = "";
                decimal insertedDataKey = 0;
                string dataSTORlink = "";
                string dataStorParameters = correspondence.OutputFile.Trim();
                bool emailPrintedPDFtoConsultant = false;

                genericKey = correspondence.GenericKey;
                clientEmailAddress = correspondence.DestinationValue;

                bool batchPrint = false;
                bool writeToDataStor = String.IsNullOrEmpty(dataStorName) ? false : true;
                if (writeToDataStor && correspondenceExt.ExcludeFromDataSTOR == true)
                    writeToDataStor = false;

                if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post)
                {
                    if (reportData.BatchPrint)
                    {
                        writeToDataStor = false;
                        batchPrint = true;
                    }
                    if (emailProcessedPDFtoConsultant)
                        emailPrintedPDFtoConsultant = true;
                }

                // if this is not a batch print then update the completed date on the correspondence record
                if (batchPrint == false)
                    correspondence.CompletedDate = DateTime.Now;

                // if we are writing to data STOR then we need to store the output file path in the correspondence record
                if (writeToDataStor)
                {
                    guid = System.Guid.NewGuid().ToString("B").ToUpper();
                    dataSTORDirectory = stor.Folder + @"\" + DateTime.Today.Year.ToString() + @"\" + DateTime.Today.Month.ToString("00");
                    outputFileDataSTOR = dataSTORDirectory + @"\" + guid;
                    dataStageDirectory = _lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.CorrespondenceDataStagingFolder].ControlText;

                    correspondence.OutputFile = outputFileDataSTOR;
                }

                // insert the Correspondence record into the database
                SaveCorrespondence(correspondence);

                // add description of correspondence to confirmation email body
                confirmationEmailDocuments.Add(correspondence.ReportStatement.ReportName + " : " + correspondence.CorrespondenceMedium.Description + (!String.IsNullOrEmpty(correspondence.DestinationValue) ? " : " + correspondence.DestinationValue : ""));

                if (writeToDataStor)
                {
                    #region write to data STOR

                    // create an empty data object
                    IData data = dataStorRepository.CreateEmptyData();
                    string origSource = "";
                    IOriginationSourceProduct osp = _applicationRepo.GetOriginationSourceProductByKey(reportData.OriginationSourceProductKey);
                    if (osp != null)
                        origSource = osp.OriginationSource.Description + " : " + osp.Product.Description;

                    // populate the data object
                    DateTime dateNow = System.DateTime.Now;
                    string now = dateNow.ToString("yyyy-MM-dd hh:mm:ss");
                    string userName = adUser.LegalEntity == null ? adUser.ADUserName : adUser.LegalEntity.GetLegalName(LegalNameFormat.FullNoSalutation) + " (" + adUser.ADUserName + ")";

                    data.ArchiveDate = now;
                    data.STOR = stor.Key;
                    data.GUID = guid;
                    data.Extension = "PDF";
                    data.MsgSubject = loanNumber.ToString();
                    data.Key1 = loanNumber.ToString();
                    data.Key2 = origSource;
                    data.Key3 = genericKey.ToString();
                    data.Key4 = correspondence.ReportStatement.ReportName;
                    data.Key5 = correspondence.CorrespondenceMedium.Description;
                    data.Key6 = String.IsNullOrEmpty(correspondence.DestinationValue) ? "" : correspondence.DestinationValue;
                    data.Key7 = now;
                    data.Key8 = userName;
                    data.Key9 = !String.IsNullOrEmpty(dataStorParameters) ? dataStorParameters.Trim('|') : "";
                    data.Key10 = correspondence.ReportStatement.Key.ToString();

                    // setup the filename to use for emailing/faxing
                    // filename is made up as follows: ReportName_LoanNumber_CorrespondenceMedium_DateTime.pdf
                    string dateTime = dateNow.Year.ToString() + dateNow.Month.ToString("00") + dateNow.Day.ToString("00") + dateNow.Hour.ToString("00") + dateNow.Minute.ToString("00") + dateNow.Second.ToString("00");
                    string reportNameFormatted = data.Key4.Replace(" - ", "-"); // strip out spaces around the hypen
                    string origFileName = reportNameFormatted + "_" + loanNumber.ToString() + "_" + correspondence.CorrespondenceMedium.Description + "_" + dateTime + ".pdf";
                    data.OriginalFilename = origFileName;
                    data.Key11 = origFileName;

                    outputFileClientEmail = dataStageDirectory + @"\" + data.OriginalFilename;

                    // save the data
                    dataStorRepository.SaveData(data);

                    // get the key of the inserted data record
                    insertedDataKey = data.Key;

                    // save the rendered pdf - note that we need to impersonate a user with local permissions and rights
                    // to the remote folder otherwise we get authentication issues thanks to Kerberos vs. the SAHL network setup
                    wic = securityService.BeginImpersonation();

                    try
                    {
                        // Check if our output directory exists - if not then create
                        if (!Directory.Exists(dataSTORDirectory))
                            Directory.CreateDirectory(dataSTORDirectory);

                        // save to the report to the dataSTOR server path
                        FileStream fs = new FileStream(outputFileDataSTOR, FileMode.Create);
                        fs.Write(renderedReport, 0, renderedReport.Length);
                        fs.Close();
                        fs.Dispose();

                        if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Email
                        || correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Fax
                        || emailPrintedPDFtoConsultant == true)
                        {
                            // create a copy of the document in the correspondence data staging directory
                            // from where it will be emailed/faxed
                            // we r saving a copy of the doc to print here aswell so we can email it to comsultant
                            if (!String.IsNullOrEmpty(dataStageDirectory))
                            {
                                FileStream fs2 = new FileStream(outputFileClientEmail, FileMode.Create);
                                fs2.Write(renderedReport, 0, renderedReport.Length);
                                fs2.Close();
                                fs2.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        // end impersonation
                        securityService.EndImpersonation(wic);
                    }

                    #endregion write to data STOR
                }

                if (batchPrint == false && !String.IsNullOrEmpty(outputFileClientEmail))
                {
                    if (writeToDataStor)
                    {
                        #region create a dataSTOR link to email the user

                        try
                        {
                            // activate impersonation so we can create the file on the server
                            wic = securityService.BeginImpersonation();

                            dataSTORlink = outputFileClientEmail.Replace(".pdf", ".STOR");

                            StreamWriter sw = new StreamWriter(dataSTORlink);
                            sw.WriteLine("[General]");
                            sw.WriteLine("Requested By=" + adusername);
                            sw.WriteLine("Requested Date=" + System.DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            sw.WriteLine("[STOR]");
                            sw.WriteLine("STORid=" + stor.Key.ToString());
                            sw.WriteLine("STORname=" + stor.Name);
                            sw.WriteLine("Total GUIDs=2");
                            sw.WriteLine("GUID001=ID" + insertedDataKey.ToString());
                            sw.WriteLine("GUID002=");
                            sw.Close();
                            sw.Dispose();
                        }
                        finally
                        {
                            // cancel impersonation
                            securityService.EndImpersonation(wic);
                        }

                        #endregion create a dataSTOR link to email the user
                    }

                    #region send correspondence to client

                    switch (correspondence.CorrespondenceMedium.Key)
                    {
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Email:
                            destinationDesc = "Email Address       : ";
                            //set the subject and body
                            GetEmailTemplate(selectedLegalEntity, adUser.LegalEntity.GetLegalName(LegalNameFormat.Full), userEmailAddress, 0, correspondenceTemplate, out subject, out body);

                            // send the email to the Client using the attachement created above
                            if (reportData.CombineDocumentsIfEmailing)
                            {
                                emailAttachments.Add(outputFileClientEmail);
                            }
                            else
                            {
                                messageService.SendEmailExternal(correspondence.GenericKey
                                   , userEmailAddress
                                   , correspondence.DestinationValue
                                   , ""
                                   , ""
                                   , subject
                                   , body
                                   , outputFileClientEmail
                                   , ""
                                   , "");
                            }
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Fax:
                            // send the fax to the Client using the attachement created above
                            destinationDesc = "Fax Number          : ";
                            messageService.SendFax(correspondence.GenericKey
                               , userEmailAddress
                               , correspondence.DestinationValue
                               , outputFileClientEmail);

                            break;

                        case (int)SAHL.Common.Globals.CorrespondenceMediums.Post:
                            destinationDesc = "";
                            break;
                        case (int)SAHL.Common.Globals.CorrespondenceMediums.SMS:
                            destinationDesc = "Cell Number          : ";
                            break;
                        default:
                            break;
                    }

                    #endregion send correspondence to client

                    #region send confirmation email to user

                    if (sendUserConfirmationEmail && !String.IsNullOrEmpty(userEmailAddress))
                    {
                        // send confirmation email to user
                        subject = "HALO Correspondence : "
                            + (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post ? "Printing Required" : "Confirmation")
                            + " : "
                            + correspondence.ReportStatement.ReportName
                            + " : "
                            + "Loan Number " + loanNumber.ToString();

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("The following correspondence has been sent from HALO");
                        sb.AppendLine("----------------------------------------------------");
                        sb.AppendLine("Document Name       : " + correspondence.ReportStatement.ReportName);
                        sb.AppendLine("Medium              : " + correspondence.CorrespondenceMedium.Description);
                        if (!String.IsNullOrEmpty(destinationDesc))
                            sb.AppendLine(destinationDesc + correspondence.DestinationValue);
                        if (writeToDataStor)
                        {
                            sb.AppendLine("Written to DataSTOR : " + (writeToDataStor == true ? "Yes" : "No"));
                            sb.AppendLine("");
                            if (correspondence.CorrespondenceMedium.Key == (int)SAHL.Common.Globals.CorrespondenceMediums.Post)
                            {
                                sb.AppendLine("Clicking the attached link will enable you to Print the document from DataSTOR.");
                            }
                            else
                                sb.AppendLine("Clicking the attached link will enable you to View the document in DataSTOR.");

                            if (emailPrintedPDFtoConsultant)
                            {
                                sb.AppendLine("");
                                sb.AppendLine("Alternatively you can Print the document from the PDF attached to this email.");
                            }
                        }

                        IList<string> attachements = new List<string>();
                        // attach the datastor link
                        attachements.Add(dataSTORlink);

                        // attach the processed pdf to the email if required
                        if (emailPrintedPDFtoConsultant)
                            attachements.Add(outputFileClientEmail);

                        messageService.SendEmailInternal("halo@sahomeloans.com", userEmailAddress, "", "", subject, sb.ToString(), false, attachements);
                    }

                    #endregion send confirmation email to user
                }
            }

            if (reportData.CombineDocumentsIfEmailing && emailAttachments.Count > 0)
            {
                messageService.SendEmailExternal(genericKey
                                   , userEmailAddress
                                   , clientEmailAddress
                                   , ""
                                   , ""
                                   , subject
                                   , body
                                   , emailAttachments.ToArray()
                                    );
            }

            return success;
        }

        public void GetEmailTemplate(ILegalEntity legalEntity, string consultantName, string emailFrom, Int32 reference, CorrespondenceTemplates correspondenceTemplate, out string subject, out string body)
        {
            string legalEntityName = legalEntity.GetLegalName(LegalNameFormat.Full);
            var commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            var template = commonRepo.GetCorrespondenceTemplateByKey(correspondenceTemplate);

            subject = String.Format(template.Subject, reference);
            if (reference == 0)
                subject = subject.Replace("0 - ",string.Empty);

            body = String.Format(template.Template, consultantName, emailFrom, legalEntityName);

            return;
        }

        /// <summary>
        /// This will remove all client email entries for the specified accountkey and email subject
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="emailSubject"></param>
        /// <returns></returns>
        public void RemoveClientEmailByAccountKeyAndSubject(int accountKey, string emailSubject)
        {
            string query = string.Format("LoanNumber = {0} and EmailSubject = '{1}'", accountKey, emailSubject);

            // remove the correspondence records
            ClientEmail_DAO.DeleteAll(query);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            //using (IDbConnection con = Helper.GetSQLDBConnection())
            //{
            //    string query = string.Format("Delete from [SAHLDB].[dbo].ClientEmail where LoanNumber = {0} and EmailSubject = '{1}'", accountKey, emailSubject);
            //    ParameterCollection parameters = new ParameterCollection();
            //    object o = Helper.ExecuteScalar(con, query, parameters);
            //}
        }

        #endregion Correspondence Generation
    }
}