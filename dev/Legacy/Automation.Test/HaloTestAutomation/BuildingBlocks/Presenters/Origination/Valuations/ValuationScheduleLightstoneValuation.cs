using Common.Constants;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.Origination.Valuations
{
    public class ValuationScheduleLightstoneValuation : ValuationScheduleLightstoneValuationControls
    {
        public void Populate(DateTime assessmentDate, string contact01, string workPhone, string phone)
        {
            base.AssessmentDate.Value = assessmentDate.Date.ToString(Formats.DateFormat);
            base.Contact1.TypeText(contact01);
            base.WorkPhone1.TypeText(workPhone);
            base.Phone01.TypeText(phone);
        }

        public void Instruct()
        {
            base.InstructValuer.Click();
            try
            {
                if (base.WarningSubmit.Exists)
                    base.WarningSubmit.Click();
            }
            catch { }
        }
    }
}