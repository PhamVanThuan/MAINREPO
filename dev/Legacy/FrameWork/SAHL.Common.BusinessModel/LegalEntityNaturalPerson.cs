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
	/// LegalEntityNaturalPerson_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type "Natural Person."
	/// </summary>
	public partial class LegalEntityNaturalPerson : LegalEntity, ILegalEntityNaturalPerson
	{
		protected new SAHL.Common.BusinessModel.DAO.LegalEntityNaturalPerson_DAO _DAO;
		public LegalEntityNaturalPerson(SAHL.Common.BusinessModel.DAO.LegalEntityNaturalPerson_DAO LegalEntityNaturalPerson) : base(LegalEntityNaturalPerson)
		{
			this._DAO = LegalEntityNaturalPerson;
		}
		/// <summary>
		/// The foreign key reference to the PopulationGroup table. A Natural Person Legal Entity belongs to a single Population Group type.
		/// </summary>
		public IPopulationGroup PopulationGroup 
		{
			get
			{
				if (null == _DAO.PopulationGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IPopulationGroup, PopulationGroup_DAO>(_DAO.PopulationGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.PopulationGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.PopulationGroup = (PopulationGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The Preferred Name of the Natural Person.
		/// </summary>
		public String PreferredName 
		{
			get { return _DAO.PreferredName; }
			set { _DAO.PreferredName = value;}
		}
		/// <summary>
		/// The Date of Birth of the Natural Person.
		/// </summary>
		public DateTime? DateOfBirth
		{
			get { return _DAO.DateOfBirth; }
			set { _DAO.DateOfBirth = value;}
		}
		/// <summary>
		/// The highest education level that the Legal Entity has achieved.
		/// </summary>
		public IEducation Education 
		{
			get
			{
				if (null == _DAO.Education) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IEducation, Education_DAO>(_DAO.Education);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Education = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Education = (Education_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The Legal Entity's Home Language.
		/// </summary>
		public ILanguage HomeLanguage 
		{
			get
			{
				if (null == _DAO.HomeLanguage) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ILanguage, Language_DAO>(_DAO.HomeLanguage);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HomeLanguage = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HomeLanguage = (Language_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityNaturalPerson_DAO.ITCs
		/// </summary>
		private DAOEventList<ITC_DAO, IITC, ITC> _ITCs;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.LegalEntityNaturalPerson_DAO.ITCs
		/// </summary>
		public IEventList<IITC> ITCs
		{
			get
			{
				if (null == _ITCs) 
				{
					if(null == _DAO.ITCs)
						_DAO.ITCs = new List<ITC_DAO>();
					_ITCs = new DAOEventList<ITC_DAO, IITC, ITC>(_DAO.ITCs);
					_ITCs.BeforeAdd += new EventListHandler(OnITCs_BeforeAdd);					
					_ITCs.BeforeRemove += new EventListHandler(OnITCs_BeforeRemove);					
					_ITCs.AfterAdd += new EventListHandler(OnITCs_AfterAdd);					
					_ITCs.AfterRemove += new EventListHandler(OnITCs_AfterRemove);					
				}
				return _ITCs;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ITCs = null;
			
		}
	}
}


