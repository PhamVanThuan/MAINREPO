using Automation.DataModels;
using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Presenters.PersonalLoans;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanMaintainExternalLifePolicy : PersonalLoanMaintainExternalLifePolicyControls
    {
        private readonly IWatiNService watinService;

        public PersonalLoanMaintainExternalLifePolicy()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void AddPolicy(ExternalLifePolicy externalLife)
        {
            base.Insurer.SelectByValue(externalLife.InsurerKey.ToString());
            base.PolicyNumber.TypeText(externalLife.PolicyNumber.ToString());
            base.CommencementDate.Value = externalLife.CommencementDate.ToString(Common.Constants.Formats.DateFormat);
            base.Status.SelectByValue(externalLife.LifePolicyStatusKey.ToString());
            base.SumInsured.TypeText(externalLife.SumInsured.ToString());
            base.PolicyCeded.Checked = externalLife.PolicyCeded;
            base.Save.Click();
        }

        public void AssertFieldsExist()
        {
            var fields = new List<Element>() {
                base.Insurer,
                base.PolicyNumber,
                base.CommencementDate,
                base.CommencementDatePicker,
                base.Status,
                base.CloseDate,
                base.CloseDatePicker,
                base.SumInsured,
                base.PolicyCeded,
                base.Save,
                base.Cancel
            };
            WatiNAssertions.AssertFieldsExist(fields);
            WatiNAssertions.AssertFieldsAreEnabled(fields);
        }
    }
}