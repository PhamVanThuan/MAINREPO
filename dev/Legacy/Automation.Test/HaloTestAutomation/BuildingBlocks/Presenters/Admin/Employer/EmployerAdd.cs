using NUnit.Framework;

namespace BuildingBlocks.Presenters.Admin
{
    public sealed class EmployerAdd : EmployerBase
    {
        public override void Populate(Automation.DataModels.Employer employer)
        {
            base.EmployerName.TypeText(employer.Name);
            base.Populate(employer);
        }

        public void ClickAdd()
        {
            base.AddButton.Click();
        }

        public void AssertEmployerNameMandatoryMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Employer Name is a mandatory field"));
        }
    }
}