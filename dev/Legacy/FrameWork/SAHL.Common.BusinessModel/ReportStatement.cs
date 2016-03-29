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
	/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO
	/// </summary>
	public partial class ReportStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportStatement_DAO>, IReportStatement
	{
				public ReportStatement(SAHL.Common.BusinessModel.DAO.ReportStatement_DAO ReportStatement) : base(ReportStatement)
		{
			this._DAO = ReportStatement;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.ReportName
		/// </summary>
		public String ReportName 
		{
			get { return _DAO.ReportName; }
			set { _DAO.ReportName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.GroupBy
		/// </summary>
		public String GroupBy 
		{
			get { return _DAO.GroupBy; }
			set { _DAO.GroupBy = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.OrderBy
		/// </summary>
		public String OrderBy 
		{
			get { return _DAO.OrderBy; }
			set { _DAO.OrderBy = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.ReportOutputPath
		/// </summary>
		public String ReportOutputPath 
		{
			get { return _DAO.ReportOutputPath; }
			set { _DAO.ReportOutputPath = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.Feature
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
		/// <summary>
		/// The OriginationSourceProduct for which this Report is defined.
		/// </summary>
		public IOriginationSourceProduct OriginationSourceProduct 
		{
			get
			{
				if (null == _DAO.OriginationSourceProduct) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSourceProduct, OriginationSourceProduct_DAO>(_DAO.OriginationSourceProduct);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSourceProduct = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// a grouping of reports
		/// </summary>
		public IReportGroup ReportGroup 
		{
			get
			{
				if (null == _DAO.ReportGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReportGroup, ReportGroup_DAO>(_DAO.ReportGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReportGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReportGroup = (ReportGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The type of this report.
		/// </summary>
		public IReportType ReportType 
		{
			get
			{
				if (null == _DAO.ReportType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReportType, ReportType_DAO>(_DAO.ReportType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReportType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReportType = (ReportType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.CorrespondenceMediums
		/// </summary>
		private DAOEventList<CorrespondenceMedium_DAO, ICorrespondenceMedium, CorrespondenceMedium> _CorrespondenceMediums;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportStatement_DAO.CorrespondenceMediums
		/// </summary>
		public IEventList<ICorrespondenceMedium> CorrespondenceMediums
		{
			get
			{
				if (null == _CorrespondenceMediums) 
				{
					if(null == _DAO.CorrespondenceMediums)
						_DAO.CorrespondenceMediums = new List<CorrespondenceMedium_DAO>();
					_CorrespondenceMediums = new DAOEventList<CorrespondenceMedium_DAO, ICorrespondenceMedium, CorrespondenceMedium>(_DAO.CorrespondenceMediums);
					_CorrespondenceMediums.BeforeAdd += new EventListHandler(OnCorrespondenceMediums_BeforeAdd);					
					_CorrespondenceMediums.BeforeRemove += new EventListHandler(OnCorrespondenceMediums_BeforeRemove);					
					_CorrespondenceMediums.AfterAdd += new EventListHandler(OnCorrespondenceMediums_AfterAdd);					
					_CorrespondenceMediums.AfterRemove += new EventListHandler(OnCorrespondenceMediums_AfterRemove);					
				}
				return _CorrespondenceMediums;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CorrespondenceMediums = null;
			
		}
	}
}


