using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO
    /// </summary>
    public partial class ApplicationInformationAppliedRateAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO>, IApplicationInformationAppliedRateAdjustment
    {
        public ApplicationInformationAppliedRateAdjustment(SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO ApplicationInformationAppliedRateAdjustment)
            : base(ApplicationInformationAppliedRateAdjustment)
        {
            this._DAO = ApplicationInformationAppliedRateAdjustment;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ApplicationElementValue
        /// </summary>
        public String ApplicationElementValue
        {
            get { return _DAO.ApplicationElementValue; }
            set { _DAO.ApplicationElementValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ChangeDate
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ADUser
        /// </summary>
        public IADUser ADUser
        {
            get
            {
                if (null == _DAO.ADUser) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ADUser);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ADUser = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ADUser = (ADUser_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.ApplicationInformationFinancialAdjustment
        /// </summary>
        public IApplicationInformationFinancialAdjustment ApplicationInformationFinancialAdjustment
        {
            get
            {
                if (null == _DAO.ApplicationInformationFinancialAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationFinancialAdjustment, ApplicationInformationFinancialAdjustment_DAO>(_DAO.ApplicationInformationFinancialAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationFinancialAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationFinancialAdjustment = (ApplicationInformationFinancialAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInformationAppliedRateAdjustment_DAO.RateAdjustmentElement
        /// </summary>
        public IRateAdjustmentElement RateAdjustmentElement
        {
            get
            {
                if (null == _DAO.RateAdjustmentElement) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IRateAdjustmentElement, RateAdjustmentElement_DAO>(_DAO.RateAdjustmentElement);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.RateAdjustmentElement = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.RateAdjustmentElement = (RateAdjustmentElement_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}