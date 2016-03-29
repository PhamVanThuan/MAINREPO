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
	/// Valuator_DAO describes a Valuator who carries out the property valuations.
	/// </summary>
	public partial class Valuator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Valuator_DAO>, IValuator
	{
				public Valuator(SAHL.Common.BusinessModel.DAO.Valuator_DAO Valuator) : base(Valuator)
		{
			this._DAO = Valuator;
		}
		/// <summary>
		/// Contact Person at the Valuator
		/// </summary>
		public String ValuatorContact 
		{
			get { return _DAO.ValuatorContact; }
			set { _DAO.ValuatorContact = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Valuator_DAO.ValuatorPassword
		/// </summary>
		public String ValuatorPassword 
		{
			get { return _DAO.ValuatorPassword; }
			set { _DAO.ValuatorPassword = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Valuator_DAO.LimitedUserGroup
		/// </summary>
		public Byte LimitedUserGroup 
		{
			get { return _DAO.LimitedUserGroup; }
			set { _DAO.LimitedUserGroup = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The status of the Valuator e.g. Active or Inactive.
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
				if(value == null)
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
		/// Foreign Key reference to the Legal Entity table. Each Valuator exists as a Legal Entity on the database.
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
				if(value == null)
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
		/// SAHL.Common.BusinessModel.DAO.Valuator_DAO.OriginationSources
		/// </summary>
		private DAOEventList<OriginationSource_DAO, IOriginationSource, OriginationSource> _OriginationSources;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Valuator_DAO.OriginationSources
		/// </summary>
		public IEventList<IOriginationSource> OriginationSources
		{
			get
			{
				if (null == _OriginationSources) 
				{
					if(null == _DAO.OriginationSources)
						_DAO.OriginationSources = new List<OriginationSource_DAO>();
					_OriginationSources = new DAOEventList<OriginationSource_DAO, IOriginationSource, OriginationSource>(_DAO.OriginationSources);
					_OriginationSources.BeforeAdd += new EventListHandler(OnOriginationSources_BeforeAdd);					
					_OriginationSources.BeforeRemove += new EventListHandler(OnOriginationSources_BeforeRemove);					
					_OriginationSources.AfterAdd += new EventListHandler(OnOriginationSources_AfterAdd);					
					_OriginationSources.AfterRemove += new EventListHandler(OnOriginationSources_AfterRemove);					
				}
				return _OriginationSources;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_OriginationSources = null;
			
		}
	}
}


