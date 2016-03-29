using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IExternalRoleService
    {
        void DeleteExternalRole(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType);

        QueryResults GetExternalRoleCorrespondenceDetails(int debtCounsellingKey, ExternalRoleTypeEnum externalRoleType);

        string GetActiveExternalRoleEmailAddress(int debtCounsellingKey, ExternalRoleTypeEnum roleType);

        int InsertExternalRole(int genericKey, GenericKeyTypeEnum genericKeyTypeKey, int legalEntityKey, ExternalRoleTypeEnum externalRoleTypeKey, GeneralStatusEnum status);

        int GetActiveExternalRoleCount(GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, int legalentitykey, int genericKey);

        List<Automation.DataModels.ExternalRole> GetActiveExternalRoleList(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, bool isfullLegalName = false);

        Automation.DataModels.ExternalRole GetFirstActiveExternalRole(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, bool isfullLegalName = false);

        Automation.DataModels.ExternalRole GetActiveExternalRoleByLegalEntity(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, int legalentitykey, bool isfullLegalName = false);

        Automation.DataModels.ExternalRole GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType, int legalentitykey, bool isfullLegalName = false);

        Dictionary<int, string> GetExternalRoleEmailAddress(int debtCounsellingKey, ExternalRoleTypeEnum externalRoleType);
    }
}