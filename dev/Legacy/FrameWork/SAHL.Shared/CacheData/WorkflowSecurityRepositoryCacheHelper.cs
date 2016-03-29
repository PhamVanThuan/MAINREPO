using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.CacheData
{
    public class WorkflowSecurityRepositoryCacheHelper
    {
        private readonly object offerRoleLock = new object();
        private readonly object workflowRoleLock = new object();

        private bool offerRoleCached;
        private bool workflowRoleCached;

        # region Static Visibility stuff

        static WorkflowSecurityRepositoryCacheHelper instance;
        static readonly object instanceLock = new object();

        public static WorkflowSecurityRepositoryCacheHelper Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new WorkflowSecurityRepositoryCacheHelper();
                    }
                    return instance;
                }
            }
            set
            {
                lock (instanceLock)
                {
                    instance = value;
                }
            }
        }

        #endregion

        public bool IsOfferRoleCached
        {
            get
            {
                lock (this.offerRoleLock)
                {
                    return this.offerRoleCached;
                }
            }
            set
            {
                lock (this.offerRoleLock)
                {
                    this.offerRoleCached = value;
                }
            }
        }

        public bool IsWorkflowRoleCached
        {
            get
            {
                lock (this.workflowRoleLock)
                {
                    return this.workflowRoleCached;
                }
            }
            set
            {
                lock (this.workflowRoleLock)
                {
                    this.workflowRoleCached = value;
                }
            }
        }

        public void ClearCache()
        {
            this.IsOfferRoleCached = false;
            this.IsWorkflowRoleCached = false;
        }
    }
}