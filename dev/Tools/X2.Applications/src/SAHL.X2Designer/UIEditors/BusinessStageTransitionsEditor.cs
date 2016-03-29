using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.UiEditors
{
    internal class BusinessStageTransitionsEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            List<BusinessStageItem> SP = value as List<BusinessStageItem>;
            if (SP == null)
                return value;

            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                //frmManageBusinessStageTransitions sp = new frmManageBusinessStageTransitions(SP, MainForm.App.GetCurrentView().Document);
                //BusinessStageTransitionsView sp = new BusinessStageTransitionsView();

                //edSvc.ShowDialog(sp);
                //sp.m_Stages
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    public class BusinessStageTransitionsConvertor : TypeConverter
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
            value is List<BusinessStageItem>)
            {
                return "Click To Edit";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}