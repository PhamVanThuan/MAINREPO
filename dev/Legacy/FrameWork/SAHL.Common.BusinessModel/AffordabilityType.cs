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
	/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO
	/// </summary>
	public partial class AffordabilityType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO>, IAffordabilityType
	{
				public AffordabilityType(SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO AffordabilityType) : base(AffordabilityType)
		{
			this._DAO = AffordabilityType;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.IsExpense
		/// </summary>
		public Boolean IsExpense 
		{
			get { return _DAO.IsExpense; }
			set { _DAO.IsExpense = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.LegalEntityAffordabilities
		/// </summary>
		private DAOEventList<LegalEntityAffordability_DAO, ILegalEntityAffordability, LegalEntityAffordability> _LegalEntityAffordabilities;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.LegalEntityAffordabilities
		/// </summary>
		public IEventList<ILegalEntityAffordability> LegalEntityAffordabilities
		{
			get
			{
				if (null == _LegalEntityAffordabilities) 
				{
					if(null == _DAO.LegalEntityAffordabilities)
						_DAO.LegalEntityAffordabilities = new List<LegalEntityAffordability_DAO>();
					_LegalEntityAffordabilities = new DAOEventList<LegalEntityAffordability_DAO, ILegalEntityAffordability, LegalEntityAffordability>(_DAO.LegalEntityAffordabilities);
					_LegalEntityAffordabilities.BeforeAdd += new EventListHandler(OnLegalEntityAffordabilities_BeforeAdd);					
					_LegalEntityAffordabilities.BeforeRemove += new EventListHandler(OnLegalEntityAffordabilities_BeforeRemove);					
					_LegalEntityAffordabilities.AfterAdd += new EventListHandler(OnLegalEntityAffordabilities_AfterAdd);					
					_LegalEntityAffordabilities.AfterRemove += new EventListHandler(OnLegalEntityAffordabilities_AfterRemove);					
				}
				return _LegalEntityAffordabilities;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.AffordabilityTypeGroup
		/// </summary>
		public IAffordabilityTypeGroup AffordabilityTypeGroup 
		{
			get
			{
				if (null == _DAO.AffordabilityTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IAffordabilityTypeGroup, AffordabilityTypeGroup_DAO>(_DAO.AffordabilityTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.AffordabilityTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.AffordabilityTypeGroup = (AffordabilityTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.DescriptionRequired
		/// </summary>
		public Boolean DescriptionRequired 
		{
			get { return _DAO.DescriptionRequired; }
			set { _DAO.DescriptionRequired = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.AffordabilityType_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		public override void Refresh()
		{
			base.Refresh();
			_LegalEntityAffordabilities = null;
			
		}
	}
}


