using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Logging;

namespace SAHL.Batch
{
    public class CapExtract_Batch : BatchBase
    {
        private const string m_NumberFormat = "0.00";

        public CapExtract_Batch()
            : base()
        {
        }

        //public CapExtract_Batch(int BatchKey)
        //    :base(BatchKey)
        //{
        //}

        protected override void RunBatch()
        {
            IDbConnection conn = null;
            DataTable dt = null;

            try
            {
                ValidateBulkBatch();

                string filePath = CurrentBulkBatch.BulkBatchType.FilePath;

                // resolve config parameters
                string spvKeys = GetStringParameterValue(BulkBatchParameterNames.SPV);
                int capTypeConfigurationKey = GetIntParameterValue(BulkBatchParameterNames.CapTypeConfigurationKey);
                int arrearBalance = GetIntParameterValue(BulkBatchParameterNames.ArrearBalance);
                string arrearBalanceOperator = GetStringParameterValue(BulkBatchParameterNames.MathematicalOperator);
                BulkBatchParameter_DAO parmExcludeOffersAfterDate = GetParameter(BulkBatchParameterNames.ExcludeOffersAfterDate);

                // load up the ICapTypeConfiguration
                CapTypeConfiguration_DAO capTypeConfig = CapTypeConfiguration_DAO.Find(capTypeConfigurationKey);

                filePath = filePath + "\\Cap Client Extract_" + DateTime.Now.ToString("ddMMyyyy") + "_" + capTypeConfigurationKey.ToString() + ".csv";
                if (File.Exists(filePath))
                {
                    throw new Exception("File [" + filePath + "] already exists - Cannot Overwrite File");
                }

                LogPlugin.Logger.LogFormattedInfo("CapExtract_Batch.RunBatch", "Creating Export Starting - {0}", filePath);

                // get the sql statement and add formatted info
                string sql = String.Format(UIStatementRepository.GetStatement("COMMON", "GetCapOffersExtract"),
                    spvKeys, arrearBalanceOperator, "");

                // execute the query with all parameters and write it to the report
                conn = Helper.GetSQLDBConnection();
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddIntParameter(parameters, "@CapTypeConfigurationKey", capTypeConfigurationKey);
                Helper.AddIntParameter(parameters, "@ResetConfigurationKey", capTypeConfig.ResetConfiguration.Key);
                Helper.AddIntParameter(parameters, "@ArrearBalance", arrearBalance);
                if (parmExcludeOffersAfterDate != null)
                    Helper.AddDateParameter(parameters, "@ExcludeDate", Convert.ToDateTime(parmExcludeOffersAfterDate.ParameterValue, CultureInfo.GetCultureInfo(BatchBase.CultureGb).DateTimeFormat));
                else
                    Helper.AddDateParameter(parameters, "@ExcludeDate", DateTime.Now.AddDays(1));

                conn.Open();

                // execute the query
                dt = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = (SqlCommand)conn.CreateCommand();
                adapter.SelectCommand.CommandText = sql;
                adapter.SelectCommand.CommandTimeout = 0;

                parameters.TransferParameters(adapter.SelectCommand.Parameters);

                adapter.Fill(dt);
                adapter.SelectCommand.Parameters.Clear();
                adapter.Dispose();

                WriteReport(filePath, dt);

                // update the BulkBatch object
                CurrentBulkBatch.FileName = filePath;

                LogPlugin.Logger.LogFormattedInfo("Creating Export Complete - {0}", filePath);

                #region Old Code

                /*
                 *
                CAPView.CapAccountDataTable AccountsForCapDT = new CAPView.CapAccountDataTable();
                CapSFE.GetAccountsForCap(AccountsForCapDT, spvKeys, CapDS.CapTypeConfiguration[0].ResetConfigurationKey, arrearBalance, arrearBalanceOperator, excludeOffersAfterDate , BatchMetrics);

                for (int accIndex = 0; accIndex < AccountsForCapDT.Count; accIndex++)
                {
                    CAPView CapDetailsDS = new CAPView();
                    if (CapSFE.GetCapOfferViewForBatch(CapDetailsDS, CapDS, AccountsForCapDT[accIndex].AccountKey, LookupsDS, BatchMetrics) == true)
                    {
                        //add record details to file
                        StringBuilder CapLine = new StringBuilder();

                        double totalCurrentBalance = CapDetailsDS.AccountTotalInfo[0].LoanCurrentBalance;
                        double balanceToCap = CapDetailsDS.AccountTotalInfo[0].BalanceToCap;

                        CapLine.Append(CapDetailsDS.AccountAndLoanInfo[0].AccountKey.ToString()); CapLine.Append(",");
                        CapLine.Append(totalCurrentBalance.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(balanceToCap.ToString(m_NumberFormat)); CapLine.Append(",");

                        double balance1 = 0;
                        double effectiveRate1 = 0;
                        double payment1 = 0;
                        double capPremium1 = 0;
                        double capInstallment1 = 0;

                        double balance2 = 0;
                        double effectiveRate2 = 0;
                        double payment2 = 0;
                        double capPremium2 = 0;
                        double capInstallment2 = 0;

                        double balance3 = 0;
                        double effectiveRate3 = 0;
                        double payment3 = 0;
                        double capPremium3 = 0;
                        double capInstallment3 = 0;

                        for (int capTypeIndex = 0; capTypeIndex < CapDS.CapOfferDetail.Count; capTypeIndex++)
                        {
                            int capType = CapDS.CapTypeConfigurationDetail.FindByCapTypeConfigurationDetailKey(CapDS.CapOfferDetail[capTypeIndex].CapTypeConfigurationDetailKey).CapTypeKey;
                            if (capType == 1)
                            {
                                balance1 = CapDS.CapOfferDetail[capTypeIndex].Fee + totalCurrentBalance;
                                effectiveRate1 = CapDS.CapOfferDetail[capTypeIndex].EffectiveRate * 100;
                                payment1 = CapDS.CapOfferDetail[capTypeIndex].Payment;
                                capPremium1 = CapDS.CapOfferDetail[capTypeIndex].Fee;
                                capInstallment1 = CapDS.CapOfferDetail[capTypeIndex].Payment - CapDetailsDS.AccountTotalInfo[0].VariableLoanInstallment;
                            }
                            else if (capType == 2)
                            {
                                balance2 = CapDS.CapOfferDetail[capTypeIndex].Fee + totalCurrentBalance;
                                effectiveRate2 = CapDS.CapOfferDetail[capTypeIndex].EffectiveRate * 100;
                                payment2 = CapDS.CapOfferDetail[capTypeIndex].Payment;
                                capPremium2 = CapDS.CapOfferDetail[capTypeIndex].Fee;
                                capInstallment2 = CapDS.CapOfferDetail[capTypeIndex].Payment - CapDetailsDS.AccountTotalInfo[0].VariableLoanInstallment;
                            }
                            else if (capType == 3)
                            {
                                balance3 = CapDS.CapOfferDetail[capTypeIndex].Fee + totalCurrentBalance;
                                effectiveRate3 = CapDS.CapOfferDetail[capTypeIndex].EffectiveRate * 100;
                                payment3 = CapDS.CapOfferDetail[capTypeIndex].Payment;
                                capPremium3 = CapDS.CapOfferDetail[capTypeIndex].Fee;
                                capInstallment3 = CapDS.CapOfferDetail[capTypeIndex].Payment - CapDetailsDS.AccountTotalInfo[0].VariableLoanInstallment;
                            }
                        }
                        CapLine.Append(balance1.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(balance2.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(balance3.ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(((double)(CapDS.CapOffer[0].LinkRate*100)).ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(effectiveRate1.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(effectiveRate2.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(effectiveRate3.ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(CapDetailsDS.AccountTotalInfo[0].TotalLoanInstallment.ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(payment1.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(payment2.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(payment3.ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(capPremium1.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(capPremium2.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(capPremium3.ToString(m_NumberFormat)); CapLine.Append(",");

                        CapLine.Append(capInstallment1.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(capInstallment2.ToString(m_NumberFormat)); CapLine.Append(",");
                        CapLine.Append(capInstallment3.ToString(m_NumberFormat)); CapLine.Append(",");

                        string LegalEntityName = AccountsForCapDT[accIndex].LEName;

                        CapLine.Append(LegalEntityName); CapLine.Append(",");

                        ////////////////////
                        string addressBuilding = "";
                        string addressStreet = "";
                        string addressSuburb = "";
                        string addressCity = "";
                        string addressPostalCode = "";
                        string postBox = "";
                        string postOffice = "";
                        string postalCode = "";

                        if (!(AccountsForCapDT[accIndex].IsAddressFormatKeyNull()))
                        {
                            if (AccountsForCapDT[accIndex].AddressFormatKey == 1)
                            {
                                if ((!AccountsForCapDT[accIndex].IsUnitNumberNull()) && (!AccountsForCapDT[accIndex].IsBuildingNumberNull()) && (!AccountsForCapDT[accIndex].IsBuildingNameNull()))
                                {
                                    addressBuilding = AccountsForCapDT[accIndex].UnitNumber + " " + AccountsForCapDT[accIndex].BuildingNumber + " " + AccountsForCapDT[accIndex].BuildingName;
                                }
                                if ((!AccountsForCapDT[accIndex].IsStreetNumberNull()) && (!AccountsForCapDT[accIndex].IsStreetNameNull()))
                                    addressStreet = AccountsForCapDT[accIndex].StreetNumber + " " + AccountsForCapDT[accIndex].StreetName;
                                if (!AccountsForCapDT[accIndex].IsRRR_SuburbDescriptionNull())
                                {
                                    addressSuburb = AccountsForCapDT[accIndex].RRR_SuburbDescription;
                                }
                                if (!AccountsForCapDT[accIndex].IsRRR_CityDescriptionNull())
                                    addressCity = AccountsForCapDT[accIndex].RRR_CityDescription;
                                if (!AccountsForCapDT[accIndex].IsRRR_PostalCodeNull())
                                {
                                    addressPostalCode = AccountsForCapDT[accIndex].RRR_PostalCode;
                                }
                            }
                            else
                            {
                                if (!AccountsForCapDT[accIndex].IsRRR_SuburbDescriptionNull())
                                {
                                    postOffice = AccountsForCapDT[accIndex].RRR_SuburbDescription;
                                }
                                if (!AccountsForCapDT[accIndex].IsRRR_PostalCodeNull())
                                {
                                    postalCode = AccountsForCapDT[accIndex].RRR_PostalCode;
                                }

                                if (AccountsForCapDT[accIndex].AddressFormatKey == 2)
                                {
                                    if (!AccountsForCapDT[accIndex].IsBoxNumberNull())
                                        postBox = "PO Box " + AccountsForCapDT[accIndex].BoxNumber;
                                }
                                else
                                    if (AccountsForCapDT[accIndex].AddressFormatKey == 3)
                                    {
                                        if (!AccountsForCapDT[accIndex].IsBoxNumberNull())
                                            postBox = "Private Bag  " + AccountsForCapDT[accIndex].BoxNumber;
                                    }
                                    else
                                        if (AccountsForCapDT[accIndex].AddressFormatKey == 4)
                                        {
                                            if ((!AccountsForCapDT[accIndex].IsSuiteNumberNull()) && (!AccountsForCapDT[accIndex].IsBoxNumberNull()))
                                                postBox = "Post Net Suite  " + AccountsForCapDT[accIndex].SuiteNumber + " Private Bag  " + AccountsForCapDT[accIndex].BoxNumber;
                                        }
                                        else
                                            if (AccountsForCapDT[accIndex].AddressFormatKey == 6)
                                            {
                                                if (!AccountsForCapDT[accIndex].IsBoxNumberNull())
                                                    postBox = "Cluster Box " + AccountsForCapDT[accIndex].BoxNumber;
                                            }
                            }
                        }

                        CapLine.Append(addressBuilding); CapLine.Append(",");
                        CapLine.Append(addressStreet); CapLine.Append(",");
                        CapLine.Append(addressSuburb); CapLine.Append(",");
                        CapLine.Append(addressCity); CapLine.Append(",");
                        CapLine.Append(addressPostalCode); CapLine.Append(",");
                        CapLine.Append(postBox); CapLine.Append(",");
                        CapLine.Append(postOffice); CapLine.Append(",");
                        CapLine.Append(postalCode); CapLine.Append(",");
                        //////////////////////
                        //string MailingAddress = "";
                        //CapLine.Append(MailingAddress); CapLine.Append(",");
                        ////////////////////////
                        CapLine.Append(","); //empty column for brokerkey

                        extractFile.WriteLine(CapLine.ToString());
                    }
                    else
                    {
                        LogException( CapSFE.GetCapValidationError(),"Account",AccountsForCapDT[accIndex].AccountKey.ToString());
                    }
                }
                 * */

                #endregion Old Code
            }
            catch (Exception ex)
            {

                LogPlugin.Logger.LogErrorMessageWithException("CapExtract_Batch.RunBatch", "Cap Export", ex);
                ExceptionPolicy.HandleException(ex, "UI Exception");
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
                if (conn != null)
                    conn.Dispose();
            }
        }
    }
}