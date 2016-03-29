using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2Designer.Connectivity;

namespace SAHL.X2Designer.Forms
{
    public partial class ConnectionForm : Form
    {
        SqlConnection m_Connection = null;
        string m_ConnectStr = "";
        string _defaultCatalog = "X2";

        public string DefaultCatalog
        {
            get
            {
                return _defaultCatalog;
            }
            set
            {
                _defaultCatalog = value;
            }
        }

        public ConnectionForm()
        {
            InitializeComponent();

            MainForm.App.OnServersReady += new MainForm.ServerReadyHandler(ServersReady);
            ServersReady();        
        }

        private static int _CmdTimeOut = 90;

        public static int CmdTimeout { get { return _CmdTimeOut; } }

        public string ConnectionString
        {
            get
            {
                return m_ConnectStr;
            }
            set
            {
                m_ConnectStr = value;
            }
        }

        private void ServersReady()
        {
            MainForm.App.OnServersReady -= new MainForm.ServerReadyHandler(ServersReady);

            cbxServerName.Text = "";
            cbxServerName.SelectedIndexChanged -= new EventHandler(cbxServerName_SelectedIndexChanged);
            cbxServerName.BeginUpdate();

            //get list of servers
            string[] servers = Servers.GetServerNames().ToArray();
            cbxServerName.Items.AddRange(servers);
            cbxServerName.EndUpdate();
            cbxServerName.SelectedIndexChanged += new EventHandler(cbxServerName_SelectedIndexChanged);

            // find the server
            if (m_ConnectStr != "")
            {
                SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder(m_ConnectStr);
                cbxServerName.Text = SCSB.DataSource;
                if (SCSB.IntegratedSecurity)
                {
                    radSQLAuth.Checked = false;
                }
                else
                {
                    radSQLAuth.Checked = true;
                }
                txtUserName.Text = SCSB.UserID;
                txtPassword.Text = SCSB.Password;

                // get the available catalogs
                m_Connection = new SqlConnection(m_ConnectStr);
                try
                {
                    m_Connection.Open();
                    m_Connection.ChangeDatabase("master");
                    SqlDataAdapter SDA = new SqlDataAdapter("use master; select name from sysdatabases order by name", m_Connection);
                    DataTable Dbs = new DataTable();
                    SDA.Fill(Dbs);
                    for (int i = 0; i < Dbs.Rows.Count; i++)
                        cbxCatalog.Items.Add(Dbs.Rows[i].ItemArray[0].ToString());
                    cbxCatalog.Enabled = true;
                }

                catch (Exception e)
                {
                    ExceptionPolicy.HandleException(e, "X2Designer");
                }
                finally
                {
                    m_Connection.Dispose();
                }

                int index = cbxCatalog.Items.IndexOf(SCSB.InitialCatalog);
                if (index != -1)
                    cbxCatalog.SelectedIndex = index;

                m_ConnectStr = SCSB.ToString();
            }
        }

        private void cbxCatalog_Enter(object sender, EventArgs e)
        {
            if (cbxCatalog.Items.Count > 0)
            {
                return;
            }
            else
            {
                PopulateCatalog();
            }
        }

