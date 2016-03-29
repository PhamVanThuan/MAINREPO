using System;
using System.ComponentModel;
using System.Windows.Forms;
using SAHL.X2Designer.Views;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer
{
    public partial class ProcessForm : DockContent
    {
        private string m_OpenMethod;

        public ProcessForm(string OpenMethod, string ProcessName) // argument is used to set whether the form is new or being loaded ie "new" or "open"
        {
            InitializeComponent();
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            m_OpenMethod = OpenMethod;
            if (OpenMethod == "new")
            {
                m_View.Document.CreateWorkFlow("new", null);
            }
            m_View.Document.Name = ProcessName;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.m_View = new SAHL.X2Designer.Views.ProcessView();
            this.SuspendLayout();
            //
            // m_View
            //
            this.m_View.ActiveItemType = SAHL.X2Designer.Items.WorkflowItemType.None;
            this.m_View.ArrowMoveLarge = 10F;
            this.m_View.ArrowMoveSmall = 1F;
            this.m_View.BackColor = System.Drawing.Color.White;
            this.m_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_View.Location = new System.Drawing.Point(0, 0);
            this.m_View.Name = "m_View";
            this.m_View.Size = new System.Drawing.Size(292, 266);
            this.m_View.TabIndex = 0;
            this.m_View.Text = "processView1";
            //
            // ProcessForm
            //
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.m_View);
            this.Name = "ProcessForm";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Deactivate += new System.EventHandler(this.ProcessForm_Deactivate);
            this.Enter += new System.EventHandler(this.ProcessForm_Enter);
            this.Activated += new System.EventHandler(this.ProcessForm_Activated);
            this.Leave += new System.EventHandler(this.ProcessForm_Leave);
            this.Load += new System.EventHandler(this.ProcessForm_Load);
            this.ResumeLayout(false);
        }

        public ProcessView View
        {
            get { return m_View; }
        }

        private void ProcessForm_Load(object sender, EventArgs e)
        {
        }

        private void ProcessForm_Activated(object sender, EventArgs e)
        {
            if (MainForm.App.m_CodeView != null)
            {
                MainForm.App.m_CodeView.AttachCode();
            }
            MainForm.App.onActivityItemChecked += new MainForm.ActivityItemCheckedHandler(m_View.App_onActivityItemChecked);
            MainForm.App.onActivityItemUnChecked += new MainForm.ActivityItemUnCheckedHandler(m_View.App_onActivityItemUnChecked);

            MainForm.App.ProcessViewActivated();
            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
            {
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm != null)
                {
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm.Clear();
                }
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar != null)
                {
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.Clear();
                }
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles != null)
                {
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles.Clear();
                }
            }
        }

        private void ProcessForm_Deactivate(object sender, EventArgs e)
        {
            if (MainForm.App.m_CodeView != null)
            {
                MainForm.App.m_CodeView.SaveCode();
                MainForm.App.m_CodeView.DetachCode();
            }

            MainForm.App.onActivityItemChecked -= new MainForm.ActivityItemCheckedHandler(m_View.App_onActivityItemChecked);
            MainForm.App.onActivityItemUnChecked -= new MainForm.ActivityItemUnCheckedHandler(m_View.App_onActivityItemUnChecked);
            MainForm.App.ProcessViewDeactivated();
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm != null)
                    {
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableForm.Clear();
                    }
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar != null)
                    {
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableVar.Clear();
                    }
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles != null)
                    {
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.hashTableRoles.Clear();
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MainForm.App.mainFormIsClosing == false)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (MainForm.App.GetCurrentView().CheckIfModified())
                    {
                        DialogResult res = MessageBox.Show("Save Changes to this document before closing ?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (res == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                        }

                        else if (res == DialogResult.Yes)
                        {
                            if (MainForm.App.Save(this.View, this.Text, true) == true)
                            {
                                if (MainForm.App.m_CodeView != null)
                                {
                                    MainForm.App.m_CodeView.ClearView();
                                }

                                base.OnClosing(e);
                            }

                            else
                            {
                                e.Cancel = true;
                            }
                        }

                        else if (res == DialogResult.No)
                        {
                            base.OnClosing(e);
                        }
                    }
                    else
                    {
                        base.OnClosing(e);
                    }
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (MainForm.App.m_BrowserView != null)
            {
                if (MainForm.App.m_BrowserView.hashTable != null)
                {
                    MainForm.App.m_BrowserView.hashTable.Clear();
                }
                if (MainForm.App.m_BrowserView.hashTableRole != null)
                {
                    MainForm.App.m_BrowserView.hashTableRole.Clear();
                }
            }
            base.OnActivated(e);
        }

        private void ProcessForm_Leave(object sender, EventArgs e)
        {
            MainForm.App.CutCopyPasteTarget = null;
        }

        private void ProcessForm_Enter(object sender, EventArgs e)
        {
            MainForm.App.CutCopyPasteTarget = m_View;
        }
    }
}