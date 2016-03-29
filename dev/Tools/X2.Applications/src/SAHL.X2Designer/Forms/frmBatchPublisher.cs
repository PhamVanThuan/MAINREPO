using System;
using System.Collections.Generic;
using System.Windows.Forms;

//using SAHL.Common.VFS;
using SAHL.X2Designer.XML;

namespace SAHL.X2Designer.Forms
{
    public partial class frmBatchPublisher : Form
    {
        private bool _cancelClose = false;
        private string _currentMapName = "";

        #region properties

        private List<MapListItem> _listMaps;

        public List<MapListItem> ListMaps
        {
            get
            {
                // populate internal list
                _listMaps = new List<MapListItem>();
                foreach (ListViewItem li in lstMaps.Items)
                {
                    MapListItem itm = new MapListItem();
                    itm.MapName = li.SubItems[0].Text;
                    itm.Version = li.SubItems[1].Text;
                    _listMaps.Add(itm);
                }
                return _listMaps;
            }
        }

        private bool _batchMode;

        public bool BatchMode
        {
            get { return _batchMode; }
            set { _batchMode = value; }
        }

        private bool _backupMaps;

        public bool BackupMaps
        {
            get { return _backupMaps; }
            set { _backupMaps = value; }
        }

        public string X2EngineServer
        {
            get
            {
                return cbxEngine.Text + " : " + txtPort.Text;
            }
        }

        public string X2DatabaseServer
        {
            get
            {
                return cbxX2Server.Text;
            }
        }

        #endregion properties

        public frmBatchPublisher(string currentMapName)
        {
            InitializeComponent();
            _listMaps = new List<MapListItem>();
            _currentMapName = currentMapName;
        }

        private void frmBatchPublisher_Load(object sender, EventArgs e)
        {
            if (MainForm.App.GetCurrentView() == null)
            {
                chkCurrentMap.Checked = false;
                chkCurrentMap.Enabled = false;
            }

            //string[] servers = Servers.GetServerNames().ToArray();

            //X2 Engine Settings
            //cbxEngine.Items.AddRange(servers);
            foreach (string sname in Properties.Settings.Default.X2EngineNames)
            {
                cbxEngine.Items.Add(sname);
            }
            cbxEngine.SelectedText = MainForm.App.cfg.Settings["X2Engine"];

            txtPort.Text = MainForm.App.cfg.Settings["X2Port"];
            if (String.IsNullOrEmpty(txtPort.Text))
                txtPort.Text = "8089";

            //X2 Connection Settings
            //cbxX2Server.Items.AddRange(servers);
            foreach (string sname in Properties.Settings.Default.DBServerNames)
            {
                cbxX2Server.Items.Add(sname);
            }

            cbxX2Server.SelectedText = MainForm.App.cfg.Settings["X2Server"];

            foreach (string defaultCatalog in Properties.Settings.Default.X2DefaultCatalogs)
            {
                cbxX2Catalog.Items.Add(defaultCatalog);
            }
            cbxX2Catalog.SelectedText = MainForm.App.cfg.Settings["X2Catalog"];
            if (String.IsNullOrEmpty(MainForm.App.cfg.Settings["X2Catalog"]))
                cbxX2Catalog.SelectedText = "X2";

            txtX2User.Text = MainForm.App.cfg.Settings["X2Username"];
            txtX2Password.Text = MainForm.App.cfg.Settings["X2Password"];

            string X2Auth = MainForm.App.cfg.Settings["X2Authentication"];
            radX2Windows.Checked = X2Auth == "windows" ? true : false;
            radX2Sql.Checked = !radX2Windows.Checked;

            //2am Connection Settings
            //cbxDbServer.Items.AddRange(servers);
            foreach (string sname in Properties.Settings.Default.DBServerNames)
            {
                cbxDbServer.Items.Add(sname);
            }
            cbxDbServer.SelectedText = MainForm.App.cfg.Settings["DbServer"];

            foreach (string defaultCatalog in Properties.Settings.Default.dbDefaultCatalogs)
            {
                cbxDbCatalog.Items.Add(defaultCatalog);
            }
            cbxDbCatalog.SelectedText = MainForm.App.cfg.Settings["DbCatalog"];
            if (String.IsNullOrEmpty(MainForm.App.cfg.Settings["DbCatalog"]))
                cbxDbCatalog.SelectedText = "2AM";

            txtDbUser.Text = MainForm.App.cfg.Settings["DbUsername"];
            txtDbPassword.Text = MainForm.App.cfg.Settings["DbPassword"];

            string dbAuth = MainForm.App.cfg.Settings["DbAuthentication"];
            radDbWindows.Checked = dbAuth == "windows" ? true : false;
            radDbSql.Checked = !radDbWindows.Checked;

            // Timeout Settings
            txtConnectionTimeout.Text = MainForm.App.cfg.Settings["ConnectionTimeout"];
            if (String.IsNullOrEmpty(txtConnectionTimeout.Text))
                txtConnectionTimeout.Text = "900";

            txtCmdTimeout.Text = MainForm.App.cfg.Settings["CommandTimeout"];
            if (String.IsNullOrEmpty(txtCmdTimeout.Text))
                txtCmdTimeout.Text = "900";

            // Backup Settings
            string autoBackup = MainForm.App.cfg.Settings["AutomaticMapBackup"];
            chkBackupMaps.Checked = autoBackup == "True" ? true : false;

            txtBackupFolder.Text = MainForm.App.cfg.Settings["AutomaticBackupFolder"];
            txtBackupFolder.Enabled = autoBackup == "True" ? true : false;
            lblBackup.Enabled = autoBackup == "True" ? true : false;
            btnBrowse.Enabled = autoBackup == "True" ? true : false;

            // List of Maps
            string[] maps = MainForm.App.cfg.GetSetting("MapList").Split(',');
            foreach (string s in maps)
            {
                if (s.Length > 0)
                {
                    lstMaps.Items.Add(new ListViewItem(new string[] { s, XMLHandling.GetMapVersion(s, XMLHandling.GetXMLFile(s)) }));
                }
            }
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colVersion = new ColumnHeader();
            colName.Text = "Map Path and Name";
            colName.Width = 400;
            colName.TextAlign = HorizontalAlignment.Left;
            colVersion.Text = "Version";
            colVersion.Width = lstMaps.Width - 410;
            colVersion.TextAlign = HorizontalAlignment.Left;
            lstMaps.Columns.Add(colName);
            lstMaps.Columns.Add(colVersion);

            lstMaps.View = System.Windows.Forms.View.Details;

            if (lstMaps.Items.Count > 0)
                chkCurrentMap.Checked = false;

            if (MainForm.App.GetCurrentView() == null)
            {
                chkCurrentMap.Checked = false;
                chkCurrentMap.Enabled = false;
            }
        }

