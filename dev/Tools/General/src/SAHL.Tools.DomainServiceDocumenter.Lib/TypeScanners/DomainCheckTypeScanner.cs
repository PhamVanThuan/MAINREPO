using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class DomainCheckTypeScanner : ITypeScanner
    {
        List<DomainCheckModel> checks;
        public DomainCheckTypeScanner(List<DomainCheckModel> checks)
        {
            this.checks = checks;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == true && typeToProcess.HasInterfaces == true && typeToProcess.Interfaces.Any(x => x.Name == "IDomainCommandCheck"))
            {
                DomainCheckModel check = new DomainCheckModel();
                check.Name = typeToProcess.Name;
                check.FullType = typeToProcess.FullName;
                foreach (var property in typeToProcess.Properties)
                {
                    Property prop = new Property();
                    prop.Name = property.Name;
                    prop.Type = property.PropertyType.Name;
                    prop.FullType = property.PropertyType.FullName;
                    check.Properties.Add(prop);
                }

                checks.Add(check);
                return true;
            }

            return false;
        }
    }
}
