using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.CreditScoring
{
    [RuleDBTag("ITCDisputeIndicated",
   "",
   "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCDisputeIndicated")]
    [RuleInfo]
    public class ITCDisputeIndicated : BusinessRuleBase
    {
        public ITCDisputeIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            // assume AccountKey, LEKey, AccountKey passed in
            //if (Parameters.Length < 1)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCDisputeIndicated, expected 1 of type IITC");

            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;
            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCDisputeIndicated");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();

            if (!string.IsNullOrEmpty(Response))
            {
                string Err = string.Format("{0} {1} has disputes indicated.", lenp.FirstNames, lenp.Surname);
                AddMessage(Err, "", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("ITCDebtReviewIndicated",
       "",
       "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCDebtReviewIndicated ")]
    [RuleInfo]
    public class ITCDebtReviewIndicated : BusinessRuleBase
    {
        public ITCDebtReviewIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            // assume AccountKey, LEKey, AccountKey passed in
            //if (Parameters.Length < 2)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCDebtReviewIndicated, expected 1 of type IITC");

            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;
            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCDebtReviewIndicated");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();

            if (!string.IsNullOrEmpty(Response))
            {
                string Err = string.Format("{0} {1} is under debt review.", lenp.FirstNames, lenp.Surname);
                AddMessage(Err, "", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("ITCAccountJudgementsIndicated",
       "",
       "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCAccountJudgementsIndicated")]
    [RuleInfo]
    public class ITCAccountJudgementsIndicated : BusinessRuleBase
    {
        public ITCAccountJudgementsIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            // assume AccountKey, LEKey, AccountKey passed in
            //if (Parameters.Length != 2)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCAccountJudgementsIndicated, expected 2 of type IITC and int");

            int ApplicationKey = Convert.ToInt32(Parameters[0]);
            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCAccountJudgementsIndicated");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();
            int Months = 0;

            if (int.TryParse(Response, out Months))
            {
                if (Months >= 0 && Months <= 12)
                {
                    //check if the client has provided a Letter of Settlement
                    IDocumentCheckListRepository dcRepo = RepositoryFactory.GetRepository<IDocumentCheckListRepository>();
                    IList<IApplicationDocument> docs = dcRepo.GetApplicationDocumentsForApplication(ApplicationKey);

                    bool settled = false;

                    for (int i = 0; i < docs.Count; i++)
                    {
                        // used Desc, not sure if keys are managed. THis isnt a lookup
                        if (docs[i].DocumentType.Description == "ITC Judgement - Letter Of Settlement")
                        {
                            settled = true;
                            break;
                        }
                    }

                    if (!settled)
                    {
                        string Err = string.Format("{0} {1} has Judgements currently against them.", lenp.FirstNames, lenp.Surname);
                        AddMessage(Err, "", Messages);
                        return 1;
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ITCAccountLegalNoticesIndicated",
       "",
       "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCAccountLegalNoticesIndicated")]
    [RuleInfo]
    public class ITCAccountLegalNoticesIndicated : BusinessRuleBase
    {
        public ITCAccountLegalNoticesIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //if (Parameters.Length != 1)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCAccountLegalNoticesIndicated, expected 1 of type IITC");

            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCAccountLegalNoticesIndicated");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();

            int Count = 0;
            if (int.TryParse(Response, out Count))
            {
                if (Count > 0)
                {
                    string Err = string.Format("{0} {1} has legal notices currently against them.", lenp.FirstNames, lenp.Surname);
                    AddMessage(Err, "", Messages);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ITCAccountDefaultsIndicated",
   "",
   "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCAccountDefaultsIndicated")]
    [RuleInfo]
    public class ITCAccountDefaultsIndicated : BusinessRuleBase
    {
        public ITCAccountDefaultsIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //if (Parameters.Length != 1)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCAccountDefaultsIndicated, expected 1 of type IITC");

            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCAccountDefaultsIndicated");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();
            int Count = 0;
            if (int.TryParse(Response, out Count))
            {
                if (Count >= 0 && Count <= 7)
                {
                    string Err = string.Format("{0} {1} has defaults currently against them.", lenp.FirstNames, lenp.Surname);
                    AddMessage(Err, "", Messages);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ITCAccountCustomerWorstEverPaymentProfileStatus",
"",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCAccountCustomerWorstEverPaymentProfileStatus")]
    [RuleInfo]
    public class ITCAccountCustomerWorstEverPaymentProfileStatus : BusinessRuleBase
    {
        public ITCAccountCustomerWorstEverPaymentProfileStatus(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        [SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation")]
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            // assume AccountKey, LEKey, AccountKey passed in
            //if (Parameters.Length != 1)
            //    throw new ArgumentException("Incorrect no of params passed to rule:ITCAccountCustomerWorstEverPaymentProfileStatus, expected 1 of type IITC");

            IITC itc = (IITC)Parameters[1];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ITCAccountCustomerWorstEverPaymentProfileStatus");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();
            int Score = 0;
            if (int.TryParse(Response, out Score))
            {
                if (Score >= 4)
                {
                    string Err = string.Format("{0} {1} has a poor payment profile.", lenp.FirstNames, lenp.Surname);
                    AddMessage(Err, "", Messages);
                    return 1;
                }
                if (Score == -2)
                {
                    string Err = string.Format("{0} {1} has a poor payment profile.", lenp.FirstNames, lenp.Surname);
                    AddMessage(Err, "", Messages);
                    return 2;
                }
            }
            else if (Response.ToUpper() == "E" || Response.ToUpper() == "H" || Response.ToUpper() == "I" || Response.ToUpper() == "J" ||
                Response.ToUpper() == "L" || Response.ToUpper() == "W" || Response.ToUpper() == "Z" || Response.ToUpper() == "AA" || Response.ToUpper() == "AC")
            {
                string Err = string.Format("{0} {1} has a poor payment profile.", lenp.FirstNames, lenp.Surname);
                AddMessage(Err, "", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("ITCValidEmpericaScore",
    "",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.CreditScoring.ITCValidEmpericaScore")]
    [RuleInfo]
    public class ITCValidEmpericaScore : BusinessRuleBase
    {
        public ITCValidEmpericaScore(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        [SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", Justification = "exclusion is used repeatedly, simpler to read/use by creating a copy")]
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //if (Parameters.Length != 2)
            //    throw new Exception(string.Format("Incorrect no of params passed to rule:ITCValidEmpericaScore, expected 1 of type IITC"));

            IITC itc = (IITC)Parameters[1];
            int riskMatrixRevisionKey = (int)Parameters[2];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            // check for valid emperica score
            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ValidEmpericaScore");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms);

            string Err = string.Format("{0} {1} has missing empirica score and will not be used for credit scoring.", lenp.FirstNames, lenp.Surname);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || (ds.Tables[0].Rows[0]["Score"] == DBNull.Value && ds.Tables[0].Rows[0]["Exclusion"] == DBNull.Value))
            {
                AddMessage(Err, "", Messages);
                return 1;
            }

            DataRow row = ds.Tables[0].Rows[0];
            double EScore = -1;

            if (row["Exclusion"] != DBNull.Value)
            {
                string exclusion = Convert.ToString(row["Exclusion"]).Trim().ToUpper();

                if (!String.IsNullOrEmpty(exclusion))
                {
                    switch (exclusion)
                    {
                        case "D":
                            Err += " (Reason code D - Consumer is deceased)";
                            break;
                        case "N":
                            Err += " (Reason code N - Insufficient data to be scored)";
                            break;
                        case "L":
                            Err += " (Reason code L - Legal)";
                            break;
                        case "Z":
                            Err += " (Reason code Z - EMPIRICA is not available)";
                            break;
                        default:
                            Err += " (Reason code " + exclusion + " - Unknown)";
                            break;
                    }

                    AddMessage(Err, "", Messages);
                    return 1;
                }
            }

            if (row["Score"] != DBNull.Value)
            {
                string score = Convert.ToString(row["Score"]);
                double.TryParse(score, out EScore);
            }

            //check the actual score is not less than this:
            SQL = UIStatementRepository.GetStatement("CreditScoring", "GetMinimumEmpiricaScoreForRiskMatrix");
            prms.Clear();
            prms.Add(new SqlParameter("@RiskMatrixRevisionKey", riskMatrixRevisionKey));
            object obj = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();

            if (obj == DBNull.Value || String.IsNullOrEmpty(Convert.ToString(obj)))
            {
                return 0; //no minimum
            }
            else
            {
                double min = Convert.ToDouble(obj);

                if (EScore < min)
                {
                    Err = string.Format("{0} {1} has an empirica score below the defined minimum and will not be used for credit scoring.", lenp.FirstNames, lenp.Surname);
                    AddMessage(Err, "", Messages);
                    return 1;
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ITCValidSBC",
"",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.CreditScoring.ITCValidSBC")]
    [RuleInfo]
    public class ITCValidSBC : BusinessRuleBase
    {
        public ITCValidSBC(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            //if (Parameters.Length != 2)
            //    throw new Exception(string.Format("Incorrect no of params passed to rule:ITCValidSBC, expected 1 of type IITC"));

            IITC itc = (IITC)Parameters[1];
            IScoreCard scoreCard = (IScoreCard)Parameters[3];
            LegalEntityNaturalPerson lenp = (LegalEntityNaturalPerson)itc.LegalEntity;

            string SQL = UIStatementRepository.GetStatement("CreditScoring", "ValidSBCValues");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ITCKey", itc.Key));
            string Response = castleTransactionService.ExecuteScalarOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms).ToString();
            try
            {
                string Err = string.Empty;
                XmlDocument xd = new XmlDocument();

                // get rid of the namespace to make XPath easy.
                Response = Response.Replace("https://secure.transunion.co.za/TUBureau", "");
                xd.LoadXml(Response);
                XmlNode xn = null;

                //ScoreCardAttribute_DAO[] attribs = ScoreCardAttribute_DAO.FindAll();

                foreach (IScoreCardAttribute attr in scoreCard.ScoreCardAttributes)
                {
                    double value = 0;
                    string code = attr.Code.Trim();
                    xn = xd.SelectSingleNode(String.Format("//StandardBatchCharsSB33/{0}", code));

                    if (null == xn)
                    {
                        Err = string.Format("{0} {1} has missing SBC attribute: {2} and will not be used for CreditScoring", lenp.FirstNames, lenp.Surname, code);
                        AddMessage(Err, "", Messages);
                    }
                    else if (!double.TryParse(xn.InnerText, out value))
                    {
                        Err = string.Format("{0} {1} has invalid SBC attribute: {2} and will not be used for CreditScoring", lenp.FirstNames, lenp.Surname, code);
                        AddMessage(Err, "", Messages);
                    }
                    else
                    {
                        bool matched = false;

                        foreach (IScoreCardAttributeRange range in attr.ScoreCardAttributeRanges)
                        {
                            if (!range.Min.HasValue && !range.Max.HasValue)
                            {
                                matched = true;
                                break;
                            }

                            if (!range.Min.HasValue)
                                if (value <= range.Max.Value)
                                {
                                    matched = true;
                                    break;
                                }
                                else
                                    continue;

                            if (!range.Max.HasValue)
                                if (value >= range.Min.Value)
                                {
                                    matched = true;
                                    break;
                                }
                                else
                                    continue;

                            if (value >= range.Min.Value && value <= range.Max.Value)
                            {
                                matched = true;
                                break;
                            }
                        }

                        if (!matched)
                        {
                            Err = string.Format("{0} {1} has an out-of-range SBC attribute: {2} and will not be used for CreditScoring", lenp.FirstNames, lenp.Surname, code);
                            AddMessage(Err, "", Messages);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Err))
                    return 1;
            }
            catch (XmlException)
            {
                string Err = string.Format("{0} {1} has missing or invalid SBC score(s) and will not be used for credit scoring.", lenp.FirstNames, lenp.Surname);
                AddMessage(Err, "", Messages);
                return 1;
            }
            return 0;
        }
    }
}