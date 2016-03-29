using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO
	/// </summary>
	public partial class ExternalLifePolicy : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO>, IExternalLifePolicy
	{
				public ExternalLifePolicy(SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO ExternalLifePolicy) : base(ExternalLifePolicy)
		{
			this._DAO = ExternalLifePolicy;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.PolicyNumber
		/// </summary>
		public String PolicyNumber 
		{
			get { return _DAO.PolicyNumber; }
			set { _DAO.PolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.Insurer
		/// </summary>
		public IInsurer Insurer 
		{
			get
			{
				if (null == _DAO.Insurer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IInsurer, Insurer_DAO>(_DAO.Insurer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Insurer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Insurer = (Insurer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.CommencementDate
		/// </summary>
		public DateTime CommencementDate 
		{
			get { return _DAO.CommencementDate; }
			set { _DAO.CommencementDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.LifePolicyStatus
		/// </summary>
		public ILifePolicyStatus LifePolicyStatus 
		{
			get
			{
				if (null == _DAO.LifePolicyStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILifePolicyStatus, LifePolicyStatus_DAO>(_DAO.LifePolicyStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.LifePolicyStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.LifePolicyStatus = (LifePolicyStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.CloseDate
		/// </summary>
		public DateTime? CloseDate 
		{
            get
            {
                return _DAO.CloseDate; 
            }
			set { _DAO.CloseDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.SumInsured
		/// </summary>
		public Double SumInsured 
		{
			get { return _DAO.SumInsured; }
			set { _DAO.SumInsured = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.PolicyCeded
		/// </summary>
		public Boolean PolicyCeded 
		{
			get { return _DAO.PolicyCeded; }
			set { _DAO.PolicyCeded = value;}
		}
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ExternalLifePolicy_DAO.LegalEntity
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
	}
}


