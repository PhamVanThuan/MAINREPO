using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace SAHLDomainService
{
    public partial class SAHLDomainServiceClass : ServiceBase
    {
        DomainService.X2DomainService svc = null;
        public SAHLDomainServiceClass()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                svc = new DomainService.X2DomainService();
                svc.OnStart(null);
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }

        protected override void OnStop()
        {
            try
            {
                svc.OnStop();
            }
            catch(Exception ex)
            {
                string s = ex.ToString();
            }
        }
    }
}
