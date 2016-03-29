using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Capitec.CSJsonifier.Templates.Shared
{
    public partial class PropertyDefinitionTemplate : ITemplate, ITemplateForModel<PropertyDefinition>
    {
        public PropertyDefinition Model { get; protected set; }

        public string Process(dynamic model)
        {
            this.Model = model;
            return this.TransformText();
        }

        //hack for STUPID IL CODE and INT32 pushing small numbers onto the stack
        public string ProcessInfo()
        {
            var instruction = this.Model.GetMethod.Body.Instructions.FirstOrDefault(x => x.Operand != null);
            if (this.Model.PropertyType.Name == "Int32")
            {
				instruction = this.Model.GetMethod.Body.Instructions.FirstOrDefault(x => x.Operand != null && x.OpCode.ToString().StartsWith("ldc"));
                return instruction != null ? instruction.Operand.ToString() : ConvertCodeToNumber(this.Model.GetMethod.Body.Instructions.First(x => x.OpCode.ToString().StartsWith("ldc.i4")).OpCode.ToString());
            }
            return TextHelper.CleanOperandText(instruction.Operand.ToString());
        }

        public string ConvertCodeToNumber(string operation)
        {
            operation = operation.Replace("ldc.i4.", "");
            operation = operation.Replace("m", "-");
            operation = operation.Replace("M", "-");
            if (operation.StartsWith("s", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("We don't know how to handle this operation type");
            }
            return operation;
        }
    }
}
