﻿using WatiN.Core;
using ObjectMaps;

namespace BuildingBlocks
{
    public static partial class Views
    {
        public class AdminPaymentDistributionAgentLegalEntityRemove : MaintenanceLegalEntityRemoveControls
        {
            public enum btn
            {
                Remove = 1,
                Cancel = 2,
                None = 3
            }

            public void ClickButton(btn buttonLabel)
            {

                switch (buttonLabel)
                {
                    case btn.Remove:
                        base.ctl00MainbtnSubmitButton.Click();
                        break;
                    case btn.Cancel:
                        base.ctl00MainbtnCancelButton.Click();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}