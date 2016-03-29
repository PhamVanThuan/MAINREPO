using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DashboardViewGenerator.Conventions
{
    public class DashboardTileConvention : IScanConvention, IScanResult
    {
        public DashboardTileConvention()
        {
            this.Types = new List<TypeDefinition>();
        }


        public void ProcessType(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.Interfaces.Any(x=>x.Name.Contains("IHaloRootTileConfiguration")))
            {
                this.Types.Add(typeToProcess);
            }
        }

        public IList<TypeDefinition> Types
        {
            get;
            protected set;
        }
    }
}
