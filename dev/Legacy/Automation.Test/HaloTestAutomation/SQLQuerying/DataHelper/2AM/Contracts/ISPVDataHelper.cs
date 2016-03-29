using System.Collections.Generic;
namespace Automation.DataAccess.DataHelper._2AM.Contracts
{
    public interface ISPVDataHelper : IDataHelper
    {
        IEnumerable<Automation.DataModels.SPV> GetActiveChildSPVs();
        void UpdateSPVDescription(int spvKey, string description);
    }
}