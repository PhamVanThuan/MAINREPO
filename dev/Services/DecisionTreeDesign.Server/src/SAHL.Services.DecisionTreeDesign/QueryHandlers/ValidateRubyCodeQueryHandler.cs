using IronRuby.Builtins;
using IronRuby.Compiler;
using IronRuby.Compiler.Ast;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using Newtonsoft.Json;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared;
using SAHL.Services.Interfaces.DecisionTreeDesign.Models;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using SAHL.Services.DecisionTreeDesign.Templates;
using System.CodeDom.Compiler;

namespace SAHL.Services.DecisionTreeDesign.QueryHandlers
{
    

    public class ValidateRubyCodeQueryHandler : IServiceQueryHandler<ValidateRubyCodeQuery>
    {
       
        public Core.SystemMessages.ISystemMessageCollection HandleQuery(ValidateRubyCodeQuery query)
        {
            ValidateRubyCodeQueryResult vResult = new ValidateRubyCodeQueryResult();
           
            SystemMessageCollection messages = new SystemMessageCollection();
            string resultMessage="";
            string resultWord="";
            bool resultValid=true;
            //public VariableObject(string rawJson, string defaultNameSpace, string version, string rawEnumJson, string rawEnumJsonVersion)
            SAHL.Services.DecisionTreeDesign.Templates.VariableObject varTest1 = new Templates.VariableObject(query.GlobalVariables, "SAHL.DecisionTree.Shared", "1", query.Enums, "1");
            varTest1.ParseJson();
            string VarFileString = varTest1.TransformText();

            SAHL.Services.DecisionTreeDesign.Templates.EnumObject enumTest1 = new Templates.EnumObject(query.Enums, "SAHL.DecisionTree.Shared", "1");
            enumTest1.ParseJson();
            string EnumFileString = enumTest1.TransformText();



            var compiler = CodeDomProvider.CreateProvider("CSharp");
            var parms = new System.CodeDom.Compiler.CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false
            };
            var results = compiler.CompileAssemblyFromSource(parms, new string[] { EnumFileString, VarFileString });
            var Variables = results.CompiledAssembly.CreateInstance("SAHL.DecisionTree.Shared.Globals.Variables");
            var Enums = results.CompiledAssembly.CreateInstance("SAHL.DecisionTree.Shared.Globals.Enumerations");
            

            //dynamic queryVariables = JsonConvert.DeserializeObject<dynamic>(query.InputVariables);

            
            //variablesObj.inputs = new ExpandoObject();
            //variablesObj.outputs = new ExpandoObject();
            //SAHL.Tools.ObjectFromJsonGenerator.Lib.Templates.VariableObject varObj= new Tools.ObjectFromJsonGenerator.Lib.Templates.VariableObject(query.GlobalVariables, "SAHL.Services.DecisionTreeDesign.QueryHandlers", query.Enums);
            //varObj.ParseJson();
            //string outfile=varObj.TransformText();
            
            try
            {
                var runtime = IronRuby.Ruby.CreateRuntime();
                var engine = runtime.GetEngine("rb");
                engine.Runtime.LoadAssembly(results.CompiledAssembly);
                engine.Runtime.Globals.SetVariable("Variables", Variables);
                //engine.Runtime.Globals.SetVariable("Enumerations", Enums);
                query.RubyCode = "require '" + results.CompiledAssembly.Location + "'" + "\n" + "include SAHL::DecisionTree::Shared::Globals" + "\n" + query.RubyCode;
                var src = engine.CreateScriptSourceFromString(query.RubyCode);
                var srcUnit = HostingHelpers.GetSourceUnit(src);
                var parser = new Parser();
                var srcTreeUnit = parser.Parse(srcUnit, new RubyCompilerOptions(), ErrorSink.Default);
                //check for errors in parser
                //var rec = parser.ErrorRecovery();
                src.Execute();
            }
            catch (Exception ex)
            {
                string exString=ex.Message.ToString();
                resultValid=false;
                resultMessage = exString.ToString();
                int innerQuote = exString.IndexOf("`");
                int outerQuote = exString.IndexOf("'", innerQuote + 1);
                if (innerQuote>=0&&outerQuote>0) {
                    resultWord = exString.Substring(innerQuote + 1, outerQuote - innerQuote - 1);
                }

                if (resultMessage.Contains("undefined method")) //check if variable is undefined, or just read only
                {

                }

            }
            vResult.Message = string.IsNullOrEmpty(resultMessage)?vResult.Message:resultMessage;
            vResult.Valid = resultValid & vResult.Valid;
            vResult.ErrorString = resultWord;
            query.Result = new ServiceQueryResult<ValidateRubyCodeQueryResult>(new ValidateRubyCodeQueryResult[] { vResult });
            return messages;
        }

