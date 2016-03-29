using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using System;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationInformationVariableLoan : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationVariableLoan_DAO>, IApplicationInformationVariableLoan,
        IApplicationInformationVariableLoanForSwitchAndRefinance
    {
        #region IApplicationInformationVariableLoan Members

        /// <summary>
        /// <list type="">
        /// <item>
        /// NewPurchaseApplicationInformationPropertyValuationEditForNoActiveValuation: For a New Purchase loan if there is no active Valuation record then the  ApplicationInformationVariableLoan.PropertyValuation is not editable.
        /// </item>
        /// <item>
        /// SwitchApplicationInformationVariableLoanPropertyValuationEditForNoActiveValuation: For a Switch loan If there is no active Valuation record then the  ApplicationInformationVariableLoan.PropertyValuation is editable.
        /// </item>
        /// <item>
        /// ReFinanceApplicationInformationVariableLoanPropertyValuationEditForNoActiveValuation: For a ReFinance loan If there is no active Valuation record then the  ApplicationInformationVariableLoan.PropertyValuation is editable.
        /// </item>
        /// <item>
        /// ApplicationInformationPropertyValuationEditForActiveValuation: For an Application Mortgage Loan if there is an active Valuation record then the  ApplicationInformationVariableLoan.PropertyValuation is not editable.
        /// </item>
        /// </list>
        /// </summary>
        public Double? PropertyValuation
        {
            get { return _DAO.PropertyValuation; }
            set
            {
                bool editable = true;

                if (this._DAO.ApplicationInformation.Application is ApplicationMortgageLoanRefinance_DAO)
                {
                    ApplicationMortgageLoanRefinance_DAO applicationMortgageLoan_DAO = this._DAO.ApplicationInformation.Application as ApplicationMortgageLoanRefinance_DAO;

                    if (applicationMortgageLoan_DAO != null
                        && applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property != null)
                    {
                        // Do we have any active valuations
                        if (applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property != null)
                        {
                            //foreach (Valuation_DAO valuation in applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property.Valuations)
                            //{
                            //    if (valuation.IsActive)
                            //    {
                            //        editable = false;
                            //        break;
                            //    }
                            //}
                        }
                    }

                    if (editable)
                    {
                        // Sync the ClientEstimatePropertyValuation ...
                        applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.ClientEstimatePropertyValuation = value;
                    }
                    else
                    {
                        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                        string msg = "For a Mortgage Loan Application, if there is an active Valuation record, the PropertyValuation may not be modified.";
                        spc.DomainMessages.Add(new Error(msg, msg));
                        return;
                    }
                }

                if (this._DAO.ApplicationInformation.Application is ApplicationMortgageLoanSwitch_DAO)
                {
                    ApplicationMortgageLoanSwitch_DAO applicationMortgageLoan_DAO = this._DAO.ApplicationInformation.Application as ApplicationMortgageLoanSwitch_DAO;

                    if (applicationMortgageLoan_DAO != null
                        && applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property != null)
                    {
                        // Do we have any active valuations
                        //foreach (Valuation_DAO valuation in applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property.Valuations)
                        //{
                        //    if (valuation.IsActive)
                        //    {
                        //        editable = false;
                        //        break;
                        //    }
                        //}
                    }

                    if (editable)
                    {
                        // Sync the ClientEstimatePropertyValuation ...
                        applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.ClientEstimatePropertyValuation = value;
                    }
                    else
                    {
                        SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                        string msg = "For a Mortgage Loan Application, if there is an active Valuation record, the PropertyValuation may not be modified.";
                        spc.DomainMessages.Add(new Error(msg, msg));
                        return;
                    }
                }

                if (this._DAO.ApplicationInformation.Application is ApplicationMortgageLoanNewPurchase_DAO)
                {
                    ApplicationMortgageLoanNewPurchase_DAO applicationMortgageLoan_DAO = this._DAO.ApplicationInformation.Application as ApplicationMortgageLoanNewPurchase_DAO;

                    if (applicationMortgageLoan_DAO != null
                        && applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property != null)
                    {
                        // Do we have any active valuations
                        foreach (Valuation_DAO valuation in applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.Property.Valuations)
                        {
                            if (valuation.IsActive)
                            {
                                editable = false;
                                break;
                            }
                        }
                    }

                    if (editable)
                    {
                        // Sync the ClientEstimatePropertyValuation ...
                        applicationMortgageLoan_DAO.ApplicationMortgageLoanDetail.ClientEstimatePropertyValuation = value;
                    }
                    //Can change the purchase price/prop val for new purchase
                    //else
                    //{
                    //    SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

                    //    string msg = "For a Mortgage Loan Application, if there is an active Valuation record, the PropertyValuation may not be modified.";
                    //    spc.DomainMessages.Add(new Error(msg, msg));
                    //    return;
                    //}
                }

                _DAO.PropertyValuation = value;
            }
        }

        public Double? LoanAgreementAmount
        {
            get { return _DAO.LoanAgreementAmount; }
            //set { _DAO.LoanAgreementAmount = value; }
        }

        /// <summary>
        /// The sum total of all the Fees applicable to the Application. Consists of fees such as the Cancellation Fee, an Initiation Fee
        /// and the Registration Fee.
        /// </summary>
        public Double? FeesTotal
        {
            get { return _DAO.FeesTotal; }
            set
            {
                _DAO.FeesTotal = value;
                //need to use a common method to do this
                ApplicationProductMortgageLoanHelper.SetLoanAgreementAmount(Application.CurrentProduct as IApplicationProductMortgageLoan);
            }
        }

        /// <summary>
        /// The Loan Amount with No Fees
        /// </summary>
        public Double? LoanAmountNoFees
        {
            get { return _DAO.LoanAmountNoFees; }
            set
            {
                _DAO.LoanAmountNoFees = value;
                //need to use a common method to do this
                ApplicationProductMortgageLoanHelper.SetLoanAgreementAmount(Application.CurrentProduct as IApplicationProductMortgageLoan);
            }
        }

        /// <summary>
        /// The percentage discount applied to the initiation fee
        /// </summary>
        public Double? AppliedInitiationFeeDiscount
        {
            get
            {
                return _DAO.AppliedInitiationFeeDiscount;
            }
            set
            {
                _DAO.AppliedInitiationFeeDiscount = value;
            }
        }

        private IApplication Application
        {
            get
            {
                if (_DAO == null || _DAO.ApplicationInformation == null || _DAO.ApplicationInformation.Application == null)
                    return null;

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.ApplicationInformation.Application);
            }
        }

        #endregion IApplicationInformationVariableLoan Members

        #region IApplicationInformationVariableLoanForSwitchAndRefinance Members

        public double? RequestedCashAmount
        {
            get
            {
                return _DAO.RequestedCashAmount;
            }
            set
            {
                _DAO.RequestedCashAmount = value;
            }
        }

        #endregion IApplicationInformationVariableLoanForSwitchAndRefinance Members
    }
}