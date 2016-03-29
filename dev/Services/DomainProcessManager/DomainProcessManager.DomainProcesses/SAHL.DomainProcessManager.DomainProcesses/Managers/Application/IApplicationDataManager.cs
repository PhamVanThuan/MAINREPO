using System;
using SAHL.DomainProcessManager.Models;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Application
{
    public interface IApplicationDataManager
    {
        bool DoesOpenApplicationExistForComcorpProperty(string clientIdNumber, ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail);
        bool DoesSuppliedVendorExist(string vendorCode);  
        void RollbackCriticalPathApplicationData(int applicationNumber, IEnumerable<int> employmentKeys);
    }
}