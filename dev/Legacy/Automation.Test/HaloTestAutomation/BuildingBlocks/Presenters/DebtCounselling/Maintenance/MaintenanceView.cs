using System.Threading;
using WatiN.Core;
using WatiN.Core.Constraints;
using System.Text.RegularExpressions;
using ObjectMaps;
using CommonData.Constants;
using CommonData.Enums;
using NUnit.Framework;
using WatiN.Core.Logging;
using BuildingBlocks.Common;

namespace BuildingBlocks.Presenters.DebtCounselling
{
    public sealed class MaintenanceView: ObjectMaps.MaintenanceViewControls
    {
        public void SelectTier(params string[] displaynames)
        {
            //Wait for 10 seconds in intervals of 1 sec
            foreach (string displayname in displaynames)
            {
                GeneralTimer.WaitFor<bool>(1000, 10, base.tblOrgStructure.Exists, (exist) =>
                {
                    if (exist)
                        return true;
                    return false;
                });
                foreach (TableCell cell in base.tblOrgStructure.TableCells)
                    if (cell.Text != null && cell.Text.Trim() == displayname)
                    {
                        cell.Focus();
                        cell.ContainingTableRow.Click();
                        foreach (Image img in cell.ContainingTableRow.Images)
                        {
                            // dxTreeList_ExpandedButton_SoftOrange dxtl__Collapse - when expanded
                            // dxTreeList_CollapsedButton_SoftOrange dxtl__Expand - when collapsed
                            if (!string.IsNullOrEmpty(img.ClassName) && img.ClassName.Contains("Collapsed"))
                            {
                                img.MouseDown();
                                img.MouseUp();
                                img.Click();
                            }
                        }
                        Thread.Sleep(2000);
                    }
            }
        }
        public void ExpandAll()
        {
            bool expand = false;
            foreach (TableCell cell in base.tblOrgStructure.TableCells)
            {
                if (cell.Text != null && cell.Exists)
                {
                    if (cell.Text.Equals(OrganisationType.RegionOrChannel))
                        expand = true;
                    if (cell.Text.Equals(OrganisationType.Company))
                        expand = true;
                    if (cell.Text.Equals(OrganisationType.Designation))
                        expand = true;
                    if (cell.Text.Equals(OrganisationType.BranchOrOriginator))
                        expand = true;
                    if (cell.Text.Equals(OrganisationType.Department))
                        expand = true;
                    if (expand)
                    {
                        foreach (Image img in cell.ContainingTableRow.Images)
                        {
                            if (!string.IsNullOrEmpty(img.ClassName) && img.ClassName.Contains("Expand"))
                            {
                                img.MouseDown();
                                img.MouseUp();
                            }
                        }
                    }
                }
            }
        }
        public void AddToMenu()
        {
            base.btnAddToMenu.Click();
        }
        public void ClickAdd()
        {
            base.btnAdd.Click();
        }
        public void ClickRemove()
        {
            base.btnRemove.Click();
        }
        public void ClickView()
        {
            base.btnView.Click();
        }
        public void ClickUpdate()
        {
            base.btnUpdate.Click();
        }
        public void Select()
        {
            base.btnSelect.Click();
        }
        public void SearchByNCRNumber(string ncrNumber)
        {
            base.SearchCriteria.TypeText(ncrNumber);
            Element ele = base.Document.Div(Find.ByClass("SAHLAutoComplete_DefaultItem"));
            ele.WaitUntilExists();
            ele.MouseDown();
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