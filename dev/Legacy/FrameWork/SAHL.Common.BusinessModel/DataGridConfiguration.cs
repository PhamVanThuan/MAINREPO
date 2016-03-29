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
	/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO
	/// </summary>
	public partial class DataGridConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO>, IDataGridConfiguration
	{
				public DataGridConfiguration(SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO DataGridConfiguration) : base(DataGridConfiguration)
		{
			this._DAO = DataGridConfiguration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.StatementName
		/// </summary>
		public String StatementName 
		{
			get { return _DAO.StatementName; }
			set { _DAO.StatementName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.ColumnName
		/// </summary>
		public String ColumnName 
		{
			get { return _DAO.ColumnName; }
			set { _DAO.ColumnName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.ColumnDescription
		/// </summary>
		public String ColumnDescription 
		{
			get { return _DAO.ColumnDescription; }
			set { _DAO.ColumnDescription = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Sequence
		/// </summary>
		public Int32 Sequence 
		{
			get { return _DAO.Sequence; }
			set { _DAO.Sequence = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Width
		/// </summary>
		public String Width 
		{
			get { return _DAO.Width; }
			set { _DAO.Width = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Visible
		/// </summary>
		public Boolean Visible 
		{
			get { return _DAO.Visible; }
			set { _DAO.Visible = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.IndexIdentifier
		/// </summary>
		public Boolean IndexIdentifier 
		{
			get { return _DAO.IndexIdentifier; }
			set { _DAO.IndexIdentifier = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.DataGridConfigurationType
		/// </summary>
		public IDataGridConfigurationType DataGridConfigurationType 
		{
			get
			{
				if (null == _DAO.DataGridConfigurationType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IDataGridConfigurationType, DataGridConfigurationType_DAO>(_DAO.DataGridConfigurationType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.DataGridConfigurationType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.DataGridConfigurationType = (DataGridConfigurationType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.DataGridConfiguration_DAO.FormatType
		/// </summary>
		public IFormatType FormatType 
		{
			get
			{
				if (null == _DAO.FormatType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFormatType, FormatType_DAO>(_DAO.FormatType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FormatType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FormatType = (FormatType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


