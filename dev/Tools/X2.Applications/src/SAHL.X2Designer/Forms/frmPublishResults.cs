using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmPublishResults : Form
    {
        private List<PublisherResults> _publisherResults;

        private string _environment;

        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

        private string _x2EngineServer;

        public string X2EngineServer
        {
            get { return _x2EngineServer; }
            set { _x2EngineServer = value; }
        }

        private string _x2DatabaseServer;

        public string x2DatabaseServer
        {
            get { return _x2DatabaseServer; }
            set { _x2DatabaseServer = value; }
        }

        public frmPublishResults(List<PublisherResults> publisherResults)
        {
            InitializeComponent();

            _publisherResults = publisherResults;
        }

        private void frmPublishResults_Load(object sender, EventArgs e)
        {
            //set the datasource for the grid
            gvResults.DataSource = _publisherResults;

            // setup general grid properties
            gvResults.RowHeadersVisible = false;
            gvResults.ReadOnly = true;
            //gvResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            gvResults.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            // setup gridview columns
            gvResults.Columns["Status"].Visible = false;

            gvResults.Columns["MapName"].Visible = true;
            gvResults.Columns["MapName"].Width = 200;
            gvResults.Columns["MapName"].HeaderText = "Map";

            gvResults.Columns["Version"].Visible = true;
            gvResults.Columns["Version"].Width = 100;

            gvResults.Columns["Message"].Visible = true;
            gvResults.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //setup image colum
            //Bitmap bmp = Properties.Resources.status_success;
            DataGridViewImageColumn resultColumn = new DataGridViewImageColumn();
            resultColumn.Width = 50;
            //resultColumn.Image = bmp;
            resultColumn.Name = "Result";
            resultColumn.HeaderText = "Result";
            gvResults.Columns.Insert(0, resultColumn);

            //setup the form header fields
            lblEnvironment.Text = _environment;
            lblX2EngineServer.Text = _x2EngineServer;
            lblX2DatabaseServer.Text = _x2DatabaseServer;

            //loop thru each row and set the image
            for (int i = 0; i < gvResults.RowCount; i++)
            {
                SetRowStatus(i, (PublishingStatus)gvResults["Status", i].Value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetRowStatus(int iRow, PublishingStatus publishingStatus)
        {
            Bitmap bmp = null;
            Color colour = Color.White;
            string toolTip = "";

            switch (publishingStatus)
            {
                case PublishingStatus.Success:
                    colour = Color.FromArgb(192, 255, 192); // light green;
                    toolTip = "Success";
                    bmp = Properties.Resources.status_success;
                    break;
                case PublishingStatus.Error:
                    colour = Color.FromArgb(255, 128, 128); // light red
                    toolTip = "Error";
                    bmp = Properties.Resources.status_error;
                    break;
                case PublishingStatus.Warning:
                    colour = Color.FromArgb(244, 182, 122); // light orange
                    toolTip = "Warning";
                    bmp = Properties.Resources.status_warning;
                    break;
                default:
                    break;
            }

            // Set the Rows Status Icon
            if (bmp != null)
                gvResults[0, iRow].Value = bmp;

            // Set the row Background Colour
            gvResults.Rows[iRow].DefaultCellStyle.BackColor = colour;

            // set the Tooltip for this row on the Status Column
            gvResults[0, iRow].ToolTipText = toolTip;
        }
    }

    public class PublisherResults
    {
        private PublishingStatus _status;

        public PublishingStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _mapName;

        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }

        private string _version;

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    public enum PublishingStatus : int { Success, Error, Warning };
}