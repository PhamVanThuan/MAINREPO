using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;

namespace SAHL.Websites.Halo.Shared
{
    public abstract class UnitOfWorkActionBase : IUnitOfWorkAction
    {
        public IPrincipal CurrentUser { get; set; }
        public int Sequence { get; protected set; }

        public abstract bool Execute();
    }
}
