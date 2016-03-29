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
	/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO
	/// </summary>
	public partial class FailedPostalMigration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO>, IFailedPostalMigration
	{
				public FailedPostalMigration(SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO FailedPostalMigration) : base(FailedPostalMigration)
		{
			this._DAO = FailedPostalMigration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.RecordType
		/// </summary>
		public String RecordType 
		{
			get { return _DAO.RecordType; }
			set { _DAO.RecordType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientNumber
		/// </summary>
		public Decimal ClientNumber 
		{
			get { return _DAO.ClientNumber; }
			set { _DAO.ClientNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientBoxNumber
		/// </summary>
		public String ClientBoxNumber 
		{
			get { return _DAO.ClientBoxNumber; }
			set { _DAO.ClientBoxNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientBoxNumber2
		/// </summary>
		public String ClientBoxNumber2 
		{
			get { return _DAO.ClientBoxNumber2; }
			set { _DAO.ClientBoxNumber2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewAdd3
		/// </summary>
		public String NewAdd3 
		{
			get { return _DAO.NewAdd3; }
			set { _DAO.NewAdd3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientPostOffice
		/// </summary>
		public String ClientPostOffice 
		{
			get { return _DAO.ClientPostOffice; }
			set { _DAO.ClientPostOffice = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ClientPostalCode
		/// </summary>
		public String ClientPostalCode 
		{
			get { return _DAO.ClientPostalCode; }
			set { _DAO.ClientPostalCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewCity
		/// </summary>
		public String NewCity 
		{
			get { return _DAO.NewCity; }
			set { _DAO.NewCity = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewProvince
		/// </summary>
		public String NewProvince 
		{
			get { return _DAO.NewProvince; }
			set { _DAO.NewProvince = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.NewCountry
		/// </summary>
		public String NewCountry 
		{
			get { return _DAO.NewCountry; }
			set { _DAO.NewCountry = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.Faults
		/// </summary>
		public String Faults 
		{
			get { return _DAO.Faults; }
			set { _DAO.Faults = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.ResultSet
		/// </summary>
		public String ResultSet 
		{
			get { return _DAO.ResultSet; }
			set { _DAO.ResultSet = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.FailedLegalEntityAddresses
		/// </summary>
		private DAOEventList<FailedLegalEntityAddress_DAO, IFailedLegalEntityAddress, FailedLegalEntityAddress> _FailedLegalEntityAddresses;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedPostalMigration_DAO.FailedLegalEntityAddresses
		/// </summary>
		public IEventList<IFailedLegalEntityAddress> FailedLegalEntityAddresses
		{
			get
			{
				if (null == _FailedLegalEntityAddresses) 
				{
					if(null == _DAO.FailedLegalEntityAddresses)
						_DAO.FailedLegalEntityAddresses = new List<FailedLegalEntityAddress_DAO>();
					_FailedLegalEntityAddresses = new DAOEventList<FailedLegalEntityAddress_DAO, IFailedLegalEntityAddress, FailedLegalEntityAddress>(_DAO.FailedLegalEntityAddresses);
					_FailedLegalEntityAddresses.BeforeAdd += new EventListHandler(OnFailedLegalEntityAddresses_BeforeAdd);					
					_FailedLegalEntityAddresses.BeforeRemove += new EventListHandler(OnFailedLegalEntityAddresses_BeforeRemove);					
					_FailedLegalEntityAddresses.AfterAdd += new EventListHandler(OnFailedLegalEntityAddresses_AfterAdd);					
					_FailedLegalEntityAddresses.AfterRemove += new EventListHandler(OnFailedLegalEntityAddresses_AfterRemove);					
				}
				return _FailedLegalEntityAddresses;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FailedLegalEntityAddresses = null;
			
		}
	}
}


