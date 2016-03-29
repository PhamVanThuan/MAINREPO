using Microsoft.VisualStudio.Shell;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using StructureMap;
using System;
using System.ComponentModel.Design;

namespace SAHomeloans.SAHL_VSExtensions.Internal
{
    public class VSMenuManager : IVSMenuManager
    {
        private OleMenuCommandService omcs;
        private IContainer container;

        public VSMenuManager(IVSServices serviceProvider, IContainer container)
        {
            this.omcs = serviceProvider.GetService<OleMenuCommandService, IMenuCommandService>();
            this.container = container;
        }

        public void Initilize()
        {
            OleMenuCommand command = new OleMenuCommand(SAHL_Add_Clicked, new CommandID(GuidList.guidSAHL_VSExtensionsCmdSet, PkgCmdIDList.IDM_SAHL_ADD));
            omcs.AddCommand(command);
        }

        private void SAHL_Add_Clicked(object sender, EventArgs e)
        {
            IMainDialog mainDialog = container.GetInstance<IMainDialog>();

            if (mainDialog.ShowModal() == true)
            {
            }
        }
    }
}