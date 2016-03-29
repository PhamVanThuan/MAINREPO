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
	/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO
	/// </summary>
	public partial class FailedStreetMigration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO>, IFailedStreetMigration
	{
				public FailedStreetMigration(SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO FailedStreetMigration) : base(FailedStreetMigration)
		{
			this._DAO = FailedStreetMigration;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.RecordType
		/// </summary>
		public String RecordType 
		{
			get { return _DAO.RecordType; }
			set { _DAO.RecordType = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ClientNumber
		/// </summary>
		public Decimal ClientNumber 
		{
			get { return _DAO.ClientNumber; }
			set { _DAO.ClientNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add1
		/// </summary>
		public String Add1 
		{
			get { return _DAO.Add1; }
			set { _DAO.Add1 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add2
		/// </summary>
		public String Add2 
		{
			get { return _DAO.Add2; }
			set { _DAO.Add2 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add3
		/// </summary>
		public String Add3 
		{
			get { return _DAO.Add3; }
			set { _DAO.Add3 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add4
		/// </summary>
		public String Add4 
		{
			get { return _DAO.Add4; }
			set { _DAO.Add4 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Add5
		/// </summary>
		public String Add5 
		{
			get { return _DAO.Add5; }
			set { _DAO.Add5 = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.PCode
		/// </summary>
		public String PCode 
		{
			get { return _DAO.PCode; }
			set { _DAO.PCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.City
		/// </summary>
		public String City 
		{
			get { return _DAO.City; }
			set { _DAO.City = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Province
		/// </summary>
		public String Province 
		{
			get { return _DAO.Province; }
			set { _DAO.Province = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Country
		/// </summary>
		public String Country 
		{
			get { return _DAO.Country; }
			set { _DAO.Country = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ErrCode
		/// </summary>
		public String ErrCode 
		{
			get { return _DAO.ErrCode; }
			set { _DAO.ErrCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.ResultSet
		/// </summary>
		public String ResultSet 
		{
			get { return _DAO.ResultSet; }
			set { _DAO.ResultSet = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedLegalEntityAddresses
		/// </summary>
		private DAOEventList<FailedLegalEntityAddress_DAO, IFailedLegalEntityAddress, FailedLegalEntityAddress> _FailedLegalEntityAddresses;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedLegalEntityAddresses
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedPropertyAddresses
		/// </summary>
		private DAOEventList<FailedPropertyAddress_DAO, IFailedPropertyAddress, FailedPropertyAddress> _FailedPropertyAddresses;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FailedStreetMigration_DAO.FailedPropertyAddresses
		/// </summary>
		public IEventList<IFailedPropertyAddress> FailedPropertyAddresses
		{
			get
			{
				if (null == _FailedPropertyAddresses) 
				{
					if(null == _DAO.FailedPropertyAddresses)
						_DAO.FailedPropertyAddresses = new List<FailedPropertyAddress_DAO>();
					_FailedPropertyAddresses = new DAOEventList<FailedPropertyAddress_DAO, IFailedPropertyAddress, FailedPropertyAddress>(_DAO.FailedPropertyAddresses);
					_FailedPropertyAddresses.BeforeAdd += new EventListHandler(OnFailedPropertyAddresses_BeforeAdd);					
					_FailedPropertyAddresses.BeforeRemove += new EventListHandler(OnFailedPropertyAddresses_BeforeRemove);					
					_FailedPropertyAddresses.AfterAdd += new EventListHandler(OnFailedPropertyAddresses_AfterAdd);					
					_FailedPropertyAddresses.AfterRemove += new EventListHandler(OnFailedPropertyAddresses_AfterRemove);					
				}
				return _FailedPropertyAddresses;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FailedLegalEntityAddresses = null;
			_FailedPropertyAddresses = null;
			
		}
	}
}


