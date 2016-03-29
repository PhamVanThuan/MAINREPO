using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.UiEditors
{
    internal class ExternalActivityDropDownListEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;
        public static bool UserHasClicked = false;

        public ExternalActivityDropDownListEditor()
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
            bool mustAddFirstStrings = true;
            if (context.ToString() == "System.Windows.Forms.PropertyGridInternal.PropertyDescriptorGridEntry InvokedBy")
            {
                mustAddFirstStrings = false;
            }

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x].ExternalActivity == "None" && mustAddFirstStrings == false)
                {
                    continue;
                }

                mListBox.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x].ExternalActivity.ToString());
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x].ExternalActivity == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection[x];
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

    public class ExternalActivityConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(ExternalActivityItem))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is ExternalActivityItem)
            {
                ExternalActivityItem mItem = value as ExternalActivityItem;
                return mItem.ExternalActivity.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}