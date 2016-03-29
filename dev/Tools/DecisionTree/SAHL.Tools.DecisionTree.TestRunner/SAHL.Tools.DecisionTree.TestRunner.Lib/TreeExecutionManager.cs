using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using SAHL.Core.SystemMessages;
using SAHL.Tools.DecisionTree.TestRunner.Interfaces;
using SAHL.Tools.DecisionTree.TestRunner.Lib.Interfaces;
using System.Collections;

namespace SAHL.Tools.DecisionTree.TestRunner.Lib
{
    public class TreeExecutionManager : ITreeExecutionManager
    {
        private int variablesVersion;
        private int enumerationsVersion;
        private int messagesVersion;
        private Assembly assembly;

        public dynamic TreeVariablesObject { get; private set; }
        public dynamic Enumerations { get; private set; }
        public ISystemMessageCollection SystemMessages { get; private set; }

        public event DebugLocationChanged DebugLocationChanged;
        public event DecisionTreeExecutionStarted DecisionTreeExecutionStarted;
        public event DecisionTreeExecutionEnded DecisionTreeExecutionEnded;

        public void SetupTreeExecutionManager(string assemblyPath, ISystemMessageCollection systemMessages)
        {
            this.assembly = Assembly.LoadFile(assemblyPath);
            this.SystemMessages = systemMessages;
        }

        public void Process(string treeName, int treeVersion, List<ITestInput> testCaseInputs, List<ITestInput> scenarioInputs)
        {
            // Get latest global versions
            enumerationsVersion = GetMaxGlobalVersion("Enumerations");
            variablesVersion = GetMaxGlobalVersion("Variables");
            messagesVersion = GetMaxGlobalVersion("Messages");

            // Create globalsVersion
            Type globalsVersionType = assembly.GetType("SAHL.DecisionTree.Shared.Helpers.QueryGlobalsVersion");
            var globalsVersion = Activator.CreateInstance(globalsVersionType, new object[] { enumerationsVersion, messagesVersion, variablesVersion });

            // Create enumerations
            Type enumType = assembly.GetType("SAHL.DecisionTree.Shared.Globals.Enumerations");
            var enumerations = Activator.CreateInstance(enumType);

            this.Enumerations = enumerations;

            // Create messages
            Type messageType = assembly.GetType("SAHL.DecisionTree.Shared.Globals.Messages");
            var messages = Activator.CreateInstance(messageType, new object[] { SystemMessages });

            this.SystemMessages = (ISystemMessageCollection)messageType.GetProperty("SystemMessages").GetValue(messages);

            // Create variables
            Type globalVariablesType = assembly.GetType("SAHL.DecisionTree.Shared.Globals.Variables");
            var globalVariables = Activator.CreateInstance(globalVariablesType, new object[] { enumerations, messages });

            // Set up variable objects
            var variablesObject = GetVariablesObjectForTree(treeName, treeVersion, this.SystemMessages, assembly, globalVariables, enumerations);
            var testCaseVariables = AssignValuesToTreeInputs(variablesObject, testCaseInputs.ToList(), enumerations);
            var scenarioVariables = AssignValuesToTreeInputs(testCaseVariables, scenarioInputs.ToList(), enumerations);

            var inputs = getInputValuesForDecisionTree(Utilities.ToDynamic(scenarioVariables));

            // Create TreeExecutionManager
            Type treeExecutionManagerType = assembly.GetType("SAHL.DecisionTree.Shared.Core.TreeProcessingContext");
            var treeExecutionManager = Activator.CreateInstance(treeExecutionManagerType,
                new object[] { inputs, treeName, treeVersion.ToString(),globalsVersion, SystemMessages, true });

            EventInfo eventInfo = treeExecutionManagerType.GetEvent("DebugLocationChanged");
            Action<object, object> handler = OnDebugLocationChanged;
            Delegate convertedHandler = ConvertDelegate(handler, eventInfo.EventHandlerType);
            eventInfo.AddEventHandler(treeExecutionManager, convertedHandler);

            var treeNodes = treeExecutionManagerType.GetProperty("TreeNodes").GetValue(treeExecutionManager);
            var treeLinks = (IList)treeExecutionManagerType.GetProperty("TreeLinks").GetValue(treeExecutionManager);
            OnDecisionTreeExecutionStarted(treeName, Utilities.ToDynamic(treeNodes), treeLinks);

            treeExecutionManagerType.InvokeMember("Execute", BindingFlags.Default | BindingFlags.InvokeMethod, null, treeExecutionManager, null);
            this.TreeVariablesObject = treeExecutionManagerType.GetProperty("Variables").GetValue(treeExecutionManager, null);

            OnDecisionTreeExecutionEnded(treeName);
        }

        private void OnDebugLocationChanged(object sender, object args)
        {
            if (DebugLocationChanged != null)
            {
                var argsType = args.GetType();
                int? justExecuted = (int?)argsType.GetProperty("JustExecutedNodeId").GetValue(args);
                int? previousNode = (int?)argsType.GetProperty("PreviousNodeId").GetValue(args);
                bool? previousNodeResult = (bool?)argsType.GetProperty("PreviousNodeResult").GetValue(args);
                bool nodeResult = (bool)argsType.GetProperty("NodeResult").GetValue(args);

                DebugLocationChangedArgs debugArgs = new DebugLocationChangedArgs(justExecuted, previousNode, nodeResult, previousNodeResult);
                DebugLocationChanged(this, debugArgs);
            }
        }

