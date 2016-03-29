using NUnit.Framework;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_LoanStatement : Correspondence_LoanStatementControls
    {
        public void Populate(DateTime fromdate, DateTime toDate, string transactiontype, string format, string language)
        {
            base.FromDate.Value = fromdate.ToString(Common.Constants.Formats.DateFormat);
            base.ToDate.Value = toDate.ToString(Common.Constants.Formats.DateFormat);
            base.TransactionTypes.Option(transactiontype).Select();
            base.Language.Option(language).Select();
        }

        public void AssertControlsValid()
        {
            Assert.True(base.FromDate.Exists, "FromDate does not exist.");
            Assert.True(base.ToDate.Exists, "ToDate does not exist.");
            Assert.True(base.TransactionTypes.Exists, "TransactionTypes does not exist.");
            Assert.True(base.Language.Exists, "Language does not exist.");
        }

        public void AssertValidationMessagesExist()
        {
            NUnit.Framework.Assert.True(base.divValidationSummaryBody.Text.Contains("FromDate is Required") &&
                 base.divValidationSummaryBody.Text.Contains("ToDate is Required"), "FromDate or ToDate validation message did not display");
        }
    }
}