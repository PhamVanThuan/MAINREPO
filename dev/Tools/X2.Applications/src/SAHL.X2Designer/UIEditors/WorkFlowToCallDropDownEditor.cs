using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.UiEditors
{
    internal class WorkFlowToCallDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        public WorkFlowToCallDropDownEditor()
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
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                if (MainForm.App.GetCurrentView().Document.WorkFlows[x] != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                {
                    if (MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName.Length > 0)
                    {
                        mListBox.Items.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName.ToString());
                    }
                }
            }

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName == mListBox.SelectedItem.ToString())
                        {
                            return MainForm.App.GetCurrentView().Document.WorkFlows[x];
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

    public class WorkFlowToCallConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(WorkFlow))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is WorkFlow)
            {
                WorkFlow mItem = value as WorkFlow;
                return mItem.WorkFlowName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}