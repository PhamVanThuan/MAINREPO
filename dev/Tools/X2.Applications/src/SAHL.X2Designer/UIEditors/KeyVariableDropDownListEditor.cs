using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.UiEditors
{
    internal class KeyVariableDropDownListEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        public KeyVariableDropDownListEditor()
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
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; x++)
            {
                mListBox.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x].Name.ToString());
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x].Name == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x];
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

    public class KeyVariableConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(CustomVariableItem))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is CustomVariableItem)
            {
                CustomVariableItem mItem = value as CustomVariableItem;
                return mItem.Name.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}