        private void OnDecisionTreeExecutionStarted(string treeName, dynamic treeNodes, IList treeLinks)
        {
            if (DecisionTreeExecutionStarted != null)
            {
                Dictionary<int, Node> nodes = new Dictionary<int, Node>();
                List<Link> links = new List<Link>();
                foreach (var treeNode in treeNodes.Values)
                {
                    int id = Convert.ToInt32(treeNode.id);
                    string type = Convert.ToString(treeNode.nodeType);
                    string name = Convert.ToString(treeNode.Name);
                    var node = new Node(id, name, type);

                    nodes.Add(id, node);
                }
                foreach (var treeLink in treeLinks)
                {
                    var dynamicTreeLink = Utilities.ToDynamic(treeLink);
                    int id = Convert.ToInt32(dynamicTreeLink.ID);
                    int fromNodeID = Convert.ToInt32(dynamicTreeLink.FromNodeID);
                    int toNodeID = Convert.ToInt32(dynamicTreeLink.ToNodeID);
                    string linkType = Convert.ToString(dynamicTreeLink.Type);

                    links.Add(new Link(id, fromNodeID, toNodeID, linkType));
                }
                DecisionTreeExecutionStarted(this, new DecisionTreeExecutionStartedArgs(treeName, nodes, links));
            }
        }

        private void OnDecisionTreeExecutionEnded(string treeName)
        {
            if (DecisionTreeExecutionEnded!=null)
            {
                DecisionTreeExecutionEnded(this, new DecisionTreeExecutionEndedArgs(treeName));
            }
        }

        private Delegate ConvertDelegate(Delegate originalDelegate, Type targetDelegateType)
        {
            return Delegate.CreateDelegate(targetDelegateType, originalDelegate.Target, originalDelegate.Method);
        }

        private Dictionary<string, object> getInputValuesForDecisionTree(ExpandoObject inputValues)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            var inputs = Utilities.ToDynamic(((IDictionary<string, object>)inputValues)["inputs"]);
            foreach (var key in ((IDictionary<string, object>)inputs).Keys)
            {
                result.Add(key, ((IDictionary<string, object>)inputs)[key]);
            }
            return result;
        }

        private int GetMaxGlobalVersion(string globalName)
        {
            globalName = String.Format("{0}_", globalName);
            var globalTypes = assembly.GetTypes().Where(x=>x.Name.StartsWith(globalName));
            var maxVersion = globalTypes.Max(x=>Int32.Parse(x.Name.Replace(globalName, String.Empty)));
            return maxVersion;
        }

        private dynamic GetVariablesObjectForTree(string treeName, int version, ISystemMessageCollection systemMessageCollection, Assembly assembly, dynamic globals, dynamic enumerations)
        {
            ExpandoObject variablesObj = Utilities.ToDynamic(globals);

            string treeSpecificVariableTypeName = String.Format("SAHL.DecisionTree.Shared.{0}_{1}Variables", treeName, version);
            Type treeSpecificVariableType = assembly.GetType(treeSpecificVariableTypeName);
            var boxedTreeSpecificVariables = Activator.CreateInstance(treeSpecificVariableType, new object[] { enumerations });
            var unboxedTreeSpecificVariables = Convert.ChangeType(boxedTreeSpecificVariables, treeSpecificVariableType);
            ExpandoObject treeSpecificVariables = Utilities.ToDynamic(unboxedTreeSpecificVariables);

            ((dynamic)variablesObj).inputs = ((dynamic)treeSpecificVariables).inputs;
            ((dynamic)variablesObj).outputs = ((dynamic)treeSpecificVariables).outputs;
            ((dynamic)variablesObj).outputs.NodeResult = false;

            return variablesObj;
        }

        private dynamic AssignValuesToTreeInputs(dynamic treeVariablesObject, List<ITestInput> testInputs, dynamic enumerations)
        {
            var testCaseInputs = treeVariablesObject.inputs;
            foreach (var testInput in testInputs)
            {
                var inputValueName = Utilities.StripInvalidChars(testInput.Name);
                var inputPropertyToSet = testCaseInputs.GetType().GetProperty(inputValueName);
                var inputValueAsPropertyType = Convert.ChangeType(testInput.Value, inputPropertyToSet.PropertyType);
                if (testInput.Value.StartsWith("Enumerations::"))
                {
                    inputPropertyToSet.SetValue(testCaseInputs, Utilities.GetEnumerationValueForRubyEnumString(inputValueAsPropertyType, enumerations));
                }
                else
                {
                    inputPropertyToSet.SetValue(testCaseInputs, inputValueAsPropertyType);
                }
            }
            treeVariablesObject.inputs = testCaseInputs;
            return treeVariablesObject;
        }
    }

    public class Node
    {
        public string nodeType { get;set; }
        public int ID { get; set; }
        public string Name { get;set; }
        public Node(int id, string name, string type)
        {
            this.ID = id;
            this.Name = name;
            this.nodeType = type;
        }
    }

    public class Link
    {
        public Link(int id, int fromNodeID, int toNodeID, string linkType)
        {
            this.ID = id;
            this.FromNodeID = fromNodeID;
            this.ToNodeID = toNodeID;
            this.LinkType = linkType;
        }

        public int ID { get; set; }
        public int FromNodeID { get; set; }
        public int ToNodeID { get; set; }
        public string LinkType { get; set; }
    }
}
