using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Batch
{
    public class CapImport_Batch : BatchBase
    {
        private ICampaignRepository _campaignRepo = RepositoryFactory.GetRepository<ICampaignRepository>();
        private ISecurityRepository _securityRepo = RepositoryFactory.GetRepository<ISecurityRepository>();

        public CapImport_Batch()
            : base()
        {
        }

        //public CapImport_Batch(int BatchKey)
        //    : base(BatchKey)
        //{
        //}

        protected override void RunBatch()
        {
            IDbConnection conn = null;

            try
            {
                ValidateBulkBatch();

                string filePath = CurrentBulkBatch.BulkBatchType.FilePath;
                string ADUserName = CurrentBulkBatch.UserID;
                IADUser ADUser = _securityRepo.GetADUserByPrincipal(ADUserName);
                int UserID = ADUser.Key;

                // resolve config parameters
                int capTypeConfigKey = GetIntParameterValue(BulkBatchParameterNames.CapTypeConfigurationKey);
                string fileName = GetStringParameterValue(BulkBatchParameterNames.FileName);

                // load the cap type config object
                CapTypeConfiguration_DAO config = CapTypeConfiguration_DAO.Find(capTypeConfigKey);

                //load cap config

                filePath = Path.Combine(filePath, fileName);
                if (!File.Exists(filePath))
                {
                    throw new Exception("File [" + filePath + "] does not exist");
                }

                LogPlugin.Logger.LogFormattedInfo("Processing Import Starting - {0}", filePath);

                StreamReader importFile = new StreamReader(filePath);
                string importLine = "";

                // open a connection to the database
                conn = Helper.GetSQLDBConnection();
                conn.Open();

                char[] splitChar = { ',' };
                int brokerNameCol = -1;
                int leNameCol = -1;

                // skip the header but use it to work out the index of the broker name and legal entity name columns
                if (!(importFile.EndOfStream))
                {
                    importLine = importFile.ReadLine();
                    string[] headers = importLine.Split(splitChar);
                    for (int col = 0; col < headers.Length; col++)
                    {
                        if (headers[col].ToLower().IndexOf("broker name") > -1)
                            brokerNameCol = col;
                        else if (headers[col].ToLower().IndexOf("legal entity name") > -1)
                            leNameCol = col;

                        if (brokerNameCol > -1 && leNameCol > -1)
                            break;
                    }
                }

                if (brokerNameCol == -1)
                {
                    LogPlugin.Logger.LogErrorMessage("CapImport_Batch.RunBatch", "Unable to locate Broker Name column index");
                    throw new Exception("Unable to locate Broker Name column index");
                }
                if (leNameCol == -1)
                {
                    LogPlugin.Logger.LogErrorMessage("CapImport_Batch.RunBatch", "Unable to locate Legal Entity Name column index");
                    throw new Exception("Unable to locate Legal Entity Name column index");
                }

                int count = 0;

                while (!(importFile.EndOfStream))
                {
                    count++;
                    bool isValid = true;

                    importLine = importFile.ReadLine();
                    string[] splitLine = importLine.Split(splitChar);

                    int accountKey = int.Parse(splitLine[0]);
                    string brokerName = splitLine[brokerNameCol];
                    string leName = splitLine[leNameCol];

                    LogPlugin.Logger.LogFormattedInfo("CapImport_Batch.RunBatch", "Cap Import - Processing Record {0} & Account {1}", count, accountKey);

                    Broker_DAO broker = null;
                    if (!String.IsNullOrEmpty(brokerName))
                    {
                        Broker_DAO[] brokers = Broker_DAO.FindAllByProperty("FullName", brokerName.Trim());
                        if (brokers.Length > 0)
                            broker = brokers[0];
                    }
                    if (broker == null)
                    {
                        LogException(null, String.Format("Broker '{0}' Not Found", brokerName), "Account", accountKey.ToString());
                        LogPlugin.Logger.LogFormattedError("Cap Import - {0}", String.Format("Broker '{0}' Not Found", brokerName), "Account", accountKey.ToString());
                        continue;
                    }

                    CapApplication_DAO offer = null;

                    TransactionScope txn = new TransactionScope();
                    try
                    {
                        LogPlugin.Logger.LogFormattedInfo("CapImport_Batch.RunBatch", "Cap Import - Creating Cap Offer ({0})", accountKey);

                        // create the cap offer for the account key
                        offer = CreateCapOffer(conn, accountKey, broker, config);

                        // create the X2 case
                        if (offer != null)
                        {
                            //CreateX2Case(offer, leName);
                            txn.VoteCommit();
                        }
                        else
                        {
                            txn.VoteRollBack();
                            isValid = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionPolicy.HandleException(ex, "UI Exception");
                        LogException(ex, ex.Message, "Error", "Error");
                        txn.VoteRollBack();
                        isValid = false;
                    }
                    finally
                    {
                        txn.Dispose();
                    }

                    //-----------------------
                    if (offer != null)
                        AddCampaignDefinition(offer, UserID);

                    //-----------------------
                    LogPlugin.Logger.LogFormattedInfo("CapImport_Batch.RunBatch", "Cap Import - Creating X2 Instance ({0})", accountKey);

                    // Creation of the X2 Instance can only occur after
                    // the transaction has been committed and disposed.
                    if (isValid && offer != null && offer.Key > 0)
                    {
                        long instanceID = CreateX2Case(offer, leName);
                        LogPlugin.Logger.LogFormattedInfo("CapImport_Batch.RunBatch", "Cap Import - Cap Application Created : Account ({0}) CapOffer ({1}) Instance ({2})", accountKey, offer.Key, instanceID);
                    }
                }

                importFile.Close();
                UpdateStatus(BulkBatchStatuses.Successful);

                LogPlugin.Logger.LogFormattedInfo("Processing Import Complete - {0}", filePath);
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException("CapImport_Batch.RunBatch", "Cap Import", ex);
                ExceptionPolicy.HandleException(ex, "UI Exception");
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
            }
        }

        private bool AddCampaignDefinition(CapApplication_DAO capOffer, int UserID)
        {
            string CampaignName = "CAP 2 Sales";

            //Check if Campaign Defintion already exists
            //IList<ICampaignDefinition> CampaignList = _campaignRepo.GetCampaignDefinitionByName(CampaignName);
            IList<ICampaignDefinition> CampaignList = _campaignRepo.GetCampaignDefinitionByNameAndConfiguration(CampaignName, capOffer.CapTypeConfiguration.ApplicationStartDate,
                                                                                                capOffer.CapTypeConfiguration.ApplicationEndDate, Convert.ToString(capOffer.CapTypeConfiguration.Key));

            //Get Parent Campaign Defintion
            string ParentCampaignName = "CAP CAMPAIGN: Client List";
            IList<ICampaignDefinition> ParentCampaignDefinitionList = _campaignRepo.GetCampaignDefinitionByName(ParentCampaignName);
            ICampaignDefinition campaignDefinition = null;
            ICampaignTargetContact campaignTargetContact = null;
            ICampaignTarget campaignTarget = null;
            TransactionScope txn = new TransactionScope();
            try
            {
                if (CampaignList.Count == 0)
                {
                    // Setup CampaignDefinition
                    ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                    campaignDefinition = _campaignRepo.CreateEmptyCampaignDefinition();
                    campaignDefinition.CampaignName = CampaignName;
                    campaignDefinition.CampaignReference = Convert.ToString(capOffer.CapTypeConfiguration.Key);
                    campaignDefinition.Startdate = capOffer.CapTypeConfiguration.ApplicationStartDate;
                    campaignDefinition.EndDate = capOffer.CapTypeConfiguration.ApplicationEndDate;
                    campaignDefinition.MarketingOptionKey = (int)MarketingOptions.CustomerLists;
                    campaignDefinition.OrganisationStructureKey = 27; // (int)SAHL.Common.Globals.OrganisationStructures.CAP;
                    campaignDefinition.GeneralStatusKey = (int)GeneralStatuses.Active;
                    if (ParentCampaignDefinitionList != null)
                        campaignDefinition.ParentCampaignDefinition = ParentCampaignDefinitionList[0];
                    campaignDefinition.ReportStatement = null;
                    campaignDefinition.ADUserKey = UserID;

                    campaignDefinition.DataProviderDataServiceKey = (int)DataProviderDataServices.SAHLMarketingcampaign;
                    campaignDefinition.MarketingOptionRelevanceKey = (int)MarketingOptionRelevances.NotRelevant;

                    // Setup CampaignTarget
                    campaignTarget = _campaignRepo.CreateEmptyCampaignTarget();
                    campaignTarget.CampaignDefinition = campaignDefinition;
                    campaignTarget.GenericKey = capOffer.Account.Key;
                    campaignTarget.GenericKeyTypeKey = (int)GenericKeyTypes.CapOffer;
                    campaignTarget.ADUserKey = UserID;

                    // Add CampaignTarget to CampaignDefinition
                    campaignDefinition.CampaignTargets.Add(null, campaignTarget);
                }
                else
                {
                    //The Campaign definition already exists, so just add the campaign target to it
                    // Setup CampaignTarget
                    campaignDefinition = CampaignList[0];
                    campaignTarget = _campaignRepo.CreateEmptyCampaignTarget();
                    campaignTarget.CampaignDefinition = campaignDefinition;
                    campaignTarget.GenericKey = capOffer.Account.Key;
                    campaignTarget.GenericKeyTypeKey = (int)GenericKeyTypes.CapOffer;
                    campaignTarget.ADUserKey = UserID;

                    // Add CampaignTarget to CampaignDefinition
                    campaignDefinition.CampaignTargets.Add(null, campaignTarget);
                }

                // Setup CampaignTargetContact
                campaignTargetContact = _campaignRepo.CreateEmptyCampaignTargetContact();
                campaignTargetContact.CampaignTarget = campaignTarget;
                campaignTargetContact.LegalEntityKey = capOffer.Account.Roles[0].LegalEntity.Key;

                campaignTargetContact.ChangeDate = DateTime.Now;
                campaignTargetContact.AdUserKey = capOffer.Broker.ADUser.Key;
                campaignTargetContact.CampaignTargetResponse = _campaignRepo.GetCampaignTargetResponseByKey((int)CampaignTargetResponses.Callbackrequest);

                // Add CampaignTargetContact to CampaignTarget
                campaignTarget.CampaignTargetContacts.Add(null, campaignTargetContact);

                // Save CampaignDefinition
                _campaignRepo.SaveCampaignDefinition(campaignDefinition);
                txn.VoteCommit();
                return true;
            }
            catch (Exception ex)
            {
                txn.VoteRollBack();
                MethodBase methodBase = MethodBase.GetCurrentMethod();
                LogPlugin.Logger.LogErrorMessageWithException(methodBase.Name, "Campaign Definition", ex, new Dictionary<string, object>() { { Logger.METHODPARAMS, methodBase.GetParameters() } });
                ExceptionPolicy.HandleException(ex, "UI Exception");
                return false;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private CapApplication_DAO CreateCapOffer(IDbConnection conn, int accountKey, Broker_DAO broker, CapTypeConfiguration_DAO capTypeConfig)
        {
            //string sql = String.Format(UIStatementRepository.GetStatement("BatchApplication", "GetCapOffersExtract"),
            //    "select SPVKey from [2am]..SPV", "<", String.Format("and a.AccountKey = {0}", accountKey));

            string sql = String.Format(UIStatementRepository.GetStatement("COMMON", "ParseAccountForCapImport"));

            // get the cap offer information
            ParameterCollection parameters = new ParameterCollection();
            Helper.AddIntParameter(parameters, "@AccountKey", accountKey);
            Helper.AddIntParameter(parameters, "@CapTypeConfigurationKey", capTypeConfig.Key);
            Helper.AddIntParameter(parameters, "@ResetConfigurationKey", capTypeConfig.ResetConfiguration.Key);
            Helper.AddIntParameter(parameters, "@ArrearBalance", 100000);   // make it a very high number because we don't really car about this value
            Helper.AddDateParameter(parameters, "@ExcludeDate", DateTime.Now.AddDays(1));

            // execute the query
            DataTable dt = new DataTable();
            Helper.FillFromQuery(dt, sql, conn, parameters);

            if (dt.Rows.Count == 0)
            {
                LogPlugin.Logger.LogFormattedError("Cap Import - Unable to create Cap Offer for Account {0}", accountKey.ToString());
                LogWarning("Unable to get Cap Offer", accountKey.ToString(), "Account");
                dt.Dispose();
                return null;
            }

            // get the data we need out of the row
            DataRow row = dt.Rows[0];
            double currentBalance = Convert.ToDouble(row["Current Balance"]);
            double currentInstallment = Convert.ToDouble(row["Current Installment"]);
            double linkRate = Convert.ToDouble(row["Link Rate"]);
            dt.Dispose();

            // get the open status
            CapStatus_DAO status = CapStatus_DAO.Find((int)CapStatuses.Open);

            // create the new cap offer
            CapApplication_DAO app = new CapApplication_DAO();
            app.Account = Account_DAO.Find(accountKey);
            app.ApplicationDate = DateTime.Now;
            app.Broker = broker;
            app.CapStatus = status;
            app.CapTypeConfiguration = capTypeConfig;
            app.ChangeDate = DateTime.Now;
            app.CurrentBalance = currentBalance;
            app.CurrentInstallment = currentInstallment;
            app.LinkRate = linkRate / 100;
            app.RemainingInstallments = Convert.ToInt32(row["Remaining Installments"]);
            app.UserID = CurrentBulkBatch.UserID;
            app.SaveAndFlush();

            // get the CapType configuration detail records we need
            CapTypeConfigurationDetail_DAO[] capTypeConfigDetails = GetCapTypeConfigurationDetails(capTypeConfig.Key);
            if (capTypeConfigDetails.Length != 3)
                throw new Exception(String.Format("Expecting 3 configuration details, {0} found for CapTypeConfiguration key of {1}", capTypeConfigDetails.Length, capTypeConfig.Key));

            // create the detail records
            for (int i = 1; i <= 3; i++)
            {
                CapApplicationDetail_DAO detail = new CapApplicationDetail_DAO();
                detail.CapApplication = app;
                detail.CapStatus = status;
                foreach (CapTypeConfigurationDetail_DAO ctcd in capTypeConfigDetails)
                {
                    if (ctcd.CapType.Key == i)
                    {
                        detail.CapTypeConfigurationDetail = ctcd;
                        break;
                    }
                }
                if (detail.CapTypeConfigurationDetail == null)
                    throw new Exception("Unable to find CapTypeConfigurationDetail");
                detail.ChangeDate = DateTime.Now;
                detail.EffectiveRate = Convert.ToDouble(row[String.Format("Effective Rate ({0}%)", i)]) / 100;
                detail.Fee = Convert.ToDouble(row[String.Format("Cap Premium ({0}%)", i)]);
                detail.Payment = Convert.ToDouble(row[String.Format("Payment ({0}%)", i)]);
                detail.UserID = CurrentBulkBatch.UserID;
                detail.SaveAndFlush();
            }

            return app;
        }

        private CapTypeConfigurationDetail_DAO[] GetCapTypeConfigurationDetails(int capTypeConfigKey)
        {
            string hql = "from CapTypeConfigurationDetail_DAO c where c.CapTypeConfiguration.Key = ? and c.GeneralStatus.Key = 1";
            SimpleQuery<CapTypeConfigurationDetail_DAO> q = new SimpleQuery<CapTypeConfigurationDetail_DAO>(hql, capTypeConfigKey);
            return q.Execute();
        }

        public long CreateX2Case(CapApplication_DAO offer, string legalEntityName)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("CapOfferKey", offer.Key.ToString());
            fields.Add("CapBroker", offer.Broker.ADUserName);
            if (legalEntityName.Length >= 255)
                legalEntityName = legalEntityName.Substring(0, 255);
            fields.Add("LegalEntityName", legalEntityName);
            fields.Add("AccountKey", offer.Account.Key.ToString());
            fields.Add("CapExpireDate", offer.CapTypeConfiguration.ApplicationEndDate.ToString());

            SAHLPrincipal principal = SetThreadPrincipal("X2");
            IX2Service x2Service = ServiceFactory.GetService<IX2Service>();
            x2Service.CreateWorkFlowInstance(principal, Properties.Settings.Default.X2ProcessName, "-1",
                Properties.Settings.Default.X2WorkFlowName,
                Properties.Settings.Default.X2ActivityName,
                fields, true);
			x2Service.CreateCompleteActivity(principal, null, true, fields);

            // Extract the Instance ID
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
            IX2Info x2Info = spc.X2Info as IX2Info;
            long instanceID = x2Info.InstanceID;

            ClearThreadPrincipal();
            return instanceID;
        }
    }
}