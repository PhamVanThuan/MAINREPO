using Automation.DataAccess;
using BuildingBlocks.Services;
using ObjectMaps.Presenters.Origination;
using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Origination
{
    public class ConfirmApplicationEmployment : ConfirmApplicationEmploymentControls
    {
        public void SelectEmploymentType(string employmentTypeDescription)
        {
            base.EmploymentType.Select(employmentTypeDescription);
            base.ConfirmButton.Click();
        }

        public void SelectEmploymentType(int employmentTypeKey)
        {
            base.EmploymentType.SelectByValue(employmentTypeKey.ToString());
            base.ConfirmButton.Click();
        }

        public void SelectEmploymentType(int offerKey, string workflow, string employmentType)
        {
            SelectEmploymentType(employmentType);
            AssertApplicationEmploymentActionCompleted(offerKey, workflow);
        }

        public void AssertApplicationEmploymentActionCompleted(int offerKey, string workflow)
        {
            var service = new X2WorkflowService();
            QueryResults instance = new QueryResults();
            switch (workflow)
            {
                case Common.Constants.Workflows.ApplicationManagement:

                    instance = service.GetAppManInstanceDetails(offerKey);
                    break;

                case Common.Constants.Workflows.Credit:
                    instance = service.GetCreditInstanceDetails(offerKey);
                    break;

                default:
                    break;
            }
            var instanceID = instance.Rows(0).Column("ID").GetValueAs<int>();
            instance = service.GetCreditInstanceDetails(offerKey);
            service.WaitForX2WorkflowHistoryActivity(Common.Constants.WorkflowActivities.ApplicationManagement.ConfirmApplicationEmployment, instanceID, 1);
        }

        public void AssertPageLoaded()
        {
            var controls = new List<Element> {
                base.EmploymentType,
                base.ConfirmButton,
                base.CancelButton
            };

            Assertions.WatiNAssertions.AssertFieldsExist(controls);
            Assertions.WatiNAssertions.AssertFieldsAreEnabled(controls);

            var employmentTypes = new List<string> {
                Common.Constants.EmploymentType.Salaried,
                Common.Constants.EmploymentType.SelfEmployed,
                Common.Constants.EmploymentType.SalariedWithDeductions,
                Common.Constants.EmploymentType.Unemployed
            };

            Assertions.WatiNAssertions.AssertSelectListContents(base.EmploymentType, employmentTypes);
            Assertions.WatiNAssertions.AssertSelectedOption("- Please select -", base.EmploymentType);
        }
    }
}