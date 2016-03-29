using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmStateOrder : Form
    {
        public frmStateOrder()
        {
            InitializeComponent();
            foreach (BaseState bs in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States)
            {
                listStateOrder.Items.Add(bs.Text);
            }
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count; x++)
            {
                bool found = false;
                for (int y = 0; y < listStateOrder.Items.Count; y++)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x].Text == listStateOrder.Items[y].ToString())
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    listStateOrder.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x].Text.ToString());
                }
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdUp_Click(object sender, EventArgs e)
        {
            string[] holdItems = new string[listStateOrder.Items.Count];
            string switchItem = "";
            BaseState switchState;
            int holdSelIndex = listStateOrder.SelectedIndex;
            if (listStateOrder.SelectedIndex != -1)
            {
                if (listStateOrder.SelectedIndex > 0)
                {
                    for (int x = 0; x < listStateOrder.Items.Count; x++)
                    {
                        holdItems[x] = listStateOrder.Items[x].ToString();
                    }
                    switchItem = holdItems[listStateOrder.SelectedIndex - 1];
                    switchState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex - 1];

                    holdItems[listStateOrder.SelectedIndex - 1] = holdItems[listStateOrder.SelectedIndex];
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex - 1] = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex];
                    holdItems[listStateOrder.SelectedIndex] = switchItem;
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex] = switchState;
                    listStateOrder.Items.Clear();
                    listStateOrder.Items.AddRange(holdItems);
                    listStateOrder.SelectedIndex = holdSelIndex - 1;
                }
            }
        }

        private void cmdDown_Click(object sender, EventArgs e)
        {
            string[] holdItems = new string[listStateOrder.Items.Count];
            string switchItem = "";
            BaseState switchState;
            int holdSelIndex = listStateOrder.SelectedIndex;
            if (listStateOrder.SelectedIndex != -1)
            {
                if (listStateOrder.SelectedIndex < listStateOrder.Items.Count - 1)
                {
                    for (int x = 0; x < listStateOrder.Items.Count; x++)
                    {
                        holdItems[x] = listStateOrder.Items[x].ToString();
                    }
                    switchItem = holdItems[listStateOrder.SelectedIndex + 1];
                    switchState = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex + 1];
                    holdItems[listStateOrder.SelectedIndex + 1] = holdItems[listStateOrder.SelectedIndex];
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex + 1] = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex];
                    holdItems[listStateOrder.SelectedIndex] = switchItem;
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[listStateOrder.SelectedIndex] = switchState;

                    listStateOrder.Items.Clear();
                    listStateOrder.Items.AddRange(holdItems);
                    listStateOrder.SelectedIndex = holdSelIndex + 1;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void frmStateOrder_Load(object sender, EventArgs e)
        {
        }
    }

    [Serializable]
    public class StateOrderPropertiesEditor : UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Uses the IWindowsFormsEditorService to display a modal dialog
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                frmStateOrder sp = new frmStateOrder();
                edSvc.ShowDialog(sp);
            }
            return value;
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        [Serializable]
        public class StateOrderConverter : TypeConverter
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
                    return "Click to edit State Order";
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}