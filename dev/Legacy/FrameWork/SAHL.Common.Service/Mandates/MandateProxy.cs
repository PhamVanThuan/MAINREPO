using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.Service.Mandates
{
    public class MandateProxy : MarshalByRefObject, IMandate
    {
        protected MandateRemoteDomainLoader RemotePluginLoader = null;
        IMandate i = null;
        public MandateProxy(MandateRemoteDomainLoader rl, string MandateName)
        {
            this.RemotePluginLoader = rl;
            i = RemotePluginLoader.GetRemotePlugin(MandateName);
            if (i == null)
                throw new Exception(string.Format("Unable to load Mandate:{0} from Mandate.dll", MandateName));
        }

        #region IMandate Members

        public bool StartMandate(IAllocationMandate Mandate)
        {
            return i.StartMandate(Mandate);
        }

        public bool ExecuteMandate(params object[] Parameters)
        {
            return i.ExecuteMandate(Parameters);
        }

        public void CompleteMandate()
        {
            i.CompleteMandate();
        }

        #endregion
    }
}
