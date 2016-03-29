using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.TileModelConfiguraton.AddTileModelView
{
    /// <summary>
    /// Interaction logic for AddTileModelViewControl.xaml
    /// </summary>
    public partial class AddTileModelViewControl : UserControl, ISAHLControlForConfiguration<AddTileModelViewConfiguration>
    {
        private InterfaceConvention ITileModel = new InterfaceConvention("ITileModel");
        private Interfaces.Reflection.ITypeScanner typeScanner;
        private IEnumerable<IScannableAssembly> assemblies;

        public AddTileModelViewControl()
        {
            InitializeComponent();
            cbxTileModel.DataContext = this.ITileModel.Results;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            this.typeScanner = typeScanner;
            assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI"));
            typeScanner.Scan(assemblies, new IScanConvention[] { ITileModel });
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Tile Model has not been set", () => this.cbxTileModel.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.TileModelName = this.ITileModel.Results[cbxTileModel.SelectedIndex].Name;
            model.TileModelFullName = this.ITileModel.Results[cbxTileModel.SelectedIndex].FullName;
            IDictionary<string, Object> dictionary = model as IDictionary<string, Object>;
            TypePropertyConvention properties = new TypePropertyConvention(model.TileModelFullName);
            this.typeScanner.Scan(assemblies, new IScanConvention[] { properties });

            int i = 0;
            foreach (ITypeInfo info in properties.Results)
            {
                dictionary.Add(string.Format("Prop{0}", i), info.Name);
                i++;
            }
        }
    }
}