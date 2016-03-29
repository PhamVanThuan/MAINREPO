using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SAHL.X2Designer.Forms
{
    public partial class frmCheckedListProperty : Form
    {
        CheckedListCollection m_SP;

        public frmCheckedListProperty(CheckedListCollection SP)
        {
            InitializeComponent();
            m_SP = SP;
            for (int x = 0; x < SP.Count; x++)
            {
                checkedListMain.Items.Add(m_SP[x].Name, m_SP[x].isChecked);
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            m_SP.Clear();
            for (int x = 0; x < checkedListMain.Items.Count; x++)
            {
                CheckedListItem myItem = new CheckedListItem();
                myItem.Name = checkedListMain.Items[x].ToString();
                myItem.isChecked = checkedListMain.GetItemChecked(x);
                m_SP.Add(myItem);
            }
            Close();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmCheckedListPropertyValue myFrmVal = new frmCheckedListPropertyValue();
            myFrmVal.ShowDialog();
            if (myFrmVal.DialogResult == DialogResult.OK)
            {
                checkedListMain.Items.Add(myFrmVal.txtValue.Text.ToString());
            }
            myFrmVal.Dispose();
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (checkedListMain.SelectedItem != null)
            {
                checkedListMain.Items.Remove(checkedListMain.SelectedItem);
            }
        }
    }

    [Serializable]
    public class CheckedListPropertiesEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            CheckedListCollection SP = value as CheckedListCollection;
            if (SP == null)
                return value;

            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmCheckedListProperty sp = new frmCheckedListProperty(SP);
                edSvc.ShowDialog(sp);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    [Serializable]
    public class CheckedListConvertor : TypeConverter
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
            return "Click to edit";
        }
    }

    [Serializable]
    public class CheckedListCollection : List<CheckedListItem>
    {
    }

    [Serializable]
    public class CheckedListItem
    {
        String m_Name = "";
        bool m_Checked = false;

        public CheckedListItem()
        {
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public bool isChecked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                m_Checked = value;
            }
        }
    }
}