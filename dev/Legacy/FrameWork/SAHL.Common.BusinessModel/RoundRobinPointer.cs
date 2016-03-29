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
	/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO
	/// </summary>
	public partial class RoundRobinPointer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO>, IRoundRobinPointer
	{
				public RoundRobinPointer(SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO RoundRobinPointer) : base(RoundRobinPointer)
		{
			this._DAO = RoundRobinPointer;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.RoundRobinPointerIndexID
		/// </summary>
		public Int32 RoundRobinPointerIndexID 
		{
			get { return _DAO.RoundRobinPointerIndexID; }
			set { _DAO.RoundRobinPointerIndexID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.RoundRobinPointerDefinitions
		/// </summary>
		private DAOEventList<RoundRobinPointerDefinition_DAO, IRoundRobinPointerDefinition, RoundRobinPointerDefinition> _RoundRobinPointerDefinitions;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.RoundRobinPointerDefinitions
		/// </summary>
		public IEventList<IRoundRobinPointerDefinition> RoundRobinPointerDefinitions
		{
			get
			{
				if (null == _RoundRobinPointerDefinitions) 
				{
					if(null == _DAO.RoundRobinPointerDefinitions)
						_DAO.RoundRobinPointerDefinitions = new List<RoundRobinPointerDefinition_DAO>();
					_RoundRobinPointerDefinitions = new DAOEventList<RoundRobinPointerDefinition_DAO, IRoundRobinPointerDefinition, RoundRobinPointerDefinition>(_DAO.RoundRobinPointerDefinitions);
					_RoundRobinPointerDefinitions.BeforeAdd += new EventListHandler(OnRoundRobinPointerDefinitions_BeforeAdd);					
					_RoundRobinPointerDefinitions.BeforeRemove += new EventListHandler(OnRoundRobinPointerDefinitions_BeforeRemove);					
					_RoundRobinPointerDefinitions.AfterAdd += new EventListHandler(OnRoundRobinPointerDefinitions_AfterAdd);					
					_RoundRobinPointerDefinitions.AfterRemove += new EventListHandler(OnRoundRobinPointerDefinitions_AfterRemove);					
				}
				return _RoundRobinPointerDefinitions;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.RoundRobinPointer_DAO.GeneralStatus
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
		public override void Refresh()
		{
			base.Refresh();
			_RoundRobinPointerDefinitions = null;
			
		}
	}
}


