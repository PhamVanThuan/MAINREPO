using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
	/// <summary>
	///
	/// </summary>
	public partial class ApplicationRole : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO>, IApplicationRole
	{
		private IApplication _application;
		private ILegalEntity _legalEntity;
		private IApplicationRoleDomicilium _applicationRoleDomicilium;

		public void OnApplicationRoleAttributes_AfterAdd(ICancelDomainArgs args, object Item)
		{
			UpdateCommonApplicationDetails();
		}

		public void OnApplicationRoleAttributes_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationRoleAttributes_AfterRemove(ICancelDomainArgs args, object Item)
		{
			UpdateCommonApplicationDetails();
		}

		public void OnApplicationRoleAttributes_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationRole_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationDeclarations_BeforeRemove(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationDeclarations_BeforeAdd(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationDeclarations_AfterRemove(ICancelDomainArgs args, object Item)
		{
		}

		public void OnApplicationDeclarations_AfterAdd(ICancelDomainArgs args, object Item)
		{
		}

		private void UpdateCommonApplicationDetails()
		{
			//If the offer is open (statuskey == 1 and
			//application information has not been accepted then update income
			IApplicationInformation ai = this.Application.GetLatestApplicationInformation();
			if (this.Application.IsOpen
				&& ai != null
				&& ai.ApplicationInformationType.Key != (int)OfferInformationTypes.AcceptedOffer)
			{
				// Recalculate Household Income
				this.Application.CalculateHouseHoldIncome();
				// Set Application Applicant Type
				this.Application.SetApplicantType();
				// And the employment type
				this.Application.SetEmploymentType();
			}
		}

		public override void OnPopulateRules(List<string> Rules)
		{
			base.OnPopulateRules(Rules);
			//Nazir J => 2008-07-14
			//
			//LegalEntityNatrualPerson
			Rules.Add("LegalEntityNaturalPersonMandatorySaluation");
			Rules.Add("LegalEntityNaturalPersonMandatoryInitials");
			// Preferred Name is optional ticket #9693
			//Rules.Add("LegalEntityNaturalPersonMandatoryPreferredName");
			Rules.Add("LegalEntityNaturalPersonMandatoryGender");
			Rules.Add("LegalEntityNaturalPersonMandatoryMaritalStatus");
			Rules.Add("LegalEntityNaturalPersonMandatoryPopulationGroup");
			Rules.Add("LegalEntityNaturalPersonMandatoryEducation");
			Rules.Add("LegalEntityNaturalPersonMandatoryCitizenType");
			Rules.Add("LegalEntityNaturalPersonMandatoryIDNumber");
			Rules.Add("LegalEntityNaturalPersonMandatoryPassportNumber");
			Rules.Add("LegalEntityNaturalPersonMandatoryDateOfBirth");
			Rules.Add("LegalEntityNaturalPersonMandatoryHomeLanguage");
			Rules.Add("LegalEntityNaturalPersonMandatoryDocumentLanguage");
			Rules.Add("LegalEntityNaturalPersonMandatoryLegalEntityStatus");
			Rules.Add("LegalEntityNaturalPersonValidateIDNumber");
			Rules.Add("LegalEntityNaturalPersonValidatePassportNumber");
			Rules.Add("LegalEntityNaturalPersonIsPassportNumberUnique");
			Rules.Add("LegalEntityNaturalPersonIsIDNumberUnique");
			//
			//LegalEntityCompanyCCTrust
			Rules.Add("LegalEntityCompanyCCTrustMandatoryTradingName");
			Rules.Add("LegalEntityCompanyCCTrustMandatoryContact");
			Rules.Add("LegalEntityCompanyCCTrustMandatoryRegisteredName");
			Rules.Add("LegalEntityCompanyCCTrustMandatoryRegistrationNumber");
			//The Tax Number is optional  - #8968
			//Rules.Add("LegalEntityCompanyCCTrustMandatoryTaxNumber");
			Rules.Add("LegalEntityCompanyCCTrustMandatoryDocumentLanguage");
			Rules.Add("LegalEntityCompanyCCTrustMandatoryLegalEntityStatus");
			Rules.Add("ApplicationRoleUpdateLegalEntityMinimum");
			// #14940 - Enabling CheckFurtherLendingApplicationRoleBeforeUpdate
			Rules.Add("CheckFurtherLendingApplicationRoleBeforeUpdate");
			Rules.Add("OfferRoleFLAddMainApplicant");
		}

		/// <summary>
		/// Gets/sets the Application to which the role applies.
		/// </summary>
		public IApplication Application
		{
			get
			{
				if (ApplicationKey == 0)
					return null;

				// if the application object is null or the ApplicationKey has changed since we loaded the object,
				// then reload
				if (_application == null || _application.Key != ApplicationKey)
				{
					Application_DAO appDao = Application_DAO.Find(ApplicationKey);
					IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					_application = bmtm.GetMappedType<IApplication>(appDao);
				}
				return _application;
			}
			set
			{
				_application = value;
				ApplicationKey = _application.Key;
			}
		}

		/// <summary>
		/// Gets/sets the LegalEntity to which the role applies.
		/// </summary>
		public ILegalEntity LegalEntity
		{
            get
            {
                if (LegalEntityKey == 0)
                    return null;

                if (_legalEntity == null || _legalEntity.Key != LegalEntityKey)
                {
                    LegalEntity_DAO legalEntity_DAO = LegalEntity_DAO.Find(LegalEntityKey);
                    IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    _legalEntity = bmtm.GetMappedType<ILegalEntity>(legalEntity_DAO);
                }
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
                LegalEntityKey = _legalEntity.Key;
            }
		}

		public IApplicationRoleDomicilium ApplicationRoleDomicilium
		{
			get
			{
				var spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());

				IBusinessModelTypeMapper businessModelTypeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();

				if (_DAO.ApplicationRoleDomiciliums == null || _DAO.ApplicationRoleDomiciliums.Count == 0)
					return null;

				var applicationRoleDomiciliumDAO  = _DAO.ApplicationRoleDomiciliums.FirstOrDefault();
				if (applicationRoleDomiciliumDAO == null)
					return null;

				IApplicationRoleDomicilium applicationRoleDomicilium = businessModelTypeMapper.GetMappedType<IApplicationRoleDomicilium>(applicationRoleDomiciliumDAO);

				return applicationRoleDomicilium;
			}
            set
            {
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationRoleDomiciliums.Add((ApplicationRoleDomicilium_DAO)obj.GetDAOObject());
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OfferRoleAttributeType"></param>
        /// <returns></returns>
        public bool HasAttribute(OfferRoleAttributeTypes OfferRoleAttributeType)
        {
            bool hasAttribute = false;

            foreach (IApplicationRoleAttribute applicationRoleAttribute in this.ApplicationRoleAttributes)
            {
                if (applicationRoleAttribute.OfferRoleAttributeType.Key == (int)OfferRoleAttributeType)
                {
                    hasAttribute = true;
                    break;
                }
            }

            return hasAttribute;
        }
	}
}