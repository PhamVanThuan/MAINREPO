using System;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Win32;
using SAHL.X2.Framework;
using SAHL.X2.Framework.Interfaces;
using SAHL.X2Designer.Publishing;

namespace SAHL.X2Designer.Forms
{
    public partial class frmEngine : Form
    {
        internal static string ConnString = string.Empty;
        string RegPath = @"Software\Elzaris Technologies\X2Designer";

        public frmEngine()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            object o = RegistryHelper.GetObject(RegistryHive.CurrentUser, RegPath, "X2EngineConnection");
            if (null == o)
            {
                // create
                RegistryHelper.SetObject(RegistryHive.CurrentUser, RegPath, "X2EngineConnection", "", RegistryValueKind.String);
                o = RegistryHelper.GetObject(RegistryHive.CurrentUser, RegPath, "X2EngineConnection");
            }
            string Path = o.ToString();
            Path = Path.Replace("tcp://", "");
            Path = Path.Replace(":8089/X2Engine", "");
            ddlEngine.Text = Path;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            GenConnString();
            try
            {
                // connect to the engine
                X2EngineProvider engine = new X2EngineProvider(ConnString);
                X2ResponseBase resp = engine.LogIn();
                if (resp.IsErrorResponse)
                {
                    MessageBox.Show(((X2ErrorResponse)resp).Exception.Value, "Connected with Error Response");
                }
                else
                {
                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unable to connect to engine");
                ExceptionPolicy.HandleException(ex, "X2Designer");
            }
        }

        private void GenConnString()
        {
            ConnString = string.Format("tcp://{0}:{1}/X2Engine", ddlEngine.Text, txtPortNo.Text);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            // save
            GenConnString();
            RegistryHelper.SetObject(RegistryHive.CurrentUser, RegPath, "X2EngineConnection", ConnString, RegistryValueKind.String);
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}