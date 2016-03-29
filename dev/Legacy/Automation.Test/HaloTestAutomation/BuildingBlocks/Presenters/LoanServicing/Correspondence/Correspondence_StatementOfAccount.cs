using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_StatementOfAccount : Correspondence_StatementOfAccountControls
    {
        public void Populate(string legalentities, DateTime sequestrationDate, string supervisor)
        {
            base.LegalEntities.Value = legalentities;
            base.SequestrationDate.Value = sequestrationDate.ToString(Formats.DateFormat);
            base.Supervisor.Value = supervisor;
        }

        public void AssertControlsValid()
        {
            var message = "";
            if (!base.LegalEntities.Exists)
                message += "Could not locate LegalEntities field, ";
            if (!base.SequestrationDate.Exists)
                message += "Could not locate SequestrationDate, ";
            if (!base.Supervisor.Exists)
                message += "Could not locate Supervisor field, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }

        public void AssertValidationMessagesExist()
        {
            var message = "";
            if (!base.divValidationSummaryBody.Text.Contains("Must select at least one Correspondence Option"))
                message += "Must select at least one Correspondence Option, ";
            if (!base.divValidationSummaryBody.Text.Contains("Supervisor is Required"))
                message += "Supervisor is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("Sequestration Date is Required"))
                message += "Sequestration Date is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("Legal Entities is Required"))
                message += "Legal Entities is Required, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }
    }
}