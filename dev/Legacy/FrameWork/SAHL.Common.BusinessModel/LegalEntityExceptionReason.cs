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
	/// The LegalEntityExceptionReason_DAO is used to store the reasons why a Legal Entity  failed validation.
	/// </summary>
	public partial class LegalEntityExceptionReason : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO>, ILegalEntityExceptionReason
	{
				public LegalEntityExceptionReason(SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO LegalEntityExceptionReason) : base(LegalEntityExceptionReason)
		{
			this._DAO = LegalEntityExceptionReason;
		}
		/// <summary>
		/// The description of the exception reason. e.g. Missing Salutation, Missing Surname
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// The priority of the exception reason.
		/// </summary>
		public Byte Priority 
		{
			get { return _DAO.Priority; }
			set { _DAO.Priority = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO.LegalEntityExceptionReasons
		/// </summary>
		private DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity> _LegalEntityExceptionReasons;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityExceptionReason_DAO.LegalEntityExceptionReasons
		/// </summary>
		public IEventList<ILegalEntity> LegalEntityExceptionReasons
		{
			get
			{
				if (null == _LegalEntityExceptionReasons) 
				{
					if(null == _DAO.LegalEntityExceptionReasons)
						_DAO.LegalEntityExceptionReasons = new List<LegalEntity_DAO>();
					_LegalEntityExceptionReasons = new DAOEventList<LegalEntity_DAO, ILegalEntity, LegalEntity>(_DAO.LegalEntityExceptionReasons);
					_LegalEntityExceptionReasons.BeforeAdd += new EventListHandler(OnLegalEntityExceptionReasons_BeforeAdd);					
					_LegalEntityExceptionReasons.BeforeRemove += new EventListHandler(OnLegalEntityExceptionReasons_BeforeRemove);					
					_LegalEntityExceptionReasons.AfterAdd += new EventListHandler(OnLegalEntityExceptionReasons_AfterAdd);					
					_LegalEntityExceptionReasons.AfterRemove += new EventListHandler(OnLegalEntityExceptionReasons_AfterRemove);					
				}
				return _LegalEntityExceptionReasons;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_LegalEntityExceptionReasons = null;
			
		}
	}
}


