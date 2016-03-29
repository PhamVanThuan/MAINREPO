using Automation.DataAccess;
using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IMemoService
    {
        void InsertMemo(GenericKeyTypeEnum genericKeyType, int genericKey, string memoText, int adUserKey, GeneralStatusEnum generalStatus);

        QueryResults GetFLAppReceivedMemo(int accountKey, int offerKey);

        Dictionary<int, string> GetLatestMemoColumn(int genericKey, GenericKeyTypeEnum genericKeyTypeKey, string columnName);

        string GetMemoColumn(int genericKey, int genericKeyTypeKey, string columnName);
    }
}