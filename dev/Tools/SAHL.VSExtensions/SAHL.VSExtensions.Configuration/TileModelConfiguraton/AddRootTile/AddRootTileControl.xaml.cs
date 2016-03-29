using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddRootTile
{
    /// <summary>
    /// Interaction logic for AddRootTileControl.xaml
    /// </summary>
    public partial class AddRootTileControl : UserControl, ISAHLControlForConfiguration<AddRootTileConfiguration>
    {
        private InterfaceConvention ITileModel = new InterfaceConvention("ITileModel");
        private TypeConvention ApplicationModule = new TypeConvention("ApplicationModule");

        public AddRootTileControl()
        {
            InitializeComponent();
            this.cbxTileModel.DataContext = ITileModel.Results;
            this.cbxModule.DataContext = ApplicationModule.Results;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            IEnumerable<IScannableAssembly> assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI") || x.StartsWith("SAHL.Config"));
            typeScanner.Scan(assemblies, new IScanConvention[] { ITileModel, ApplicationModule });
            var objects = ITileModel.Results.Where(x => !x.Name.Contains("Major")).ToArray();
            foreach (var type in objects)
            {
                ITileModel.Results.Remove(type);
            }
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Configuration Name has not been set", () => txtTileType.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Tile Model has not been set", () => cbxTileModel.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Module has not been set", () => cbxModule.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.ConfigurationName = txtTileType.Text;
            model.TileModel = ITileModel.Results.Single(x => x.Name == cbxTileModel.Text).FullName;
            model.Module = ApplicationModule.Results.Single(x => x.Name == cbxModule.Text).FullName;
            model.FeatureAccess = txtFeatureAccess.Text;
            model.Description = txtDescription.Text;
        }
    }
}