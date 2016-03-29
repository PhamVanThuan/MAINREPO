using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Services
{
    public class ExternalRoleService : _2AMDataHelper, IExternalRoleService
    {
        /// <summary>
        /// Gets the correspondence details of the legal entity playing an external role against a case.
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetExternalRoleEmailAddress(int debtCounsellingKey, ExternalRoleTypeEnum externalRoleType)
        {
            Dictionary<int, string> contactDetails = new Dictionary<int, string>();
            var r = base.GetExternalRoleCorrespondenceDetails(debtCounsellingKey, externalRoleType);
            contactDetails.Add(r.Rows(0).Column("LegalEntityKey").GetValueAs<int>(),
                r.Rows(0).Column("EmailAddress").GetValueAs<string>());
            return contactDetails;
        }

        /// <summary>
        /// Gets the active external role against a generic key
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        public Automation.DataModels.ExternalRole GetActiveExternalRoleByLegalEntity(int genericKey, GenericKeyTypeEnum genericKeyType,
            ExternalRoleTypeEnum externalRoleType, int legalentitykey, bool isfullLegalName = false)
        {
            var externalRoleList = base.GetExternalRoles(genericKey, genericKeyType, isfullLegalName);
            return (from extRole in externalRoleList
                    where extRole.LegalEntityKey == legalentitykey
                             && extRole.ExternalRoleTypeKey == externalRoleType
                             && extRole.GeneralStatusKey == GeneralStatusEnum.Active
                    select extRole).FirstOrDefault();
        }

        /// <summary>
        /// Gets the active external role against a legalentity key
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        public Automation.DataModels.ExternalRole GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum genericKeyType,
            ExternalRoleTypeEnum externalRoleType, int legalentitykey, bool isfullLegalName = false)
        {
            var externalRoleList = base.GetExternalRoles(legalentitykey, isfullLegalName);
            return (from extRole in externalRoleList
                    where extRole.LegalEntityKey == legalentitykey
                             && extRole.ExternalRoleTypeKey == externalRoleType
                             && extRole.GeneralStatusKey == GeneralStatusEnum.Active
                    select extRole).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first active external role against a generic key
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <returns></returns>
        public Automation.DataModels.ExternalRole GetFirstActiveExternalRole(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType,
            bool isfullLegalName = false)
        {
            var externalRoleList = base.GetExternalRoles(genericKey, genericKeyType, isfullLegalName);
            return (from extRole in externalRoleList
                    where extRole.ExternalRoleTypeKey == externalRoleType
                             && extRole.GeneralStatusKey == GeneralStatusEnum.Active
                    select extRole).FirstOrDefault();
        }

        /// <summary>
        /// Gets all the active external roles against a generic key
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyType"></param>
        /// <returns></returns>
        public List<Automation.DataModels.ExternalRole> GetActiveExternalRoleList(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType,
            bool isfullLegalName = false)
        {
            var externalRoleList = base.GetExternalRoles(genericKey, genericKeyType, isfullLegalName);
            return (from extRole in externalRoleList
                    where extRole.ExternalRoleTypeKey == externalRoleType
                             && extRole.GeneralStatusKey == GeneralStatusEnum.Active
                    select extRole).ToList();
        }

        /// <summary>
        /// Gets active external role count against a generic key
        /// </summary>
        /// <param name="genericKeyType"></param>
        /// <param name="externalRoleType"></param>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public int GetActiveExternalRoleCount(GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, int legalentitykey,
            int genericKey)
        {
            var externalRoleList = base.GetExternalRoles(genericKey, genericKeyType, true);
            return (from extRole in externalRoleList
                    where extRole.LegalEntityKey == legalentitykey
                             && extRole.ExternalRoleTypeKey == externalRoleType
                             && extRole.GeneralStatusKey == GeneralStatusEnum.Active
                    select extRole).Count();
        }

        /// <summary>
        /// This building block will insert a new External Role. If you do not have a legal entity to create in the role and you want to create a new
        /// one then you can pass a 0 as the LegalEntityKey. This will result in a new legal entity being inserted into the database and being added
        /// as the external role specified.
        /// </summary>
        /// <param name="genericKey">genericKey</param>
        /// <param name="genericKeyTypeKey">genericKeyTypeKey</param>
        /// <param name="legalEntityKey">legalEntityKey (0 = Create New LE)</param>
        /// <param name="externalRoleTypeKey">externalRoleTypeKey</param>
        /// <param name="status">status</param>
        public int InsertExternalRole(int genericKey, GenericKeyTypeEnum genericKeyTypeKey, int legalEntityKey, ExternalRoleTypeEnum externalRoleTypeKey,
            GeneralStatusEnum status)
        {
            var idNumber = IDNumbers.GetNextIDNumber();
            if (legalEntityKey == 0)
            {
                legalEntityKey = base.CreateNewLegalEntity("test@test.co.za", idNumber);
            }
            base.InsertExternalRole(genericKey, genericKeyTypeKey, legalEntityKey, externalRoleTypeKey,
            status);
            return legalEntityKey;
        }

        /// <summary>
        /// Gets the debt counsellor's email address
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public string GetActiveExternalRoleEmailAddress(int debtCounsellingKey, ExternalRoleTypeEnum roleType)
        {
            var contactDetails = GetExternalRoleEmailAddress(debtCounsellingKey, roleType);
            string emailAddress = (from c in contactDetails
                                   select c.Value).FirstOrDefault();
            return emailAddress;
        }
    }
}