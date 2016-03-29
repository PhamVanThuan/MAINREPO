using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Logging;

namespace SAHL.Batch
{
    public class CapMailingHouseExtract : BatchBase
    {
        private const string m_NumberFormat = "0.00";

        public CapMailingHouseExtract()
            : base()
        {
        }

        //public CapMailingHouseExtract(int BatchKey)
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

                //resolve config parameters
                string filePath = CurrentBulkBatch.BulkBatchType.FilePath;

                // resolve the parameters
                int capTypeConfigurationKey = GetIntParameterValue(BulkBatchParameterNames.CapTypeConfigurationKey);

                // load up the ICapTypeConfiguration
                CapTypeConfiguration_DAO capTypeConfig = CapTypeConfiguration_DAO.Find(capTypeConfigurationKey);

                filePath = filePath + "\\Cap Mailing Extract_" + DateTime.Now.ToString("ddMMyyyy") + "_" + capTypeConfigurationKey.ToString() + ".csv";

                if (File.Exists(filePath))
                {
                    throw new Exception("File [" + filePath + "] already exists - Cannot Overwrite File");
                }

                LogPlugin.Logger.LogFormattedInfo("CapMailingHouseExtract.RunBatch", "CapMailingHouseExtract Start - {0}", filePath);

                // get the sql statement and add formatted info
                string sql = UIStatementRepository.GetStatement("COMMON", "GetCapOffersExtractMailingHouse");

                // execute the query with all parameters and write it to the report
                conn = Helper.GetSQLDBConnection();
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddIntParameter(parameters, "@CapTypeConfigurationKey", capTypeConfigurationKey);
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

                LogPlugin.Logger.LogFormattedInfo("CapMailingHouseExtract.RunBatch", "CapMailingHouseExtract Complete - {0}", filePath);

                #region Old Code

