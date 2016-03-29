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
	/// ReasonTypeGroup_DAO is used to group the various Reason Types together. It also contains a ParentKey which allows 
		/// a hierarchy of ReasonTypeGroups to be built up.
	/// </summary>
	public partial class ReasonTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReasonTypeGroup_DAO>, IReasonTypeGroup
	{
				public ReasonTypeGroup(SAHL.Common.BusinessModel.DAO.ReasonTypeGroup_DAO ReasonTypeGroup) : base(ReasonTypeGroup)
		{
			this._DAO = ReasonTypeGroup;
		}
		/// <summary>
		/// The description of the ReasonTypeGroup.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
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
		/// There is a one-to-many relationship between ReasonTypeGroups and ReasonTypes. A single ReasonTypeGroup has many ReasonTypes
		/// which form part of the group.
		/// </summary>
		private DAOEventList<ReasonType_DAO, IReasonType, ReasonType> _ReasonTypes;
		/// <summary>
		/// There is a one-to-many relationship between ReasonTypeGroups and ReasonTypes. A single ReasonTypeGroup has many ReasonTypes
		/// which form part of the group.
		/// </summary>
		public IEventList<IReasonType> ReasonTypes
		{
			get
			{
				if (null == _ReasonTypes) 
				{
					if(null == _DAO.ReasonTypes)
						_DAO.ReasonTypes = new List<ReasonType_DAO>();
					_ReasonTypes = new DAOEventList<ReasonType_DAO, IReasonType, ReasonType>(_DAO.ReasonTypes);
					_ReasonTypes.BeforeAdd += new EventListHandler(OnReasonTypes_BeforeAdd);					
					_ReasonTypes.BeforeRemove += new EventListHandler(OnReasonTypes_BeforeRemove);					
					_ReasonTypes.AfterAdd += new EventListHandler(OnReasonTypes_AfterAdd);					
					_ReasonTypes.AfterRemove += new EventListHandler(OnReasonTypes_AfterRemove);					
				}
				return _ReasonTypes;
			}
		}
		/// <summary>
		/// This property will retrieve the children ReasonTypeGroups. e.g. Credit Decline Income and Credit Decline Profile Reason Type
		/// groups could all belong to a parent Reason Type group of Credit. This property could consist of many
		/// Reason Type groups.
		/// </summary>
		private DAOEventList<ReasonTypeGroup_DAO, IReasonTypeGroup, ReasonTypeGroup> _Children;
		/// <summary>
		/// This property will retrieve the children ReasonTypeGroups. e.g. Credit Decline Income and Credit Decline Profile Reason Type
		/// groups could all belong to a parent Reason Type group of Credit. This property could consist of many
		/// Reason Type groups.
		/// </summary>
		public IEventList<IReasonTypeGroup> Children
		{
			get
			{
				if (null == _Children) 
				{
					if(null == _DAO.Children)
						_DAO.Children = new List<ReasonTypeGroup_DAO>();
					_Children = new DAOEventList<ReasonTypeGroup_DAO, IReasonTypeGroup, ReasonTypeGroup>(_DAO.Children);
					_Children.BeforeAdd += new EventListHandler(OnChildren_BeforeAdd);					
					_Children.BeforeRemove += new EventListHandler(OnChildren_BeforeRemove);					
					_Children.AfterAdd += new EventListHandler(OnChildren_AfterAdd);					
					_Children.AfterRemove += new EventListHandler(OnChildren_AfterRemove);					
				}
				return _Children;
			}
		}
		/// <summary>
		/// This property is the ReasonTypeGroupKey which is serving as the Parent group. This property would only be a single
		/// Reason type group.
		/// </summary>
		public IReasonTypeGroup Parent 
		{
			get
			{
				if (null == _DAO.Parent) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReasonTypeGroup, ReasonTypeGroup_DAO>(_DAO.Parent);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Parent = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Parent = (ReasonTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ReasonTypes = null;
			_Children = null;
			
		}
	}
}


