
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO
	/// </summary>
    public partial class LegalEntity_WTF : BusinessModelBase<LegalEntity_WTF_DAO>, ILegalEntity_WTF
	{
        public LegalEntity_WTF(LegalEntity_WTF_DAO LegalEntity_WTF) : base(LegalEntity_WTF)
		{
            this._DAO = LegalEntity_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityTypeKey
		/// </summary>
		public Int32 LegalEntityTypeKey 
		{
			get { return _DAO.LegalEntityTypeKey; }
			set { _DAO.LegalEntityTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.MaritalStatusKey
		/// </summary>
		public Int32 MaritalStatusKey 
		{
			get { return _DAO.MaritalStatusKey; }
			set { _DAO.MaritalStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.GenderKey
		/// </summary>
		public Int32 GenderKey 
		{
			get { return _DAO.GenderKey; }
			set { _DAO.GenderKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PopulationGroupKey
		/// </summary>
		public Int32 PopulationGroupKey 
		{
			get { return _DAO.PopulationGroupKey; }
			set { _DAO.PopulationGroupKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.IntroductionDate
		/// </summary>
		public DateTime? IntroductionDate 
		{
			get { return _DAO.IntroductionDate; }
			set { _DAO.IntroductionDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Salutationkey
		/// </summary>
		public Int32 Salutationkey 
		{
			get { return _DAO.Salutationkey; }
			set { _DAO.Salutationkey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FirstNames
		/// </summary>
		public String FirstNames 
		{
			get { return _DAO.FirstNames; }
			set { _DAO.FirstNames = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Initials
		/// </summary>
		public String Initials 
		{
			get { return _DAO.Initials; }
			set { _DAO.Initials = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Surname
		/// </summary>
		public String Surname 
		{
			get { return _DAO.Surname; }
			set { _DAO.Surname = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PreferredName
		/// </summary>
		public String PreferredName 
		{
			get { return _DAO.PreferredName; }
			set { _DAO.PreferredName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.IDNumber
		/// </summary>
		public String IDNumber 
		{
			get { return _DAO.IDNumber; }
			set { _DAO.IDNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.PassportNumber
		/// </summary>
		public String PassportNumber 
		{
			get { return _DAO.PassportNumber; }
			set { _DAO.PassportNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.TaxNumber
		/// </summary>
		public String TaxNumber 
		{
			get { return _DAO.TaxNumber; }
			set { _DAO.TaxNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.RegistrationNumber
		/// </summary>
		public String RegistrationNumber 
		{
			get { return _DAO.RegistrationNumber; }
			set { _DAO.RegistrationNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.RegisteredName
		/// </summary>
		public String RegisteredName 
		{
			get { return _DAO.RegisteredName; }
			set { _DAO.RegisteredName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.TradingName
		/// </summary>
		public String TradingName 
		{
			get { return _DAO.TradingName; }
			set { _DAO.TradingName = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.DateOfBirth
		/// </summary>
		public DateTime? DateOfBirth 
		{
			get { return _DAO.DateOfBirth; }
			set { _DAO.DateOfBirth = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomePhoneCode
		/// </summary>
		public String HomePhoneCode 
		{
			get { return _DAO.HomePhoneCode; }
			set { _DAO.HomePhoneCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomePhoneNumber
		/// </summary>
		public String HomePhoneNumber 
		{
			get { return _DAO.HomePhoneNumber; }
			set { _DAO.HomePhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.WorkPhoneCode
		/// </summary>
		public String WorkPhoneCode 
		{
			get { return _DAO.WorkPhoneCode; }
			set { _DAO.WorkPhoneCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.WorkPhoneNumber
		/// </summary>
		public String WorkPhoneNumber 
		{
			get { return _DAO.WorkPhoneNumber; }
			set { _DAO.WorkPhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.CellPhoneNumber
		/// </summary>
		public String CellPhoneNumber 
		{
			get { return _DAO.CellPhoneNumber; }
			set { _DAO.CellPhoneNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.EmailAddress
		/// </summary>
		public String EmailAddress 
		{
			get { return _DAO.EmailAddress; }
			set { _DAO.EmailAddress = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FaxCode
		/// </summary>
		public String FaxCode 
		{
			get { return _DAO.FaxCode; }
			set { _DAO.FaxCode = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.FaxNumber
		/// </summary>
		public String FaxNumber 
		{
			get { return _DAO.FaxNumber; }
			set { _DAO.FaxNumber = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Password
		/// </summary>
		public String Password 
		{
			get { return _DAO.Password; }
			set { _DAO.Password = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.CitizenTypeKey
		/// </summary>
		public Int32 CitizenTypeKey 
		{
			get { return _DAO.CitizenTypeKey; }
			set { _DAO.CitizenTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityStatusKey
		/// </summary>
		public Int32 LegalEntityStatusKey 
		{
			get { return _DAO.LegalEntityStatusKey; }
			set { _DAO.LegalEntityStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Comments
		/// </summary>
		public String Comments 
		{
			get { return _DAO.Comments; }
			set { _DAO.Comments = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.LegalEntityExceptionStatusKey
		/// </summary>
		public Int32 LegalEntityExceptionStatusKey 
		{
			get { return _DAO.LegalEntityExceptionStatusKey; }
			set { _DAO.LegalEntityExceptionStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.UserID
		/// </summary>
		public String UserID 
		{
			get { return _DAO.UserID; }
			set { _DAO.UserID = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ChangeDate
		/// </summary>
		public DateTime? ChangeDate 
		{
			get { return _DAO.ChangeDate; }
			set { _DAO.ChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.EducationKey
		/// </summary>
		public Int32 EducationKey 
		{
			get { return _DAO.EducationKey; }
			set { _DAO.EducationKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.HomeLanguageKey
		/// </summary>
		public Int32 HomeLanguageKey 
		{
			get { return _DAO.HomeLanguageKey; }
			set { _DAO.HomeLanguageKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.DocumentLanguageKey
		/// </summary>
		public Int32 DocumentLanguageKey 
		{
			get { return _DAO.DocumentLanguageKey; }
			set { _DAO.DocumentLanguageKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ResidenceStatusKey
		/// </summary>
		public Int32 ResidenceStatusKey 
		{
			get { return _DAO.ResidenceStatusKey; }
			set { _DAO.ResidenceStatusKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ADUsers
		/// </summary>
        private DAOEventList<ADUser_WTF_DAO, IADUser_WTF, ADUser_WTF> _ADUsers;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ADUsers
		/// </summary>
        public IEventList<IADUser_WTF> ADUsers
		{
			get
			{
				if (null == _ADUsers) 
				{
					if(null == _DAO.ADUsers)
                        _DAO.ADUsers = new List<ADUser_WTF_DAO>();
                    _ADUsers = new DAOEventList<ADUser_WTF_DAO, IADUser_WTF, ADUser_WTF>(_DAO.ADUsers);
					_ADUsers.BeforeAdd += new EventListHandler(OnADUsers_BeforeAdd);					
					_ADUsers.BeforeRemove += new EventListHandler(OnADUsers_BeforeRemove);					
					_ADUsers.AfterAdd += new EventListHandler(OnADUsers_AfterAdd);					
					_ADUsers.AfterRemove += new EventListHandler(OnADUsers_AfterRemove);					
				}
				return _ADUsers;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ApplicationRoles
		/// </summary>
        private DAOEventList<ApplicationRole_WTF_DAO, IApplicationRole_WTF, ApplicationRole_WTF> _ApplicationRoles;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntity_DAO.ApplicationRoles
		/// </summary>
        public IEventList<IApplicationRole_WTF> ApplicationRoles
		{
			get
			{
				if (null == _ApplicationRoles) 
				{
					if(null == _DAO.ApplicationRoles)
                        _DAO.ApplicationRoles = new List<ApplicationRole_WTF_DAO>();
                    _ApplicationRoles = new DAOEventList<ApplicationRole_WTF_DAO, IApplicationRole_WTF, ApplicationRole_WTF>(_DAO.ApplicationRoles);
					_ApplicationRoles.BeforeAdd += new EventListHandler(OnApplicationRoles_BeforeAdd);					
					_ApplicationRoles.BeforeRemove += new EventListHandler(OnApplicationRoles_BeforeRemove);					
					_ApplicationRoles.AfterAdd += new EventListHandler(OnApplicationRoles_AfterAdd);					
					_ApplicationRoles.AfterRemove += new EventListHandler(OnApplicationRoles_AfterRemove);					
				}
				return _ApplicationRoles;
			}
		}
	}
}



