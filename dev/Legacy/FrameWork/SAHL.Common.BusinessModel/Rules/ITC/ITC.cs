using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Utils;

namespace SAHL.Common.BusinessModel.Rules.ITC
{
    [RuleDBTag("ITCApplication",
    "Each Legal Entity in an application must have a valid ITC.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCApplication")]
    [RuleInfo]
    public class ITCApplication : BusinessRuleBase
    {
        public ITCApplication(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameter[0] is not of type IApplication.");

            IApplication App = (IApplication)Parameters[0];
            int appKey = App.Key;
            int accKey = App.ReservedAccount.Key;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@accKey", accKey));
            prms.Add(new SqlParameter("@appKey", appKey));

            // adding a check - if the ApplicationType is ReAdvance AND the LTV is under 80% then we can exit - no need to do this rule check.
            if (App.ApplicationType.Key == (int)OfferTypes.ReAdvance)
            {
                double ltv = 1;
                string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ReadvanceLTV");

                object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);
                if (o != null)
                    ltv = (double)0;

                if (ltv < 0.8)
                    return 0;
            }

            string sqlQuery1 = UIStatementRepository.GetStatement("COMMON", "ITCApplicationCheck");
            object ob = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery1, typeof(GeneralStatus_DAO), prms);

            if (ob != null)
            {
                AddMessage("Every Legal Entity must have a valid ITC enquiry.", "Every Legal Entity must have a valid ITC enquiry.", Messages);
                return 1;
            }
            return 0;
        }
    }

    [RuleDBTag("ITCRequestLegalEntityNaturalPersonAddress",
    "An ITC can only be done on a Legal Entity with a valid residential or postal street address.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCRequestLegalEntityNaturalPersonAddress")]
    [RuleInfo]
    public class ITCRequestLegalEntityNaturalPersonAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IITC))
                throw new ArgumentException("Rule ITCRequestLegalEntityNaturalPersonAddress: Parameter[0] is not of type IITC.");

            bool addressFound = false;

            IITC itc = (IITC)Parameters[0];
            ILegalEntityNaturalPerson lenp = itc.LegalEntity as ILegalEntityNaturalPerson;

            string errMsg = "Legal Entity: " + lenp.DisplayName + " requires a valid street address before requesting an ITC.";

            if (lenp != null) // seperate rule to check the LE is a Natural Person, dont need to worry here
            {
                foreach (ILegalEntityAddress add in lenp.LegalEntityAddresses)
                {
                    if ((add.AddressType.Key == (int)AddressTypes.Postal || add.AddressType.Key == (int)AddressTypes.Residential)
                        && add.Address.AddressFormat.Key == (int)AddressFormats.Street)
                    {
                        addressFound = true;
                        break;
                    }
                }

                if (!addressFound)
                    AddMessage(errMsg, errMsg, Messages);
            }

            return 0;
        }
    }

    [RuleDBTag("ITCRequestLegalEntityNaturalPersonIDNumber",
    "An ITC can only be done on a Legal Entity with a valid ID number. ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCRequestLegalEntityNaturalPersonIDNumber")]
    [RuleInfo]
    public class ITCRequestLegalEntityNaturalPersonIDNumber : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IITC))
                throw new ArgumentException("Rule ITCRequestLegalEntityNaturalPersonIDNumber: Parameter[0] is not of type IITC.");

            IITC itc = (IITC)Parameters[0];
            ILegalEntityNaturalPerson lenp = itc.LegalEntity as ILegalEntityNaturalPerson;

            if (lenp != null) // seperate rule to check the LE is a Natural Person, dont need to worry here
            {
                // if the LE is a foreigner and has no id number then do not validate
                if (lenp.CitizenType != null && String.IsNullOrEmpty(lenp.IDNumber) &&
                        (
                            lenp.CitizenType.Key == (int)CitizenTypes.Foreigner
                            || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentConsulate
                            || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentDiplomat
                            || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentHighCommissioner
                            || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentRefugee
                            || lenp.CitizenType.Key == (int)CitizenTypes.NonResident
                        )
                    )
                    return 0;

                if (lenp.IDNumber == null || lenp.IDNumber.Length == 0 || !ValidationUtils.ValidateID(lenp.IDNumber.Replace(" ", String.Empty)))
                {
                    string errMsg = "Legal Entity: " + lenp.DisplayName + " requires a valid ID Number before requesting an ITC";
                    AddMessage(errMsg, errMsg, Messages);
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ITCRequestLegalEntityNaturalPersonIDNumberForeigner",
    "ITC - Warn if a Foreigner does not have a ID Number. ",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCRequestLegalEntityNaturalPersonIDNumberForeigner")]
    [RuleInfo]
    public class ITCRequestLegalEntityNaturalPersonIDNumberForeigner : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IITC))
                throw new ArgumentException("Rule ITCRequestLegalEntityNaturalPersonIDNumberForeigner: Parameter[0] is not of type IITC.");

            IITC itc = (IITC)Parameters[0];
            ILegalEntityNaturalPerson lenp = itc.LegalEntity as ILegalEntityNaturalPerson;

            // if the LE is a foreigner and has no id number then fire warning
            if (lenp != null) // seperate rule to check the LE is a Natural Person, dont need to worry here
            {
                if (lenp.CitizenType != null && String.IsNullOrEmpty(lenp.IDNumber) &&
                        (
                               lenp.CitizenType.Key == (int)CitizenTypes.Foreigner
                               || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentConsulate
                               || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentDiplomat
                               || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentHighCommissioner
                               || lenp.CitizenType.Key == (int)CitizenTypes.NonResidentRefugee
                               || lenp.CitizenType.Key == (int)CitizenTypes.NonResident
                        )
                    )
                {
                    if (String.IsNullOrEmpty(lenp.IDNumber))
                    {
                        string errMsg = "Legal Entity: " + lenp.DisplayName + " is a foreign citizen without a valid ID Number, no ITC can be requested without valid ID number.";
                        AddMessage(errMsg, errMsg, Messages);
                    }
                }
            }
            return 0;
        }
    }

    [RuleDBTag("ITCRequestLegalEntityNaturalPerson",
   "An ITC can only be done on a Legal Entity of type Natural Person.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCRequestLegalEntityNaturalPerson")]
    [RuleInfo]
    public class ITCRequestLegalEntityNaturalPerson : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IITC))
                throw new ArgumentException("Rule ITCRequestLegalEntityNaturalPerson: Parameter[0] is not of type IITC.");

            IITC itc = (IITC)Parameters[0];
            ILegalEntityNaturalPerson lenp = itc.LegalEntity as ILegalEntityNaturalPerson;

            string errMsg = "Legal Entity: " + itc.LegalEntity.DisplayName + ". An ITC can only be done on a Legal Entity of type Natural Person.";

            if (lenp == null)
                AddMessage(errMsg, errMsg, Messages);

            return 0;
        }
    }

    [RuleDBTag("ITCApplicationITCperLegalEntity",
   "For each Legal Entity of Type Natural Person  should have a valid less than 1 month old ITC.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.ITC.ITCApplicationITCperLegalEntity")]
    [RuleParameterTag(new string[] { "@ITCValidDays,31,9" })]
    [RuleInfo]
    public class ITCApplicationITCperLegalEntity : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplicationMortgageLoan))
                throw new ArgumentException("Rule ITCApplicationITCperLegalEntity: Parameter[0] is not of type IApplicationMortgageLoan.");

            IApplicationMortgageLoan appML = Parameters[0] as IApplicationMortgageLoan;

            foreach (IApplicationRole ar in appML.ApplicationRoles)
            {
                if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                {
                    ILegalEntityNaturalPerson lenp = ar.LegalEntity as ILegalEntityNaturalPerson;
                    if (lenp != null)
                    {
                        string errMsg = "Legal Entity: " + lenp.DisplayName + ". For each Legal Entity of Type Natural Person the application should have a valid ITC.";

                        //if (lenp.ITCs == null || lenp.ITCs.Count == 0)
                        //    AddMessage(errMsg, errMsg, Messages);
                        //else
                        //{
                        bool addErr = false;
                        bool found = false;
                        foreach (IITC itc in lenp.ITCs)//Will always only have one, but least error free way is to loop.
                        {
                            if (itc.ReservedAccount.Key == appML.ReservedAccount.Key)
                            {
                                found = true;

                                int ITCValidDays = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

                                if (itc.ChangeDate < DateTime.Now.AddDays(-ITCValidDays))
                                {
                                    errMsg += " The ITC is more than a month old.";
                                    addErr = true;
                                }

                                if (!itc.ResponseXML.Contains(lenp.IDNumber))
                                {
                                    errMsg += " The IDNumber for the Legal Entity is not the same as the IDNumer in the ITC report.";
                                    addErr = true;
                                }

                                if (addErr)
                                    AddMessage(errMsg, errMsg, Messages);
                            }
                        }

                        //}
                        if (!found)
                            AddMessage(errMsg, errMsg, Messages);
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ITCRequestFrequency",
    "User should not be allowed to redo an ITC within 5 days of extraction one only if the original ITC was successful and without errors.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCRequestFrequency")]
    [RuleParameterTag(new string[] { "@ITCMinRedoDays,5,9", "@DisputeIndicated,DisputeIndicator, 12", "@FraudPrevention,SAFPSNF01, 12", "@ErrorMessage,ErrorMessage, 12" })]
    [RuleInfo]
    public class ITCRequestFrequency : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IITC))
                throw new ArgumentException("Rule ITCRequestFrequency: Parameter[0] is not of type IITC.");

            IITC newitc = (IITC)Parameters[0];

            if (newitc.LegalEntity != null)
            {
                ILegalEntityNaturalPerson lenp = newitc.LegalEntity as ILegalEntityNaturalPerson;
                if (lenp != null && lenp.ITCs != null && lenp.ITCs.Count > 0)
                {
                    int ITCMinRedoDays = Convert.ToInt32(RuleItem.RuleParameters[0].Value);
                    foreach (IITC itc in lenp.ITCs)
                    {
                        if (itc.ChangeDate > DateTime.Now.AddDays(-ITCMinRedoDays) && newitc.ReservedAccount.Key == itc.ReservedAccount.Key)
                        {
                            bool addRule = true;

                            //loop through all possible errors that should be ignored when redoing an ITC
                            for (int i = 0; i < RuleItem.RuleParameters.Count; i++)
                            {
                                string desc = RuleItem.RuleParameters[i].Value;
                                if (desc != ITCMinRedoDays.ToString()) //make sure we dont validate against this number
                                {
                                    if (itc.ResponseXML.Contains(desc))
                                    {
                                        addRule = false;
                                        break;
                                    }
                                }
                            }

                            if (addRule)
                            {
                                string err = "ITC for " + lenp.DisplayName + " is valid and less than " + ITCMinRedoDays + " days old. You can not redo this ITC.";
                                AddMessage(err, err, Messages);
                            }
                        }
                    }
                }
            }

            return 0;
        }
    }

    [RuleDBTag("ITCAccountApplicationDisputeIndicated",
    "Determine if any ITC enquiries for an Account contain disputes.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCAccountApplicationDisputeIndicated")]
    [RuleInfo]
    public class ITCAccountApplicationDisputeIndicated : BusinessRuleBase
    {
        public ITCAccountApplicationDisputeIndicated(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication) && !(Parameters[0] is IAccount))
                throw new ArgumentException("Rule ITCAccountApplicationDisputeIndicated: Parameter[0] is not of type IApplication or IAccount.");

            int accountKey = 0;

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                accountKey = app.ReservedAccount.Key;
            }

            if (Parameters[0] is IAccount)
            {
                IAccount acc = Parameters[0] as IAccount;
                accountKey = acc.Key;
            }

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ITCDisputeByAccountKey");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (o != null)
                AddMessage(o.ToString(), o.ToString(), Messages);

            return 0;
        }
    }

    [RuleDBTag("ITCApplicationEmpiricaScore",
    "Determine if any ITC enquiries for an Application contain the minimum required EmpiricaScore.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.ITC.ITCApplicationEmpiricaScore")]
    [RuleInfo]
    public class ITCApplicationEmpiricaScore : BusinessRuleBase
    {
        public ITCApplicationEmpiricaScore(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication) && !(Parameters[0] is IAccount))
                throw new ArgumentException("Rule ITCApplicationEmpiricaScore: Parameter[0] is not of type IApplication.");

            int appKey = 0;

            if (Parameters[0] is IApplication)
            {
                IApplication app = Parameters[0] as IApplication;
                appKey = app.Key;
            }

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "ITCApplicationEmpiricaScoreAndLTV");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OfferKey", appKey));

            object o = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (o != null)
                AddMessage(o.ToString(), o.ToString(), Messages);

            return 0;
        }
    }
}