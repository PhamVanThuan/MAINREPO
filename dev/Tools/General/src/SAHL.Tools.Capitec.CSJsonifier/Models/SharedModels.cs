using SAHL.Tools.Capitec.CSJsonifier.Reflection;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Models
{
    public class SharedModels : TypeScanConventionBase,IScanResult
    {
        public bool UseContainer
        {
            get { return true; }
        }

        public string Type
        {
            get { return "SharedModels"; }
        }

        public string FakeType
        {
            get { return "sharedModels"; }
        }

        public IList<Mono.Cecil.TypeDefinition> CommandTypes
        {
            get { return base.commandTypes; }
        }

        public IList<Mono.Cecil.TypeDefinition> CommandResultTypes
        {
            get { return base.commandResultTypes; }
        }

        public void AddTypeDefinition(TypeDefinition definition)
        {
            if (!commandTypes.Contains(definition))
                commandTypes.Add(definition);
        }
    }
}
