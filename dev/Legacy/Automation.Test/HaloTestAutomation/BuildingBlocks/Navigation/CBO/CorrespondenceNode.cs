using Common.Enums;
using ObjectMaps.FloboControls;

namespace BuildingBlocks.Navigation
{
    public class CorrespondenceNode : CorrespondenceNodeControls
    {
        public void Correspondence()
        {
            base.Correspondence.Click();
        }

        public void CorrespondenceSummary(ReportTypeEnum r)
        {
            base.CorrespondenceSummary.Click();
            switch (r)
            {
                case ReportTypeEnum.ITCDisputeIndicatedEng:
                    base.ITCDisputeIndicatedEng.Click();
                    break;

                case ReportTypeEnum.ITCDisputeIndicatedAfr:
                    base.ITCDisputeIndicatedAfr.Click();
                    break;
            }
        }

        public void SendEmail()
        {
            base.SendEmail.Click();
        }

        public void SendSMS()
        {
            base.SendSMS.Click();
        }
    }
}