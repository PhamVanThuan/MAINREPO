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
	/// SAHL.Common.BusinessModel.DAO.HOC_DAO
	/// </summary>
	public partial class HOC : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOC_DAO>, IHOC
	{
				public HOC(SAHL.Common.BusinessModel.DAO.HOC_DAO HOC) : base(HOC)
		{
			this._DAO = HOC;
			OnConstruction();
		}
		/// <summary>
		/// Used for Activerecord exclusively, please use Key.
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCPolicyNumber
		/// </summary>
		public String HOCPolicyNumber 
		{
			get { return _DAO.HOCPolicyNumber; }
			set { _DAO.HOCPolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCProrataPremium
		/// </summary>
		public Double HOCProrataPremium 
		{
			get { return _DAO.HOCProrataPremium; }
			set { _DAO.HOCProrataPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCMonthlyPremium
		/// </summary>
		public Double? HOCMonthlyPremium
		{
			get { return _DAO.HOCMonthlyPremium; }
			set { _DAO.HOCMonthlyPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCThatchAmount
		/// </summary>
		public Double? HOCThatchAmount
		{
			get { return _DAO.HOCThatchAmount; }
			set { _DAO.HOCThatchAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCConventionalAmount
		/// </summary>
		public Double? HOCConventionalAmount
		{
			get { return _DAO.HOCConventionalAmount; }
			set { _DAO.HOCConventionalAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCShingleAmount
		/// </summary>
		public Double? HOCShingleAmount
		{
			get { return _DAO.HOCShingleAmount; }
			set { _DAO.HOCShingleAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCStatusID
		/// </summary>
		public Int32? HOCStatusID
		{
			get { return _DAO.HOCStatusID; }
			set { _DAO.HOCStatusID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCSBICFlag
		/// </summary>
		public Boolean? HOCSBICFlag
		{
			get { return _DAO.HOCSBICFlag; }
			set { _DAO.HOCSBICFlag = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.CapitalizedMonthlyBalance
		/// </summary>
		public Double? CapitalizedMonthlyBalance
		{
			get { return _DAO.CapitalizedMonthlyBalance; }
			set { _DAO.CapitalizedMonthlyBalance = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.CommencementDate
		/// </summary>
		public DateTime? CommencementDate
		{
			get { return _DAO.CommencementDate; }
			set { _DAO.CommencementDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.Ceded
		/// </summary>
		public Boolean Ceded 
		{
			get { return _DAO.Ceded; }
			set { _DAO.Ceded = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.SAHLPolicyNumber
		/// </summary>
		public String SAHLPolicyNumber 
		{
			get { return _DAO.SAHLPolicyNumber; }
			set { _DAO.SAHLPolicyNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.CancellationDate
		/// </summary>
		public DateTime? CancellationDate
		{
			get { return _DAO.CancellationDate; }
			set { _DAO.CancellationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCHistory
		/// </summary>
		public IHOCHistory HOCHistory 
		{
			get
			{
				if (null == _DAO.HOCHistory) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCHistory, HOCHistory_DAO>(_DAO.HOCHistory);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCHistory = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCHistory = (HOCHistory_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCHistories
		/// </summary>
		private DAOEventList<HOCHistory_DAO, IHOCHistory, HOCHistory> _HOCHistories;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCHistories
		/// </summary>
		public IEventList<IHOCHistory> HOCHistories
		{
			get
			{
				if (null == _HOCHistories) 
				{
					if(null == _DAO.HOCHistories)
						_DAO.HOCHistories = new List<HOCHistory_DAO>();
					_HOCHistories = new DAOEventList<HOCHistory_DAO, IHOCHistory, HOCHistory>(_DAO.HOCHistories);
					_HOCHistories.BeforeAdd += new EventListHandler(OnHOCHistories_BeforeAdd);					
					_HOCHistories.BeforeRemove += new EventListHandler(OnHOCHistories_BeforeRemove);					
					_HOCHistories.AfterAdd += new EventListHandler(OnHOCHistories_AfterAdd);					
					_HOCHistories.AfterRemove += new EventListHandler(OnHOCHistories_AfterRemove);					
				}
				return _HOCHistories;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCInsurer
		/// </summary>
		public IHOCInsurer HOCInsurer 
		{
			get
			{
				if (null == _DAO.HOCInsurer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCInsurer, HOCInsurer_DAO>(_DAO.HOCInsurer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCInsurer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCInsurer = (HOCInsurer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCConstruction
		/// </summary>
		public IHOCConstruction HOCConstruction 
		{
			get
			{
				if (null == _DAO.HOCConstruction) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCConstruction, HOCConstruction_DAO>(_DAO.HOCConstruction);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCConstruction = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCConstruction = (HOCConstruction_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCRoof
		/// </summary>
		public IHOCRoof HOCRoof 
		{
			get
			{
				if (null == _DAO.HOCRoof) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCRoof, HOCRoof_DAO>(_DAO.HOCRoof);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCRoof = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCRoof = (HOCRoof_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCStatus
		/// </summary>
		public IHOCStatus HOCStatus 
		{
			get
			{
				if (null == _DAO.HOCStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCStatus, HOCStatus_DAO>(_DAO.HOCStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCStatus = (HOCStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCSubsidence
		/// </summary>
		public IHOCSubsidence HOCSubsidence 
		{
			get
			{
				if (null == _DAO.HOCSubsidence) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCSubsidence, HOCSubsidence_DAO>(_DAO.HOCSubsidence);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCSubsidence = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCSubsidence = (HOCSubsidence_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCAdministrationFee
		/// </summary>
		public Double HOCAdministrationFee 
		{
			get { return _DAO.HOCAdministrationFee; }
			set { _DAO.HOCAdministrationFee = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.HOCBasePremium
		/// </summary>
		public Double HOCBasePremium 
		{
			get { return _DAO.HOCBasePremium; }
			set { _DAO.HOCBasePremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.SASRIAAmount
		/// </summary>
		public Double SASRIAAmount 
		{
			get { return _DAO.SASRIAAmount; }
			set { _DAO.SASRIAAmount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOC_DAO.FinancialService
		/// </summary>
		public IFinancialService FinancialService 
		{
			get
			{
				if (null == _DAO.FinancialService) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialService = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_HOCHistories = null;
			
		}
	}
}


