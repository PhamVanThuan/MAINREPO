using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModel
{
    /// <summary>
    /// Interaction logic for AddTileModelControl.xaml
    /// </summary>
    public partial class AddTileModelControl : UserControl, ISAHLControlForConfiguration<AddTileModelConfiguration>
    {
        public AddTileModelControl()
        {
            InitializeComponent();
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Tile Model Name has not been set", () => txtTileModelName.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.TileType = cbxTileType.Text;
            model.ModelName = string.Format("{0}{1}TileModel", txtTileModelName.Text, cbxTileType.Text);
            model.DataProvider = txtTileDataProviderName.Text;
            model.DataProviderIsSQL = chkDataProviderIsSQL.IsChecked;
            model.ContentProvider = txtTileContentProviderName.Text;
            model.ContentProviderIsSQL = chkContentProviderIsSQL.IsChecked;
        }

        #region internal events

        private void txtTileModelName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetItemNames(cbxTileType.Text);
        }

        private void cbxTileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetItemNames(((ComboBoxItem)cbxTileType.SelectedValue).Content.ToString());
        }

        private void SetItemNames(string suffix)
        {
            if (!this.IsInitialized) return;

            txtTileDataProviderName.Text = string.Format("{0}{1}TileDataProvider", txtTileModelName.Text, suffix);
            txtTileContentProviderName.Text = string.Format("{0}{1}TileContentProvider", txtTileModelName.Text, suffix);
        }

        #endregion internal events
    }
}