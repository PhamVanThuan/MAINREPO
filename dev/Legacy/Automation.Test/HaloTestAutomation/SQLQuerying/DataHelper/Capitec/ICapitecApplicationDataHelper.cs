using Automation.DataAccess.Interfaces;
using Automation.DataModels;
using Common.Enums;
using System;
using System.Collections.Generic;
namespace Automation.DataAccess.DataHelper.Capitec
{
    public interface ICapitecApplicationDataHelper: IDataHelper
    {
        IEnumerable<ReservedApplication> GetReservedApplications();
        void UpdateReservedApplicationNumber(int applicationKey, bool isUsed);
    }
}
