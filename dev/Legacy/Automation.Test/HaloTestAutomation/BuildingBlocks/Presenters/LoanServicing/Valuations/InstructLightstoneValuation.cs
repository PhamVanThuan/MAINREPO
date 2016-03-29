using BuildingBlocks.Assertions;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Linq;

namespace BuildingBlocks.Presenters.LoanServicing.Valuations
{
    public class InstructLightstoneValuation : ValuationScheduleLightstoneValuationControls
    {
        public void PupulateValuationDetails
            (
                string reason,
                DateTime assessmentDate,
                Automation.DataModels.PropertyAccessDetail propAccessDetail
            )
        {
            base.Contact1.Value = propAccessDetail.Contact1;
            base.Phone01.Value = propAccessDetail.Contact1Phone;

            base.WorkPhone1.Value = propAccessDetail.Contact1WorkPhone;
            base.MobilePhone1.Value = propAccessDetail.Contact1MobilePhone;

            base.Contact2.Value = propAccessDetail.Contact2;
            base.Phone02.Value = propAccessDetail.Contact2Phone;

            base.AssessmentDate.Value = assessmentDate.ToString(Formats.DateFormat);
            base.AssessmentReasons.Option(reason).Select();
        }

        public void ClickInstruct()
        {
            base.SubmitButton.Click();
        }

        public void AssertPendingValuationInProgress()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Property has a pending valuation request.")
                              select msg).FirstOrDefault(), "Expected pending valuation message.");
        }

        public void AssertReasonsValid()
        {
            var reasons = new System.Collections.Generic.List<string>() { "HOC", "Loss Control", "Client Request", "Audit" };
            WatiNAssertions.AssertSelectListContents(base.AssessmentReasons, reasons);
        }

        public void AssertInspectionContactDetailsDisplayed(Automation.DataModels.PropertyAccessDetail propAccessDetail)
        {
            Assert.AreEqual(propAccessDetail.Contact1, base.Contact1.Text);
            Assert.AreEqual(propAccessDetail.Contact1Phone, base.Phone01.Text);
            Assert.AreEqual(propAccessDetail.Contact1WorkPhone, base.WorkPhone1.Text);
            Assert.AreEqual(propAccessDetail.Contact1MobilePhone, base.MobilePhone1.Text);
            Assert.AreEqual(propAccessDetail.Contact2, base.Contact2.Text);
            Assert.AreEqual(propAccessDetail.Contact2Phone, base.Phone02.Text);
        }

        public void AssertContact02PhoneNumberProvided()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Inspection Telephone 2 must be entered.")
                              select msg).FirstOrDefault(), "Expected Inspection Telephone 2 must be entered message.");
        }

        public void AssertContact01Required()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Contact1 is a mandatory field")
                              select msg).FirstOrDefault(), "Expected Contact1 is a mandatory message.");
        }

        public void AssertContact01PhoneRequired()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Contact1 Phone is a mandatory field")
                              select msg).FirstOrDefault(), "Expected Contact1 Phone is a mandatory message.");
        }

        public void AssertAssessmentDateRequired()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Please select an Assessment Date.")
                              select msg).FirstOrDefault(), "Expected Please select an Assessment Date message.");
        }

        public void AssertAssessmentReasonRequired()
        {
            Assert.IsNotNull((from msg in base.listErrorMessages.ToArray()
                              where msg.Text.Contains("Please select an Assessment Reason.")
                              select msg).FirstOrDefault(), "Expected Please select an Assessment Reason message.");
        }
    }
}