        private void frmBatchPublisher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cancelClose)
            {
                e.Cancel = true;
                _cancelClose = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dlgOpenMap.DefaultExt = "x2p";
            dlgOpenMap.Filter = "x2p Files (*.x2p)|*.x2p";
            dlgOpenMap.Multiselect = true;
            dlgOpenMap.Title = "Add X2 Map(s)";

            if (dlgOpenMap.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in dlgOpenMap.FileNames)
                {
                    string mapFileName = fileName;
                    string mapVersion = XMLHandling.GetMapVersion(mapFileName, XMLHandling.GetXMLFile(fileName));
                    //string mapVersion = XMLHandling.GetMapVersion(mapFileName, XMLHandling.GetXMLFile(mapFileName));
                    ListViewItem itemMap = new ListViewItem(new string[] { mapFileName, mapVersion });

                    // check if it exists in the list and add if it doesnt
                    bool found = false;
                    foreach (ListViewItem item in lstMaps.Items)
                    {
                        if (item.Text == itemMap.Text)
                        {
                            found = true;
                            break;
                        }
                    }
                    // populate listbox
                    if (found == false)
                        lstMaps.Items.Add(itemMap);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // remove map from listbox
            if (lstMaps.Items.Count > 0)
            {
                foreach (ListViewItem item in lstMaps.SelectedItems)
                {
                    lstMaps.Items.Remove(item);
                }
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (txtCmdTimeout.Text.Length < 1
                || cbxDbCatalog.Text.Length == -1
                || cbxDbServer.Text.Length == -1
                || cbxEngine.Text.Length == -1
                || cbxX2Catalog.Text.Length == -1
                || cbxX2Server.Text.Length == -1
                || txtConnectionTimeout.Text.Length < 1
                || txtDbPassword.Text.Length < 1
                || txtDbUser.Text.Length < 1
                || txtPort.Text.Length < 1
                || txtX2Password.Text.Length < 1
                || txtX2User.Text.Length < 1)
            {
                MessageBox.Show("Fields in Engine, Connection, and Timeout sections are all mandatory", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _cancelClose = true;
                return;
            }

            if (chkCurrentMap.Checked)
            {
                if (String.IsNullOrEmpty(_currentMapName) || String.Compare(_currentMapName, "m_view", true) == 0)
                {
                    MessageBox.Show("Please load map before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _cancelClose = true;
                    return;
                }
            }
            else
            {
                if (lstMaps.Items.Count < 1)
                {
                    MessageBox.Show("Please add the maps to be published before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _cancelClose = true;
                    return;
                }
            }

            if (chkBackupMaps.Checked && txtBackupFolder.Text.Length < 1)
            {
                MessageBox.Show("Please specify a backup folder before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _cancelClose = true;
                return;
            }
            SaveSettings();

            btnPublish.DialogResult = DialogResult.OK;
        }

        private void SaveSettings()
        {
            string maps = string.Empty;
            foreach (ListViewItem i in lstMaps.Items)
            {
                if (i.Text.ToString().Length > 0)
                    maps += "," + i.Text.ToString();
            }
            if (maps.Length > 0)
            {
                maps = maps.Remove(0, 1);
            }
            MainForm.App.cfg.SaveSetting("MapList", maps);

            MainForm.App.cfg.SaveSetting("X2Engine", cbxEngine.Text);
            MainForm.App.cfg.SaveSetting("X2Port", txtPort.Text);
            MainForm.App.cfg.SaveSetting("X2Server", cbxX2Server.Text);
            MainForm.App.cfg.SaveSetting("X2Username", txtX2User.Text);
            MainForm.App.cfg.SaveSetting("X2Password", txtX2Password.Text);
            MainForm.App.cfg.SaveSetting("X2Authentication", radX2Sql.Checked ? "sql" : "windows");
            MainForm.App.cfg.SaveSetting("X2Catalog", cbxX2Catalog.Text);

            MainForm.App.cfg.SaveSetting("DbServer", cbxDbServer.Text);
            MainForm.App.cfg.SaveSetting("DbUsername", txtDbUser.Text);
            MainForm.App.cfg.SaveSetting("DbPassword", txtDbPassword.Text);
            MainForm.App.cfg.SaveSetting("DbAuthentication", radDbSql.Checked ? "sql" : "windows");
            MainForm.App.cfg.SaveSetting("DbCatalog", cbxDbCatalog.Text);

            MainForm.App.cfg.SaveSetting("ConnectionTimeout", txtConnectionTimeout.Text);
            MainForm.App.cfg.SaveSetting("CommandTimeout", txtCmdTimeout.Text);

            MainForm.App.cfg.SaveSetting("AutomaticMapBackup", chkBackupMaps.Checked.ToString());

            MainForm.App.cfg.SaveSetting("AutomaticBackupFolder", txtBackupFolder.Text);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (dlgBackupFolder.ShowDialog() == DialogResult.OK)
            {
                txtBackupFolder.Text = dlgBackupFolder.SelectedPath;
            }
        }

        private void chkBackupMaps_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBackupMaps.Checked)
            {
                _backupMaps = true;
                txtBackupFolder.Enabled = true;
                btnBrowse.Enabled = true;
                lblBackup.Enabled = true;
            }
            else
            {
                _backupMaps = false;
                txtBackupFolder.Enabled = false;
                btnBrowse.Enabled = false;
                lblBackup.Enabled = false;
            }
        }

        private void chkCurrentMap_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCurrentMap.Checked)
            {
                _batchMode = false;
                lblMapsToBePublished.Enabled = false;
                lstMaps.Enabled = false;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                btnChangeVersion.Enabled = false;
            }
            else
            {
                _batchMode = true;
                lblMapsToBePublished.Enabled = true;
                lstMaps.Enabled = true;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                btnChangeVersion.Enabled = true;
            }
        }

        private void btnChangeVersion_Click(object sender, EventArgs e)
        {
            ListViewItem[] items = new ListViewItem[lstMaps.SelectedItems.Count];
            lstMaps.SelectedItems.CopyTo(items, 0);

            if (items.Length > 0)
            {
                frmMapVersion frmVer = new frmMapVersion();
                frmVer.lblMapName.Text = "";
                for (int i = 0; i < lstMaps.SelectedIndices.Count; i++)
                {
                    frmVer.lblMapName.Text += items[i].SubItems[0].Text + Environment.NewLine;
                    frmVer.Height += 10;
                }
                if (items[0].SubItems.Count > 1)
                    frmVer.lblCurrentVersion.Text = items[0].SubItems[1].Text;
                frmVer.ShowDialog();
                if (frmVer.DialogResult == DialogResult.OK)
                {
                    //change map version
                    for (int i = 0; i < items.Length; i++)
                    {
                        //string xmlFile = XMLHandling.GetXMLFile(items[i].SubItems[0].Text);
                        string xmlFile = items[i].SubItems[0].Text;
                        MainForm.App.documentIsBeingOpened = true;
                        if (MainForm.App.GetCurrentView() != null)
                        {
                        }
                        //           Cursor = Cursors.WaitCursor;
                        if (frmVer.txtNewVersion.Text.Length < 1)
                            XMLHandling.SetMapVersion(items[i].SubItems[0].Text, xmlFile, "1");
                        else
                            XMLHandling.SetMapVersion(items[i].SubItems[0].Text, xmlFile, frmVer.txtNewVersion.Text);
                    }
                    for (int i = 0; i < items.Length; i++)
                    {
                        string holdFilename = items[i].SubItems[0].Text;
                        lstMaps.Items.RemoveAt(lstMaps.SelectedIndices[0]);
                        lstMaps.Items.Add(new ListViewItem(new string[] { holdFilename, XMLHandling.GetMapVersion(holdFilename, holdFilename) }));
                    }
                }
            }
        }

        private void txtX2User_KeyUp(object sender, KeyEventArgs e)
        {
            txtDbUser.Text = txtX2User.Text;
        }

        private void txtX2Password_KeyUp(object sender, KeyEventArgs e)
        {
            txtDbPassword.Text = txtX2Password.Text;
        }
    }

    public class MapListItem
    {
        public string MapName;
        public string Version;
    }
}