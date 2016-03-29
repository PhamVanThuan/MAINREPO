using System.Threading;
using WatiN.Core;
using ObjectMaps;
using NUnit.Framework;
using WatiN.Core.Logging;
using CommonData.Enums;

namespace BuildingBlocks
{
        public class AdminPaymentDistributionAgentView : AdminPaymentDistributionAgentViewControls
        {
            /// <summary>
            /// Select a tier from the 'Payment Distribution Agencies / Agents' tree structure
            /// </summary>
            /// <param name="b">Watin.Core.TestBrowser object</param>
            /// <param name="buttonLabel"></param>
            /// <param name="tierDisplayNames">list of LegalEntity names in order of precedence in the organisational structure</param>
            public void SelectTier(Btn buttonLabel, params string[] tierDisplayNames)
            {               
                //Identify the root node of the 'Payment Distribution Agencies / Agents' tree structure
                TableCell tierCell = base.DescriptionCell("Payment Distribution Agencies");

                for (int i = 0; i < tierDisplayNames.Length; i++)
                {
                    //Expand the selected tier if it is not already expanded
                    if (!base.ColapseButton(tierCell).Exists)
                    {
                        base.ExpandButton(tierCell).Click();
                        Thread.Sleep(2000);
                    }
                    //Identify and select the next tier
                    tierCell = base.DisplayNameCell(tierDisplayNames[i]);
                    base.TierRow(tierCell).Click();
                    Thread.Sleep(2000);
                }
                Thread.Sleep(1000);
                ClickButton(buttonLabel);
            }

            public void SelectTier(params string[] tierDisplayNames)
            {
                SelectTier(Btn.None, tierDisplayNames);
            }

            public void ClickButton(Btn buttonLabel)
            {
                switch (buttonLabel)
                {
                    case Btn.AddToMenu:
                        base.btnAddToMenu.Click();
                        break;
                    case Btn.Add:
                        base.btnAdd.Click();
                        break;
                    case Btn.Remove:
                        base.btnRemove.Click();
                        break;
                    case Btn.Update:
                        base.btnUpdate.Click();
                        break;
                    case Btn.View:
                        base.btnView.Click();
                        break;
                    case Btn.Select:
                        base.btnSelect.Click();
                        break;
                    default:
                        break;
                }
            }

          
            public void ExpandAll()
            {
                throw new System.NotImplementedException();
            }
        }
}