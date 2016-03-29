using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationLife : Application, IApplicationLife
    {
        ApplicationLifeHelper _applicationLifeHelper;

        /// <summary>
        ///
        /// </summary>
        public void OnConstruction()
        {
            _applicationLifeHelper = new ApplicationLifeHelper(_DAO);
        }

        public override void Refresh()
        {
            base.Refresh();

            _DAO.ApplicationLifeDetail.Refresh();
        }

        #region IApplicationLife Members

        public IADUser Consultant
        {
            get
            {
                return _applicationLifeHelper.Consultant;
            }
        }

        public double? CurrentSumAssured
        {
            get
            {
                return _applicationLifeHelper.CurrentSumAssured;
            }
            set
            {
                _applicationLifeHelper.CurrentSumAssured = value;
            }
        }

        public DateTime? DateCeded
        {
            get
            {
                return _applicationLifeHelper.DateCeded;
            }
            set
            {
                _applicationLifeHelper.DateCeded = value;
            }
        }

        public DateTime? DateLastUpdated
        {
            get
            {
                return _applicationLifeHelper.DateLastUpdated;
            }
            set
            {
                _applicationLifeHelper.DateLastUpdated = value;
            }
        }

        public DateTime? DateOfAcceptance
        {
            get
            {
                return _applicationLifeHelper.DateOfAcceptance;
            }
            set
            {
                _applicationLifeHelper.DateOfAcceptance = value;
            }
        }

        public DateTime DateOfExpiry
        {
            get
            {
                return _applicationLifeHelper.DateOfExpiry;
            }
            set
            {
                _applicationLifeHelper.DateOfExpiry = value;
            }
        }

        public double DeathBenefit
        {
            get
            {
                return _applicationLifeHelper.DeathBenefit;
            }
            set
            {
                _applicationLifeHelper.DeathBenefit = value;
            }
        }

        public double DeathBenefitPremium
        {
            get
            {
                return _applicationLifeHelper.DeathBenefitPremium;
            }
            set
            {
                _applicationLifeHelper.DeathBenefitPremium = value;
            }
        }

        public string ExternalPolicyNumber
        {
            get
            {
                return _applicationLifeHelper.ExternalPolicyNumber;
            }
            set
            {
                _applicationLifeHelper.ExternalPolicyNumber = value;
            }
        }

        public double InstallmentProtectionBenefit
        {
            get
            {
                return _applicationLifeHelper.InstallmentProtectionBenefit;
            }
            set
            {
                _applicationLifeHelper.InstallmentProtectionBenefit = value;
            }
        }

        public double InstallmentProtectionPremium
        {
            get
            {
                return _applicationLifeHelper.InstallmentProtectionPremium;
            }
            set
            {
                _applicationLifeHelper.InstallmentProtectionPremium = value;
            }
        }

        public IInsurer Insurer
        {
            get
            {
                return _applicationLifeHelper.Insurer;
            }
            set
            {
                _applicationLifeHelper.Insurer = value;
            }
        }

        public decimal JointDiscountFactor
        {
            get
            {
                return _applicationLifeHelper.JointDiscountFactor;
            }
            set
            {
                _applicationLifeHelper.JointDiscountFactor = value;
            }
        }

        public double MonthlyPremium
        {
            get
            {
                return _applicationLifeHelper.MonthlyPremium;
            }
            set
            {
                _applicationLifeHelper.MonthlyPremium = value;
            }
        }

        public ILegalEntity PolicyHolderLegalEntity
        {
            get
            {
                return _applicationLifeHelper.PolicyHolderLegalEntity;
            }
            set
            {
                _applicationLifeHelper.PolicyHolderLegalEntity = value;
            }
        }

        public double? PremiumShortfall
        {
            get
            {
                return _applicationLifeHelper.PremiumShortfall;
            }
            set
            {
                _applicationLifeHelper.PremiumShortfall = value;
            }
        }

        public IPriority Priority
        {
            get
            {
                return _applicationLifeHelper.Priority;
            }
            set
            {
                _applicationLifeHelper.Priority = value;
            }
        }

        public string RPARInsurer
        {
            get
            {
                return _applicationLifeHelper.RPARInsurer;
            }
            set
            {
                _applicationLifeHelper.RPARInsurer = value;
            }
        }

        public string RPARPolicyNumber
        {
            get
            {
                return _applicationLifeHelper.RPARPolicyNumber;
            }
            set
            {
                _applicationLifeHelper.RPARPolicyNumber = value;
            }
        }

        public double SumAssured
        {
            get
            {
                return _applicationLifeHelper.SumAssured;
            }
            set
            {
                _applicationLifeHelper.SumAssured = value;
            }
        }

        public decimal UpliftFactor
        {
            get
            {
                return _applicationLifeHelper.UpliftFactor;
            }
            set
            {
                _applicationLifeHelper.UpliftFactor = value;
            }
        }

        public double YearlyPremium
        {
            get
            {
                return _applicationLifeHelper.YearlyPremium;
            }
            set
            {
                _applicationLifeHelper.YearlyPremium = value;
            }
        }

        public string ConsultantADUserName
        {
            get
            {
                return _applicationLifeHelper.ConsultantADUserName;
            }
            set
            {
                _applicationLifeHelper.ConsultantADUserName = value;
            }
        }

        public ILifePolicyType LifePolicyType
        {
            get
            {
                return _applicationLifeHelper.LifePolicyType;
            }
            set
            {
                _applicationLifeHelper.LifePolicyType = value;
            }
        }

        #endregion IApplicationLife Members

        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("LifeApplicationCreateDebtCounselling");
        }
    }
}