using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class LegalEntityTrust : LegalEntity, ILegalEntityTrust
    {
        #region Properties

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.
        /// </summary>
        public String RegistrationNumber
        {
            get { return _DAO.RegistrationNumber; }
            set
            {
                if (!this.IsUpdatable && value != _DAO.RegistrationNumber)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "RegistrationNumber: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                _DAO.RegistrationNumber = value;
            }
        }

        /// <summary>
        /// Domain Rule: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account
        /// and the Legal Entity does not have the Exception Status of InvalidIDNumber.
        /// </summary>
        public String RegisteredName
        {
            get { return _DAO.RegisteredName; }
            set
            {
                if (!this.IsUpdatable && value != _DAO.RegisteredName)
                {
                    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                    string msg = "RegisteredName: May not update certain Legal Entity details where a Legal Entity is connected to an open Mortgage Loan Account and the Legal Entity does not have the Exception Status of InvalidIDNumber.";
                    spc.DomainMessages.Add(new Error(msg, msg));

                    return;
                }

                _DAO.RegisteredName = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Overridden to return the registration number of the trust.
        /// </summary>
        /// <remarks>This is for display purposes only.</remarks>
        public override string LegalNumber
        {
            get
            {
                return RegistrationNumber;
            }
        }

        public override string GetLegalName(LegalNameFormat Format)
        {
            string Name = RegisteredName;
            if (!String.IsNullOrEmpty(TradingName) && String.Compare(TradingName.Trim(), RegisteredName.Trim(), true) != 0)
                Name += " trading as " + TradingName;
            return Name;
        }

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ValidateUniqueRegistrationNumber");
            //Additional Rules Added As per Ticket #8968
            Rules.Add("LegalEntityCompanyCCTrustMandatoryTradingName");
            Rules.Add("LegalEntityCompanyCCTrustMandatoryContact");
            Rules.Add("LegalEntityCompanyCCTrustMandatoryRegisteredName");
            Rules.Add("LegalEntityCompanyCCTrustMandatoryRegistrationNumber");
            //Tax number is optional - #8968
            //Rules.Add("LegalEntityCompanyCCTrustMandatoryTaxNumber");
            Rules.Add("LegalEntityCompanyCCTrustMandatoryDocumentLanguage");
            Rules.Add("LegalEntityCompanyCCTrustMandatoryLegalEntityStatus");
        }
    }
}