        public static string getTestCode() {
            return @"

namespace SAHL.DecisionTree.Shared
{
	public class Variables
	{
		public Globals globals = new Globals();
		public Inputs inputs = new Inputs();
        public Outputs outputs = new Outputs();
	}
	
	public class Inputs 
	{
        public int newvar1 { get; private set; }
	}

    public class Outputs
    {
        public int newvar2 { private get; set; }
    }

	public class Globals
	{
		public Credit credit = new Credit();
		public LossControl lossControl = new LossControl();
		public TestGroup1 testGroup1 = new TestGroup1();
	}
	public class Credit
	{
		public FurtherLending furtherLending = new FurtherLending();
		public class FurtherLending
		{
		}
	}
	public class LossControl
	{
		public float MaxAttorneyFees { get; set; } 
		public DebtCounselling debtCounselling = new DebtCounselling();
		public Litigation litigation = new Litigation();
		public class DebtCounselling
		{
			public int MaxCollections { get { return 1;} } 
		}
		public class Litigation
		{
			public string DefaultMagistratesCourt { get { return null;} } 
		}
	}
	public class TestGroup1
	{
		public bool TestVar1 { get { return true;} } 
		public TestSubGroup1 testSubGroup1 = new TestSubGroup1();
		public class TestSubGroup1
		{
		}
	}
}";

    }
        private List<VariableSpec> GetUnSpecifiedVariablesUsedInRubyCode(dynamic queryVariables, dynamic variablesObj)
        {
            List<VariableSpec> unspecifiedVariables = new List<VariableSpec>();

            List<VariableSpec> inputVariables = GetInputVariablesList(queryVariables);
            List<VariableSpec> outputVariables = GetOutputVariablesList(queryVariables);
            var inputsDictionary = (IDictionary<string, object>)variablesObj.inputs;
            foreach (var key in inputsDictionary.Keys)
            {
                if (!inputVariables.Any(v => v.VariableName.Equals(key)))
                {
                    unspecifiedVariables.Add(new VariableSpec(key, "input"));
                }
            }

            var outputsDictionary = (IDictionary<string, object>)variablesObj.outputs;
            foreach (var key in outputsDictionary.Keys)
            {
                if (!outputVariables.Any(v => v.VariableName.Equals(key)))
                {
                    unspecifiedVariables.Add(new VariableSpec(key, "output"));
                }
            }

            return unspecifiedVariables;
        }

        private List<VariableSpec> GetInputVariablesList(dynamic queryVariables)
        {
            List<VariableSpec> variablesList = new List<VariableSpec>();
            foreach (var input in queryVariables.inputs)
            {
                foreach (var member in input.classMembers)
                {
                    variablesList.Add(new VariableSpec(Convert.ToString(member.memberName), "input"));
                }
            }

            
            return variablesList;
        }

        private List<VariableSpec> GetOutputVariablesList(dynamic queryVariables)
        {
            List<VariableSpec> variablesList = new List<VariableSpec>();
            

            foreach (var output in queryVariables.outputs)
            {
                foreach (var member in output.classMembers)
                {
                    variablesList.Add(new VariableSpec(Convert.ToString(member.memberName), "output"));
                }
            }
            return variablesList;
        }

        private string GetMissingVariablesWarningMessage(IEnumerable<string> requiredInputs, IEnumerable<string> requiredOutputs)
        {
            string message = string.Empty;
            if (requiredInputs.Count() > 0 && requiredOutputs.Count() > 0)
            {
                var inputsString = string.Join(",", requiredInputs.ToArray());
                var outputsString = string.Join(",", requiredOutputs.ToArray());
                message = string.Format("You should add the following input variables {0} and the following output variables {1}", inputsString, outputsString);
            }
            else if (requiredInputs.Count() > 0 && requiredOutputs.Count() == 0)
            {
                var inputsString = string.Join(",", requiredInputs.ToArray());
                message = string.Format("You should add the following input variables {0}.", inputsString);
            }
            else if (requiredOutputs.Count() > 0 && requiredInputs.Count() == 0)
            {
                var outputsString = string.Join(",", requiredOutputs.ToArray());
                message = string.Format("You should add the following output variables {0}.", outputsString);
            }
            return message;
        }
    }

    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }

    public static class DynamicExtensionsLocal
    {
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, null);

            IDictionary<string, object> expando2 = new ExpandoObject();
            expando.Add("group", expando2);
            return expando as ExpandoObject;
        }
    }

    public class VariableSpec
    {
        public string Usage { get; protected set; }

        public string VariableName { get; protected set; }

        public VariableSpec(string name, string usage)
        {
            this.VariableName = name;
            this.Usage = usage;
        }
    }

}