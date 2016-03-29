using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.ActionConfiguration.AddActionTileModel
{
    /// <summary>
    /// Interaction logic for AddActionTileModelControl.xaml
    /// </summary>
    public partial class AddActionTileModelControl : UserControl, ISAHLControlForConfiguration<AddActionTileModelConfiguration>
    {
        public AddActionTileModelControl()
        {
            InitializeComponent();
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Action Tile Model Name has not been set", () => this.txtActionTileModelName.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.ActionTileModelName = string.Format("{0}ActionTileModel", this.txtActionTileModelName.Text);
            model.Title = this.txtTitle.Text;
            model.Icon = this.txtIcon.Text;
            model.ExtendConstructor = this.chkExtendedAction.IsChecked;
            model.BottomLeftOverlay = this.txtBottomLeftOverlayIcon.Text;
            model.BottomRightOverlay = this.txtBottomRightOverlayIcon.Text;
        }
    }
}