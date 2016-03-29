using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared.Helpers
{
    public class RubyEnumHelper
    {
        private ScriptScope scope;
        private RubyValueObject valueObject;
        public RubyEnumHelper(RubyValueObject valueObject, object enumerations)
        {
            ScriptRuntime runtime = IronRuby.Ruby.CreateRuntime();
            var runningScope = runtime.CreateScope("Ruby");
            runningScope.Engine.Runtime.Globals.SetVariable("ValueObject", valueObject);
            runningScope.Engine.Runtime.Globals.SetVariable("Enumerations", enumerations);
            Assembly r = enumerations.GetType().Assembly;
            runningScope.Engine.Runtime.LoadAssembly(r);
            //runningScope.Engine.Execute("include SAHL::DecisionTree::Shared::Globals", runningScope);
            this.scope = runningScope;
            this.valueObject = valueObject;
        }

        public void Run()
        {
            string code = "ValueObject.EnumValue = " + this.valueObject.EnumAccessor;
            this.scope.Engine.Execute(code, this.scope);
        }

        public class RubyValueObject
        {
            public dynamic EnumValue { get; set; }
            public string EnumAccessor { get; protected set; }
            public RubyValueObject(string enumAccessor)
            {
                this.EnumAccessor = enumAccessor;
            }
        }
    }
}
