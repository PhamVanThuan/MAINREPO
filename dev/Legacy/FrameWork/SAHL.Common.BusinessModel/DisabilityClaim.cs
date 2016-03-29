using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO
    /// </summary>
    public partial class DisabilityClaim : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO>, IDisabilityClaim
    {
        public DisabilityClaim(SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO DisabilityClaim)
            : base(DisabilityClaim)
        {
            this._DAO = DisabilityClaim;
        }

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DateClaimReceived
        /// </summary>
        public DateTime DateClaimReceived
        {
            get { return _DAO.DateClaimReceived; }
            set { _DAO.DateClaimReceived = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.LastWorkingDate
        /// </summary>
        public DateTime? LastDateWorked
        {
            get { return _DAO.LastDateWorked; }
            set { _DAO.LastDateWorked = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DateOfDiagnosis
        /// </summary>
        public DateTime? DateOfDiagnosis
        {
            get { return _DAO.DateOfDiagnosis; }
            set { _DAO.DateOfDiagnosis = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ClaimantOccupation
        /// </summary>
        public string ClaimantOccupation
        {
            get { return _DAO.ClaimantOccupation; }
            set { _DAO.ClaimantOccupation = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DisabilityType
        /// </summary>
        public IDisabilityType DisabilityType
        {
            get
            {
                if (null == _DAO.DisabilityType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDisabilityType, DisabilityType_DAO>(_DAO.DisabilityType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DisabilityType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DisabilityType = (DisabilityType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.OtherDisabilityComments
        /// </summary>
        public string OtherDisabilityComments
        {
            get { return _DAO.OtherDisabilityComments; }
            set { _DAO.OtherDisabilityComments = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.ExpectedReturnToWorkDate
        /// </summary>
        public DateTime? ExpectedReturnToWorkDate
        {
            get { return _DAO.ExpectedReturnToWorkDate; }
            set { _DAO.ExpectedReturnToWorkDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.DisabilityClaimStatus
        /// </summary>
        public IDisabilityClaimStatus DisabilityClaimStatus
        {
            get
            {
                if (null == _DAO.DisabilityClaimStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDisabilityClaimStatus, DisabilityClaimStatus_DAO>(_DAO.DisabilityClaimStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DisabilityClaimStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DisabilityClaimStatus = (DisabilityClaimStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.PaymentStartDate
        /// </summary>
        public DateTime? PaymentStartDate
        {
            get { return _DAO.PaymentStartDate; }
            set { _DAO.PaymentStartDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.NumberOfInstalmentsAuthorised
        /// </summary>
        public int? NumberOfInstalmentsAuthorised
        {
            get { return _DAO.NumberOfInstalmentsAuthorised; }
            set { _DAO.NumberOfInstalmentsAuthorised = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.DisabilityClaim_DAO.PaymentEndDate
        /// </summary>
        public DateTime? PaymentEndDate
        {
            get { return _DAO.PaymentEndDate; }
            set { _DAO.PaymentEndDate = value; }
        }
    }
}