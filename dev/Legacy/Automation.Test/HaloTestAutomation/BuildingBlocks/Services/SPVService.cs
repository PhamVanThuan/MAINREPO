using Automation.DataAccess.DataHelper;
using Automation.DataAccess.DataHelper._2AM.Contracts;
using BuildingBlocks.Services.Contracts;
using System.Collections.Generic;

namespace BuildingBlocks.Services
{
    public class SPVTestService : ISPVTestService
    {
        private ISPVDataHelper dataHelper;
        public SPVTestService(ISPVDataHelper dataHelper)
        {
            this.dataHelper = dataHelper;
        }
        public IEnumerable<Automation.DataModels.SPV> GetActiveChildSPVs()
        {
            return dataHelper.GetActiveChildSPVs();
        }

        public void UpdateSPVDescription(int spvKey, string description)
        {
            dataHelper.UpdateSPVDescription(spvKey, description);
        }
    }
}