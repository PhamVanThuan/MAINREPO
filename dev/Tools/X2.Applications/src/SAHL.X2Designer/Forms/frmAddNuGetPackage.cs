using NuGet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace SAHL.X2Designer.Forms
{
    public partial class frmAddNuGetPackage : Form
    {
        EditMode editMode;
        public string nugetPackageID { get; set; }
        public string nugetPackageVersion { get; set; }

        public frmAddNuGetPackage(EditMode editMode)
        {
            InitializeComponent();

            this.editMode = editMode;

        }

        private void frmAddNuGetPackage_Load(object sender, EventArgs e)
        {
            this.Text = editMode.ToString() + " NuGet Package";

            switch (editMode)
            {
                case EditMode.Add:
                    txtNuGetPackageID.Enabled = true;
                    break;
                case EditMode.Edit:
                    txtNuGetPackageID.Enabled = false;
                    txtNuGetPackageID.Text = this.nugetPackageID;
                    txtNuGetPackageVersion.Text = this.nugetPackageVersion;
                    break;
                default:
                    break;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput() == true)
            {
                this.nugetPackageID = txtNuGetPackageID.Text.Trim();
                this.nugetPackageVersion = txtNuGetPackageVersion.Text.Trim();

                this.DialogResult = DialogResult.OK;
            }
        }

        private bool ValidateInput()
        {
            string errorMessage = String.Empty;

            string[] nugetRepositories = new string[] { Properties.Settings.Default.SAHLDevNuGetInstall, Properties.Settings.Default.OfficialNuGetApi };
            var aggregateRepository = new AggregateRepository(PackageRepositoryFactory.Default, nugetRepositories, false);
            PackageManager manager = new PackageManager(aggregateRepository, @"C:\Temp");
            IPackage package = null;

            // validate package ID is entered
            if (String.IsNullOrEmpty(txtNuGetPackageID.Text))
            {
                MessageBox.Show("PackageID must be entered.");
                return false;
            }

            package = manager.SourceRepository.FindPackage(txtNuGetPackageID.Text);

            if (editMode == EditMode.Add)
            {
                // validate package exists                
                if (package == null)
                {
                    MessageBox.Show("Package with this PackageID does not exist on SAHL NuGet server.");
                    return false;
                }

                // validate that we dont already have this package
                foreach (var pkg in MainForm.App.GetCurrentView().Document.NuGetPackages)
                {
                    if (package.Id == pkg.PackageID)
                    {
                        MessageBox.Show("Package already exists for this map - use the 'Edit' option to amend the version if required.");
                        return false;
                    }
                }
            }

            // validate package version format
            errorMessage = ParseVersion(txtNuGetPackageVersion.Text);
            if (!String.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage);
                return false;
            }

            // validate package and version exist
            IPackage packageWithVersion = manager.SourceRepository.FindPackage(txtNuGetPackageID.Text, new SemanticVersion(txtNuGetPackageVersion.Text));
            if (packageWithVersion == null)
            {
                MessageBox.Show("Package exists but not with this version number. Latest version is (" + package.Version.ToString() + ")");
                return false;
            }

            return true;
        }
        private static string ParseVersion(string input)
        {
            try
            {
                Version ver = Version.Parse(input);
            }
            catch (ArgumentNullException)
            {
                return "Version must be entered.";
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Version cannot contain negative values.";
            }
            catch (ArgumentException)
            {
                return "Version has a bad number of components - must be in format (9.9.9.9).";
            }
            catch (FormatException)
            {
                return "Version contains non-integer values.";
            }
            catch (OverflowException)
            {
                return "Version is out of range.";
            }

            return String.Empty;
        }

        private void btnGetLatestVersion_Click(object sender, EventArgs e)
        {
            string[] nugetRepositories = new string[] { Properties.Settings.Default.SAHLDevNuGetInstall, Properties.Settings.Default.OfficialNuGetApi };
            var aggregateRepository = new AggregateRepository(PackageRepositoryFactory.Default, nugetRepositories, false);
            PackageManager manager = new PackageManager(aggregateRepository, @"C:\Temp");

            IPackage package = manager.SourceRepository.FindPackage(txtNuGetPackageID.Text);
            if (package != null)
            {
                txtNuGetPackageVersion.Text = package.Version.ToString();
            }
        }
    }
}