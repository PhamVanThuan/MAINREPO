using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.Publishing;

namespace SAHL.X2Designer.Forms
{
    public partial class frmMapVariables : Form
    {
        private WorkFlow m_WorkFlow;
        private SqlTransaction m_Transaction;
        private SqlConnection m_Connection;
        private ProcessDocument m_Doc;
        private List<MatchVariableItem> m_LstMatchVariable;
        private bool _cancelClose;

        public frmMapVariables(SqlTransaction Transaction, SqlConnection Connection, ProcessDocument pDocument, WorkFlow workFlow, List<MatchVariableItem> lstMatchVar)
        {
            InitializeComponent();
            m_Connection = Connection;
            m_Transaction = Transaction;
            m_Doc = pDocument;
            m_WorkFlow = workFlow;
            m_LstMatchVariable = lstMatchVar;
        }

        private void frmMapVariables_Load(object sender, EventArgs e)
        {
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colType = new ColumnHeader();
            ColumnHeader colName2 = new ColumnHeader();
            ColumnHeader colType2 = new ColumnHeader();
            ColumnHeader colName3 = new ColumnHeader();
            ColumnHeader colType3 = new ColumnHeader();
            ColumnHeader colName4 = new ColumnHeader();
            ColumnHeader colType4 = new ColumnHeader();

            colName.Text = "Variable";
            colName.Width = 100;
            colName.TextAlign = HorizontalAlignment.Left;
            colType.Text = "Type";
            colType.Width = 75;

            colName2.Text = "Variable";
            colName2.Width = 100;
            colName2.TextAlign = HorizontalAlignment.Left;
            colType2.Text = "Type";
            colType2.Width = 75;

            colName3.Text = "Variable";
            colName3.Width = 100;
            colName3.TextAlign = HorizontalAlignment.Left;
            colType3.Text = "Type";
            colType3.Width = 75;

            colName4.Text = "Variable";
            colName4.Width = 100;
            colName4.TextAlign = HorizontalAlignment.Left;
            colType4.Text = "Type";
            colType4.Width = 75;

            colType.TextAlign = HorizontalAlignment.Left;
            listViewWorkFlowVar.Columns.Add(colName);
            listViewWorkFlowVar.Columns.Add(colType);

            listViewDatabaseVar.Columns.Add(colName2);
            listViewDatabaseVar.Columns.Add(colType2);

            listViewAddVar.Columns.Add(colName3);
            listViewAddVar.Columns.Add(colType3);

            listViewDeleteVar.Columns.Add(colName4);
            listViewDeleteVar.Columns.Add(colType4);

            ColumnHeader mColName = new ColumnHeader();
            ColumnHeader mColType = new ColumnHeader();
            ColumnHeader mColName2 = new ColumnHeader();
            ColumnHeader mColType2 = new ColumnHeader();

            mColName.Text = "Variable";
            mColName.Width = 100;
            mColName.TextAlign = HorizontalAlignment.Left;
            mColType.Text = "Type";
            mColType.Width = 75;

            mColName2.Text = "Variable";
            mColName2.Width = 100;
            mColName2.TextAlign = HorizontalAlignment.Left;
            mColType2.Text = "Type";
            mColType2.Width = 75;

            listViewMap.Columns.Add(mColName);
            listViewMap.Columns.Add(mColType);
            listViewMap.Columns.Add(mColName2);
            listViewMap.Columns.Add(mColType2);

            for (int x = 0; x < m_LstMatchVariable.Count; x++)
            {
                if (m_LstMatchVariable[x].FoundMatch)
                {
                    for (int y = 0; y < m_WorkFlow.CustomVariables.Count; y++)
                    {
                        if (X2Generator.BuildItemName(m_WorkFlow.CustomVariables[y].Name) == m_LstMatchVariable[x].VariableName)
                        {
                            string[] lst = new string[4];
                            lst[0] = X2Generator.BuildItemName(m_WorkFlow.CustomVariables[y].Name);
                            lst[1] = MatchType(m_WorkFlow.CustomVariables[y].Type);
                            lst[2] = m_LstMatchVariable[x].VariableName;
                            lst[3] = m_LstMatchVariable[x].VariableType;
                            string[] newItem = new string[] { lst[0], lst[1], lst[2], lst[3] };
                            ListViewItem li = new ListViewItem(newItem);
                            listViewMap.Items.Add(li);
                        }
                    }
                }
                else
                {
                    string[] lst = new string[2];
                    lst[0] = m_LstMatchVariable[x].VariableName;
                    lst[1] = m_LstMatchVariable[x].VariableType.ToString();
                    string[] newItem = new string[] { lst[0], lst[1] };
                    ListViewItem li = new ListViewItem(newItem);
                    listViewDatabaseVar.Items.Add(li);
                }
            }
            for (int x = 0; x < m_WorkFlow.CustomVariables.Count; x++)
            {
                bool found = false;
                for (int y = 0; y < listViewMap.Items.Count; y++)
                {
                    if (listViewMap.Items[y].SubItems[0].Text == m_WorkFlow.CustomVariables[x].Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    string[] lst = new string[2];
                    lst[0] = m_WorkFlow.CustomVariables[x].Name;
                    if (m_WorkFlow.CustomVariables[x].Type == CustomVariableType.ctstring)
                    {
                        lst[1] = MatchType(m_WorkFlow.CustomVariables[x].Type) + "(" + m_WorkFlow.CustomVariables[x].Length.ToString() + ")";
                    }
                    else
                    {
                        lst[1] = MatchType(m_WorkFlow.CustomVariables[x].Type);
                    }
                    string[] newItem = new string[] { lst[0], lst[1] };
                    ListViewItem li = new ListViewItem(newItem);
                    listViewWorkFlowVar.Items.Add(li);
                }
            }
        }

        private string MatchType(CustomVariableType type)
        {
            switch (type)
            {
                case SAHL.X2Designer.Misc.CustomVariableType.ctbigstring:
                    return "text";

                case SAHL.X2Designer.Misc.CustomVariableType.ctbool:
                    return "bit";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdate:
                    return "datetime";
                case SAHL.X2Designer.Misc.CustomVariableType.ctfloat:
                    return "numeric";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdouble:
                    return "float";
                case SAHL.X2Designer.Misc.CustomVariableType.ctinteger:
                    return "int";
                case SAHL.X2Designer.Misc.CustomVariableType.ctlong:
                    return "bigint";
                case SAHL.X2Designer.Misc.CustomVariableType.ctstring:
                    return "varchar";
                default:
                    return "";
            }
        }

        private void cmdAddVar_Click(object sender, EventArgs e)
        {
            if (listViewWorkFlowVar.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want add custom variable " + listViewWorkFlowVar.SelectedItems[0].Text + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem mItem = listViewWorkFlowVar.SelectedItems[0];
                    string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                    ListViewItem addItem = new ListViewItem(mStr);
                    listViewAddVar.Items.Add(addItem);
                    listViewWorkFlowVar.Items.RemoveAt(listViewWorkFlowVar.SelectedIndices[0]);
                }
            }
            else
            {
                MessageBox.Show("Please select a variable to add", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdDeleteVar_Click(object sender, EventArgs e)
        {
            if (listViewDatabaseVar.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want remove custom variable " + listViewDatabaseVar.SelectedItems[0].Text + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem mItem = listViewDatabaseVar.SelectedItems[0];
                    string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                    ListViewItem delItem = new ListViewItem(mStr);
                    listViewDeleteVar.Items.Add(delItem);
                    listViewDatabaseVar.Items.RemoveAt(listViewDatabaseVar.SelectedIndices[0]);
                }
            }
            else
            {
                MessageBox.Show("Please select a variable to remove", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdUndoAdd_Click(object sender, EventArgs e)
        {
            if (listViewAddVar.SelectedIndices.Count != 0)
            {
                ListViewItem mItem = listViewAddVar.SelectedItems[0];
                string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                ListViewItem addItem = new ListViewItem(mStr);
                listViewWorkFlowVar.Items.Add(addItem);
                listViewAddVar.Items.RemoveAt(listViewAddVar.SelectedIndices[0]);
            }
        }

        private void cmdUndoDelete_Click(object sender, EventArgs e)
        {
            if (listViewDeleteVar.SelectedIndices.Count != 0)
            {
                ListViewItem mItem = listViewDeleteVar.SelectedItems[0];
                string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                ListViewItem delItem = new ListViewItem(mStr);
                listViewDatabaseVar.Items.Add(delItem);
                listViewDeleteVar.Items.RemoveAt(listViewDeleteVar.SelectedIndices[0]);
            }
        }

        private void cmdUndoMap_Click(object sender, EventArgs e)
        {
            if (listViewMap.SelectedIndices.Count != 0)
            {
                ListViewItem mItem = listViewMap.SelectedItems[0];
                string[] mStr = new string[2] { mItem.SubItems[0].Text, mItem.SubItems[1].Text };
                ListViewItem undoItem = new ListViewItem(mStr);
                listViewWorkFlowVar.Items.Add(undoItem);

                ListViewItem mItem2 = listViewMap.SelectedItems[0];
                string[] mStr2 = new string[2] { mItem.SubItems[2].Text, mItem.SubItems[3].Text };
                ListViewItem undoItem2 = new ListViewItem(mStr2);
                listViewDatabaseVar.Items.Add(undoItem2);

                listViewMap.Items.RemoveAt(listViewMap.SelectedIndices[0]);
            }
        }

        private void btnMap_Click(object sender, EventArgs e)
        {
            if (listViewWorkFlowVar.SelectedIndices.Count > 0 && listViewDatabaseVar.SelectedIndices.Count > 0)
            {
                if (listViewWorkFlowVar.Items[listViewWorkFlowVar.SelectedIndices[0]].SubItems[1].Text ==
                   listViewDatabaseVar.Items[listViewDatabaseVar.SelectedIndices[0]].SubItems[1].Text)
                {
                    string[] lst = new string[4];
                    lst[0] = listViewWorkFlowVar.Items[listViewWorkFlowVar.SelectedIndices[0]].SubItems[0].Text;
                    lst[1] = listViewWorkFlowVar.Items[listViewWorkFlowVar.SelectedIndices[0]].SubItems[1].Text;
                    lst[2] = listViewDatabaseVar.Items[listViewDatabaseVar.SelectedIndices[0]].SubItems[0].Text;
                    lst[3] = listViewDatabaseVar.Items[listViewDatabaseVar.SelectedIndices[0]].SubItems[1].Text;

                    string[] newItem = new string[] { lst[0], lst[1], lst[2], lst[3] };
                    ListViewItem li = new ListViewItem(newItem);
                    listViewMap.Items.Add(li);

                    listViewWorkFlowVar.Items.RemoveAt(listViewWorkFlowVar.SelectedIndices[0]);
                    listViewDatabaseVar.Items.RemoveAt(listViewDatabaseVar.SelectedIndices[0]);
                }
                else
                {
                    MessageBox.Show("In order to Map, the selected Variables types must be the same!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_cancelClose)
            {
                e.Cancel = true;
                _cancelClose = false;
            }
            base.OnClosing(e);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewWorkFlowVar.Items.Count > 0 || listViewDatabaseVar.Items.Count > 0)
                {
                    MessageBox.Show("You are required to take action on all variables before proceeding!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _cancelClose = true;
                    return;
                }
                else
                {
                    for (int x = 0; x < listViewMap.Items.Count; x++)
                    {
                        SqlCommand cmd = m_Connection.CreateCommand();
                        cmd.Transaction = m_Transaction;
                        //string str =  "declare @Query nvarchar(1000); set @Query = 'sp_rename ' @OLD_NAME, @NEW_NAME, ''COLUMN''' ; exec sp_executesql @Query";
                        string str = "exec sp_rename @OLD_NAME, @NEW_NAME, 'COLUMN' ;";
                        cmd.CommandText = str;
                        cmd.Parameters.Add("@OLD_NAME", SqlDbType.VarChar);
                        cmd.Parameters.Add("@NEW_NAME", SqlDbType.VarChar);
                        cmd.Parameters["@OLD_NAME"].Value = "[X2DATA]." + X2Generator.FixWorkFlowName(m_WorkFlow.WorkFlowName) + "." + listViewMap.Items[x].SubItems[2].Text;
                        cmd.Parameters["@NEW_NAME"].Value = listViewMap.Items[x].SubItems[0].Text;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception exc)
                        {
                            Debug.WriteLine(exc.ToString());
                            ExceptionPolicy.HandleException(exc, "X2Designer");
                        }
                    }

                    for (int x = 0; x < listViewDeleteVar.Items.Count; x++)
                    {
                        SqlCommand cmd = m_Connection.CreateCommand();
                        cmd.Transaction = m_Transaction;
                        string str = "declare @Query nvarchar(1000); set @Query = 'alter table ' + @TABLE_NAME + ' drop column [' + @COLUMN_NAME + ']'; exec sp_executesql @Query";
                        cmd.CommandText = str;
                        cmd.Parameters.Add("@TABLE_NAME", SqlDbType.VarChar);
                        cmd.Parameters.Add("@COLUMN_NAME", SqlDbType.VarChar);

                        cmd.Parameters["@TABLE_NAME"].Value = "[X2DATA]." + X2Generator.FixWorkFlowName(m_WorkFlow.WorkFlowName);
                        cmd.Parameters["@COLUMN_NAME"].Value = listViewDeleteVar.Items[x].SubItems[0].Text;
                        cmd.ExecuteNonQuery();
                    }

                    for (int x = 0; x < listViewAddVar.Items.Count; x++)
                    {
                        SqlCommand cmd = m_Connection.CreateCommand();
                        cmd.Transaction = m_Transaction;
                        string str = "declare @Query nvarchar(1000); set @Query = 'alter table ' + @TABLE_NAME + ' add [' + @COLUMN_NAME + '] ' + @COLUMN_TYPE + ' NULL'; exec sp_executesql @Query";
                        cmd.CommandText = str;
                        cmd.Parameters.Add("@TABLE_NAME", SqlDbType.VarChar);
                        cmd.Parameters.Add("@COLUMN_NAME", SqlDbType.VarChar);
                        cmd.Parameters.Add("@COLUMN_TYPE", SqlDbType.VarChar);

                        cmd.Parameters["@TABLE_NAME"].Value = "[X2DATA]." + X2Generator.FixWorkFlowName(m_WorkFlow.WorkFlowName);
                        cmd.Parameters["@COLUMN_NAME"].Value = listViewAddVar.Items[x].SubItems[0].Text;
                        cmd.Parameters["@COLUMN_TYPE"].Value = listViewAddVar.Items[x].SubItems[1].Text;
                        cmd.ExecuteNonQuery();
                    }
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                ExceptionPolicy.HandleException(ex, "X2Designer");
                throw;
            }
        }

        private void listViewDatabaseVar_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listViewWorkFlowVar_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listViewMap_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }
    }
}