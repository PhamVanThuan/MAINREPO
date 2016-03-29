using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Mandates
{
    public abstract class BaseMandate : IMandate
    {

        #region IMandate Members
        protected IAllocationMandate Mandate;
        public virtual void CompleteMandate()
        {
            
        }

        public abstract bool ExecuteMandate(params object[] args);

        public virtual bool StartMandate(IAllocationMandate Mandate)
        {
            this.Mandate = Mandate;
            return true;
        }

        #endregion
    }
}
