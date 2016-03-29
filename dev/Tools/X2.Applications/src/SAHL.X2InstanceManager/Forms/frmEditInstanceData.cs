using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using SAHL.X2InstanceManager.Items;

namespace SAHL.X2InstanceManager.Forms
{
    
    public partial class frmEditInstanceData  : Form
    {
        public long InstanceID;
        private string m_WorkFlowName;
        private string m_ConnectionString;
        private List<ColumnItem> lstColumns = new List<ColumnItem>();
        public frmEditInstanceData(string workFlowName,string connectionString)
        {
            MainForm.SetThreadPrincipal("X2");
            m_WorkFlowName = workFlowName.Replace(' ','_');
            m_ConnectionString = connectionString;
            InitializeComponent();
        }

        private void frmEditInstanceData_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = null;
            SqlConnection Connection = new SqlConnection(m_ConnectionString);

            Connection.Open();
            cmd = Connection.CreateCommand();

            string mQuery = "select Column_Name,Data_Type,Character_Maximum_Length from INFORMATION_SCHEMA.columns where table_name = '" + m_WorkFlowName + "' and TABLE_SCHEMA = 'X2DATA'";
            cmd.CommandText = mQuery;

            SqlDataReader mReader = cmd.ExecuteReader();
            while (mReader.Read())
            {
                ColumnItem mItem = new ColumnItem();
                mItem.ColumnName = mReader[0].ToString();
                mItem.ColumnType = mReader[1].ToString();
                if (mReader[2].ToString().Length > 0)
                {
                    mItem.MaxLength = int.Parse(mReader[2].ToString());
                }
                else
                {
                    mItem.MaxLength = 0;
                }
                lstColumns.Add(mItem);
            }
            mReader.Close();
            Panel mControl = new Panel();
            mControl.Left = 20;
            mControl.Top = 5;
            mControl.Width = 475;
            mControl.Height = 200;

            mControl.AutoScroll = true;

            this.Controls.Add(mControl);

            int lastTop = -10;

