using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class CapApplication : IEntityValidation, ICapApplication, IDAOObject
	{
		private SAHL.Common.BusinessModel.DAO.CapApplication_DAO _CapApplication;
        public CapApplication(SAHL.Common.BusinessModel.DAO.CapApplication_DAO CapApplication)
		{
			this._CapApplication = CapApplication;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns><see cref="CapApplication_DAO"/></returns>		
		public object GetDAOObject()
		{
			return _CapApplication;
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 RemainingInstallments 
		{
			get { return _CapApplication.RemainingInstallments; }
			set { _CapApplication.RemainingInstallments = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double CurrentBalance 
		{
			get { return _CapApplication.CurrentBalance; }
			set { _CapApplication.CurrentBalance = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double CurrentInstallment 
		{
			get { return _CapApplication.CurrentInstallment; }
			set { _CapApplication.CurrentInstallment = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Double LinkRate 
		{
			get { return _CapApplication.LinkRate; }
			set { _CapApplication.LinkRate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OfferDate 
		{
			get { return _CapApplication.OfferDate; }
			set { _CapApplication.OfferDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Boolean? Promotion
		{
			get { return _CapApplication.Promotion; }
			set { _CapApplication.Promotion = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CapitalisationDate
		{
			get { return _CapApplication.CapitalisationDate; }
			set { _CapApplication.CapitalisationDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime ChangeDate 
		{
			get { return _CapApplication.ChangeDate; }
			set { _CapApplication.ChangeDate = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public String UserID 
		{
			get { return _CapApplication.UserID; }
			set { _CapApplication.UserID = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public Int32 Key 
		{
			get { return _CapApplication.Key; }
			set { _CapApplication.Key = value;}
		}
		/// <summary>
		/// 
		/// </summary>
		private DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail> _CapApplicationDetails;
		/// <summary>
		/// 
		/// </summary>
		public IEventList<ICapApplicationDetail> CapApplicationDetails
		{
			get
			{
				if (null == _CapApplicationDetails) 
				{
					if(null == _CapApplication.CapApplicationDetails)
						_CapApplication.CapApplicationDetails = new List<CapApplicationDetail_DAO>();
					_CapApplicationDetails = new DAOEventList<CapApplicationDetail_DAO, ICapApplicationDetail, CapApplicationDetail>(_CapApplication.CapApplicationDetails);
					_CapApplicationDetails.BeforeAdd += new EventListHandler(OnCapApplicationDetails_BeforeAdd);					
					_CapApplicationDetails.BeforeRemove += new EventListHandler(OnCapApplicationDetails_BeforeRemove);
				}
				return _CapApplicationDetails;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IAccount Account 
		{
			get
			{
				if (null == _CapApplication.Account) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<IAccount, Account_DAO>(_CapApplication.Account);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplication.Account = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplication.Account = (Account_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public IBroker Broker 
		{
			get
			{
				if (null == _CapApplication.Broker) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<IBroker, Broker_DAO>(_CapApplication.Broker);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplication.Broker = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplication.Broker = (Broker_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapStatus CapStatus 
		{
			get
			{
				if (null == _CapApplication.CapStatus) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapStatus, CapStatus_DAO>(_CapApplication.CapStatus);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplication.CapStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplication.CapStatus = (CapStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public ICapTypeConfiguration CapTypeConfiguration 
		{
			get
			{
				if (null == _CapApplication.CapTypeConfiguration) return null;
				{
				IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
				return BMTM.GetMappedType<ICapTypeConfiguration, CapTypeConfiguration_DAO>(_CapApplication.CapTypeConfiguration);
				}

			}

			set
			{
				if(value == null)
				{
					_CapApplication.CapTypeConfiguration = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_CapApplication.CapTypeConfiguration = (CapTypeConfiguration_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


