using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO
    /// </summary>
    public partial class AffordabilityAssessment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO>, IAffordabilityAssessment
    {
        public AffordabilityAssessment(SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO AffordabilityAssessment)
            : base(AffordabilityAssessment)
        {
            this._DAO = AffordabilityAssessment;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.Key
        /// </summary>
        public int Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GenericKey
        /// </summary>
        public int GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ModifiedDate
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return _DAO.ModifiedDate; }
            set { _DAO.ModifiedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.NumberOfContributingApplicants
        /// </summary>
        public int NumberOfContributingApplicants
        {
            get { return _DAO.NumberOfContributingApplicants; }
            set { _DAO.NumberOfContributingApplicants = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.NumberOfHouseholdDependants
        /// </summary>
        public int NumberOfHouseholdDependants
        {
            get { return _DAO.NumberOfHouseholdDependants; }
            set { _DAO.NumberOfHouseholdDependants = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.MinimumMonthlyFixedExpenses
        /// </summary>
        public int? MinimumMonthlyFixedExpenses
        {
            get { return _DAO.MinimumMonthlyFixedExpenses; }
            set { _DAO.MinimumMonthlyFixedExpenses = value; }
        }


        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GeneralStatus
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.AffordabilityAssessmentStatus
        /// </summary>
        public IAffordabilityAssessmentStatus AffordabilityAssessmentStatus
        {
            get
            {
                if (null == _DAO.AffordabilityAssessmentStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAffordabilityAssessmentStatus, AffordabilityAssessmentStatus_DAO>(_DAO.AffordabilityAssessmentStatus);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.AffordabilityAssessmentStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AffordabilityAssessmentStatus = (AffordabilityAssessmentStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.AffordabilityAssessmentStressFactor
        /// </summary>
        public IAffordabilityAssessmentStressFactor AffordabilityAssessmentStressFactor
        {
            get
            {
                if (null == _DAO.AffordabilityAssessmentStressFactor) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAffordabilityAssessmentStressFactor, AffordabilityAssessmentStressFactor_DAO>(_DAO.AffordabilityAssessmentStressFactor);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.AffordabilityAssessmentStressFactor = (AffordabilityAssessmentStressFactor_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ModifiedByUser
        /// </summary>
        public IADUser ModifiedByUser
        {
            get
            {
                if (null == _DAO.ModifiedByUser) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_DAO>(_DAO.ModifiedByUser);
                }
            }
            set
            {
                if (value == null)
                {
                    _DAO.ModifiedByUser = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ModifiedByUser = (ADUser_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.ConfirmedDate
        /// </summary>
        public DateTime? ConfirmedDate
        {
            get { return _DAO.ConfirmedDate; }
            set { _DAO.ConfirmedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AffordabilityAssessment_DAO.Notes
        /// </summary>
        public string Notes
        {
            get { return _DAO.Notes; }
            set { _DAO.Notes = value; }
        }
    }
}
