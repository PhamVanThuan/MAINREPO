using Common.Enums;

using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IDetailTypeService
    {
        IEnumerable<Automation.DataModels.DetailClass> GetDetailClassRecords(int detailclasskey = 0);

        IEnumerable<Automation.DataModels.DetailType> GetDetailTypesForLoanServicingScreen(DetailClassEnum detailClass, GeneralStatusEnum status, bool allowScreen);

        Automation.DataModels.LoanDetail GetEmptyDetailRecord();

        Automation.DataModels.LoanDetail GetLoanDetailRecord(DetailTypeEnum expectedDetailType, int accountkey, DetailClassEnum expectedloanDetailClass);

        int GetOpenAccountWithDetailType(DetailTypeEnum detailTypeKey);

        void InsertDetailType(DetailTypeEnum detailType, int accountKey);

        void RemoveDetailTypes(int accountKey, List<DetailTypeEnum> detailTypes = null);
    }
}