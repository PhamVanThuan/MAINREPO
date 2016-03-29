using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public enum EditMode { Add, Edit } 

    public partial class frmNuGetPackages : Form
    {
        public frmNuGetPackages()
        {
            InitializeComponent();
        }

        private void frmManageNuGetPackages_Load(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            listViewNuGetPackages.Items.Clear();
            int nugetPackageCount = MainForm.App.GetCurrentView().Document.NuGetPackages.Count;
            for (int x = 0; x < nugetPackageCount; x++)
            {
                listViewNuGetPackages.Items.Add(new ListViewItem(new string[] { MainForm.App.GetCurrentView().Document.NuGetPackages[x].PackageID, MainForm.App.GetCurrentView().Document.NuGetPackages[x].PackageVersion.ToString() }));
            }
        }
    }
}