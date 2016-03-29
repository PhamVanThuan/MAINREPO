using SAHL.Tools.DomainServiceDocumenter.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.TypeScanners
{
    public class EnumerationTypeScanner : ITypeScanner
    {
        private List<EnumerationModel> enumerations;
        public EnumerationTypeScanner(List<EnumerationModel> enumerations)
        {
            this.enumerations = enumerations;
        }

        public bool ProcessTypeDefinition(Mono.Cecil.TypeDefinition typeToProcess)
        {
            if (typeToProcess.IsInterface == false && typeToProcess.IsEnum == true)
            {
                EnumerationModel enumeration = new EnumerationModel();
                enumeration.Name = typeToProcess.Name;
                enumeration.FullType = typeToProcess.FullName;
                foreach(var field in typeToProcess.Fields)
                {
                    if(!field.Attributes.HasFlag(Mono.Cecil.FieldAttributes.RTSpecialName))
                    {
                        enumeration.Values.Add(new EnumValue() { Name = field.Name, Value = (int)field.Constant });
                    }
                     
                }

                this.enumerations.Add(enumeration);
                return true;
            }

            return false;
        }
    }
}