                /*
                //load cap config
                CAP CapDS = new CAP();
                CapSFE.GetCapTypeConfigByConfigKey(CapDS, CapTypeConfigurationKey, BatchMetrics);
                CapSFE.GetCapOfferByConfigKeyAndStatus(CapDS, CapTypeConfigurationKey, 1, BatchMetrics);

                string filename = filePath + "/Cap Mailing Extract_" + DateTime.Now.ToString("ddMMyyyy") + "_" + CapTypeConfigurationKey.ToString() + ".csv";
                if (System.IO.File.Exists(filename))
                {
                    throw new Exception("File [" + filename + "] already exists - Cannot Overwrite File");
                }
                BulkBatchDS._BulkBatch[0].FileName = filename;
                System.IO.StreamWriter extractFile = new System.IO.StreamWriter(filename);

                WriteHeader(extractFile);

                for (int offerIndex = 0; offerIndex < CapDS.CapOffer.Count; offerIndex++)
                {
                    CAP.CapOfferRow offerRow = CapDS.CapOffer[offerIndex];
                    CAPView.CapAccountDataTable AccountInfo = new CAPView.CapAccountDataTable();
                    CapSFE.GetAccountsDetailsForCapAccount(AccountInfo,offerRow.AccountKey,BatchMetrics);

                    if ((AccountInfo == null) || (AccountInfo.Count == 0))
                    {
                        LogException( "No Mailing address details found" ,"Account" ,offerRow.AccountKey.ToString());
                        continue;
                    }

                    DataView dv = CapDS.CapOfferDetail.DefaultView;
                    dv.RowFilter = "CapOfferKey = " + offerRow.CapOfferKey.ToString();

                    double loanCurrentBalance1 = 0;
                    double loanCurrentBalance2 = 0;
                    double loanCurrentBalance3 = 0;
                    double loanRate1 = 0;
                    double loanRate2 = 0;
                    double loanRate3 = 0;
                    double installmentAmount1 = 0;
                    double installmentAmount2 = 0;
                    double installmentAmount3 = 0;
                    double premium1 = 0;
                    double premium2 = 0;
                    double premium3 = 0;
                    double installmentDiff1 = 0;
                    double installmentDiff2 = 0;
                    double installmentDiff3 = 0;

                    for (int detailIndex = 0; detailIndex < dv.Count; detailIndex++)
                    {
                        CAP.CapOfferDetailRow detailRow = CapDS.CapOfferDetail.FindByCapOfferDetailKey(int.Parse(dv[detailIndex][0].ToString()));
                        int capType = CapDS.CapTypeConfigurationDetail.FindByCapTypeConfigurationDetailKey(detailRow.CapTypeConfigurationDetailKey).CapTypeKey;
                        if (capType == 1)
                        {
                            loanCurrentBalance1 = offerRow.CurrentBalance + detailRow.Fee;
                            loanRate1 = detailRow.EffectiveRate * 100;
                            installmentAmount1 = detailRow.Payment;
                            premium1 = detailRow.Fee;
                            installmentDiff1 = detailRow.Payment - offerRow.CurrentInstallment;
                        }
                        else if (capType == 2)
                        {
                            loanCurrentBalance2 = offerRow.CurrentBalance + detailRow.Fee;
                            loanRate2 = detailRow.EffectiveRate * 100;
                            installmentAmount2 = detailRow.Payment;
                            premium2 = detailRow.Fee;
                            installmentDiff2 = detailRow.Payment - offerRow.CurrentInstallment;
                        }
                        else if (capType == 3)
                        {
                            loanCurrentBalance3 = offerRow.CurrentBalance + detailRow.Fee;
                            loanRate3 = detailRow.EffectiveRate * 100;
                            installmentAmount3 = detailRow.Payment;
                            premium3 = detailRow.Fee;
                            installmentDiff3 = detailRow.Payment - offerRow.CurrentInstallment;
                        }
                    }

                    //add record details to file
                    StringBuilder CapLine = new StringBuilder();

                    CapLine.Append(offerRow.AccountKey.ToString()); CapLine.Append(",");
                    CapLine.Append(offerRow.RemainingInstallments.ToString()); CapLine.Append(",");
                    CapLine.Append(offerRow.CurrentBalance.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(loanCurrentBalance1.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(loanCurrentBalance2.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(loanCurrentBalance3.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(((double)(offerRow.LinkRate*100)).ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(((double)(AccountInfo[0].InterestRate * 100)).ToString(m_NumberFormat)); CapLine.Append(",");

                    CapLine.Append(loanRate1.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(loanRate2.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(loanRate3.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(offerRow.CurrentInstallment.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentAmount1.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentAmount2.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentAmount3.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(premium1.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(premium2.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(premium3.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentDiff1.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentDiff2.ToString(m_NumberFormat)); CapLine.Append(",");
                    CapLine.Append(installmentDiff3.ToString(m_NumberFormat)); CapLine.Append(",");

                    Lookup.BrokerRow BrokerRow = LookupsDS.Broker.FindByBrokerKey(offerRow.BrokerKey);
                    CapLine.Append(BrokerRow.FullName); CapLine.Append(",");
                    CapLine.Append(BrokerRow.Initials); CapLine.Append(",");
                    if (!BrokerRow.IsFaxNumberNull())
                        CapLine.Append(BrokerRow.FaxNumber);
                    CapLine.Append(",");
                    if (!BrokerRow.IsEmailAddressNull())
                        CapLine.Append(BrokerRow.EmailAddress);
                    CapLine.Append(",");

                    string nameLine1 = "";
                    string nameLine2 = "";
                    string addressBuilding = "";
                    string addressStreet = "";
                    string addressSuburb = "";
                    string addressCity = "";
                    string addressPostalCode = "";
                    string postBox = "";
                    string postOffice = "";
                    string postalCode = "";

                    if (!AccountInfo[0].IsLENameNull())
                    {
                        if (AccountInfo[0].LegalEntityTypeKey == 2)
                            nameLine1 = nameLine1 + AccountInfo[0].LEName;
                        else
                            if (AccountInfo[0].LegalEntityTypeKey == 3)
                                nameLine1 = nameLine1 +"The Directors "+ AccountInfo[0].LEName;
                            else
                                if (AccountInfo[0].LegalEntityTypeKey == 4)
                                    nameLine1 = nameLine1 +"The Members "+ AccountInfo[0].LEName;
                                else
                                    if (AccountInfo[0].LegalEntityTypeKey == 5)
                                        nameLine1 = nameLine1 + "The Trustees " + AccountInfo[0].LEName;

                        if ((AccountInfo[0].LegalEntityTypeKey == 3)||(AccountInfo[0].LegalEntityTypeKey == 4)||(AccountInfo[0].LegalEntityTypeKey == 5))
                            nameLine2 = AccountInfo[0].LEName;
                    }

                    if (!(AccountInfo[0].IsAddressFormatKeyNull()))
                    {
                        if (AccountInfo[0].AddressFormatKey == 1)
                        {
                            if ((!AccountInfo[0].IsUnitNumberNull()) && (!AccountInfo[0].IsBuildingNumberNull()) && (!AccountInfo[0].IsBuildingNameNull()))
                            {
                                addressBuilding = AccountInfo[0].UnitNumber + " " + AccountInfo[0].BuildingNumber + " " + AccountInfo[0].BuildingName;
                            }
                            if ((!AccountInfo[0].IsStreetNumberNull()) && (!AccountInfo[0].IsStreetNameNull()))
                                addressStreet = AccountInfo[0].StreetNumber + " " + AccountInfo[0].StreetName;
                            if (!AccountInfo[0].IsRRR_SuburbDescriptionNull())
                            {
                                addressSuburb = AccountInfo[0].RRR_SuburbDescription;
                            }
                            if (!AccountInfo[0].IsRRR_CityDescriptionNull())
                                addressCity = AccountInfo[0].RRR_CityDescription;
                            if (!AccountInfo[0].IsRRR_PostalCodeNull())
                            {
                                addressPostalCode = AccountInfo[0].RRR_PostalCode;
                            }
                        }
                        else
                        {
                            if (!AccountInfo[0].IsRRR_SuburbDescriptionNull())
                            {
                                postOffice = AccountInfo[0].RRR_SuburbDescription;
                            }
                            if (!AccountInfo[0].IsRRR_PostalCodeNull())
                            {
                                postalCode = AccountInfo[0].RRR_PostalCode;
                            }

                            if (AccountInfo[0].AddressFormatKey == 2)
                            {
                                if (!AccountInfo[0].IsBoxNumberNull())
                                    postBox = "PO Box " + AccountInfo[0].BoxNumber;
                            }
                            else
                                if (AccountInfo[0].AddressFormatKey == 3)
                                {
                                    if (!AccountInfo[0].IsBoxNumberNull())
                                        postBox = "Private Bag  " + AccountInfo[0].BoxNumber;
                                }
                                else
                                    if (AccountInfo[0].AddressFormatKey == 4)
                                    {
                                        if ((!AccountInfo[0].IsSuiteNumberNull()) && (!AccountInfo[0].IsBoxNumberNull()))
                                            postBox = "Post Net Suite  " + AccountInfo[0].SuiteNumber + " Private Bag  " + AccountInfo[0].BoxNumber;
                                    }
                                    else
                                        if (AccountInfo[0].AddressFormatKey == 6)
                                        {
                                            if (!AccountInfo[0].IsBoxNumberNull())
                                                postBox = "Cluster Box " + AccountInfo[0].BoxNumber;
                                        }
                        }
                    }

                    CapLine.Append(nameLine1); CapLine.Append(",");
                    CapLine.Append(nameLine2); CapLine.Append(",");
                    CapLine.Append(addressBuilding); CapLine.Append(",");
                    CapLine.Append(addressStreet); CapLine.Append(",");
                    CapLine.Append(addressSuburb); CapLine.Append(",");
                    CapLine.Append(addressCity); CapLine.Append(",");
                    CapLine.Append(addressPostalCode); CapLine.Append(",");
                    CapLine.Append(postBox); CapLine.Append(",");
                    CapLine.Append(postOffice); CapLine.Append(",");
                    CapLine.Append(postalCode); CapLine.Append(",");

                    string spvDescription = LookupsDS.SPV.FindBySPVKey(AccountInfo[0].SPVKey).Description;
                    CapLine.Append(spvDescription); CapLine.Append(",");

                    extractFile.WriteLine(CapLine.ToString());
                }

                extractFile.Close();
                //update the batch status to completed
                BulkBatchDS._BulkBatch[0].BulkBatchStatusKey = 2;
                BulkBatchDS._BulkBatch[0].CompletedDateTime = DateTime.Now;
                UpdateStatus(BulkBatchStatuses.Successful);
                return 0;
                 */

