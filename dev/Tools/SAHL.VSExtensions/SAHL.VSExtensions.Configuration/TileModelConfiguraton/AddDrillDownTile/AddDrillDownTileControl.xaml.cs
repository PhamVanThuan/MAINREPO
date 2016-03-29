using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddDrillDownTile
{
    /// <summary>
    /// Interaction logic for AddDrillDownTileControl.xaml
    /// </summary>
    public partial class AddDrillDownTileControl : UserControl, ISAHLControlForConfiguration<AddDrillDownTileConfiguration>
    {
        private InterfaceConvention ITileConfiguration = new InterfaceConvention("ITileConfiguration");
        private InterfaceConvention ITileModel = new InterfaceConvention("ITileModel");

        public AddDrillDownTileControl()
        {
            InitializeComponent();
            this.cbxTileModel.DataContext = ITileModel.Results;
            this.cbxDrillDownConfig.DataContext = ITileConfiguration.Results;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            IEnumerable<IScannableAssembly> assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI") || x.StartsWith("SAHL.Config"));
            typeScanner.Scan(assemblies, new IScanConvention[] { ITileModel, ITileConfiguration });
            var objects = ITileModel.Results.Where(x => !x.Name.Contains("Major")).ToArray();
            foreach (var type in objects)
            {
                ITileModel.Results.Remove(type);
            }
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Configuration Name has not been set", () => this.txtConfigurationName.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Tile Model has not been set", () => this.cbxTileModel.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Drill Down From has not been set", () => this.cbxDrillDownConfig.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Feature Access has not been set", () => this.txtFeatureAccess.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Description has not been set", () => this.txtDescription.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.ConfigurationName = txtConfigurationName.Text;
            model.TileModel = ITileModel.Results.Single(x => x.Name == cbxTileModel.Text).FullName;
            model.ParentTileConfiguration = ITileConfiguration.Results.Single(x => x.Name == cbxDrillDownConfig.Text).FullName;
            model.FeatureAccess = txtFeatureAccess.Text;
            model.Description = txtDescription.Text;
        }
    }
}