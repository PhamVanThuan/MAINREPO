using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    internal class DropDownListEditorPriority : UITypeEditor
    {
        ListBox mListBox = new ListBox();

        IWindowsFormsEditorService edSvc;

        public DropDownListEditorPriority()
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
            string Val = Convert.ToString(value);
            BaseActivityProperties BAPs = context.Instance as BaseActivityProperties;
            if (BAPs != null)
            {
                mListBox.Items.Clear();
                mListBox.Items.AddRange(BAPs.GetPriorities());
                mListBox.Height = mListBox.PreferredHeight;
                mListBox.SelectedValue = Val;
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    edSvc.DropDownControl(mListBox);

                    if (mListBox.SelectedItem != null)
                    {
                        return Convert.ToInt32(mListBox.SelectedItem.ToString());
                    }
                }
            }
            //}
            return value;
        }

        private void mListBox_Click(object sender, EventArgs e)
        {
            edSvc.CloseDropDown();
        }
    }
}