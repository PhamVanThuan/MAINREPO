using Automation.DataModels;
using Common.Enums;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace BuildingBlocks.Services.Contracts
{
    public interface IValuationService
    {
        IEnumerable<Automation.DataModels.Valuation> GetValuations(int propertyKey);

        Automation.DataModels.Valuation GetRandomValuationForAccountWithHOC(AccountStatusEnum accountStatus, HOCInsurerEnum hocInsurer, OriginationSourceEnum originationSource, HOCRoofEnum roofType);

        void UpdateValuationDateToDateLessThan12MonthsAgo(int offerKey);

        Automation.DataModels.Valuation InsertValuation(Automation.DataModels.Valuation valuation);

        void DeleteValuationRecord(Automation.DataModels.Valuation valuation);

        void UpdateValuationStatus(ValuationStatusEnum valuationStatus, int valuationkey);

        void UpdateAllValuationStatuses(Common.Enums.ValuationStatusEnum valuationStatus, int propertyKey);

        Automation.DataModels.Valuation GetActiveValuation(int propertykey);

        IEnumerable<Automation.DataModels.Valuer> GetActiveValuators();

        void CreateXmlHistoryDummy();

        void ClearAllXmlHistoryDummy();

        bool HasXmlHistoryDummy();

        void UpdateValuationIsActive(int activeValKey, bool isActive);

        IEnumerable<XmlHistory> GetValuationXmlHistory(int generickey);

        void SubmitCompletedEzVal(int genericKey, HOCRoofEnum roof, float conventionalAmount = 0.0f, float thatchAmount = 0.0f, float otherAmount = 0.0f, float valuationAmount = 0.00f);

        void SubmitRejectedEzVal(int genericKey);

        void SubmitAmendedEzVal(int genericKey, HOCRoofEnum roof, float conventionalAmount = 0.0f, float thatchAmount = 0.0f, float otherAmount = 0.0f, float valuationAmount = 0.00f);

        void SubmitInvalidCompletedEzVal(int genericKey);
    }
}