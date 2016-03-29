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
	/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO
	/// </summary>
	public partial class ReportGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportGroup_DAO>, IReportGroup
	{
				public ReportGroup(SAHL.Common.BusinessModel.DAO.ReportGroup_DAO ReportGroup) : base(ReportGroup)
		{
			this._DAO = ReportGroup;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.ReportStatements
		/// </summary>
		private DAOEventList<ReportStatement_DAO, IReportStatement, ReportStatement> _ReportStatements;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.ReportStatements
		/// </summary>
		public IEventList<IReportStatement> ReportStatements
		{
			get
			{
				if (null == _ReportStatements) 
				{
					if(null == _DAO.ReportStatements)
						_DAO.ReportStatements = new List<ReportStatement_DAO>();
					_ReportStatements = new DAOEventList<ReportStatement_DAO, IReportStatement, ReportStatement>(_DAO.ReportStatements);
					_ReportStatements.BeforeAdd += new EventListHandler(OnReportStatements_BeforeAdd);					
					_ReportStatements.BeforeRemove += new EventListHandler(OnReportStatements_BeforeRemove);					
					_ReportStatements.AfterAdd += new EventListHandler(OnReportStatements_AfterAdd);					
					_ReportStatements.AfterRemove += new EventListHandler(OnReportStatements_AfterRemove);					
				}
				return _ReportStatements;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportGroup_DAO.Feature
		/// </summary>
		public IFeature Feature 
		{
			get
			{
				if (null == _DAO.Feature) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFeature, Feature_DAO>(_DAO.Feature);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Feature = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Feature = (Feature_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ReportStatements = null;
			
		}
	}
}


