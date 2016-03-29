using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.UiEditors
{
    public class ManageFormAppliesToEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            CustomFormItem item = ((CustomFormItemProperties)context.Instance).AppliesTo;

            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmManagedAppliedStateForForm sp = new frmManagedAppliedStateForForm(item, MainForm.App.GetCurrentView().Document);
                edSvc.ShowDialog(sp);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    public class ManageRolesEditor : UITypeEditor
    {
        private BaseItem m_OwnerItem = null;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            BaseProperties BP = context.Instance as BaseProperties;
            if (BP != null)
                m_OwnerItem = BP.Owner;

            Items.RoleInstanceCollection SP = value as Items.RoleInstanceCollection;
            if (SP == null)
                return value;

            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmManageAccess sp = new frmManageAccess(SP, MainForm.App.GetCurrentView().Document, m_OwnerItem);
                edSvc.ShowDialog(sp);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    public class AvailableGroupConvertor : TypeConverter
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

    public class b
    {
        protected virtual void Dostuff()
        {
            if (this.GetType() == typeof(d))
            {
            }
        }
    }

    public class d : b
    {
    }
}