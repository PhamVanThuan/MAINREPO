using System;
using SAHL.Core.Data;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements;
using SAHL.DomainProcessManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Managers.Application
{
    public class ApplicationDataManager : IApplicationDataManager
    {
        private IDbFactory dbFactory;

        public ApplicationDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DoesOpenApplicationExistForComcorpProperty(string clientIdNumber, ComcorpApplicationPropertyDetailsModel comcorpApplicationPropertyDetail)
        {
            var query = new DoesOpenAppExistForComcorpPropertyAndClientStatement(clientIdNumber, comcorpApplicationPropertyDetail);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                int doesExist = db.SelectOne<int>(query);
                return doesExist > 0;
            }
        }

  
        public bool DoesSuppliedVendorExist(string vendorCode)
        {
            var query = new DoesSuppliedVendorExistStatement(vendorCode);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                int doesExist = db.SelectOne<int>(query);
                return doesExist > 0;
            }
        }

        public void RollbackCriticalPathApplicationData(int applicationNumber, IEnumerable<int> employmentKeys)
        {
            // Current hack is to execute stored proc to do db clean up --- no compensation events are published
            string commaSeperatedEmploymentKeys = string.Empty;
            if (employmentKeys.Count() > 0)
            {
                commaSeperatedEmploymentKeys = string.Join(",", employmentKeys);
            }

            var query = new RollbackApplicationStatement(applicationNumber, commaSeperatedEmploymentKeys);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.ExecuteNonQuery(query);
                db.Complete();
            }
        }
    }
}