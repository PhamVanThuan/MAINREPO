using System;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationFurtherAdvance : Application, IApplicationFurtherAdvance
    {
        private DAOEventList<ApplicationInformation_DAO, IFurtherAdvanceApplicationInformation, FurtherAdvanceApplicationInformation> _furtherAdvanceApplicationInformations;

        ApplicationMortgageLoanHelper _applicationMortgageLoanHelper;
        ApplicationCalculateMortgageLoanHelper _appCalc;

        public void OnConstruction()
        {
            _applicationMortgageLoanHelper = new ApplicationMortgageLoanHelper(_DAO.ApplicationMortgageLoanDetail);
            _appCalc = new ApplicationCalculateMortgageLoanHelper(this as IApplicationMortgageLoan, OfferTypes.FurtherAdvance);
        }

        public override void CalculateApplicationDetail(bool IsBondExceptionAction, bool keepMarketRate)
        {
            _appCalc.CalculateApplicationDetail(IsBondExceptionAction, keepMarketRate);
        }

        public void SetProduct(IProduct product)
        {
            switch (product.Key)
            {
                case 1:
                    {
                        base._currentProduct = new ApplicationProductVariableLoan(this, true);
                        break;
                    }
                case 2:
                    {
                        base._currentProduct = new ApplicationProductVariFixLoan(this, true);
                        break;
                    }
                case 3:
                    {
                        // HOC
                        break;
                    }
                case 4:
                    {
                        // Life
                        break;
                    }
                case 5:
                    {
                        base._currentProduct = new ApplicationProductSuperLoLoan(this, true);
                        break;
                    }
                case 6:
                    {
                        base._currentProduct = new ApplicationProductDefendingDiscountLoan(this, true);
                        break;
                    }
                case 9:
                    {
                        base._currentProduct = new ApplicationProductNewVariableLoan(this, true);
                        break;
                    }
                case 10:
                    {
                        // QC
                        break;
                    }
                case 11:
                    {
                        base._currentProduct = new ApplicationProductEdge(this, true);
                        break;
                    }
            }
            // set the applications product
            GetLatestApplicationInformation().Product = product;
        }

        public IEventList<IFurtherAdvanceApplicationInformation> FurtherAdvanceApplicationInformations
        {
            get
            {
                if (null == _furtherAdvanceApplicationInformations)
                {
                    if (null == _DAO.ApplicationInformations)
                        _DAO.ApplicationInformations = new List<ApplicationInformation_DAO>();
                    _furtherAdvanceApplicationInformations = new DAOEventList<ApplicationInformation_DAO, IFurtherAdvanceApplicationInformation, FurtherAdvanceApplicationInformation>(_DAO.ApplicationInformations);
                    _furtherAdvanceApplicationInformations.BeforeAdd += new EventListHandler(OnFurtherAdvanceApplicationInformations_BeforeAdd);
                    _furtherAdvanceApplicationInformations.BeforeRemove += new EventListHandler(OnFurtherAdvanceApplicationInformations_BeforeRemove);
                }
                return _furtherAdvanceApplicationInformations;
            }
        }

        /// <summary>
        /// This method creates a new, empty class, adds it to the FurtherAdvanceApplicationInformations Collection and returns the new object to the caller.
        /// </summary>
        /// <param name="Messages"></param>
        /// <returns></returns>
        public IFurtherAdvanceApplicationInformation CreateEmptyApplicationInformation(IDomainMessageCollection Messages)
        {
            IFurtherAdvanceApplicationInformation EmptApp = (IFurtherAdvanceApplicationInformation)new FurtherAdvanceApplicationInformation(new ApplicationInformation_DAO());
            EmptApp.Application = this;
            EmptApp.ApplicationInsertDate = DateTime.Now;
            this.FurtherAdvanceApplicationInformations.Add(Messages, EmptApp);
            return EmptApp;
        }

        /// <summary>
        /// Returns the lastest FurtherLoanApplication object.
        /// </summary>
        public IFurtherAdvanceApplicationInformation CurrentFurtherAdvanceApplicationInformation
        {
            get
            {
                string HQL = "from ApplicationInformation_DAO ai where ai.Application.Key = :key and ai.ApplicationInsertDate = (select max(mai.ApplicationInsertDate) from ApplicationInformation_DAO mai where mai.Application.Key = :key)";

                SimpleQuery<ApplicationInformation_DAO> Q = new SimpleQuery<ApplicationInformation_DAO>(HQL);
                Q.SetParameter("key", Key);

                ApplicationInformation_DAO[] results = Q.Execute();

                if (results.Length > 0)
                    return new FurtherAdvanceApplicationInformation(results[0]);

                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFurtherAdvanceApplicationInformations_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"><see cref="ICancelDomainArgs"/></param>
        /// <param name="Item"></param>
        protected void OnFurtherAdvanceApplicationInformations_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        #region IApplicationMortgageLoan Members

        public IApplicantType ApplicantType
        {
            get
            {
                return _applicationMortgageLoanHelper.ApplicantType;
            }
            set
            {
                _applicationMortgageLoanHelper.ApplicantType = value;
            }
        }

        public double? ClientEstimatePropertyValuation
        {
            get
            {
                return _applicationMortgageLoanHelper.ClientEstimatePropertyValuation;
            }
            set
            {
                _applicationMortgageLoanHelper.ClientEstimatePropertyValuation = value;
            }
        }

        public int? NumApplicants
        {
            get
            {
                return _applicationMortgageLoanHelper.NumApplicants;
            }
            set
            {
                _applicationMortgageLoanHelper.NumApplicants = value;
            }
        }

        public IResetConfiguration ResetConfiguration
        {
            get
            {
                return _applicationMortgageLoanHelper.ResetConfiguration;
            }
            set
            {
                _applicationMortgageLoanHelper.ResetConfiguration = value;
            }
        }

        public string TransferringAttorney
        {
            get
            {
                return _applicationMortgageLoanHelper.TransferringAttorney;
            }
            set
            {
                _applicationMortgageLoanHelper.TransferringAttorney = value;
            }
        }

        public IProperty Property
        {
            get
            {
                return _applicationMortgageLoanHelper.Property;
            }
            set
            {
                _applicationMortgageLoanHelper.Property = value;
            }
        }

        //public EmploymentTypes GetEmploymentType(bool UseAllCurrentEmployment)
        //{
        //    return _applicationMortgageLoanHelper.GetEmploymentType(UseAllCurrentEmployment);
        //}

        #endregion IApplicationMortgageLoan Members

        #region IApplicationFurtherLending Members

        public double? RequestedCashAmount
        {
            get
            {
                ISupportsVariableLoanApplicationInformation VLI = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                {
                    IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                    if (VL != null)
                    {
                        return VL.RequestedCashAmount;
                    }
                }
                throw new NullReferenceException();
            }
            set
            {
                ISupportsVariableLoanApplicationInformation VLI = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;
                if (VLI != null)
                {
                    IApplicationInformationVariableLoanForSwitchAndRefinance VL = VLI.VariableLoanInformation as IApplicationInformationVariableLoanForSwitchAndRefinance;
                    VL.RequestedCashAmount = value;
                }
            }
        }

        public double EstimatedDisbursedLTV
        {
            get { return _appCalc.EstimatedDisbursedLTV(); }
        }

        #endregion IApplicationFurtherLending Members

        #region IApplicationMortgageLoan Members

        public double MinBondRequired
        {
            get { return 0; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEventList<IMargin> GetMargins()
        {
            ISupportsVariableLoanApplicationInformation supportsVariableLoanApplicationInformation = this.CurrentProduct as ISupportsVariableLoanApplicationInformation;

            if (supportsVariableLoanApplicationInformation != null)
                return _applicationMortgageLoanHelper.GetMargins(supportsVariableLoanApplicationInformation.VariableLoanInformation.CreditMatrix.Key);
            else
                return null;
        }

        public double? LoanAgreementAmount
        {
            get
            {
                return _appCalc.GetLoanAgreementAmount();
            }
        }

        public int? DependentsPerHousehold
        {
            get
            {
                return _applicationMortgageLoanHelper.DependentsPerHousehold;
            }
            set
            {
                _applicationMortgageLoanHelper.DependentsPerHousehold = value;
            }
        }

        public int? ContributingDependents
        {
            get
            {
                return _applicationMortgageLoanHelper.ContributingDependents;
            }
            set
            {
                _applicationMortgageLoanHelper.ContributingDependents = value;
            }
        }

        #endregion IApplicationMortgageLoan Members

        public ILanguage Language
        {
            get
            {
                if (null == _DAO.ApplicationMortgageLoanDetail.Language) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.ApplicationMortgageLoanDetail.Language);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.ApplicationMortgageLoanDetail.Language = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationMortgageLoanDetail.Language = (Language_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Rules"></param>
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("ProductVarifixOptInLoanTransaction");
            Rules.Add("ProductInterestOnlyOptInLoanTransaction");
            Rules.Add("ProductSuperLoOptInLoanTransaction");
            Rules.Add("AccountDebtCounseling");
            Rules.Add("AccountDetailTypeCheck");
        }
    }
}