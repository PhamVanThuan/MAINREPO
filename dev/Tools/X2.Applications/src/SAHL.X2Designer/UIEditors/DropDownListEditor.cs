using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SAHL.X2Designer.UiEditors
{
    internal class DropDownListEditor : UITypeEditor
    {
        public static bool UserHasClicked = false;
        ListBox mListBox = new ListBox();

        IWindowsFormsEditorService edSvc;
        public static string[] strList;

        public DropDownListEditor()
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
            mListBox.Items.AddRange(strList);
            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                return mListBox.SelectedItem;
            }
            return value;
        }

        private void mListBox_Click(object sender, EventArgs e)
        {
            UserHasClicked = true;
            edSvc.CloseDropDown();
        }
    }
}