        private void PopulateCatalog()
        {
            SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder();
            SCSB.DataSource = cbxServerName.Text;

            if (radSQLAuth.Checked)
            {
                SCSB.UserID = txtUserName.Text;
                SCSB.Password = txtPassword.Text;
                SCSB.PersistSecurityInfo = true;
            }
            else
            {
                SCSB.UserID = Properties.Settings.Default.DBUsername;
                SCSB.Password = Properties.Settings.Default.DBPassword;
                SCSB.IntegratedSecurity = true;
            }
            m_ConnectStr = SCSB.ToString();
            m_Connection = new SqlConnection(m_ConnectStr);
            try
            {
                cbxCatalog.Items.Clear();
                m_Connection.Open();
                m_Connection.ChangeDatabase("master");
                SqlDataAdapter SDA = new SqlDataAdapter("use master; select name from sysdatabases order by name", m_Connection);
                DataTable Dbs = new DataTable();
                SDA.Fill(Dbs);
                for (int i = 0; i < Dbs.Rows.Count; i++)
                    cbxCatalog.Items.Add(Dbs.Rows[i].ItemArray[0].ToString());
                cbxCatalog.Enabled = true;
            }
            catch (Exception e)
            {
                ExceptionPolicy.HandleException(e, "X2Designer");
            }
            finally
            {
                m_Connection.Dispose();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string Message = "";

            if (TryConnect(ref Message))
                MessageBox.Show("Connection OK", "Connection Test");
            else
                MessageBox.Show("Connection Failed");
        }

        private bool TryConnect(ref string Message)
        {
            SqlConnectionStringBuilder SCSB = new SqlConnectionStringBuilder();
            SCSB.DataSource = cbxServerName.Text;
            SCSB.ConnectTimeout = Properties.Settings.Default.ConnectionTimeout;
            SCSB.InitialCatalog = cbxCatalog.Text;
            if (radSQLAuth.Checked)
            {
                SCSB.UserID = txtUserName.Text;
                SCSB.Password = txtPassword.Text;
                SCSB.PersistSecurityInfo = true;
            }
            else
            {
                SCSB.IntegratedSecurity = true;
            }

            m_ConnectStr = SCSB.ToString();
            m_Connection = new SqlConnection(m_ConnectStr);
            try
            {
                m_Connection.Open();
                m_Connection.Close();
                _CmdTimeOut = Properties.Settings.Default.CommandTimeout;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                ExceptionPolicy.HandleException(e, "X2Designer");
                return false;
            }
            finally
            {
            }
        }

        private void radWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            cbxCatalog.Items.Clear();
            if (radWindowsAuth.Checked)
            {
                radSQLAuth.Checked = false;
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
            }
        }

        private void radSQLAuth_CheckedChanged(object sender, EventArgs e)
        {
            cbxCatalog.Items.Clear();
            if (radSQLAuth.Checked)
            {
                radWindowsAuth.Checked = false;
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            List<String> errorMessages = new List<string>();

            // validate input
            if (cbxServerName.Text.Length == -1)
                errorMessages.Add("Database Server must be selected.");

            if (cbxCatalog.Text.Length == -1)
                errorMessages.Add("Default Catalog must be selected.");

            if (radSQLAuth.Checked)
            {
                if (String.IsNullOrEmpty(txtUserName.Text))
                    errorMessages.Add("User Name must be entered.");

                if (String.IsNullOrEmpty(txtPassword.Text))
                    errorMessages.Add("Password must be entered.");
            }

            if (errorMessages.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("The following validation errors have occured");
                sb.AppendLine("--------------------------------------------");
                sb.AppendLine("");

                int idx = 0;
                foreach (string msg in errorMessages)
                {
                    idx++;
                    sb.AppendLine(idx.ToString() + ". " + msg);
                }
                MessageBox.Show(sb.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                EnableUI(false);
                // first check that there is a valid database string
                string Message = "";
                if (!TryConnect(ref Message))
                {
                    MessageBox.Show("Connection String is not valid: " + Message, "Error");
                    EnableUI(true);
                    DialogResult = DialogResult.No;
                    MainForm.App.Cursor = Cursors.Default;
                    return;
                }

                Close();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "X2Designer");
            }
            finally
            {
                EnableUI(true);
            }
        }

        private void cbxServerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCatalog.Items.Clear();
            cbxCatalog.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            Close();
        }

        private void EnableUI(bool Direction)
        {
            btnDone.Enabled = Direction;
            btnTest.Enabled = Direction;
            btnClose.Enabled = Direction;

            cbxCatalog.Enabled = Direction;
            cbxServerName.Enabled = Direction;
            txtUserName.Enabled = Direction;
            txtPassword.Enabled = Direction;
            radSQLAuth.Enabled = Direction;
            radWindowsAuth.Enabled = Direction;

            if (Direction == true)
            {
                if (radSQLAuth.Checked == true)
                {
                    txtUserName.Enabled = true;
                    txtPassword.Enabled = true;
                }
                else
                {
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                }
            }
        }

        private void btnRefreshServers_Click(object sender, EventArgs e)
        {
            MainForm.App.OnServersReady += new MainForm.ServerReadyHandler(ServersReady);
            MainForm.App.RefreshServers();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            cbxServerName.Enabled = true;
            cbxServerName.Text = "deva03";

            radSQLAuth.Checked = true;
            txtUserName.Text = Properties.Settings.Default.DBUsername;
            txtPassword.Text = Properties.Settings.Default.DBPassword;
        }

        private void btnDone_Enter(object sender, EventArgs e)
        {
            if (cbxCatalog.Text.Length == 0)
            {
                cbxCatalog.Text = _defaultCatalog;
            }
        }
    }
}