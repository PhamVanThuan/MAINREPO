using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmGenericKeyType : Form
    {
        internal class GenericKeyObj
        {
            internal string Desc;
            internal int Key;

            public override string ToString()
            {
                return Desc;
            }

            internal GenericKeyObj(int Key, string Desc)
            {
                this.Key = Key;
                this.Desc = Desc;
            }
        }

        private int PrevSelectedKey;
        internal int GenericKeyTypeKey = -1;
        internal string GenericKeyType = String.Empty;

        public frmGenericKeyType(int PrevSelectedKey)
        {
            InitializeComponent();
            this.PrevSelectedKey = PrevSelectedKey;
        }

        private void frmGenericKeyType_Load(object sender, EventArgs e)
        {
            ConnectionForm CF = new ConnectionForm();
            Helpers.ShowX2ConnForm(CF, false);

            if (CF.DialogResult == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            SqlCommand cmd = null;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(CF.ConnectionString);
                conn.Open();
                cmd = new SqlCommand("select GenericKeyTypeKey, Description from [2am]..GenericKeyType");
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    GenericKeyObj g = new GenericKeyObj(Convert.ToInt32(reader[0]), reader[1].ToString());
                    lstGenericKey.Items.Add(g);
                }
                reader.Close();
                reader.Dispose();
                for (int i = 0; i < lstGenericKey.Items.Count; i++)
                {
                    if (((GenericKeyObj)lstGenericKey.Items[i]).Key == PrevSelectedKey)
                    {
                        lstGenericKey.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Unable to connect to database.{0}{1}", Environment.NewLine, ex.ToString()), "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            finally
            {
                if (null != cmd)
                {
                    cmd.Dispose();
                }
                if (null != conn)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }
        }

        private void lstGenericKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenericKeyObj g = ((GenericKeyObj)lstGenericKey.SelectedItem);
            GenericKeyType = g.Desc;
            GenericKeyTypeKey = g.Key;
        }
    }
}