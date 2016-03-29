using SAHL.VSExtensions.Core.ScanConventions;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;

namespace SAHL.VSExtensions.Configuration.EditorConfiguration.AddEditorConfig
{
    /// <summary>
    /// Interaction logic for AddEditorControl.xaml
    /// </summary>
    public partial class AddEditorControl : UserControl, ISAHLControlForConfiguration<AddEditorConfiguration>
    {
        InterfaceConvention IEditor = new InterfaceConvention("IEditor");
        EnumElementConvention EditorAction = new EnumElementConvention("EditorAction");
        InterfaceConvention ITileConfiguration = new InterfaceConvention("ITileConfiguration");
        InterfaceConvention IEditorPageModel = new InterfaceConvention("IEditorPageModel");
        
        IEnumerable<IScannableAssembly> assemblies;
        Interfaces.Reflection.ITypeScanner typeScanner;

        ObservableCollection<ITypeInfo> pages = new ObservableCollection<ITypeInfo>();
        ObservableCollection<ITypeInfo> selectedPages = new ObservableCollection<ITypeInfo>();

        string pageSelector = "";

        public AddEditorControl()
        {
            InitializeComponent();
            this.cbxEditor.DataContext = IEditor.Results;
            this.cbxEditorAction.DataContext = EditorAction.Results;
            this.cbxTileConfiguration.DataContext = ITileConfiguration.Results;

            this.lstPages.DataContext = pages;
            this.lstSelectedPages.DataContext = selectedPages;
        }

        public void PerformScan(Interfaces.Reflection.IAssemblyFinder assemblyFinder, Interfaces.Reflection.ITypeScanner typeScanner, ISAHLProjectItem projectItem)
        {
            this.typeScanner = typeScanner;
            this.assemblies = assemblyFinder.Where(x => x.StartsWith("SAHL.Core.UI") || x.StartsWith("SAHL.Config"));
            typeScanner.Scan(assemblies, new IScanConvention[] { IEditor, EditorAction, ITileConfiguration, IEditorPageModel });
        }

        public void Validate(Interfaces.Validation.IControlValidation controlValidation)
        {
            controlValidation.ValidateControl("Editor has not been set", () => this.cbxEditor.Text.Length != 0);
            controlValidation.ValidateControl("Tile Configuration has not been set", () => this.cbxTileConfiguration.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Editor Title has not been set", () => this.txtTitle.Text.Trim().Length != 0);
            controlValidation.ValidateControl("Editor Action has not been set", () => this.cbxEditorAction.Text.Length != 0);
        }

        public void UpdateModel(dynamic model)
        {
            model.EditorShortName = IEditor.Results[cbxEditor.SelectedIndex].Name;
            model.EditorName = string.Format("{0}Configuration", model.EditorShortName);
            model.SelectorName = string.Format("{0}PageSelectorConfiguration", IEditor.Results[cbxEditor.SelectedIndex].Name);
            model.Editor = IEditor.Results[cbxEditor.SelectedIndex].FullName;
            model.TileConfiguration = ITileConfiguration.Results[cbxTileConfiguration.SelectedIndex].FullName;
            model.Title = txtTitle.Text;
            model.EditorAction = EditorAction.Results[cbxEditorAction.SelectedIndex].FullName;
            model.IsOrderedSelector = this.orderedPageSection.Visibility == System.Windows.Visibility.Visible;
            model.Pages = selectedPages.AsEnumerable();
            model.CustomPageSelector = pageSelector;
        }

        #region events
        private void cbxEditor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditorUseOrderedPageSelector())
            {
                FillPagesCollection(this.pages);
                orderedPageSection.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                orderedPageSection.Visibility = System.Windows.Visibility.Collapsed;
                FillPagesCollection(this.selectedPages);
            }
        }

        private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (ITypeInfo item in lstPages.SelectedItems)
            {
                selectedPages.Add(item);
            }
            foreach (ITypeInfo item in selectedPages)
            {
                pages.Remove(item);
            }
        }

        private void Remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (ITypeInfo item in lstSelectedPages.SelectedItems)
            {
                pages.Add(item);
            }
            foreach (ITypeInfo item in pages.Where(x=> lstSelectedPages.SelectedItems.Contains(x)))
            {
                selectedPages.Remove(item);
            }
        }

        private void MoveUp_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedTypes = GetSelectedPages();
            foreach (ITypeInfo item in selectedTypes)
            {
                MoveItem(selectedPages, item, -1);
            }
        }

        private void MoveDown_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedTypes = GetSelectedPages().Reverse();
            foreach (ITypeInfo item in selectedTypes)
            {
                MoveItem(selectedPages, item, 1);
            }
        }
        #endregion

        #region methods
        public void FillPagesCollection(ObservableCollection<ITypeInfo> collection)
        {
            pages.Clear();
            selectedPages.Clear();
            
            string editorFullName = IEditor.Results[cbxEditor.SelectedIndex].FullName;
            string editorNamespace = editorFullName.Substring(0, editorFullName.LastIndexOf("."));

            foreach (ITypeInfo info in IEditorPageModel.Results.Where(x => x.FullName.StartsWith(editorNamespace)))
            {
                collection.Add(info);
            }
        }

        public bool EditorUseOrderedPageSelector()
        {
            ITypeInfo info = IEditor.Results[this.cbxEditor.SelectedIndex];
            InterfaceWithGenericsConvention IEditorPageModelSelector = new InterfaceWithGenericsConvention("IEditorPageModelSelector<>", info.FullName);
            typeScanner.Scan(assemblies, new IScanConvention[] { IEditorPageModelSelector });
            if (IEditorPageModelSelector.Results.Count > 0)
            {
                pageSelector = IEditorPageModelSelector.Results[0].FullName;
            }
            else
            {
                pageSelector = "";
            }
            return IEditorPageModelSelector.Results.Count == 0;
        }

        private void MoveItem<T>(ObservableCollection<T> list, T objectValue, int direction)
        {
            int newIndex = list.IndexOf(objectValue) + direction;
            if (newIndex == -1)
                newIndex = 0;
            if (newIndex == list.Count)
                newIndex = list.Count - 1;

            list.Remove(objectValue);
            list.Insert(newIndex, objectValue);
        }

        private IEnumerable<ITypeInfo> GetSelectedPages()
        {
            return selectedPages.Where(x => this.lstSelectedPages.SelectedItems.Contains(x)).ToArray();
        }
        #endregion
    }
}