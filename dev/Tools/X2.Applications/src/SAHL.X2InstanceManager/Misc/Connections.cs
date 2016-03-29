using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SAHL.X2InstanceManager.Misc
{
    public partial class Connections : Form
    {
        public string X2Conn;
        public string am2Conn;
        public string EngineURL;
        public class ConnectionObj
        {
            public string X2Conn;
            public string am2Conn;
            public string EngineURL;
            public string Name;
            public ConnectionObj(XmlNode xn)
            {
                Name = xn.Attributes["id"].Value;
                X2Conn = xn.Attributes["x2"].Value;
                am2Conn = xn.Attributes["am"].Value;
                EngineURL = xn.Attributes["X2URL"].Value;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        public Connections()
        {
            InitializeComponent();
        }

        private void Connections_Load(object sender, EventArgs e)
        {
            try
            {
                string xmlFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\Enviroments.xml";
                XmlDocument xd = new XmlDocument();
                xd.Load(xmlFile);
                XmlNodeList xnl = xd.SelectNodes("//Env");
                foreach (XmlNode xn in xnl)
                {
                    li.Items.Add(new ConnectionObj(xn));
                }
            }
            catch (XmlException xe)
            {
                MessageBox.Show(xe.ToString());
            }
        }

        private void li_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectionObj obj = li.SelectedItem as ConnectionObj;
            if (null != obj)
            {
                txt2am.Text = obj.am2Conn;
                txtX2.Text = obj.X2Conn;
                txtEngine.Text = obj.EngineURL;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.am2Conn = txt2am.Text;
            this.X2Conn = txtX2.Text;
            this.EngineURL = txtEngine.Text;
            this.Close();
        }
    }
}