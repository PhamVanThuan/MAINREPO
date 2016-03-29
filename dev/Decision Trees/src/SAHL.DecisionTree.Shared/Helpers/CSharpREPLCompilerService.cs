using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using SAHL.DecisionTree.Shared.Core;
using SAHL.DecisionTree.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace SAHL.DecisionTree.Shared.Helpers
{
    public class CSharpREPLCompilerService
    {
        private ScriptEngine scriptEngine;
        private Session session;
        private TreeProcessingContext.HostObject host;

        public CSharpREPLCompilerService(TreeProcessingContext.HostObject host)
        {
            this.host = host;
            List<Assembly> references = new List<Assembly>() {   typeof(System.Int32).Assembly,
                                    typeof(INotifyPropertyChanged).Assembly,
                                    typeof(IDecisionTree).Assembly,
                                    typeof(IEnumerable<>).Assembly,
                                    typeof(IQueryable).Assembly,
                                    host.GetType().Assembly,
                                    typeof(List<>).Assembly };

            List<string> namespaces = new List<string>() { "System", "System.Collections", "System.Collections.Generic", "SAHL.DecisionTree.Shared.Core", "SAHL.DecisionTree.Shared.Core.TreeProcessingContext", "SAHL.DecisionTree.Shared", "SAHL.DecisionTree.Shared.Interfaces", "SAHL.DecisionTree.Shared.Globals" };
            scriptEngine = new ScriptEngine();
            foreach (var reference in references.Distinct())
            {
                scriptEngine.AddReference(reference);
            }

            foreach (var item in namespaces.Distinct())
            {
                scriptEngine.ImportNamespace(item);
            }

            session = scriptEngine.CreateSession(host);
            
        }

        public void AddPropertyWithUnboxedValue(dynamic propertySpec)
        {
            var propValue = string.Empty;
            string propType = Convert.ToString(propertySpec.Type);
            if (propType.Equals("float", StringComparison.InvariantCultureIgnoreCase))
            {
                propValue = string.Format(new System.Globalization.CultureInfo("en-US"), "{0:F}", propertySpec.Value);
            }
            else if (propType.Equals("int", StringComparison.InvariantCultureIgnoreCase))
            {
                propValue = string.Format("{0}", propertySpec.Value);
            }
            else if (propType.Equals("bool", StringComparison.InvariantCultureIgnoreCase))
            {
                propValue = string.Format("{0}", propertySpec.Value).ToLower();
            }
            else
            {
                propValue = propertySpec.Value;
            }

            if (propType.Equals("enum"))
            {
                propValue = GetEnumPropertyValue(propValue);
                propType = "string";
            }

            string code = string.Empty;
            if (propType.Equals("string", StringComparison.InvariantCultureIgnoreCase))
            {
                code = string.Format("if(!((IDictionary<string, object>)InputsVariablesObject).ContainsKey(\"{0}\")) {{ ((IDictionary<string, object>)InputsVariablesObject).Add(\"{0}\", ({1})\"{2}\"); }} else {{ ((IDictionary<string, object>)InputsVariablesObject)[\"{0}\"] = ({1})\"{2}\"; }}", propertySpec.Name, propType, propValue);
            }
            else
            {
                code = string.Format("if(!((IDictionary<string, object>)InputsVariablesObject).ContainsKey(\"{0}\")) {{ ((IDictionary<string, object>)InputsVariablesObject).Add(\"{0}\", ({1}){2}); }} else {{ ((IDictionary<string, object>)InputsVariablesObject)[\"{0}\"] = ({1}){2}; }}", propertySpec.Name, propType, propValue);
            }
            
            try
            {
                session.Execute(code);
            }
            catch (Exception exc)
            {
                throw new Exception(code, exc);
            }
        }

        public string GetEnumPropertyValue(string propertyValue)
        {
            propertyValue = propertyValue.Replace(" ","");
            RubyEnumHelper.RubyValueObject rubyValueObject =
                new RubyEnumHelper.RubyValueObject(propertyValue);
            RubyEnumHelper rubyEnumHelper = new RubyEnumHelper(rubyValueObject, this.host.EnumerationsObject);
            rubyEnumHelper.Run();
            return rubyValueObject.EnumValue;
        }

        public void SetOutputPropertyWithUnboxedValue(dynamic propertySpec)
        {
            dynamic propValue;
            if (propertySpec.Type.Equals("bool", StringComparison.InvariantCultureIgnoreCase))
            {
                propValue = propertySpec.Value.ToString().ToLower();
            }
            else if (propertySpec.Type.Equals("float", StringComparison.InvariantCultureIgnoreCase))
            {
                NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                propValue = propertySpec.Value.ToString().Replace(nfi.NumberDecimalSeparator, ".");
            }
            else if (propertySpec.Type.Equals("double", StringComparison.InvariantCultureIgnoreCase))
            {
                NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                propValue = propertySpec.Value.ToString().Replace(nfi.NumberDecimalSeparator, ".");
            }
            else if (propertySpec.Type.Equals("enum"))
            {
                propertySpec.Type = "string";
                propValue = propertySpec.Value.Contains("::") ? GetEnumPropertyValue(propertySpec.Value) : propertySpec.Value;
            }
            else
            {
                propValue = propertySpec.Value;
            }

            string code = string.Empty;
            if (propertySpec.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase))
            {
                code = string.Format("((IDictionary<string, object>)OutputsVariablesObject)[\"{0}\"] = ({1})\"{2}\";", propertySpec.Name, propertySpec.Type, propValue);
            }
           
            else
            {
                code = string.Format("((IDictionary<string, object>)OutputsVariablesObject)[\"{0}\"] = ({1}){2};", propertySpec.Name, propertySpec.Type, propValue);
            }

            try { 
                session.Execute(code);
            }
            catch (Exception exc)
            {
                throw new Exception(code, exc);
            }
        }
    }
}