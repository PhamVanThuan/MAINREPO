using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Presenters.PersonalLoans;
using System;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanExternalLifePolicySummary : PersonalLoanExternalLifePolicySummaryControls
    {
        private readonly IWatiNService watinService;

        public PersonalLoanExternalLifePolicySummary()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void AssertFieldsExist()
        {
            var fields = new List<Element>() {
                base.Insurer,
                base.PolicyNumber,
                base.CommencementDate,
                base.Status,
                base.CloseDate,
                base.SumInsured,
                base.PolicyCeded,
            };
            WatiNAssertions.AssertFieldsExist(fields);
            WatiNAssertions.AssertFieldsAreEnabled(fields);
        }

        public void AssertFieldValues(Automation.DataModels.ExternalLifePolicy externalLife)
        {
            WatiNAssertions.AssertFieldText(externalLife.Insurer.Descripton, base.Insurer);
            WatiNAssertions.AssertFieldText(externalLife.PolicyNumber, base.PolicyNumber);
            WatiNAssertions.AssertFieldText(externalLife.CommencementDate.ToString(Common.Constants.Formats.DateFormat), base.CommencementDate);
            WatiNAssertions.AssertFieldText(externalLife.LifePolicyStatus.Description, base.Status);
            string closeDate = externalLife.CloseDate == null ? null : Convert.ToDateTime(externalLife.CloseDate).ToString(Common.Constants.Formats.DateFormat);
            WatiNAssertions.AssertFieldText(closeDate, base.CloseDate);
            WatiNAssertions.AssertCurrencyLabel(base.SumInsured, (decimal)externalLife.SumInsured);
            WatiNAssertions.AssertCheckboxValue(externalLife.PolicyCeded, base.PolicyCeded);
        }

        public void AssertBlankForm()
        {
            WatiNAssertions.AssertFieldText("-", base.Insurer);
            WatiNAssertions.AssertFieldText("-", base.PolicyNumber);
            WatiNAssertions.AssertFieldText("-", base.CommencementDate);
            WatiNAssertions.AssertFieldText("-", base.Status);
            WatiNAssertions.AssertFieldText("-", base.CloseDate);
            WatiNAssertions.AssertFieldText("-", base.SumInsured);
            WatiNAssertions.AssertFieldsDoNotExist(new List<Element>() { base.PolicyCeded });
        }

        public void AssertSAHLLifeForm()
        {
            WatiNAssertions.AssertFieldText("SAHL Life", base.Insurer);
            WatiNAssertions.AssertFieldText("-", base.PolicyNumber);
            WatiNAssertions.AssertFieldText("-", base.CommencementDate);
            WatiNAssertions.AssertFieldText("-", base.Status);
            WatiNAssertions.AssertFieldText("-", base.CloseDate);
            WatiNAssertions.AssertFieldText("-", base.SumInsured);
            WatiNAssertions.AssertFieldsDoNotExist(new List<Element>() { base.PolicyCeded });
        }
    }
}