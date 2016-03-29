using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Properties;

namespace SAHL.X2Designer
{
    internal class Helpers
    {
        internal class ConnectionStringClass
        {
            internal string ID;
            internal string X2;
            internal string AM2;
            internal bool AlwaysMigrate = false;

            internal ConnectionStringClass(XmlNode xn)
            {
                ID = xn.Attributes["ID"].Value;
                X2 = xn.Attributes["X2"].Value;
                AM2 = xn.Attributes["AM"].Value;
                AlwaysMigrate = Convert.ToBoolean(xn.Attributes["AlwaysMigrate"].Value);
            }

            public override string ToString()
            {
                return ID;
            }
        }

        static string _XMLPath = string.Empty;

        public static string XMLPath
        {
            get
            {
                if (String.IsNullOrEmpty(_XMLPath))
                {
                    return Path.GetDirectoryName(Application.ExecutablePath) + "\\SickOfConnectionBoxes.xml";
                }
                else
                    return _XMLPath + "\\SickOfConnectionBoxes.xml";
            }
            set
            {
                _XMLPath = value + "\\SickOfConnectionBoxes.xml";
            }
        }

        static string X2 = string.Empty;
        static string AM2 = string.Empty;
        static bool AlwaysMigrate = false;

        internal static bool Migrate { get { return AlwaysMigrate; } }

        internal static void ClearCurrent()
        {
            X2 = string.Empty;
            AM2 = string.Empty;
            AlwaysMigrate = false;
            MainForm.App.GetCurrentView().Document.BusinessStageConnectionString = string.Empty;
        }

        private static bool LoadXML(ConnectionForm CF, bool IsX2)
        {
            if (IsX2 && (!String.IsNullOrEmpty(X2)))
            {
                CF.ConnectionString = X2;
                return true;
            }
            if (!IsX2 && (!String.IsNullOrEmpty(AM2)))
            {
                CF.ConnectionString = AM2;
                return true;
            }
            if (File.Exists(_XMLPath))
            {
                try
                {
                    XmlDocument xd = new XmlDocument();
                    xd.Load(_XMLPath);
                    XmlNodeList xnl = xd.SelectNodes("//ConnectionStrings/Connection");
                    List<ConnectionStringClass> List = new List<ConnectionStringClass>();
                    foreach (XmlNode xn in xnl)
                    {
                        List.Add(new ConnectionStringClass(xn));
                    }
                    frmSelectConnStr frm = new frmSelectConnStr(List);
                    frm.ShowDialog();
                    if (null != frm.conn)
                    {
                        if (IsX2)
                        {
                            CF.ConnectionString = frm.conn.X2;
                        }
                        else
                        {
                            CF.ConnectionString = frm.conn.AM2;
                        }
                        X2 = frm.conn.X2;
                        AM2 = frm.conn.AM2;
                        AlwaysMigrate = frm.conn.AlwaysMigrate;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                }
            }
            return false;
        }

        internal static bool ShowX2ConnForm(ConnectionForm CF, bool X2)
        {
            if (LoadXML(CF, X2)) return true;
            if (X2)
            {
                CF.Text = "Connect to X2 Database";
                CF.DefaultCatalog = "X2";
            }
            else
            {
                CF.Text = "Connect to 2AM Database";
                CF.DefaultCatalog = "2AM";
            }
            if (CF.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            return true;
        }
    }

    public class DTUtils
    {
        private DataTable _DT;

        public DataTable DT
        {
            get { return (_DT); }
            set { _DT = value; }
        }

        public void AddIdentityColumn()
        {
            DataColumn dc = new DataColumn("ID", typeof(int));
            dc.AllowDBNull = false;

            DT.Columns.Add(dc);
            int n = 1;
            foreach (DataRow dr in DT.Rows)
            {
                dr["ID"] = n++;
            }

            dc.Unique = true;
        }

        public DTUtils(string TableName)
        {
            _DT = new DataTable(TableName);
            _DT.TableName = TableName;
        }

        protected void AddColumn(string colName, Type colType)
        {
            DataColumn dc = new DataColumn(colName, colType);
            DT.Columns.Add(dc);
        }

        protected void AddColumnWithExpr(string colName, Type colType, string expr)
        {
            DataColumn dc = new DataColumn(colName, colType, expr);
            DT.Columns.Add(dc);
        }

        public void AddIntColumn(string colName)
        {
            AddColumn(colName, typeof(int));
        }

        public void AddGuidColumn(string colName)
        {
            AddColumn(colName, typeof(Guid));
        }

        public void AddboolColumn(string colName)
        {
            AddColumn(colName, typeof(bool));
        }

        public void AddStringColumn(string colName)
        {
            AddColumn(colName, typeof(string));
        }

        public void AddDateTimeColumn(string colName)
        {
            AddColumn(colName, typeof(DateTime));
        }

        public void AddColumnData(params object[] list)
        {
            int n = 0;
            foreach (object o in list)
            {
                DataColumn dc = _DT.Columns[n];
                if (dc.DataType != o.GetType())
                {
                    throw new Exception("Incorrect DataType");
                }
                n++;
            }
            _DT.Rows.Add(list);
        }
    }
}