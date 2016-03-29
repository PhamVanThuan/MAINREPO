using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    internal class ManageAppliedToStatesEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            AppliedStatesCollection SP = value as AppliedStatesCollection;
            if (SP == null)
                return value;

            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmManageAppliedToStates sp = new frmManageAppliedToStates(SP, MainForm.App.GetCurrentView().Document);
                edSvc.ShowDialog(sp);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    public class StateConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is CheckedListCollection)
            {
                return "Click to edit roles";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}