using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditor
{
    /// <summary>
    /// Interaction logic for AddEditorControl.xaml
    /// </summary>
    public partial class AddEditorControl : UserControl, ISAHLControlForConfiguration<AddEditorConfiguration>
    {
        private ObservableCollection<EditorModelInfo> collection = new ObservableCollection<EditorModelInfo>();

        public AddEditorControl()
        {
            InitializeComponent();
            this.dgPageModels.DataContext = collection;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Editor Name has not been set", () => txtEditorName.Text.Trim().Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.EditorName = string.Format("{0}Editor", txtEditorName.Text);
            model.UseSqlStatement = this.chkEditorProviderIsSQL.IsChecked;
            model.UseUnorderedSelector = this.chkUseUnorderedSelector.IsChecked;
            model.CreateFolder = chkCreateFolder.IsChecked;
            model.Models = collection.AsEnumerable();
        }

        private void dgPageModels_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (((EditorModelInfo)e.Row.Item).Name == null)
            {
                e.Cancel = true;
            }
        }
    }
}