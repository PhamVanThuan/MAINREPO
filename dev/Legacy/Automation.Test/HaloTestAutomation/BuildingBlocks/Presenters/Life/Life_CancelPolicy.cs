using Common.Enums;
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_CancelPolicy : LifeCancelPolicyControls
    {
        public void Populate(string cancellationReason, LifePolicyStatusEnum lifepolicyStatus)
        {
            base.ctl00MainchkLetter.Checked = true;

            switch (lifepolicyStatus)
            {
                case LifePolicyStatusEnum.Accepted:
                    base.CancelFromInception.Click();
                    break;

                case LifePolicyStatusEnum.Inforce:
                    base.CancelWithProRata.Click();
                    break;
            }
            base.ctl00MainddlReason.Option(cancellationReason).Select();
        }

        public void Submit()
        {
            base.SubmitButton.Click();
        }

        public void AssertCancelledFromInceptionEnabled()
        {
            Assert.True(base.CancelFromInception.Enabled, "CancelledFromInception checkbox not enabled.");
        }

        public void AssertCancelledWithProRataEnabled()
        {
            Assert.True(base.CancelWithProRata.Enabled, "CancelledWithProRata checkbox not enabled.");
        }
    }
}