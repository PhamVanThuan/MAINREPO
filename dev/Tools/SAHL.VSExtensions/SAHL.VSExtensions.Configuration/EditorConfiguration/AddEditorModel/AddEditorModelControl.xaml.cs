using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorModel
{
    /// <summary>
    /// Interaction logic for AddEditorModelControl.xaml
    /// </summary>
    public partial class AddEditorModelControl : UserControl, ISAHLControlForConfiguration<AddEditorModelConfiguration>
    {
        public AddEditorModelControl()
        {
            InitializeComponent();
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Model Name has not been set", () => this.txtEditorModel.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.Name = this.txtEditorModel.Text;
            model.UseSQL = this.chkUseSQL.IsChecked;
        }
    }
}