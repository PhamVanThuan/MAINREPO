using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Configuration;
using StructureMap;
using System;
using System.Runtime.InteropServices;

namespace SAHomeloans.SAHL_VSExtensions.Internal
{
    public class VSSolutionExplorer : IVSSolutionExplorer
    {
        private IVsMonitorSelection selectionManager;
        private IContainer container;

        public VSSolutionExplorer(IVSServices serviceProvider, IContainer container)
        {
            selectionManager = serviceProvider.GetService<IVsMonitorSelection>();
            this.container = container;
        }

        public ISAHLProjectItem GetCurrentProjectItem()
        {
            IntPtr selection = IntPtr.Zero;
            uint pitemId;
            IVsMultiItemSelect multiSelection;
            IntPtr currentSelectionContainer = IntPtr.Zero;
            ProjectItem selectedProjectItem = null;
            try
            {
                selectionManager.GetCurrentSelection(out selection, out pitemId, out multiSelection, out currentSelectionContainer);
                IVsHierarchy hierarchy = Marshal.GetTypedObjectForIUnknown(selection, typeof(IVsHierarchy)) as IVsHierarchy;
                if (hierarchy != null)
                {
                    object projectItemObj;
                    if (hierarchy.GetProperty(pitemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out projectItemObj) >= 0)
                    {
                        ProjectItem item = projectItemObj as ProjectItem;
                        if (item.Kind == VSConstants.ItemTypeGuid.PhysicalFolder_string)
                        {
                            selectedProjectItem = item;
                        }
                    }
                }
            }
            finally
            {
                if (selection != IntPtr.Zero)
                {
                    Marshal.Release(selection);
                }
                if (currentSelectionContainer != IntPtr.Zero)
                {
                    Marshal.Release(currentSelectionContainer);
                }
            }
            return new SAHLProjectItem(selectedProjectItem, container);
        }
    }
}