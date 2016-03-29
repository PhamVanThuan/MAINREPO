using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.Helpers
{
    public class ApplicationLifeHelper
    {
        ApplicationLifeDetail_DAO _lifeDetail;
        ApplicationLife_DAO _applicationLife;

        public ApplicationLifeHelper(ApplicationLife_DAO ApplicationLife) 
        {
            _applicationLife = ApplicationLife;
            _lifeDetail = ApplicationLife.ApplicationLifeDetail;
        }

        #region IApplicationLife Members

        /// <summary>
        ///  this will return the IADUser for the latest "consultant" OfferRoleType on the application
        /// </summary>
        public IADUser Consultant
        {
            get
            {
                IADUser consultant = null;
             
                IApplicationLife app = new ApplicationLife(_applicationLife);

                IApplicationRole applicationRole = app.GetLatestApplicationRoleByType(SAHL.Common.Globals.OfferRoleTypes.Consultant);

                if (applicationRole != null)
                {
                    ADUser_DAO[] adUser = ADUser_DAO.FindAllByProperty("LegalEntity.Key", applicationRole.LegalEntityKey);

                    if (adUser != null && adUser.Length > 0)
                    {
                        consultant = new ADUser(adUser[0]);
                    }
                }
                return consultant;
            }
        }

        public double? CurrentSumAssured
        {
            get
            {
                return _lifeDetail.CurrentSumAssured;
            }
            set
            {
                _lifeDetail.CurrentSumAssured = value;
            }
        }

        public DateTime? DateCeded
        {
            get
            {
                return _lifeDetail.DateCeded;
            }
            set
            {
                _lifeDetail.DateCeded = value;
            }
        }

        public DateTime? DateLastUpdated
        {
            get
            {
                return _lifeDetail.DateLastUpdated;
            }
            set
            {
                _lifeDetail.DateLastUpdated = value;
            }
        }

        public DateTime? DateOfAcceptance
        {
            get
            {
                return _lifeDetail.DateOfAcceptance;
            }
            set
            {
                _lifeDetail.DateOfAcceptance = value;
            }
        }

        public DateTime DateOfExpiry
        {
            get
            {
                return _lifeDetail.DateOfExpiry;
            }
            set
            {
                _lifeDetail.DateOfExpiry = value;
            }
        }

        public double DeathBenefit
        {
            get
            {
                return _lifeDetail.DeathBenefit;
            }
            set
            {
                _lifeDetail.DeathBenefit = value;
            }
        }

        public double DeathBenefitPremium
        {
            get
            {
                return _lifeDetail.DeathBenefitPremium;
            }
            set
            {
                _lifeDetail.DeathBenefitPremium = value;
            }
        }

        public string ExternalPolicyNumber
        {
            get
            {
                return _lifeDetail.ExternalPolicyNumber;
            }
            set
            {
                _lifeDetail.ExternalPolicyNumber = value;
            }
        }

        public double InstallmentProtectionBenefit
        {
            get
            {
                return _lifeDetail.InstallmentProtectionBenefit;
            }
            set
            {
                _lifeDetail.InstallmentProtectionBenefit = value;
            }
        }

        public double InstallmentProtectionPremium
        {
            get
            {
                return _lifeDetail.InstallmentProtectionPremium;
            }
            set
            {
                _lifeDetail.InstallmentProtectionPremium = value;
            }
        }

        public IInsurer Insurer
        {
            get
            {
                if (null == _lifeDetail.Insurer) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IInsurer, Insurer_DAO>(_lifeDetail.Insurer);
                }
            }
            set
            {
                if (value == null)
                {
                    _lifeDetail.Insurer = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _lifeDetail.Insurer = (Insurer_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public decimal JointDiscountFactor
        {
            get
            {
                return _lifeDetail.JointDiscountFactor;
            }
            set
            {
                _lifeDetail.JointDiscountFactor = value;
            }
        }

        public double MonthlyPremium
        {
            get
            {
                return _lifeDetail.MonthlyPremium;
            }
            set
            {
                _lifeDetail.MonthlyPremium = value;
            }
        }

        public ILegalEntity PolicyHolderLegalEntity
        {
            get
            {
                if (null == _lifeDetail.PolicyHolderLegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_lifeDetail.PolicyHolderLegalEntity);
                }
            }
            set
            {
                if (value == null)
                {
                    _lifeDetail.PolicyHolderLegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _lifeDetail.PolicyHolderLegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public double? PremiumShortfall
        {
            get
            {
                return _lifeDetail.PremiumShortfall;
            }
            set
            {
                _lifeDetail.PremiumShortfall = value;
            }
        }

        public IPriority Priority
        {
            get
            {
                if (null == _lifeDetail.PolicyHolderLegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IPriority, Priority_DAO>(_lifeDetail.Priority);
                }
            }
            set
            {
                if (value == null)
                {
                    _lifeDetail.Priority = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _lifeDetail.Priority = (Priority_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public string RPARInsurer
        {
            get
            {
                return _lifeDetail.RPARInsurer;
            }
            set
            {
                _lifeDetail.RPARInsurer = value;
            }
        }

        public string RPARPolicyNumber
        {
            get
            {
                return _lifeDetail.RPARPolicyNumber;
            }
            set
            {
                _lifeDetail.RPARPolicyNumber = value;
            }
        }

        public double SumAssured
        {
            get
            {
                return _lifeDetail.SumAssured;
            }
            set
            {
                _lifeDetail.SumAssured = value;
            }
        }

        public decimal UpliftFactor
        {
            get
            {
                return _lifeDetail.UpliftFactor;
            }
            set
            {
                _lifeDetail.UpliftFactor = value;
            }
        }

        public double YearlyPremium
        {
            get
            {
                return _lifeDetail.YearlyPremium;
            }
            set
            {
                _lifeDetail.YearlyPremium = value;
            }
        }

        public string ConsultantADUserName
        {
            get
            {
                return _lifeDetail.ConsultantADUserName;
            }
            set
            {
                _lifeDetail.ConsultantADUserName = value;
            }
        }

        public ILifePolicyType LifePolicyType
        {
            get
            {
                if (null == _lifeDetail.LifePolicyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILifePolicyType, LifePolicyType_DAO>(_lifeDetail.LifePolicyType);
                }
            }
            set
            {
                if (value == null)
                {
                    _lifeDetail.LifePolicyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _lifeDetail.LifePolicyType = (LifePolicyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        #endregion
    }
}
