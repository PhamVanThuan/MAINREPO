using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.X2.Framework.Common;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Data;

namespace SAHL.WorkflowMaps.Specs.Common
{
    public class InstanceDataStub : InstanceDataModel
    {
        public InstanceDataStub()
            :base(0,null,null,null,null,null,null,DateTime.Now,DateTime.Now,null,DateTime.Now,null,null,null,null,null) {}
    }
}