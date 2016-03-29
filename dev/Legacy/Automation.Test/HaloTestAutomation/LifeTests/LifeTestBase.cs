using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using System.Linq;
using System.Collections.Generic;
using System;
using BuildingBlocks.Timers;
using NUnit.Framework;
using BuildingBlocks.Assertions;
using WatiN.Core.Logging;
using System.Threading.Tasks;
using System.Threading;
using Automation.Framework;
namespace SAHL.LifeTests
{
    public abstract class LifeTestBase : TestBase<BasePage>
    {
        public void ProcessLifeLeads(int noLeads)
        {
            Logger.LogAction("Getting {0} accounts that qualify for life.", noLeads);
            var accounts = Service<ILifeService>().GetQualifyingMortgageAccountsForLife(noLeads);
            foreach (var mortgageLoanAccount in accounts)
            {
                var inputs = new Dictionary<string, string>();
                inputs.Add("LoanNumber", mortgageLoanAccount.MLAccountKey.ToString());
                inputs.Add("AssignTo", TestUsers.LifeConsultant);
                var helper = new X2Helper();

                var results = helper.CreateCase(TestUsers.HaloUser, Processes.Life, (-1).ToString(),
                                    Workflows.LifeOrigination,
                                    WorkflowActivities.LifeOrigination.CreateInstance, inputs, false);
                if (!String.IsNullOrEmpty(results.Error))
                {
                    Assert.Fail("Failed to create life lead error: {0}", results.X2Messages.Select(x=>x));
                }
            }
        }

        public LifeLead GetUnusedLifeLeadAtState(string state)
        {
            var lead = new LifeLead();
            BuildingBlocks.Timers.GeneralTimer.BlockWaitFor(1000, 20, () =>
            {
                lead = base.Service<ILifeService>().GetCreatedLifeLeads()
                           .Where(x => x.StateName == state && x.AssignedConsultant != null && x.AssignedConsultant.Contains(TestUsers.LifeConsultant))
                           .FirstOrDefault();
                if (lead == null)
                    return false;

                return true;
            }, () =>
            {
                Assert.Fail("Timeout retrieving life lead.");
            });
            return lead;
        }
    }
}
