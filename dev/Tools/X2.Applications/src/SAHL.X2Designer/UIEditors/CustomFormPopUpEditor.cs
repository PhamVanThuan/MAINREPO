using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.UiEditors
{
    internal class CustomFormPopUpEditor : UITypeEditor
    {
        IWindowsFormsEditorService edSvc;

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<CustomFormItem> SP = value as List<CustomFormItem>;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmCustomFormCheckList mFrm = new frmCustomFormCheckList(SP, MainForm.App.GetCurrentView().Document);
                edSvc.ShowDialog(mFrm);
            }
            return value;
        }
    }
}