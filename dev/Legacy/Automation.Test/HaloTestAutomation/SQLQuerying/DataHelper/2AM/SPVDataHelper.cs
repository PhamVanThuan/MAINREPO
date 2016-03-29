using Automation.DataAccess.DataHelper;
using Automation.DataAccess.DataHelper._2AM.Contracts;
using Automation.DataAccess.Interfaces;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public class SPVDataHelper : ISPVDataHelper
    {
        private IDataContext dataContext;
        public SPVDataHelper(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IEnumerable<Automation.DataModels.SPV> GetActiveChildSPVs()
        {
            var sql = @"select
                s.spvKey, s.description, s.reportdescription, s.resetconfigurationkey,
                s.creditprovidernumber, s.registrationNumber,
                parent.SPVKey as ParentSPVKey, parent.Description as ParentDescription,
                 parent.ReportDescription as ParentReportDescription
                from [2am].spv.spv s
                join [2am].spv.spv parent on s.parentspvkey=parent.spvkey
                where s.generalstatuskey=1
                and s.spvKey <> s.parentSPVKey";
            return dataContext.Query<Automation.DataModels.SPV>(sql);
        }

        public void UpdateSPVDescription(int spvKey, string description)
        {
            var sql = string.Format(@"Update [2am].spv.SPV set Description='{0}' where spvKey = {1}", description, spvKey);
            dataContext.Execute(sql);
        }
    }
}