            for (int x = 0; x < lstColumns.Count; x++)
            {
                Label mLabel = new Label();
                mLabel.AutoSize = true;
                mLabel.Location = new Point(20, lastTop + 30);
                mLabel.Text = lstColumns[x].ColumnName + " :";
                mControl.Controls.Add(mLabel);
                if (x == 0)
                {
                    TextBox mIDBox = new TextBox();
                    mIDBox.ReadOnly = true;
                    mIDBox.Width = 75;
                    mIDBox.Location = new Point(200, lastTop + 30);
                    mControl.Controls.Add(mIDBox);
                    lastTop = lastTop + 30;
                    continue;
                }
                MaskedTextBox mTextBox = null;
                DateTimePicker mDateTimePicker = null;
                if (lstColumns[x].ColumnType != "datetime")
                {
                    mTextBox = new MaskedTextBox();
                    mTextBox.Width = 200;
                    mTextBox.Location = new Point(200, lastTop + 30);
                    mTextBox.HidePromptOnLeave = true;
                    mControl.Controls.Add(mTextBox);
                }
                else
                {
                    mDateTimePicker = new DateTimePicker();
                    mDateTimePicker.Width = 200;
                    mDateTimePicker.Location = new Point(200, lastTop + 30);
                    mDateTimePicker.Format = DateTimePickerFormat.Custom;
                    mDateTimePicker.CustomFormat = "dd/MM/yyyy hh:mm:ss tt";

                    mControl.Controls.Add(mDateTimePicker);
                }
                switch (lstColumns[x].ColumnType)
                {
                    case "bigint":
                        {

                            mTextBox.Mask = "0999999999999999999";
                            break;
                        }
                    case "varchar":
                        {
                            string mask = "";
                            for (int z = 0; z < lstColumns[x].MaxLength; z++)
                            {
                                mask += "C";
                            }
                            mTextBox.Mask = mask;
                            break;
                        }

                    case "int":
                        {
                            mTextBox.Mask = "09999999999999999999";
                            break;
                        }
                    case "bit":
                        {
                            mTextBox.Mask = "0";
                            break;
                        }
                    case "float":
                        {
                            mTextBox.Mask = "09999999999999999999";
                            break;
                        }
                }
                lastTop = lastTop + 30;
            }
            cmd.CommandText = "Select * from [X2Data]." + Misc.Misc.FixName(m_WorkFlowName) + " where InstanceID = @InstanceID";
            cmd.Parameters.Add("@InstanceID", SqlDbType.BigInt);
            cmd.Parameters[0].Value = InstanceID;
            mReader = cmd.ExecuteReader();
            int controlNo = 1;
            while (mReader.Read())
            {
                for (int y = 0; y < lstColumns.Count; y++)
                {
                    if (this.Controls[2].Controls[controlNo] is MaskedTextBox)
                    {
                        MaskedTextBox mBox = this.Controls[2].Controls[controlNo] as MaskedTextBox;
                        mBox.Text = mReader[y].ToString();
                    }
                    else if (this.Controls[2].Controls[controlNo] is DateTimePicker)
                    {
                        DateTimePicker mDateTimePick = this.Controls[2].Controls[controlNo] as DateTimePicker;


                        if (mReader[y] != DBNull.Value)
                        {
                            DateTime mTime = Convert.ToDateTime(mReader[y]);
                            mDateTimePick.Value = mTime;
                        }
                        else
                        {
                            mDateTimePick.Value = mDateTimePick.MinDate;
                        }
                    }
                    else if (this.Controls[2].Controls[controlNo] is TextBox)
                    {
                        TextBox mBox = this.Controls[2].Controls[controlNo] as TextBox;
                        mBox.Text = mReader[y].ToString();
                    }
                    controlNo += 2;
                }
            }
            mReader.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm update of instance data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                btnOK.DialogResult = DialogResult.None;
                return;
            }
            SqlCommand cmd = null;
            SqlConnection Connection = new SqlConnection(m_ConnectionString);
            try
            {
                Connection.Open();
                cmd = Connection.CreateCommand();
                StringBuilder mStr = new StringBuilder();
                mStr.Append("update [X2Data]." + m_WorkFlowName + " set ");
                for (int x = 1; x < lstColumns.Count; x++)
                {
                    mStr.Append(lstColumns[x].ColumnName + " = @" + lstColumns[x].ColumnName + ", ");
                }
                mStr.Remove(mStr.Length - 2, 2);
                mStr.Append(" where InstanceID = @InstanceID");
                cmd.CommandText = mStr.ToString();
                int controlNo = 1;
                for (int x = 0; x < lstColumns.Count; x++)
                {
                    SqlParameter mParam = new SqlParameter("@" + lstColumns[x].ColumnName, GetSQLDBType(lstColumns[x].ColumnType));

                    if (this.Controls[2].Controls[controlNo] is MaskedTextBox)
                    {
                        MaskedTextBox mBox = this.Controls[2].Controls[controlNo] as MaskedTextBox;
                        mParam.Value = mBox.Text;
                    }
                    else if (this.Controls[2].Controls[controlNo] is DateTimePicker)
                    {
                        DateTimePicker mDateTimePick = this.Controls[2].Controls[controlNo] as DateTimePicker;
                        
                        if (mDateTimePick.Value != mDateTimePick.MinDate)
                        {
                            mParam.Value = mDateTimePick.Value;
                        }
                        else
                        {
                            mParam.Value = DBNull.Value;
                        }

                    }
                    else if (this.Controls[2].Controls[controlNo] is TextBox)
                    {
                        TextBox mBox = this.Controls[2].Controls[controlNo] as TextBox;
                        mParam.Value = mBox.Text;
                    }
                    cmd.Parameters.Add(mParam);
                    controlNo += 2;
                }
                cmd.ExecuteNonQuery();
                this.DialogResult = DialogResult.OK;
                Close();

            }
            catch (Exception errExcept)
            {
                MessageBox.Show(errExcept.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Connection.Close();
                Connection.Dispose();
                cmd.Dispose();
            }
        }

        private SqlDbType GetSQLDBType(string dbType)
        {
            switch (dbType)
            {
                case "bigint":
                    {
                        return SqlDbType.BigInt;
                    }
                case "varchar":
                    {
                        return SqlDbType.VarChar;
                    }

                case "int":
                    {
                        return SqlDbType.Int;
                    }
                case "datetime":
                    {
                        return SqlDbType.DateTime;
                    }
                case "bit":
                    {
                        return SqlDbType.Bit;
                    }
                case "float":
                    {
                        return SqlDbType.Real;
                    }

            }
            return SqlDbType.Variant;
        }

    }

    public class ColumnItem
    {
        private string m_ColumnName;
        private string m_ColumnType;
        private int m_MaxLength;

        public string ColumnName
        {
            get
            {
                return m_ColumnName;
            }
            set
            {
                m_ColumnName = value;
            }
        }

        public string ColumnType
        {
            get
            {
                return m_ColumnType;
            }
            set
            {
                m_ColumnType = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return m_MaxLength;
            }
            set
            {
                m_MaxLength = value;
            }
        }
    }
}