using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Web;
using SAHL.Common;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.EstateAgent
{
    /// <summary>
    /// Mandatory data stored in the LegalEntity Table for EstateAgents and EstateAgencies
    /// </summary>
    [RuleDBTag("LegalEntityEstateAgentMandatoryFields",
    "Mandatory fields when saving an EstateAgent or EstateAgency",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.EstateAgent.LegalEntityEstateAgentMandatoryFields")]
    [RuleInfo]
    public class LegalEntityEstateAgentMandatoryFields : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityEstateAgentMandatoryFields rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The LegalEntityEstateAgentMandatoryFields rule expects the following objects to be passed: ILegalEntity");

            #endregion Check for allowed object type(s)

            ILegalEntity le = Parameters[0] as ILegalEntity;
            ILegalEntityNaturalPerson leNP = le as ILegalEntityNaturalPerson;
            ILegalEntityCompany leC = le as ILegalEntityCompany;

            #region these checks already covered by other LE Rules

            //#region EstateAgent
            //if (le != null && leNP != null)
            //{
            //    // etateagent so need to check the following fields : FirstName, LastName, ID/PassportNumber
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.FirstNames))
            //    {
            //        string errorMessage = "Legal Entity First Name Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.Surname))
            //    {
            //        string errorMessage = "Legal Entity Surname Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.IDNumber) && Utils.StringUtils.IsNullOrEmptyTrimmed(leNP.PassportNumber))
            //    {
            //        string errorMessage = "Legal Entity ID Number or Passport Number Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //}
            //#endregion
            //#region EstateAgency
            //if (le != null && leC != null)
            //{
            //    // estateagency so need to check the following fields : TradingName, RegisteredName, RegistrationNumber
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leC.TradingName))
            //    {
            //        string errorMessage = "Legal Entity Trading Name Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leC.RegisteredName))
            //    {
            //        string errorMessage = "Legal Entity Registered Name Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //    if (Utils.StringUtils.IsNullOrEmptyTrimmed(leC.RegistrationNumber))
            //    {
            //        string errorMessage = "Legal Entity Registration Number Required";
            //        AddMessage(errorMessage, errorMessage, Messages);
            //    }
            //}
            //#endregion

            #endregion these checks already covered by other LE Rules

            #region ContactDetails

            // for both types need to check the following fields : 1 contactphonenumber, emailaddress
            if (Utils.StringUtils.IsNullOrEmptyTrimmed(le.EmailAddress)
                || (Utils.StringUtils.IsNullOrEmptyTrimmed(le.CellPhoneNumber)
                && (Utils.StringUtils.IsNullOrEmptyTrimmed(le.HomePhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(le.HomePhoneNumber))
                && (Utils.StringUtils.IsNullOrEmptyTrimmed(le.WorkPhoneCode) || Utils.StringUtils.IsNullOrEmptyTrimmed(le.WorkPhoneNumber))))
            {
                string errorMessage = "All contact fields need to be completed.";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            #endregion ContactDetails

            return 1;
        }
    }

    /// <summary>
    /// Checks that a physical and postal address have been assigned to this le.
    /// </summary>
    [RuleDBTag("LegalEntityEstateAgencyMandatoryAddress",
    "Mandatory addresses when saving an EstateAgency",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.EstateAgent.LegalEntityEstateAgencyMandatoryAddress")]
    [RuleInfo]
    public class LegalEntityEstateAgencyMandatoryAddress : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityEstateAgencyMandatoryAddress rule expects a Domain object to be passed.");

            if (!(Parameters[0] is ILegalEntity))
                throw new ArgumentException("The LegalEntityEstateAgencyMandatoryAddress rule expects the following objects to be passed: ILegalEntity");

            #endregion Check for allowed object type(s)

            ILegalEntity le = Parameters[0] as ILegalEntity;
            ILegalEntityCompany leC = le as ILegalEntityCompany;

            bool postalAddress = false;
            bool residentialAddress = false;

            #region EstateAgency

            if (le != null && leC != null)
            {
                // its a company so check address
                if (leC.LegalEntityAddresses.Count > 0)
                {
                    foreach (ILegalEntityAddress leAddress in leC.LegalEntityAddresses)
                    {
                        if (leAddress.AddressType.Key == (int)AddressTypes.Postal)
                            postalAddress = true;
                        if (leAddress.AddressType.Key == (int)AddressTypes.Residential)
                            residentialAddress = true;
                    }
                }
            }

            #endregion EstateAgency

            #region CheckIfBothAddresses

            if (!postalAddress && !residentialAddress)
            {
                string errorMessage = "Both Physical and Residential Addresses are required";
                AddMessage(errorMessage, errorMessage, Messages);
            }

            #endregion CheckIfBothAddresses

            return 1;
        }
    }

    /// <summary>
    /// Checks that a Orgstructure item has not got any children.
    /// </summary>
    [RuleDBTag("EstateAgentDeleteParent",
   "An Estate Agent Parent item can not be deleted if there are children.",
   "SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.EstateAgent.EstateAgentDeleteParent")]
    [RuleInfo]
    public class EstateAgentDeleteParent : BusinessRuleBase
    {
        public EstateAgentDeleteParent(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IOrganisationStructure))
                throw new ArgumentException("Parameter[0] is not of type IOrganisationStructure.");

            IOrganisationStructure org = (IOrganisationStructure)Parameters[0];

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@orgKey", org.Key));

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "EstateAgentDeleteParent");
            object ob = castleTransactionService.ExecuteScalarOnCastleTran(sqlQuery, typeof(GeneralStatus_DAO), prms);

            if (ob != null)
            {
                string errMsg = "An Estate Agent Parent item can not be deleted if there are children.";
                AddMessage(errMsg, errMsg, Messages);
                return 1;
            }
            return 0;
        }
    }

    /// <summary>
    /// Checks if there is a principle attached to this LE agency
    /// </summary>
    [RuleDBTag("LegalEntityEstateAgencyCheckForPrinciple",
    "Warning to check for Principle attached to agency",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.EstateAgent.LegalEntityEstateAgencyCheckForPrinciple", false)]
    [RuleInfo]
    public class LegalEntityEstateAgencyCheckForPrinciple : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegalEntityEstateAgencyCheckForPrinciple rule expects a IEstateAgentOrganisationNode be passed.");

            if (!(Parameters[0] is IEstateAgentOrganisationNode))
                throw new ArgumentException("The LegalEntityEstateAgencyCheckForPrinciple rule expects the following objects to be passed: IEstateAgentOrganisationNode");

            #endregion Check for allowed object type(s)

            IEstateAgentOrganisationNode eaon = Parameters[0] as IEstateAgentOrganisationNode;
            ILegalEntityNaturalPerson lenp = eaon.GetEstateAgentPrincipal();

            if (lenp == null)
            {
                string errMsg = "There is no Principal attached to this Estate Agency.";
                AddMessage(errMsg, errMsg, Messages);
                return 1;
            }

            return 0;
        }
    }

    //    [RuleDBTag("OneLegalEntityInstanceInOrgStructure",
    //"An estate agent or Agency can only exist once in an org structure",
    //"SAHL.Rules.DLL",
    //"SAHL.Common.BusinessModel.Rules.EstateAgent.OneLegalEntityInstanceInOrgStructure")]
    //    [RuleInfo]
    //    public class OneLegalEntityInstanceInOrgStructure : BusinessRuleBase
    //    {
    //        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
    //        {
    //            ILegalEntityOrganisationStructure los = Parameters[0] as ILegalEntityOrganisationStructure;
    //            if (los != null)
    //            {
    //                string query = UIStatementRepository.GetStatement("COMMON", "OneLegalEntityInstanceInOrgStructure");
    //                ParameterCollection pc = new ParameterCollection();
    //                pc.Add(new SqlParameter("@LEKey", los.LegalEntity.Key));
    //                pc.Add(new SqlParameter("@OrganisationTypeKey", los.OrganisationStructure.OrganisationType.Key));
    //                int cnt = 0;
    //                DataSet ds = AbstractRepositoryBase.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
    //                if (ds.Tables.Count > 0)
    //                {
    //                    foreach (DataRow dr in ds.Tables[0].Rows)
    //                    {
    //                        cnt = Convert.ToInt32(dr[0]);
    //                        if (cnt > 0)
    //                        {
    //                            string msg = string.Format("This Legal Entity can only exist once in the organisation structure.");
    //                            AddMessage(msg, msg, Messages);
    //                            return 0;
    //                        }
    //                    }
    //                }

    //            }

    //            return 1;

    //        }

    //    }

    [RuleDBTag("LegelEntityEstateAgencyOnlyOnePrinciple",
    "Checks if there is already a principal attached. If there is then throw a message.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.EstateAgent.LegelEntityEstateAgencyOnlyOnePrinciple")]
    [RuleInfo]
    public class LegelEntityEstateAgencyOnlyOnePrinciple : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)

            if (Parameters.Length == 0)
                throw new ArgumentException("The LegelEntityEstateAgencyOnlyOnePrinciple rule expects a Domain object to be passed.");

            if (!(Parameters[0] is IEstateAgentOrganisationNode))
                throw new ArgumentException("The LegelEntityEstateAgencyOnlyOnePrinciple rule expects the following objects to be passed: IEstateAgentOrganisationNode");

            #endregion Check for allowed object type(s)

            IEstateAgentOrganisationNode eaon = Parameters[0] as IEstateAgentOrganisationNode;
            int count = 0;
            IEventList<ILegalEntityOrganisationNode> childLeons = eaon.Parent.ChildOrganisationStructures;
            foreach (ILegalEntityOrganisationNode item in childLeons)
            {
                // IEstateAgentOrganisationNode
                if (item.OrganisationType.Key == (int)OrganisationTypes.Designation
                    && item.Description == Constants.EstateAgent.Principal
                    && item.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    count = item.LegalEntities.Count;
            }

            if (count > 1)
            {
                string msg = string.Format("There is already a principal estate agent at this point.");
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }
}