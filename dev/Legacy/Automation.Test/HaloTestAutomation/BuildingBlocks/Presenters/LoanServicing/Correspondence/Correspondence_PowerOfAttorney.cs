using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_PowerOfAttorney : Correspondence_PowerOfAttorneyControls
    {
        public void Populate(string litigationSupervisor, string attorney, string legalentities)
        {
            base.Supervisor.Value = litigationSupervisor;
            base.Attorney.Value = attorney;
            base.LegalEntities.Value = legalentities;
        }

        public void AssertAllValidationMessagesExist()
        {
            var message = "";
            if (!base.divValidationSummaryBody.Text.Contains("Must select at least one Correspondence Option"))
                message += "Must select at least one Correspondence Option, ";
            if (!base.divValidationSummaryBody.Text.Contains("Litigation Supervisor is Required"))
                message += "Litigation Supervisor is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("Legal Entities is Required"))
                message += "Legal Entities is Required, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }

        public void AssertControlsValid()
        {
            var message = "";
            if (!base.Attorney.Exists)
                message += "Could not locate Attorney field, ";
            if (!base.Supervisor.Exists)
                message += "Could not locate Supervisor field, ";
            if (!base.LegalEntities.Exists)
                message += "Could not locate LegalEntities field, ";
            if (!base.LegalEntities.Exists)
                message += "Could not locate Gender field, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }
    }
}