using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_ClaimAffidavit : Correspondence_ClaimAffidavitControls
    {
        public void Populate(string legalentities, string idnumber, string supervisor, string sequestration, DateTime sequestrationDate)
        {
            base.LegalEntities.Value = legalentities;
            base.LegalEntityIDs.Value = idnumber;
            base.Supervisor.Value = supervisor;
            base.SequestrationType.Option(sequestration).Select();
            base.SequestrationDate.Value = sequestrationDate.ToString(Formats.DateFormat);
        }

        public void AssertControlsValid()
        {
            var message = "";
            if (!base.LegalEntities.Exists)
                message += "Could not locate LegalEntities control, ";
            if (!base.LegalEntityIDs.Exists)
                message += "Could not locate LegalEntityIDs control, ";
            if (!base.Supervisor.Exists)
                message += "Could not locate Supervisor control, ";
            if (!base.SequestrationType.Exists)
                message += "Could not locate SequestrationType control, ";
            if (!base.SequestrationDate.Exists)
                message += "Could not locate SequestrationDate control, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }

        public void AssertValidationMessagesExist()
        {
            var message = "";
            if (!base.divValidationSummaryBody.Text.Contains("Must select at least one Correspondence Option"))
                message += "Must select at least one Correspondence Option, ";
            if (!base.divValidationSummaryBody.Text.Contains("Legal Entities is Required"))
                message += "Legal Entities is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("ID Number / Reg. Number is Required"))
                message += "ID Number / Reg. Number is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("Supervisor Name is Required"))
                message += "Supervisor Name is Required, ";
            if (!base.divValidationSummaryBody.Text.Contains("Sequestration Date is Required"))
                message = "Sequestration Date is Required, ";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }
    }
}