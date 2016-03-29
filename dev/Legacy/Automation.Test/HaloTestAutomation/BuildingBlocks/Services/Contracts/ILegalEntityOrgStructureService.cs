using Automation.DataAccess;
using Common.Enums;

namespace BuildingBlocks.Services.Contracts
{
    public interface ILegalEntityOrgStructureService
    {
        QueryResults GetLegalEntityFromLEOS(string rootNodeDescription, int legalEntityKeyToExclude = 0);

        void InsertNewDebtCounsellor();

        void InsertNewPDA();

        void DeleteLegalEntityOrganisationStructure(string firstnames_registeredname);

        void InsertLegalEntityOrganisationStructure(string name, LegalEntityTypeEnum leType, string idNumber_registrationnumber, int parentKey);

        Automation.DataModels.LegalEntityOrganisationStructure GetLegalEntityOrganisationStructure(int legalentitykey);

        void AddAndMaintainPDATestDataInOrganisationStructure();

        Automation.DataModels.LegalEntityOrganisationStructure GetCompanyLegalEntityOrganisationStructureTestData(string registeredname, string legalentitytype, string organisationtype);

        Automation.DataModels.LegalEntityOrganisationStructure GetNaturalLegalEntityOrganisationStructureAddTestData();

        Automation.DataModels.LegalEntityOrganisationStructure GetNaturalLegalEntityOrganisationStructureUpdateTestData();
    }
}