using Automation.DataAccess;
using Common.Enums;
using System;

namespace BuildingBlocks.Services.Contracts
{
    public interface ICourtDetailsService
    {
        void InsertCourtDetails(int debtCounsellingKey, HearingTypeEnum hearingType, HearingAppearanceTypeEnum hearingAppearanceType, DateTime hearingDate, string comment = "Test Automation Comment");

        void DeleteCourtDetails(int debtCounsellingKey);

        bool CourtDetailsExist(int debtCounsellingKey);

        QueryResults GetCourtDetails(int debtCounsellingKey, Automation.DataModels.CourtDetails courtDetails);

        QueryResults GetTribunalDetails(int debtCounsellingKey, Automation.DataModels.CourtDetails courtDetails);

        bool ActiveCourtDetailsExist(int debtCounsellingKey);
    }
}