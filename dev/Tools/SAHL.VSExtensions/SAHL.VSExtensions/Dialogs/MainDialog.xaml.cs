using Microsoft.VisualStudio.PlatformUI;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using SAHL.VSExtensions.Interfaces.Reflection;
using SAHL.VSExtensions.Interfaces.Validation;
using SAHomeloans.SAHL_VSExtensions.Controls;
using SAHomeloans.SAHL_VSExtensions.Internal.Reflection;
using StructureMap;
using System;
using System.Dynamic;

namespace SAHomeloans.SAHL_VSExtensions.Dialogs
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>

    public partial class MainDialog : DialogWindow, IMainDialog
    {
        private IMenuManager menuManager;
        private IControlManager controlManager;
        private IVSSolutionExplorer solutionExplorer;
        private ISAHLProjectItem currentProjectItem;
        private IContainer container;
        private IAssemblyFinder assemblyFinder;
        private ITypeScanner typeScanner;

        private int SelectedGroup = 0;
        private int SelectedItem = 0;
        private int SelectedItemGroup = 0;

        public MainDialog(IMenuManager menuManager, IControlManager controlManager, IVSSolutionExplorer solutionExplorer, IContainer container, ITypeScanner typeScanner)
        {
            this.menuManager = menuManager;
            this.controlManager = controlManager;
            this.solutionExplorer = solutionExplorer;
            this.container = container;
            this.typeScanner = typeScanner;
            this.currentProjectItem = this.solutionExplorer.GetCurrentProjectItem();
            InitializeComponent();
            BuildSolution();
            SetupAssemblyFinder();
            InitMenus();
        }

        private void InitMenus()
        {
            foreach (string group in menuManager.GetMenuGroups())
            {
                ISAHLProjectItem project = solutionExplorer.GetCurrentProjectItem();
                VsMenuItem groupMenu = new VsMenuItem();
                groupMenu.Text = group;
                foreach (IMenuItem item in menuManager.GetMenuItems(group))
                {
                    if (item.CanExecute(currentProjectItem))
                    {
                        VsMenuItem itemMenu = new VsMenuItem();
                        itemMenu.Text = item.Name;
                        itemMenu.MenuItem = item;
                        groupMenu.Items.Add(itemMenu);
                    }
                }
                if (groupMenu.Items.Count > 0)
                {
                    this.controlMenu.Items.Add(groupMenu);
                }
            }
            if (this.controlMenu.Items.Count > 0)
            {
                this.controlMenu[0].IsExpanded = true;
                this.controlMenu[0][0].IsExpanded = true;
                SetContent(this.controlMenu[0][0].MenuItem);
            }
        }

        #region events

        private void MenuItem_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            SetContent(((VsMenuItem)e.Source).MenuItem);
            int index = this.controlMenu[SelectedGroup].Items.IndexOf(e.Source);
            if (SelectedItem != index || SelectedItemGroup != SelectedGroup)
            {
                this.controlMenu[SelectedItemGroup][SelectedItem].IsExpanded = false;
                SelectedItem = index;
                SelectedItemGroup = SelectedGroup;
            }
        }

        private void GroupItem_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = this.controlMenu.Items.IndexOf(e.Source);
            if (index != SelectedGroup)
            {
                this.controlMenu[SelectedGroup].IsExpanded = false;
                SelectedGroup = index;
            }
        }

        public void Cancel_Clicked(object sender, EventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public void Ok_Clicked(object sender, EventArgs e)
        {
            ISAHLControl currentControl = contentControl.Content as ISAHLControl;

            if (currentControl != null)
            {
                if (ValidateControl(currentControl))
                {
                    dynamic model = new ExpandoObject();
                    model.Namespace = this.currentProjectItem.Namespace;
                    currentControl.UpdateModel(model);
                    ISAHLConfiguration dialog = (ISAHLConfiguration)this.controlMenu.GetSelectedMenuItem().MenuItem;
                    dialog.Execute(this.currentProjectItem, model);
                }
                else
                {
                    return;
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        #endregion events

        #region private events

        public void SetContent(IMenuItem itemSelected)
        {
            ISAHLControl control = controlManager.GetUserControlForMenuItem(itemSelected) as ISAHLControl;
            control.PerformScan(assemblyFinder, typeScanner, currentProjectItem);
            contentControl.Content = control;
        }

        private void SetupAssemblyFinder()
        {
            this.assemblyFinder = new AssemblyFinder(currentProjectItem.CurrentProject);
        }

        private bool ValidateControl(ISAHLControl control)
        {
            IControlValidation controlValidation = container.GetInstance<IControlValidation>();
            control.Validate(controlValidation);
            if (!controlValidation.Result)
            {
                errorMsg.Text = controlValidation.ResultMessage;
                errorMsg.Visibility = System.Windows.Visibility.Visible;
            }
            return controlValidation.Result;
        }

        private void BuildSolution()
        {
            if (!currentProjectItem.CurrentProject.Build())
            {
                this.errorMsg.Text = "Solution does not currently build, this might produce unexpected results";
                this.errorMsg.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.errorMsg.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        #endregion private events
    }
}