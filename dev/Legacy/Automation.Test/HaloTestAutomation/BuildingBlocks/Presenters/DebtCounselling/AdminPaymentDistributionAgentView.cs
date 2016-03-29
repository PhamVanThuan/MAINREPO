using System.Threading;
using WatiN.Core;
using ObjectMaps;
using NUnit.Framework;
using WatiN.Core.Logging;
using CommonData.Enums;
using ObjectMaps.Pages;

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
            public void SelectTier(ButtonType buttonLabel, params string[] tierDisplayNames)
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
                SelectTier(ButtonType.None, tierDisplayNames);
            }

            public void ClickButton(ButtonType buttonLabel)
            {
                switch (buttonLabel)
                {
                    case ButtonType.AddToMenu:
                        base.btnAddToMenu.Click();
                        break;
                    case ButtonType.Add:
                        base.btnAdd.Click();
                        break;
                    case ButtonType.Remove:
                        base.btnRemove.Click();
                        break;
                    case ButtonType.Update:
                        base.btnUpdate.Click();
                        break;
                    case ButtonType.View:
                        base.btnView.Click();
                        break;
                    case ButtonType.Select:
                        base.btnSelect.Click();
                        break;
                    default:
                        break;
                }
            }

            public void AssertPDADetailsAdded(params string[] expectedTierDisplayNames)
            {
                Logger.LogAction(@"Asserting that the PDA Details have been added");
                //Identify the root node of the 'Payment Distribution Agencies / Agents' tree structure
                TableCell tierCell = base.DescriptionCell("Payment Distribution Agencies");

                for (int i = 0; i < expectedTierDisplayNames.Length - 1; i++)
                {
                    //Expand the selected tier if it is not already expanded
                    if (!base.ColapseButton(tierCell).Exists)
                    {
                        base.ExpandButton(tierCell).MouseDown();
                        base.ExpandButton(tierCell).MouseUp();
                    }

                    //Identify and select the next tier
                    tierCell = base.DisplayNameCell(expectedTierDisplayNames[i]);
                    Assert.IsTrue(base.TierRow(tierCell).Exists, "The Legal Entity: {0} was not added to the PDA Organisation Structure");
                }
            }

            public void AssertPDADetailsRemoved(params string[] expectedTierDisplayNames)
            {
                Logger.LogAction(@"Asserting that the PDA Details have been removed");

                //Identify the root node of the 'Payment Distribution Agencies / Agents' tree structure
                TableCell tierCell = base.DescriptionCell("Payment Distribution Agencies");

                for (int i = 0; i < expectedTierDisplayNames.Length - 1; i++)
                {
                    Assert.IsTrue(base.TierRow(tierCell).Exists, "The Legal Entity: {0} could not be found in the PDA Organisation Structure");
                    //Expand the selected tier if it is not already expanded
                    if (!base.ColapseButton(tierCell).Exists)
                    {
                        base.ExpandButton(tierCell).MouseDown();
                        base.ExpandButton(tierCell).MouseUp();
                    }

                    //Identify and select the next tier
                    tierCell = base.DisplayNameCell(expectedTierDisplayNames[i]);
                }
                //Assert the last DisaplayName stored in the expectedTierDisplayNames array does not exist in the PDA Organisation Structure
                tierCell = base.DisplayNameCell(expectedTierDisplayNames[expectedTierDisplayNames.Length - 1]);
                Assert.IsFalse(tierCell.Exists, "The Legal Entity: {0} was not removed from the PDA Organisation Structure");
            }
        }
}