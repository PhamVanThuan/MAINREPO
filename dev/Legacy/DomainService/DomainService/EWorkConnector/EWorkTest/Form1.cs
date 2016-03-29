using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EWorkConnector;

namespace EWorkTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string SessionID = string.Empty;
        eWork engine = null;
        string ChangedField = "AttorneyNumber";
        string EFolderID = string.Empty;
        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                txtFID.Text = "";
                Dictionary<string, string> Vars = new Dictionary<string, string>();
                Vars.Add("AttorneyNumber", txtAttKey.Text);
                Vars.Add("ProspectNumber", txtAppKey.Text);
                Vars.Add("UserToDo", txtUser.Text);
                string MapName = txtMapName.Text;
                EFolderID = engine.CreateFolder(SessionID, MapName, Vars, ChangedField);
                txtFID.Text = EFolderID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAction1_Click(object sender, EventArgs e)
        {
            try
            {
                string Action = txtAction.Text;
                Dictionary<string, string> Vars = new Dictionary<string, string>();
                switch (Action)
                {
                    case "Action1":
                        {
                            ChangedField = "ReasonID";
                            Vars.Add("ReasonID", EFolderID);
                            break;
                        }
                    case "Action2":
                        {
                            ChangedField = "AttorneyNumber";
                            Vars.Add("AttorneyNumber", txtAttKey.Text);
                            Vars.Add("ProspectNumber", txtAppKey.Text);
                            Vars.Add("UserToDo", txtUser.Text);
                            break;
                        }
                    case "Action3":
                        {
                            ChangedField = "ReasonID";
                            Vars.Add("ReasonID", txtReasonID.Text);
                            break;
                        }
                    case SAHL.Common.Constants.EworkActionNames.X2NTUAdvise:
                        {
                            ChangedField = "";
                            break;
                        }
                }
                
                string resp = engine.InvokeAndSubmitAction(SessionID,txtFID.Text, Action, Vars, ChangedField);
                MessageBox.Show(resp,"OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        private void txtAppKey_TextChanged(object sender, EventArgs e)
        {
            //ChangedField = "ProspectNumber";
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            //ChangedField = "UserTodo";
        }

        private void txtAttKey_TextChanged(object sender, EventArgs e)
        {
            //ChangedField = "AttorneyNumber";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                engine = new eWork();
                SessionID = engine.LogIn(eWork.GetSimpleWindowsUserName());
                this.Text = "EWork SessionID:" + SessionID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}