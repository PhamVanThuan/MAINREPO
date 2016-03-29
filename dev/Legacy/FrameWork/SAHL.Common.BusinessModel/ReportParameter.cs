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
using System.Data;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO
	/// </summary>
    public partial class ReportParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReportParameter_DAO>, IReportParameter
	{
        Dictionary<string, object> _validValues;

        public ReportParameter(SAHL.Common.BusinessModel.DAO.ReportParameter_DAO ReportParameter) : base(ReportParameter)
		{
			this._DAO = ReportParameter;
		}

        public ReportParameter_DAO DAO
        {
            get { return _DAO; }
            set { _DAO = value; }
        }


        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.StatementName
        /// </summary>
        public string StatementName
        {
            get { return _DAO.StatementName; }
            set { _DAO.StatementName = value; }
        }

        /// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterName
		/// </summary>
		public String ParameterName 
		{
			get { return _DAO.ParameterName; }
			set { _DAO.ParameterName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ParameterLength
		/// </summary>
		public Int32? ParameterLength
		{
			get { return _DAO.ParameterLength; }
			set { _DAO.ParameterLength = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DisplayName
		/// </summary>
		public String DisplayName 
		{
			get { return _DAO.DisplayName; }
			set { _DAO.DisplayName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.Required
		/// </summary>
		public Boolean? Required
		{
			get { return _DAO.Required; }
			set { _DAO.Required = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.DomainField
		/// </summary>
		public IDomainField DomainField 
		{
			get
			{
				if (null == _DAO.DomainField) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDomainField, DomainField_DAO>(_DAO.DomainField);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DomainField = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DomainField = (DomainField_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportParameterType
		/// </summary>
		public IReportParameterType ReportParameterType 
		{
			get
			{
				if (null == _DAO.ReportParameterType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReportParameterType, ReportParameterType_DAO>(_DAO.ReportParameterType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReportParameterType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReportParameterType = (ReportParameterType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ReportParameter_DAO.ReportStatement
		/// </summary>
		public IReportStatement ReportStatement 
		{
			get
			{
				if (null == _DAO.ReportStatement) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReportStatement, ReportStatement_DAO>(_DAO.ReportStatement);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReportStatement = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReportStatement = (ReportStatement_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}

        
    }
}


