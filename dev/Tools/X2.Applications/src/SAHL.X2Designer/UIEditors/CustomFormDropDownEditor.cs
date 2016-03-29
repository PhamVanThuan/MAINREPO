using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.UiEditors
{
    internal class CustomFormDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        public static bool UserHasClicked = false;
        IWindowsFormsEditorService edSvc;

        public CustomFormDropDownEditor()
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
            UserHasClicked = false;
            mListBox.Items.Clear();
            mListBox.Items.Add(" ");
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
            {
                mListBox.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name.ToString());
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x];
                        }
                    }
                }
                return null;
            }
            return value;
        }

        private void mListBox_Click(object sender, EventArgs e)
        {
            UserHasClicked = true;
            edSvc.CloseDropDown();
        }
    }

    public class CustomFormConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(CustomFormItem))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is CustomFormItem)
            {
                CustomFormItem mItem = value as CustomFormItem;
                return mItem.Name.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}