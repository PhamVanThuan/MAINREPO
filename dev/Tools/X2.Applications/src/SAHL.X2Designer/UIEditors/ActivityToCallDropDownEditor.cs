using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.UiEditors
{
    internal class ReturnActivityConvertor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        private int workFlowIndex = -1;

        public static WorkFlow workFlowToSearch = null;
        private static WorkFlow holdWorkFlowToSearch;

        public ReturnActivityConvertor()
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
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                if (MainForm.App.GetCurrentView().Document.WorkFlows[x] == workFlowToSearch)
                {
                    workFlowIndex = x;
                    break;
                }
            }
            if (workFlowIndex != -1)
            {
                PopulateListBox();
            }

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
                        foreach (BaseActivity mActivity in workFlowToSearch.Activities)
                        {
                            if (mActivity is ReturnWorkflowActivity)
                            {
                                if (mActivity != null)
                                {
                                    if (mActivity.Name == mListBox.SelectedItem.ToString())
                                    {
                                        found = true;
                                        return mActivity;
                                    }
                                    if (found == true)
                                    {
                                        break;
                                    }
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
            if (workFlowIndex != -1)
            {
                foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[workFlowIndex])
                {
                    if (mListBox.Items.Count > 0)
                    {
                        //                               mListBox.Items.Clear();
                    }
                    if (o is ReturnWorkflowActivity)
                    {
                        ReturnWorkflowActivity mActivity = o as ReturnWorkflowActivity;
                        mListBox.Items.Add(mActivity.Name);
                    }
                }
            }
        }
    }

    internal class ActivityToCallDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;

        private int workFlowIndex = -1;

        public static WorkFlow workFlowToSearch = null;
        private static WorkFlow holdWorkFlowToSearch;

        public ActivityToCallDropDownEditor()
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
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
            {
                if (MainForm.App.GetCurrentView().Document.WorkFlows[x] == workFlowToSearch)
                {
                    workFlowIndex = x;
                    break;
                }
            }
            if (workFlowIndex != -1)
            {
                PopulateListBox();
            }

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
                        foreach (BaseActivity mActivity in workFlowToSearch.Activities)
                        {
                            if (mActivity != null)
                            {
                                if (mActivity.Name == mListBox.SelectedItem.ToString())
                                {
                                    found = true;
                                    return mActivity;
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
            if (workFlowIndex != -1)
            {
                foreach (GoObject o in MainForm.App.GetCurrentView().Document.WorkFlows[workFlowIndex])
                {
                    if (mListBox.Items.Count > 0)
                    {
                        //                               mListBox.Items.Clear();
                    }
                    if (o is BaseActivity)
                    {
                        BaseActivity mActivity = o as BaseActivity;
                        foreach (CustomLink l in mActivity.Links)
                        {
                            if (l.FromPort.GoObject.Parent.GetType() == typeof(ClapperBoard))
                            {
                                bool found = false;
                                for (int x = 0; x < mListBox.Items.Count; x++)
                                {
                                    if (mListBox.Items[x].ToString() == mActivity.Name)
                                    {
                                        found = true;
                                    }
                                }
                                if (found == false)
                                {
                                    mListBox.Items.Add(mActivity.Name);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    internal class ActivityToReturnDropDownEditor : UITypeEditor
    {
        ListBox mListBox = new ListBox();
        IWindowsFormsEditorService edSvc;
        public static WorkFlow workFlow = null;

        public ActivityToReturnDropDownEditor()
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
                    foreach (BaseActivity mActivity in workFlow.Activities)
                    {
                        if (mActivity != null)
                        {
                            if (mActivity.Name == mListBox.SelectedItem.ToString())
                            {
                                found = true;
                                return mActivity;
                            }
                            if (found == true)
                            {
                                break;
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
            if (workFlow == null || workFlow != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                workFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;
            //mListBox.Items.Clear();
            foreach (GoObject o in workFlow)
            {
                if (o is BaseActivity)
                {
                    BaseActivity mActivity = o as BaseActivity;
                    foreach (CustomLink l in mActivity.Links)
                    {
                        //if (l.FromPort.GoObject.Parent.GetType() == typeof(ClapperBoard))
                        {
                            bool found = false;
                            for (int x = 0; x < mListBox.Items.Count; x++)
                            {
                                if (mListBox.Items[x].ToString() == mActivity.Name)
                                {
                                    found = true;
                                }
                            }
                            if (found == false)
                            {
                                mListBox.Items.Add(mActivity.Name);
                            }
                        }
                    }
                }
            }
        }
    }

    public class ActivityToCallConvertor : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(BaseActivity))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
        CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
            value is BaseActivity)
            {
                BaseActivity mItem = value as BaseActivity;
                return mItem.Name.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}