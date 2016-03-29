using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.ActionConfiguration.AddActionTileConfig
{
    /// <summary>
    /// Interaction logic for AddActionTileControl.xaml
    /// </summary>
    public partial class AddActionTileControl : UserControl, ISAHLControlForConfiguration<AddActionTileConfiguration>
    {
        private InterfaceConvention IActionTileModel = new InterfaceConvention("IActionTileModel");
        private InterfaceConvention IMajorTileConfiguration = new InterfaceConvention("IMajorTileConfiguration");
        private TypeConstantsConvention UrlNames = new TypeConstantsConvention("UrlNames");
        private EnumElementConvention UrlAction = new EnumElementConvention("UrlAction");

        public AddActionTileControl()
        {
            InitializeComponent();
            this.cbxActionTileModel.DataContext = IActionTileModel.Results;
            this.cbxTileConfiguration.DataContext = IMajorTileConfiguration.Results;
            this.cbxController.DataContext = UrlNames.Results;
            this.cbxAction.DataContext = UrlNames.Results;
            this.cbxUrlAction.DataContext = UrlAction.Results;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            IEnumerable<IScannableAssembly> assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI") || x.StartsWith("SAHL.Config"));
            typeScanner.Scan(assemblies, new IScanConvention[] { IActionTileModel, IMajorTileConfiguration, UrlNames, UrlAction });
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Configuration Name has not been set", () => this.txtActionConfigurationName.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Action Tile Model has not been set", () => this.cbxActionTileModel.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Tile Configuration has not been set", () => this.cbxTileConfiguration.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Feature Access has not been set", () => this.txtFeatureAccess.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Sequence has not been set", () => this.txtSequence.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Controller Name has not been set", () => this.cbxController.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Action Name has not been set", () => this.cbxAction.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Url Action has not been set", () => this.cbxUrlAction.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.ActionConfigurationName = string.Format("{0}ActionConfiguration", this.txtActionConfigurationName.Text);
            model.ActionTileModel = IActionTileModel.Results[this.cbxActionTileModel.SelectedIndex].FullName;
            model.TileConfiguration = IMajorTileConfiguration.Results[this.cbxTileConfiguration.SelectedIndex].FullName;
            model.FeatureAccess = this.txtFeatureAccess.Text;
            model.Sequence = this.txtSequence.Text;
            model.Controller = UrlNames.Results[this.cbxController.SelectedIndex].FullName;
            model.Action = UrlNames.Results[this.cbxAction.SelectedIndex].FullName;
            model.UrlAction = UrlAction.Results[this.cbxUrlAction.SelectedIndex].FullName;
        }
    }
}