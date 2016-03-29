using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManagedAppliedStateForForm : Form
    {
        CustomFormItem form;
        ProcessDocument pd;
        Dictionary<string, UserState> AllStates = new Dictionary<string, UserState>();

        public frmManagedAppliedStateForForm(CustomFormItem form, ProcessDocument pd)
        {
            InitializeComponent();
            this.form = form;
            this.pd = pd;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
        }

        private void frmManagedAppliedStateForForm_Load(object sender, EventArgs e)
        {
            foreach (BaseState bs in pd.m_CurrentWorkFlow.States)
            {
                UserState us = bs as UserState;
                if (null != us)
                {
                    AllStates.Add(us.Name, us);
                    foreach (CustomFormItem cfi in us.CustomForms)
                    {
                        if (cfi.Name == form.Name)
                        {
                            form.AppliesTo.AddAppliesTo(us, form);
                        }
                    }
                    if (form.AppliesTo.IsAppliedToState(us.Name))
                    {
                        chb.Items.Add(us.Name, true);
                    }
                    else
                    {
                        chb.Items.Add(us.Name, false);
                    }
                }
            }
        }

        private void chb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string StateName = chb.Items[e.Index].ToString();
            UserState us = AllStates[StateName];
            if (e.NewValue == CheckState.Checked)
            {
                form.AppliesTo.AddAppliesTo(us, form);
            }
            else
            {
                form.AppliesTo.RemoveAppliesTo(us, form);
            }
        }
    }
}