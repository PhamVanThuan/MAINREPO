using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Service;

namespace SAHL.Common.BusinessModel.Helpers
{
    public class ApplicationMortgageLoanHelper
    {
        ApplicationMortgageLoanDetail_DAO _mortgageLoanDetail;
        public ApplicationMortgageLoanHelper(ApplicationMortgageLoanDetail_DAO MortgageLoanDetail)
        {
            _mortgageLoanDetail = MortgageLoanDetail;
        }

        #region IApplicationMortgageLoan Members

        public IApplicantType ApplicantType
        {
            get
            {
                if (null == _mortgageLoanDetail.ApplicantType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicantType, ApplicantType_DAO>(_mortgageLoanDetail.ApplicantType);
                }
            }
            set
            {
                if (value == null)
                {
                    _mortgageLoanDetail.ApplicantType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _mortgageLoanDetail.ApplicantType = (ApplicantType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public double? ClientEstimatePropertyValuation
        {
            get
            {
                return _mortgageLoanDetail.ClientEstimatePropertyValuation;
            }
            set
            {
                _mortgageLoanDetail.ClientEstimatePropertyValuation = value;
            }
        }

        public int? NumApplicants
        {
            get
            {
                return _mortgageLoanDetail.NumApplicants;
            }
            set
            {
                _mortgageLoanDetail.NumApplicants = value;
            }
        }

        /// <summary>
        /// NewPurchaseApplicationMortgageLoanClientEstimateValuationSynchroniseForNoActiveValuation: For a New Purchase if there is no active Valuation record then the ApplicationMortgageLoan.ClientEstiamteValuation should be syncrionised with the ApplicationMortgageLoanDetail.PurchasePrice.
        /// </summary>
        public double? PurchasePrice
        {
            get
            {
                return _mortgageLoanDetail.PurchasePrice;
            }
            set
            {
                bool activeValuationFound = false;

                if (_mortgageLoanDetail.Property != null && _mortgageLoanDetail.Property.Valuations != null)
                {
                    foreach (Valuation_DAO valuation in _mortgageLoanDetail.Property.Valuations)
                    {
                        if (valuation.IsActive)
                        {
                            activeValuationFound = true;
                            break;
                        }
                    }
                }

                // Sync the ClientEstimatePropertyValuation and PropertyValuation
                if (!activeValuationFound)
                {
                    _mortgageLoanDetail.ClientEstimatePropertyValuation = value;

                    int latestIndex = -1;
                    int maxKey = -1;

                    for ( int i = 0; i < _mortgageLoanDetail.Application.ApplicationInformations.Count; i++)
                    {
                        if (_mortgageLoanDetail.Application.ApplicationInformations[i].Key > maxKey)
                        {
                            latestIndex = i;
                            maxKey = _mortgageLoanDetail.Application.ApplicationInformations[i].Key;
                        }
                    }

                    if (latestIndex > -1)
                        _mortgageLoanDetail.Application.ApplicationInformations[latestIndex].ApplicationInformationVariableLoan.PropertyValuation = value;

                }

                _mortgageLoanDetail.PurchasePrice = value;

            }
        }

        public IResetConfiguration ResetConfiguration
        {
            get
            {
                if (null == _mortgageLoanDetail.ResetConfiguration) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IResetConfiguration, ResetConfiguration_DAO>(_mortgageLoanDetail.ResetConfiguration);
                }
            }
            set
            {
                if (value == null)
                {
                    _mortgageLoanDetail.ResetConfiguration = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _mortgageLoanDetail.ResetConfiguration = (ResetConfiguration_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public string TransferringAttorney
        {
            get
            {
                return _mortgageLoanDetail.TransferringAttorney;
            }
            set
            {
                _mortgageLoanDetail.TransferringAttorney = value;
            }
        }

        //public EmploymentTypes GetEmploymentType(bool IncludeUnconfirmedIncome)
        //{

        //    // if (!UseAllCurrentEmployment)
        //    // {
        //    //     if(confirmedIncome[active].Count > 0)
        //    //         use confirmed income
        //    //     else if Application.Account exists
        //    //         use unconfirmed
        //    // } else
        //    //     use all income


        //    return EmploymentTypes.SelfEmployed;
        //}

        /// <summary>
        /// Calculates the number of active Applicants on a Mortgage Loan Application. Only Main Applicant and Suretor roles are considered.
        /// </summary>
        /// <param name="Application"></param>
        public static void RefreshNumberofApplicants(IApplicationMortgageLoan Application)
        {
            int numApplicants = 0;

            foreach (IApplicationRole applicationRole in Application.ApplicationRoles)
            {
                int appRoleTypeKey = applicationRole.ApplicationRoleType.Key;

                if ((appRoleTypeKey == (int)OfferRoleTypes.MainApplicant
                    || appRoleTypeKey == (int)OfferRoleTypes.Suretor
                    || appRoleTypeKey == (int)OfferRoleTypes.LeadMainApplicant
                    || appRoleTypeKey == (int)OfferRoleTypes.LeadSuretor)
                    && applicationRole.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    numApplicants++;
            }

            Application.NumApplicants = numApplicants;
        }

        public IProperty Property
        {
            get
            {
                if (null == _mortgageLoanDetail.Property) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IProperty, Property_DAO>(_mortgageLoanDetail.Property);
                }
            }
            set
            {
                if (value == null)
                {
                    _mortgageLoanDetail.Property = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _mortgageLoanDetail.Property = (Property_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEventList<IMargin> GetMargins(int CreditMatrixKey)
        {
            string HQL = "Select distinct c.Margin from CreditCriteria_DAO c where c.CreditMatrix.Key = ?";

            SimpleQuery<Margin_DAO> q = new SimpleQuery<Margin_DAO>(HQL, CreditMatrixKey);
            Margin_DAO[] list = q.Execute();

            if (list == null || list.Length == 0)
                return null;

            IEventList<IMargin> listM = new DAOEventList<Margin_DAO, IMargin, Margin>(list);

            return listM as IEventList<IMargin>;
        }

        public int? DependentsPerHousehold
        {
            get
            {
                return _mortgageLoanDetail.DependentsPerHousehold;
            }
            set
            {
                _mortgageLoanDetail.DependentsPerHousehold = value;
            }
        }

        public int? ContributingDependents
        {
            get
            {
                return _mortgageLoanDetail.ContributingDependents;
            }
            set
            {
                _mortgageLoanDetail.ContributingDependents = value;
            }
        }

    }
}