                #endregion Old Code
            }
            catch (Exception ex)
            {
                LogPlugin.Logger.LogErrorMessageWithException("CapMailingHouseExtract.RunBatch", "CapMailingHouseExtract", ex);
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

        //private void WriteHeader(System.IO.StreamWriter extractFile)
        //{
        //    StringBuilder CapLine = new StringBuilder();

        //    CapLine.Append("Loan Number"); CapLine.Append(",");
        //    CapLine.Append("Remaining Term"); CapLine.Append(",");
        //    CapLine.Append("Loan Current Balance"); CapLine.Append(",");
        //    CapLine.Append("Loan Current Balance1"); CapLine.Append(",");
        //    CapLine.Append("Loan Current Balance2"); CapLine.Append(",");
        //    CapLine.Append("Loan Current Balance3"); CapLine.Append(",");
        //    CapLine.Append("Link Rate"); CapLine.Append(",");
        //    CapLine.Append("Loan Rate"); CapLine.Append(",");
        //    CapLine.Append("Loan Rate1"); CapLine.Append(",");
        //    CapLine.Append("Loan Rate2"); CapLine.Append(",");
        //    CapLine.Append("Loan Rate3"); CapLine.Append(",");
        //    CapLine.Append("Current Installment"); CapLine.Append(",");
        //    CapLine.Append("Installment Amount1"); CapLine.Append(",");
        //    CapLine.Append("Installment Amount2"); CapLine.Append(",");
        //    CapLine.Append("Installment Amount3"); CapLine.Append(",");
        //    CapLine.Append("Premium1"); CapLine.Append(",");
        //    CapLine.Append("Premium2"); CapLine.Append(",");
        //    CapLine.Append("Premium3"); CapLine.Append(",");
        //    CapLine.Append("Installment Diff1"); CapLine.Append(",");
        //    CapLine.Append("Installment Diff2"); CapLine.Append(",");
        //    CapLine.Append("Installment Diff3"); CapLine.Append(",");
        //    CapLine.Append("Broker Name"); CapLine.Append(",");
        //    CapLine.Append("Broker Initials"); CapLine.Append(",");
        //    CapLine.Append("Broker Fax"); CapLine.Append(",");
        //    CapLine.Append("Broker Email");  CapLine.Append(",");
        //    CapLine.Append("Name Line 1"); CapLine.Append(",");
        //    CapLine.Append("Name Line 2"); CapLine.Append(",");
        //    CapLine.Append("Address Building"); CapLine.Append(",");
        //    CapLine.Append("Address Street"); CapLine.Append(",");
        //    CapLine.Append("Address Suburb"); CapLine.Append(",");
        //    CapLine.Append("Address City"); CapLine.Append(",");
        //    CapLine.Append("Address Postal Code"); CapLine.Append(",");
        //    CapLine.Append("Post Box"); CapLine.Append(",");
        //    CapLine.Append("Post Office"); CapLine.Append(",");
        //    CapLine.Append("Postal Code"); CapLine.Append(",");
        //    CapLine.Append("SPV"); CapLine.Append(",");

        //    extractFile.WriteLine(CapLine.ToString());
        //}
    }
}