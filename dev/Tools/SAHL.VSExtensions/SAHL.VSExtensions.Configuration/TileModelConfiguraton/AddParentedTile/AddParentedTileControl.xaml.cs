using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddParentedTile
{
    /// <summary>
    /// Interaction logic for AddParentTileControl.xaml
    /// </summary>
    public partial class AddParentedTileControl : UserControl, ISAHLControlForConfiguration<AddParentedTileConfiguration>
    {
        private InterfaceConvention ITileModel = new InterfaceConvention("ITileModel");
        private TypeConvention MajorTileConfiguration = new TypeConvention("MajorTileConfiguration<>");

        public AddParentedTileControl()
        {
            InitializeComponent();
            cbxTileModel.DataContext = ITileModel.Results;
            cbxParentTileModel.DataContext = MajorTileConfiguration.Results;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            IEnumerable<IScannableAssembly> assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI") || x.StartsWith("SAHL.Config"));
            typeScanner.Scan(assemblies, new IScanConvention[] { ITileModel, MajorTileConfiguration });
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Configuration Name has not been set", () => this.txtConfigurationName.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Tile Model Name has not been set", () => this.cbxTileModel.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Configuration Type has not been set", () => this.cbxConfigurationType.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Parent Tile Model has not been set", () => this.cbxParentTileModel.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Sequence has not been set", () => this.txtSequence.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.ConfigurationName = txtConfigurationName.Text;
            model.TileModel = ITileModel.Results.Single(x => x.Name == cbxTileModel.Text).FullName;
            model.ConfigurationType = ((ComboBoxItem)cbxConfigurationType.SelectedItem).Tag;
            model.ParentTileModel = MajorTileConfiguration.Results.Single(x => x.Name == cbxParentTileModel.Text).FullName;
            model.FeatureAccess = txtFeatureAccess.Text;
            model.Sequence = txtSequence.Text;
        }
    }
}