using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using Northwoods.Go;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{            
    public partial class frmManageBusinessStageTransitions : Form
    {
        public  List<BusinessStageItem> m_Stages;
        private ProcessDocument m_ProcessDoc;
        private List<BusinessStageItem> lstOfStages = new List<BusinessStageItem>();

        public frmManageBusinessStageTransitions(List<BusinessStageItem> stages, ProcessDocument pDoc)
        {
            InitializeComponent();
            MainForm.App.GetBusinessStageItems(string.Empty);
            m_ProcessDoc = pDoc;
            m_Stages = stages;
            PopulateSelectedStages();            
        }


        private void PopulateSelectedStages()
        {
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.BusinessStages.Count; x++)
            {
                lstStages.Items.Add(MainForm.App.GetCurrentView().Document.BusinessStages[x].DefinitionGroupDescription +
                    " -> " + MainForm.App.GetCurrentView().Document.BusinessStages[x].DefinitionDescription);
                lstOfStages.Add(MainForm.App.GetCurrentView().Document.BusinessStages[x]);
            }
            for (int x = 0; x < lstStages.Items.Count; x++)
            {
                for (int y = 0; y < m_Stages.Count; y++)
                {
                    if (lstOfStages[x].SDSDGKey == m_Stages[y].SDSDGKey)
                    {
                        lstStages.SetItemCheckState(x, CheckState.Checked);
                    }
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_Stages.Clear();
            for (int x = 0; x < lstStages.Items.Count; x++)
            {
                if (lstStages.GetItemCheckState(x) == CheckState.Checked)
                {
                    m_Stages.Add(lstOfStages[x]);
                }
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
                MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.BusinessStageTransition);
              IBusinessStageTransitions bst =  MainForm.App.GetCurrentView().Selection.Primary as IBusinessStageTransitions;
              if (null != bst)
              {
                // set the business stage transitions here
                bst.BusinessStageTransitions = m_Stages;
              }
              
            }

            Close();
        }

        private void frmManageBusinessStageTransitions_Load(object sender, EventArgs e)
        {       

            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mObj = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mObj);
                }
            }
        }


    }
}