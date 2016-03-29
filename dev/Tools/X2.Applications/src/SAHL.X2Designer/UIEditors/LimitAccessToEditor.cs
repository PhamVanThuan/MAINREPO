using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    internal class LimitAccessToEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        public LimitAccessToEditor()
        {
            mListBox.BorderStyle = BorderStyle.None;
            mListBox.Click += new EventHandler(mListBox_Click);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            mListBox.Items.Clear();
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.Global &&
                    MainForm.App.GetCurrentView().Document.Roles[x].Name != "WorkList" &&
                    MainForm.App.GetCurrentView().Document.Roles[x].Name != "TrackList")
                {
                    mListBox.Items.Add(MainForm.App.GetCurrentView().Document.Roles[x].Name.ToString());
                }
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.Roles[x].Name == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.Roles[x];
                        }
                    }
                }
                return null;
            }
            return value;
        }

        private void mListBox_Click(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }

    public class LimitAccessToConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(RolesCollectionItem))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is RolesCollectionItem)
            {
                RolesCollectionItem mItem = value as RolesCollectionItem;
                return mItem.Name.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}