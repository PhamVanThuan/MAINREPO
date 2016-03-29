using Common.Enums;
using ObjectMaps.Pages;
using WatiN.Core;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public class DebtCounsellorSelect : AdminPaymentDistributionAgentViewControls
    {
        /// <summary>
        /// Select a tier from the 'Payment Distribution Agencies / Agents' tree structure
        /// </summary>
        /// <param name="b">Watin.Core.TestBrowser object</param>
        /// <param name="buttonLabel"></param>
        /// <param name="tierDisplayNames">list of LegalEntity names in order of precedence in the organisational structure</param>
        public void SelectTier(ButtonTypeEnum buttonLabel, params string[] tierDisplayNames)
        {
            //Identify the root node of the 'Payment Distribution Agencies / Agents' tree structure
            TableCell tierCell = base.DescriptionCell("Payment Distribution Agencies");

            for (int i = 0; i < tierDisplayNames.Length; i++)
            {
                //Expand the selected tier if it is not already expanded
                if (!base.ColapseButton(tierCell).Exists)
                {
                    base.ExpandButton(tierCell).MouseDown();
                    base.ExpandButton(tierCell).MouseUp();
                }

                //Identify and select the next tier
                tierCell = base.DisplayNameCell(tierDisplayNames[i]);
                base.TierRow(tierCell).MouseDown();
                base.TierRow(tierCell).MouseUp();
            }
            System.Threading.Thread.Sleep(1000);
            ClickButton(buttonLabel);
        }

        public void SelectTier(TestBrowser b, params string[] tierDisplayNames)
        {
            SelectTier(ButtonTypeEnum.None, tierDisplayNames);
        }

        public void ClickButton(ButtonTypeEnum buttonLabel)
        {
            switch (buttonLabel)
            {
                case ButtonTypeEnum.AddToMenu:
                    base.btnAddToMenu.Click();
                    break;

                case ButtonTypeEnum.Add:
                    base.btnAdd.Click();
                    break;

                case ButtonTypeEnum.Remove:
                    base.btnRemove.Click();
                    break;

                case ButtonTypeEnum.Update:
                    base.btnUpdate.Click();
                    break;

                case ButtonTypeEnum.View:
                    base.btnView.Click();
                    break;

                case ButtonTypeEnum.Select:
                    base.btnSelect.Click();
                    break;

                default:
                    break;
            }
        }

        public void SearchByNCRNumber(string ncrNumber)
        {
            base.SearchCriteria.TypeText(ncrNumber);
            Element ele = base.Document.Div(Find.ByClass("SAHLAutoComplete_DefaultItem"));
            ele.WaitUntilExists();
            ele.MouseDown();
        }
    }
}