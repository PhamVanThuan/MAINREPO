using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class AttorneyService : _2AMDataHelper, IAttorneyService
    {
        private readonly ILegalEntityService legalEntityService;

        public AttorneyService(ILegalEntityService legalEntityService)
        {
            this.legalEntityService = legalEntityService;
        }

        /// <summary>
        /// Get Attorney
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Attorney GetAttorney(bool litigationAttorney, bool registrationAttorney)
        {
            var attorneyList = GetAttorneys();
            return (from al in attorneyList
                    where al.IsLitigationAttorney == litigationAttorney && al.IsRegistrationAttorney == registrationAttorney
                    select al).FirstOrDefault();
        }

        /// <summary>
        /// Returns an active attorney when provided with a deeds office
        /// </summary>
        /// <param name="deedsOffice">Deeds Office</param>
        /// <returns>RegisteredName of Attorney</returns>
        public string GetActiveAttorneyNameByDeedsOffice(string deedsOffice)
        {
            var attorneyList = GetAttorneys();
            return (from al in attorneyList
                    where al.DeedsOffice == deedsOffice && al.IsRegistrationAttorney && al.Status == GeneralStatusEnum.Active
                    select al.LegalEntity.RegisteredName).FirstOrDefault();
        }

        public string SetRegistrationAttorney(string deedsOffice)
        {
            var attorneyList = GetAttorneys();
            return (from al in attorneyList
                    where al.DeedsOffice == deedsOffice && al.IsRegistrationAttorney && al.Status == GeneralStatusEnum.Active
                    select al.LegalEntity.RegisteredName).FirstOrDefault();
        }

        /// <summary>
        /// Gets a litigation attorney from the database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetLitigationAttorney()
        {
            Dictionary<int, string> attorney = new Dictionary<int, string>();
            var attorneys = GetAttorney(true, false);
            if (attorneys != null)
                attorney.Add(attorneys.LegalEntity.LegalEntityKey, attorneys.LegalEntity.RegisteredName);
            return attorney;
        }

        /// <summary>
        /// Returns the AttorneyKey when provided with the LegalEntityKey on the Attorney table
        /// </summary>
        /// <param name="legalEntityKey">legalEntityKey</param>
        /// <returns>Attorney.AttorneyKey</returns>
        public Automation.DataModels.Attorney GetAttorneyByLegalEntityKey(int legalEntityKey)
        {
            var attorneys = GetAttorneys();
            return (from a in attorneys where a.LegalEntity.LegalEntityKey == legalEntityKey select a).FirstOrDefault();
        }

        /// <summary>
        /// This will get an contact for an existing attorney.
        /// </summary>
        /// <param name="firstnames"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
        public Automation.DataModels.AttorneyContacts GetAttorneyContactRecord(string firstnames, string surname)
        {
            QueryResultsRow attorneyContactRow = attorneyContactRow = base.GetAttorneyContactRecord(firstnames, surname);
            if (attorneyContactRow == null)
                return default(Automation.DataModels.AttorneyContacts);

            //Populate new Models.AttorneyContact
            var attorneyContact = new Automation.DataModels.AttorneyContacts
                {
                    ExternalRoleType = (ExternalRoleTypeEnum)Enum.Parse(typeof(ExternalRoleTypeEnum), attorneyContactRow.Column("ExternalRoleTypeKey").Value)
                };

            //Populate new Automation.DataModels.LegalEntity
            var legalentity = new Automation.DataModels.LegalEntity
                {
                    FirstNames = attorneyContactRow.Column("firstnames").Value,
                    Surname = attorneyContactRow.Column("surname").Value,
                    WorkPhoneCode = attorneyContactRow.Column("HomePhoneCode").Value,
                    WorkPhoneNumber = attorneyContactRow.Column("HomePhoneNumber").Value,
                    FaxCode = attorneyContactRow.Column("FaxCode").Value,
                    FaxNumber = attorneyContactRow.Column("FaxNumber").Value,
                    EmailAddress = attorneyContactRow.Column("EmailAddress").Value
                };

            #region PopulateLegalEntity

            #endregion PopulateLegalEntity

            attorneyContact.LegalEntity = legalentity;
            return attorneyContact;
        }

        /// <summary>
        /// This will get an existing attorney by the company legalname
        /// </summary>
        /// <returns></returns>
        public QueryResultsRow GetAttorneyByKey(int attorneyKey)
        {
            var results = base.GetAttorneys();
            return (from r in results where r.Column("AttorneyKey").GetValueAs<int>() == attorneyKey select r).FirstOrDefault();
        }

        /// <summary>
        /// This will get an existing attorney by the company legalname
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.Attorney GetAttorneyRecord(string registeredname)
        {
            var attorneyList = GetAttorneys();
            return (from a in attorneyList where a.LegalEntity.RegisteredName == registeredname select a).FirstOrDefault();
        }

        /// <summary>
        /// This will get an existing attorney by the company legalname
        /// </summary>
        /// <returns></returns>
        public List<Automation.DataModels.Attorney> GetAttorneys()
        {
            //get the attorney record
            var attorneyRecords = base.GetAttorneys();
            var attorneys = new List<Automation.DataModels.Attorney>();
            foreach (var item in attorneyRecords)
            {
                string generalStatusKey = item.Column("GeneralStatusKey").Value;
                //we need a new attorney model
                var attorney = new Automation.DataModels.Attorney
                    {
                        AttorneyKey = item.Column("AttorneyKey").GetValueAs<int>(),
                        LegalEntity = legalEntityService.GetLegalEntity(registeredname: item.Column("registeredName").GetValueAs<string>()),
                        DeedsOffice = base.GetDeedsOfficeNameByAttorneyRegisteredName(item.Column("registeredName").GetValueAs<string>()),
                        ContactName = item.Column("AttorneyContact").Value,
                        IsRegistrationAttorney = item.Column("AttorneyRegistrationInd").GetValueAs<bool>(),
                        IsLitigationAttorney = item.Column("AttorneyLitigationInd").GetValueAs<bool>(),
                        IsWorkflowEnable = item.Column("AttorneyWorkFlowEnabled").GetValueAs<int>() == 1 ? true : false,
                        Mandate = item.Column("AttorneyMandate").GetValueAs<float>(),
                        Status = (GeneralStatusEnum)Enum.Parse(typeof(GeneralStatusEnum), generalStatusKey)
                    };
                //assign to the properties
                attorneys.Add(attorney);
            }
            return attorneys;
        }

        public void SetRegistrationIndbyAttorneykey(int attorneyKey)
        {
            base.SetRegistrationInd(attorneyKey);
        }
    }
}