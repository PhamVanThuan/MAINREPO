using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmAddEditCustomVariable : Form
    {
        public TextBox txtLength;
        private Label lblLength;
        private Label lblDataType;
        public ComboBox cbxDataType;
        public TextBox txtVariableName;
        private Label lblName;
        private Button btnOk;
        private Button btnCancel;
        private CustomVariableTypeTypeConvertor m_CustomVariable = new CustomVariableTypeTypeConvertor();

        public frmAddEditCustomVariable()
        {
            InitializeComponent();
            foreach (CustomVariableType x in m_CustomVariable.GetStandardValues())
            {
                cbxDataType.Items.Add(m_CustomVariable.ConvertTo(x, typeof(string)));
            }
            if (txtLength.Text.Length == 0)
            {
                lblLength.Enabled = false;
                txtLength.Enabled = false;
            }
        }

        private void frmAddCustomVariable_Load(object sender, EventArgs e)
        {
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (cbxDataType.Text.Length != 0 && cbxDataType.Items.Contains(cbxDataType.Text)
           && txtVariableName.Text.Length != 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Please specify a valid name and data type for the variable before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.None;
            }
        }

        private void cbxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDataType.Text.ToLower() == "string")
            {
                lblLength.Enabled = true;
                txtLength.Enabled = true;
                txtLength.Text = "255";
            }
            else
            {
                lblLength.Enabled = false;
                txtLength.Enabled = false;
                txtLength.Text = "";
            }
        }

        private void txtLength_TextChanged(object sender, EventArgs e)
        {
            if (txtLength.Text.Length > 0)
            {
                Regex regNumeric = new Regex(@"[0-9]");

                if (regNumeric.IsMatch(txtLength.Text) == false)
                {
                    MessageBox.Show("Entry must be numeric!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtLength.Text = "255";
                }
                else
                {
                    int res = Convert.ToInt32(txtLength.Text);
                    if (res > 255 || res < 1)
                    {
                        MessageBox.Show("Length must be between 1 and 255 characters!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtLength.Text = "255";
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblDataType = new System.Windows.Forms.Label();
            this.cbxDataType = new System.Windows.Forms.ComboBox();
            this.txtVariableName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // txtLength
            //
            this.txtLength.Location = new System.Drawing.Point(92, 79);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(28, 20);
            this.txtLength.TabIndex = 14;
            this.txtLength.WordWrap = false;
            this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
            //
            // lblLength
            //
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(10, 82);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(43, 13);
            this.lblLength.TabIndex = 18;
            this.lblLength.Text = "Length:";
            //
            // lblDataType
            //
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(10, 48);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(60, 13);
            this.lblDataType.TabIndex = 17;
            this.lblDataType.Text = "Data Type:";
            //
            // cbxDataType
            //
            this.cbxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDataType.FormattingEnabled = true;
            this.cbxDataType.Location = new System.Drawing.Point(92, 45);
            this.cbxDataType.Name = "cbxDataType";
            this.cbxDataType.Size = new System.Drawing.Size(218, 21);
            this.cbxDataType.TabIndex = 12;
            this.cbxDataType.SelectedIndexChanged += new System.EventHandler(this.cbxDataType_SelectedIndexChanged);
            //
            // txtVariableName
            //
            this.txtVariableName.Location = new System.Drawing.Point(92, 12);
            this.txtVariableName.Name = "txtVariableName";
            this.txtVariableName.Size = new System.Drawing.Size(218, 20);
            this.txtVariableName.TabIndex = 11;
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(10, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(79, 13);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "Variable Name:";
            //
            // btnOk
            //
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(163, 122);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.cmdOk_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(245, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // frmAddEditCustomVariable
            //
            this.ClientSize = new System.Drawing.Size(322, 147);
            this.ControlBox = false;
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.cbxDataType);
            this.Controls.Add(this.txtVariableName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAddEditCustomVariable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Variable";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddCustomVariable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}