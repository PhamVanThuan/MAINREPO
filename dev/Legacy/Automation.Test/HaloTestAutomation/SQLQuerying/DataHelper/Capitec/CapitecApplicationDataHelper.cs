using Automation.DataAccess.Interfaces;
using Automation.DataModels;
using Common.Enums;
using System;
using System.Collections.Generic;
namespace Automation.DataAccess.DataHelper.Capitec
{
    public sealed class CapitecApplicationDataHelper: ICapitecApplicationDataHelper
    {
        private IDataContext dataContext;

        public CapitecApplicationDataHelper()
        {
            dataContext = new DataContext(KnownDBs.Capitec);
        }

        public IEnumerable<ReservedApplication> GetReservedApplications()
        {
            return this.dataContext.Query<ReservedApplication>("select * from capitec.dbo.ReservedApplicationNumber");
        }
          
        public void UpdateReservedApplicationNumber(int applicationKey,bool isUsed)
        {
            this.dataContext.Execute(String.Format("update capitec.dbo.ReservedApplicationNumber set IsUsed={0} where ApplicationNumber = {1}", isUsed==true?1:0,applicationKey));
        }
    }
}
