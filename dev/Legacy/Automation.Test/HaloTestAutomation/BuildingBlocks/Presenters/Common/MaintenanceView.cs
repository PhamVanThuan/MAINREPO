using BuildingBlocks.Timers;
using Common.Constants;
using NUnit.Framework;
using System.Linq;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Presenters.CommonPresenters
{
    public sealed class LegalEntityOrganisationStructureMaintenanceView : ObjectMaps.LegalEntityOrganisationStructureMaintenanceViewControls
    {
        public void SelectTier(params string[] displaynames)
        {
            bool rowSelected = false;
            //Wait for 10 seconds in intervals of 1 sec
            foreach (string displayname in displaynames)
            {
                GeneralTimer.WaitFor<bool>(1000, 10, base.tblOrgStructure.Exists, (exist) =>
                {
                    if (exist)
                        return exist;
                    return !exist;
                });
                foreach (TableCell cell in base.tblOrgStructure.TableCells)
                {
                    if (cell.Text != null && cell.Text.Trim() == displayname)
                    {
                        cell.Focus();
                        cell.ContainingTableRow.Click();
                        foreach (Image img in cell.ContainingTableRow.Images)
                        {
                            // dxTreeList_ExpandedButton_SoftOrange dxtl__Collapse - when expanded ; dxTreeList_CollapsedButton_SoftOrange dxtl__Expand - when collapsed
                            if (!string.IsNullOrEmpty(img.ClassName) && img.ClassName.Contains("Collapsed"))
                            {
                                img.MouseDown();
                                img.MouseUp();
                                img.Click();
                                rowSelected = true;
                                break;
                            }
                        }
                        Thread.Sleep(2000);
                        if (rowSelected)
                            break;
                    }
                }
                if (rowSelected)
                    continue;
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
                        if (cell.ContainingTableRow.Images.Count > 0)
                        {
                            var imageCollection = from im in cell.ContainingTableRow.Images
                                                  where !string.IsNullOrEmpty(im.ClassName) && im.Enabled
                                                  select im;

                            foreach (Image img in imageCollection)
                            {
                                if (img.ClassName.Contains("Expand"))
                                {
                                    img.MouseDown();
                                    img.MouseUp();
                                    img.WaitForComplete();
                                    break;
                                }
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

        public void AssertDetailsAdded(string rootNode, params string[] displayName)
        {
            Logger.LogAction(string.Format(@"Asserting that the {0} has been added to the {1} structure", rootNode, displayName.ToArray()));
            //Identify the root node
            TableCell tierCell = base.DescriptionCell(rootNode);
            for (int i = 0; i < displayName.Length - 1; i++)
            {
                //Expand the selected tier if it is not already expanded
                if (!base.ColapseButton(tierCell).Exists)
                {
                    base.ExpandButton(tierCell).MouseDown();
                    base.ExpandButton(tierCell).MouseUp();
                }
                //Identify and select the next tier
                tierCell = base.DisplayNameCell(displayName[i]);
                Assert.IsTrue(base.TierRow(tierCell).Exists, string.Format(@"The Legal Entity: {0} was not added to the {1} structure", displayName, rootNode));
            }
        }

        public void AssertDetailsRemoved(string rootNode, params string[] expectedTierDisplayNames)
        {
            Logger.LogAction(string.Format(@"Asserting that the {0} node for {1} have been removed", rootNode, expectedTierDisplayNames.ToArray()));

            //Identify the root node
            TableCell tierCell = base.DescriptionCell(rootNode);

            for (int i = 0; i < expectedTierDisplayNames.Length - 1; i++)
            {
                Assert.IsTrue(base.TierRow(tierCell).Exists, string.Format(@"The Legal Entity: {0} could not be found in the {1} structure", expectedTierDisplayNames.ToArray(), rootNode));
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