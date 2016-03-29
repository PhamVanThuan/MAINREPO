using System.Threading;
using WatiN.Core;
using WatiN.Core.Constraints;
using System.Text.RegularExpressions;
using ObjectMaps;
using CommonData.Constants;
using CommonData.Enums;
using ObjectMaps.Pages;
namespace BuildingBlocks
{
    public static partial class Views
    {
        public static class AdminDebtCounsellorView
        {
            public static void SelectTier(TestBrowser TestBrowser, string displayname)
            {
                AdminDebtCounsellorViewControls controls
                    = new AdminDebtCounsellorViewControls(TestBrowser);
                foreach (TableCell cell in controls.tblOrgStructure.TableCells)
                    if (cell.Text != null && cell.Text.Equals(displayname))
                    {
                        cell.ContainingTableRow.MouseDown();
                        cell.ContainingTableRow.MouseUp();
                    }
            }
            public static void ExpandAll(TestBrowser TestBrowser)
            {
                Thread.Sleep(2000);
                AdminDebtCounsellorViewControls controls
                    = new AdminDebtCounsellorViewControls(TestBrowser);
                Thread.Sleep(2000);
                bool expand = false;
                foreach (TableCell cell in controls.tblOrgStructure.TableCells)
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
            public static void ClickAdd(TestBrowser TestBrowser)
            {
                AdminDebtCounsellorViewControls controls
                 = new AdminDebtCounsellorViewControls(TestBrowser);
                controls.btnAdd.Click();
            }
            public static void ClickRemove(TestBrowser TestBrowser)
            {
                AdminDebtCounsellorViewControls controls
                = new AdminDebtCounsellorViewControls(TestBrowser);
                controls.btnRemove.Click();
            }
            public static void ClickUpdate(TestBrowser TestBrowser)
            {
                AdminDebtCounsellorViewControls controls
              = new AdminDebtCounsellorViewControls(TestBrowser);
                controls.btnUpdate.Click();
            }
        }
    }
}