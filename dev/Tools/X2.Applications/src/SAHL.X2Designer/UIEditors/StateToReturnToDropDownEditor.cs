using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    internal class StateToReturnToDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        private int workFlowIndex = -1;

        public static WorkFlow workFlowToSearch = null;
        private static WorkFlow holdWorkFlowToSearch;

        public StateToReturnToDropDownEditor()
        {
            mListBox.BorderStyle = BorderStyle.None;
            mListBox.Click += new EventHandler(mListBox_Click);
            mListBox.Enter += new EventHandler(mListBox_Enter);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            PopulateListBox();

            mListBox.Height = mListBox.PreferredHeight;
            edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                edSvc.DropDownControl(mListBox);
                if (mListBox.SelectedItem != null)
                {
                    bool found = false;
                    if (workFlowToSearch != null)
                    {
                        foreach (BaseState state in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States)
                        {
                            if (state != null)
                            {
                                if (state.Name == mListBox.SelectedItem.ToString())
                                {
                                    found = true;
                                    return state;
                                }
                                if (found == true)
                                {
                                    break;
                                }
                            }
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

        private void mListBox_Enter(object sender, EventArgs e)
        {
            PopulateListBox();
        }

        private void PopulateListBox()
        {
            mListBox.Items.Clear();
            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
            {
                if (mListBox.Items.Count > 0)
                {
                    //                               mListBox.Items.Clear();
                }
                if (o is BaseState)
                {
                    BaseState state = o as BaseState;
                    mListBox.Items.Add(state.Name);
                }
            }
        }
    }

    //public class StateConvertor : TypeConverter
    //{
    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        if (destinationType == typeof(BaseState))
    //        {
    //            return true;
    //        }
    //        return base.CanConvertTo(context, destinationType);
    //    }

    //    public override object ConvertTo(ITypeDescriptorContext context,
    //    CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (destinationType == typeof(string) &&
    //        value is BaseState)
    //        {
    //            BaseState mItem = value as BaseState;
    //            return mItem.Name.ToString();
    //        }
    //        return base.ConvertTo(context, culture, value, destinationType);
    //    }
    //}
}