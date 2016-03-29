//using Dapper;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers;
using Roslyn;
using System.IO;
using IronRuby;
using SAHL.Tools.ObjectFromJsonGenerator.Lib;
using SAHL.DecisionTree.Shared.Helpers;
using SAHL.DecisionTree.Shared.Interfaces;

namespace SAHL.DecisionTree.Shared.Core
{
    public class TreeProcessingContext
    {
        #region properties
        public dynamic DecisionTreeJson { get; protected set; }

        public QueryGlobalsVersion GlobalsVersions { get; protected set; }

        protected dynamic GlobalVersionsJson { get; set; }

        public Dictionary<int, Node> TreeNodes { get; protected set; }

        public List<Link> TreeLinks { get; protected set; }

        public bool Paused { get; set; }

        public dynamic Variables { get; protected set; }

        public dynamic Messages { get; protected set; }

        public dynamic Enumerations { get; protected set; }

        public Dictionary<string, ISystemMessageCollection> SubtreeMessagesDictionary { get; protected set; }

        public List<string> SubtreeMessagesToClear { get; protected set; }

        public int? CurrentNodeId { get { return justExecutedNodeId; } }

        public dynamic BreakPoints { get; set; }

        private ISystemMessageCollection systemMessageCollection;
        
        private Guid debugSessionId;

        private Node nodeToRun;

        private int? justExecutedNodeId;        

        private int? nextNodeId;

        private List<KeyValuePair<int, bool>> nodeExecutionBreadcrumb;

        private bool nodeExecutionResultedInError;

        private bool NodeExecutionResultedInError { get { return nodeExecutionResultedInError; } }

        private bool currentNodeExecutionResult;

        public bool CurrentNodeExecutionResult { get { return currentNodeExecutionResult; } }

        public bool ExecutionCompleted;

        private bool debug;

        private bool generateFromDb;

        private bool subtree;

        //public ScriptScope runningScope;

        //private ScriptEngine engine;

        private IObjectGenerator objectGenerator;

        private string DbConnectionString;

        private ISubTreeContextManager SubTreeContextManager;

        public IProcessingEngine engine;

        public IRoutingEngine RoutingEngine;

        public Dictionary<string, object> InputValues;

        #endregion properties

        //Execution constructor overload for tests
        public TreeProcessingContext(Dictionary<string, object> inputValues, string treeName, string treeVersion, dynamic globalsVersions, ISystemMessageCollection messages, string dbConnectionString)
        {
            this.InputValues = inputValues;
            this.debug = false;
            this.generateFromDb = false;
            this.GlobalsVersions = globalsVersions;            
            this.TreeNodes = new Dictionary<int, Node>();
            this.TreeLinks = new List<Link>();
            this.nodeExecutionBreadcrumb = new List<KeyValuePair<int, bool>>();
            this.systemMessageCollection = messages;
            this.SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            this.SubtreeMessagesToClear = new List<string>();
            this.objectGenerator = new ObjectGenerator();
            this.DbConnectionString = dbConnectionString;
            this.SubTreeContextManager = new SubTreeContextManager(this.GlobalsVersions, this.SubtreeMessagesDictionary);
            this.engine = new ProcessingEngine(ProcessingMode.Execution);            
            Initialize(treeName, treeVersion);
        }

        //Execution constructor
        public TreeProcessingContext(Dictionary<string, object> inputValues, string treeName, string treeVersion, dynamic globalsVersions, ISystemMessageCollection messages, bool debugGeneratedClass = false)
        {
            this.InputValues = inputValues;
            this.debug = debugGeneratedClass;
            this.generateFromDb = false;
            this.GlobalsVersions = globalsVersions;
            this.TreeNodes = new Dictionary<int, Node>();
            this.TreeLinks = new List<Link>();
            this.nodeExecutionBreadcrumb = new List<KeyValuePair<int, bool>>();
            this.systemMessageCollection = messages;
            this.SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            this.SubtreeMessagesToClear = new List<string>();
            this.objectGenerator = new ObjectGenerator();
            if (!debugGeneratedClass)
            {
                this.DbConnectionString = ConfigurationManager.ConnectionStrings["DBCONNECTION_ServiceArchitect"].ConnectionString;
            }
            this.SubTreeContextManager = new SubTreeContextManager(this.GlobalsVersions, this.SubtreeMessagesDictionary);
            this.engine = new ProcessingEngine(ProcessingMode.Execution);
            Initialize(treeName, treeVersion);
        }

        //Subtree execution constructor
        public TreeProcessingContext(string treeName, string treeVersion, dynamic globalsVersions, ISystemMessageCollection messages, bool subtree, string dbConnectionString, bool debugGeneratedClass = false)
        {
            this.debug = debugGeneratedClass;
            this.generateFromDb = false;
            this.subtree = subtree;            
            this.GlobalsVersions = globalsVersions;            
            this.TreeNodes = new Dictionary<int, Node>();
            this.TreeLinks = new List<Link>();
            this.nodeExecutionBreadcrumb = new List<KeyValuePair<int, bool>>();
            this.systemMessageCollection = messages;
            this.SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            this.SubtreeMessagesToClear = new List<string>();
            this.objectGenerator = new ObjectGenerator();
            this.DbConnectionString = dbConnectionString;
            this.SubTreeContextManager = new SubTreeContextManager(this.GlobalsVersions, this.SubtreeMessagesDictionary);
            this.engine = new ProcessingEngine(ProcessingMode.Execution);            
            Initialize(treeName, treeVersion);
        }

        public TreeProcessingContext(string treeJson, string globalsVersions, ISystemMessageCollection messages)
        {
            this.debug = true;
            this.generateFromDb = true;
            this.DecisionTreeJson = JsonConvert.DeserializeObject<dynamic>(treeJson);
            this.GlobalVersionsJson = globalsVersions;
            this.GlobalsVersions = JsonConvert.DeserializeObject<QueryGlobalsVersion>(globalsVersions);
            this.TreeNodes = new Dictionary<int, Node>();
            this.TreeLinks = new List<Link>();
            this.nodeExecutionBreadcrumb = new List<KeyValuePair<int, bool>>();
            this.systemMessageCollection = messages;
            this.SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            this.SubtreeMessagesToClear = new List<string>();
            this.objectGenerator = new ObjectGenerator();
            this.DbConnectionString = ConfigurationManager.ConnectionStrings["DBCONNECTION_ServiceArchitect"].ConnectionString;
            this.SubTreeContextManager = new SubTreeContextManager(this.GlobalVersionsJson, this.SubtreeMessagesDictionary);
            this.engine = new ProcessingEngine(ProcessingMode.Debug);            
            Initialize("","");
        }

        public TreeProcessingContext(string treeJson, string globalsVersions, ISystemMessageCollection messages, IObjectGenerator objg, string dbConnectionString, ISubTreeContextManager SubTreeContextManager)
        {
            this.debug = true;
            this.generateFromDb = true;
            this.DecisionTreeJson = JsonConvert.DeserializeObject<dynamic>(treeJson);
            this.GlobalVersionsJson = globalsVersions;
            this.GlobalsVersions = JsonConvert.DeserializeObject<QueryGlobalsVersion>(globalsVersions);
            this.TreeNodes = new Dictionary<int, Node>();
            this.TreeLinks = new List<Link>();
            this.nodeExecutionBreadcrumb = new List<KeyValuePair<int, bool>>();
            this.systemMessageCollection = messages;
            this.SubtreeMessagesDictionary = new Dictionary<string, ISystemMessageCollection>();
            this.SubtreeMessagesToClear = new List<string>();
            this.objectGenerator = objg;
            this.DbConnectionString = dbConnectionString;
            this.SubTreeContextManager = SubTreeContextManager;
            this.engine = new ProcessingEngine(ProcessingMode.Debug);            
            Initialize("","");
        }

        #region events

        public event EventHandler<DebugEventsArgs> DebugLocationChanged;

        public event EventHandler<ExecutionExceptionRaisedArgs> ExecutionExceptionRaised;

        public event EventHandler<SubtreeExecutionStatusArgs> SubtreeExecutionNotificationRaised;

        protected virtual void OnDebugLocationChanged(DebugEventsArgs e)
        {
            EventHandler<DebugEventsArgs> handler = DebugLocationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnExecutionExceptionRaised(ExecutionExceptionRaisedArgs e)
        {
            EventHandler<ExecutionExceptionRaisedArgs> handler = ExecutionExceptionRaised;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSubtreeExecutionNotificationRaised(SubtreeExecutionStatusArgs e)
        {
            EventHandler<SubtreeExecutionStatusArgs> handler = SubtreeExecutionNotificationRaised;
            if (handler != null)
            {
                if (e.ExecutionStatus == SubtreeExecutionStatus.Started)
                {
                    SubtreeExecutionStartedArgs executionStartedArgs = (SubtreeExecutionStartedArgs)e;
                    handler(this, executionStartedArgs);
                }
                else if (e.ExecutionStatus == SubtreeExecutionStatus.Completed)
                {
                    SubtreeExecutionCompletedArgs executionCompletedArgs = (SubtreeExecutionCompletedArgs)e;
                    handler(this, executionCompletedArgs);
                }
            }
        }

        private void subtree_SubtreeExecutionCompleted(object sender, SubtreeExecutionCompletedArgs e)
        {
            OnSubtreeExecutionNotificationRaised(e);
        }

        private void subtree_SubtreeExecutionStarted(object sender, SubtreeExecutionStartedArgs e)
        {
            OnSubtreeExecutionNotificationRaised(e);
        }

        private void subtreeContext_ExecutionExceptionRaised(object sender, ExecutionExceptionRaisedArgs e)
        {
            // Reaise the subtree execution error status event.
            nodeExecutionResultedInError = true;
            // Wrap subtree nodes exceptions as inner exceptions.
            string subtreeErrorMsg = string.Format("Error in subtree {0}", this.CurrentNodeId.Value);
            var parentLevelException = new Exception(subtreeErrorMsg, e.GetException());
            OnExecutionExceptionRaised(new ExecutionExceptionRaisedArgs(this.CurrentNodeId.Value, 0, "", ExceptionType.General, parentLevelException));
        }

        private void NodeExceptionRaisedEventHandler(object sender, NodeExceptionEventsArgs e)
        {
            nodeExecutionResultedInError = true;
            OnExecutionExceptionRaised(new ExecutionExceptionRaisedArgs(e.NodeId, e.LineNum, e.Source, e.ErrorType, e.NodeException));
        }

        private void RaiseOnLocationChangedEvent(Guid debugSessionId)
        {
            int previousNodeIndex = nodeExecutionBreadcrumb.Count - 2;
            if (previousNodeIndex >= 0)
            {
                OnDebugLocationChanged(new DebugEventsArgs(debugSessionId, currentNodeExecutionResult, nodeExecutionBreadcrumb[previousNodeIndex].Value, nodeExecutionBreadcrumb[previousNodeIndex].Key, this.justExecutedNodeId));
            }
        }

        #endregion events

        private void GenerateExecutionNodesAndLinks(string treeName, string treeVersion)//TODO: executable trees need a default constructor with nodes and links
        {
            var treeClassName = string.Format("{0}_{1}", treeName, treeVersion.ToString());
            // Check if tree is already cached
            // If not instantiate and add to cache
            // Get the tree from cache
            var assembly = Assembly.GetAssembly(typeof(IDecisionTree));
            var fullyQualifiedTreeClassName = string.Format("SAHL.DecisionTree.Shared.Trees.{0}", treeClassName);
            Type treeType = assembly.GetType(fullyQualifiedTreeClassName);
            var boxedTreeInstance = Activator.CreateInstance(treeType, new object[] { this.systemMessageCollection });
            var unboxedTreeInstance = Convert.ChangeType(boxedTreeInstance, treeType);
            var treeInstance = (IDecisionTree)unboxedTreeInstance;
                        
            this.TreeNodes = treeInstance.Nodes;
            this.TreeLinks = treeInstance.NodeLinks;
        }

        private void GenerateExecutionVariablesMessagesAndEnumerations(string treeName, string treeVersion)
        {
            var assembly = Assembly.GetAssembly(typeof(IDecisionTree));

            var messagesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Messages_{0}", this.GlobalsVersions.MessagesVersion);
            Type msgType = assembly.GetType(messagesTypeName);
            var boxedMessages = Activator.CreateInstance(msgType, systemMessageCollection);
            var messages = Convert.ChangeType(boxedMessages, msgType);

            this.Messages = messages;

            var enumerations = assembly.CreateInstance(string.Format("SAHL.DecisionTree.Shared.Globals.Enumerations_{0}", this.GlobalsVersions.EnumerationsVersion));

            this.Enumerations = enumerations;

            // Globals and mixin with treeSpecific
            var variablesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Variables_{0}", this.GlobalsVersions.VariablesVersion);
            Type varType = assembly.GetType(variablesTypeName);
            var boxedVariables = Activator.CreateInstance(varType, new object[] { enumerations, messages });
            var unboxedVariables = Convert.ChangeType(boxedVariables, varType);

            var variablesObj = DynamicExtensions.ToDynamic(unboxedVariables);

            string treeSpecificVariablesTypeName = string.Format("SAHL.DecisionTree.Shared.{0}_{1}Variables", treeName, treeVersion);
            Type treeSpecificVariablesType = assembly.GetType(treeSpecificVariablesTypeName);
            var boxedTreeSpecificVariables = Activator.CreateInstance(treeSpecificVariablesType, new object[] { enumerations });
            var unboxedTreeSpecificVariables = Convert.ChangeType(boxedTreeSpecificVariables, treeSpecificVariablesType);
            dynamic treeSpecificVariables = DynamicExtensions.ToDynamic(unboxedTreeSpecificVariables);
            variablesObj.inputs = treeSpecificVariables.inputs;
            variablesObj.outputs = treeSpecificVariables.outputs;
            variablesObj.outputs.NodeResult = false;

            this.Variables = variablesObj;

            if (!subtree)
            {
                AssignVariablesValuesFromQueryProperties(this.InputValues);
            }                       
        }

        private void AssignVariablesValuesFromQueryProperties(IDictionary<string, object> inputValues)
        {
            foreach (string input in inputValues.Keys)
            {
                var inputPropertyToSet = this.Variables.inputs.GetType().GetProperty(input) != null
                    ? this.Variables.inputs.GetType().GetProperty(input)
                    : this.Variables.inputs.GetType().GetProperty(string.Format("{0}_Enumeration", input));

                inputPropertyToSet.SetValue(this.Variables.inputs, inputValues[input]);
            }
        }

        private void GenerateDebugNodesAndLinks()
        {
            if (DecisionTreeJson != null)
            {
                foreach (var node in DecisionTreeJson.tree.nodes)
                {
                    string nodeName = Convert.ToString(node.name);
                    string nodeType = Convert.ToString(node.type);
                    int nodeId = Convert.ToInt32(node.id);
                    string scriptCode = Convert.ToString(node.code);

                    Node newNode = null;
                    if (nodeType.Equals("subtree", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string subtreeParentVariablemap = node.subtreeVariables.ToString().Replace("\n", " ").Replace("\r", " ");
                        string subtreeName = Convert.ToString(node.subtreeName);
                        string subtreeVersion = Convert.ToString(node.subtreeVersion);
                        var subtree = new SubTree(subtreeName, subtreeVersion, subtreeParentVariablemap, nodeId);
                        subtree.SubtreeExecutionStarted += subtree_SubtreeExecutionStarted;
                        subtree.SubtreeExecutionCompleted += subtree_SubtreeExecutionCompleted;
                        newNode = subtree;
                    }
                    else if (nodeType.Equals("clearmessages", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string subtreeName = Convert.ToString(node.subtreeName);
                        string subtreeVersion = Convert.ToString(node.subtreeVersion);
                        var clearMessages = new ClearMessages(subtreeName, subtreeVersion, nodeId, nodeName);
                        newNode = clearMessages;
                    }
                    else
                    {
                        newNode = new Node(nodeId, nodeName, (NodeType)Enum.Parse(typeof(NodeType), nodeType), scriptCode);
                    }
                    newNode.NodeExceptionRaised += NodeExceptionRaisedEventHandler;
                    TreeNodes.Add(Convert.ToInt32(node.id), newNode);
                }

                foreach (var link in DecisionTreeJson.tree.links)
                {
                    string linkType = Utilities.ToPascalCase(Convert.ToString(link.type));
                    linkType = linkType.Equals("link", StringComparison.InvariantCultureIgnoreCase) ? "Standard" : linkType;
                    TreeLinks.Add(new Link(Convert.ToInt32(link.id), Convert.ToInt32(link.fromNodeId), Convert.ToInt32(link.toNodeId), (LinkType)Enum.Parse(typeof(LinkType), linkType)));
                }
            }
        }

        private void GenerateDebugVariablesMessagesAndEnumerations()
        {
            System.Collections.Generic.Dictionary<string, string> classes = this.objectGenerator.GenerateLatestDebugClasses(this.DbConnectionString, GlobalsVersions.MessagesVersion.ToString(), GlobalsVersions.VariablesVersion.ToString(), GlobalsVersions.EnumerationsVersion.ToString());

            var code = classes["MessageSet"] + classes["EnumerationSet"] + classes["VariableSet"];

            CreateDynamicObjects(code);
        }

        private void Initialize(string treeName, string treeVersion)
        {
            if (generateFromDb)
            {
                GenerateDebugNodesAndLinks();
                this.RoutingEngine = new RoutingEngine(this.TreeLinks, this.TreeNodes);
                GenerateDebugVariablesMessagesAndEnumerations();
            }
            else
            {                 
                GenerateExecutionNodesAndLinks(treeName, treeVersion);
                this.RoutingEngine = new RoutingEngine(this.TreeLinks, this.TreeNodes);
                GenerateExecutionVariablesMessagesAndEnumerations(treeName, treeVersion);
            }
            
            engine.Execute(Utilities.rubyFunctions);
            engine.SetVariable("Variables", this.Variables);
            engine.SetVariable("Messages", this.Messages);
            engine.SetVariable("Enumerations", this.Enumerations);

            currentNodeExecutionResult = true;
            ExecutionCompleted = false;
            Paused = false;
        }        

        public int? Debug(Guid debugSessionId)
        {            
            this.debugSessionId = debugSessionId;

            Execute();

            return justExecutedNodeId;
        }

        public int? Execute()
        {
            int defaultStartNodeId = this.nodeToRun != null ? this.nodeToRun.id
                : TreeNodes.Single(n => n.Value.nodeType.Equals(NodeType.Start)).Key;

            nodeToRun = TreeNodes[defaultStartNodeId];
            Node nextNode = nodeToRun;
            nextNodeId = nodeToRun.id;

            while (!(ExecutionCompleted || NodeExecutionResultedInError || Paused))
            {
                nextNode = TreeStep(nodeToRun.id);
                this.justExecutedNodeId = nodeToRun.id;
                nodeExecutionBreadcrumb.Add(new KeyValuePair<int, bool>(nodeToRun.id, currentNodeExecutionResult));

                if (debug)
                {
                    if (BreakPoints != null)
                    {
                        if (BreakpointsContainNodeId(BreakPoints, this.justExecutedNodeId))
                        {
                            Paused = true;
                        }
                    }
                
                    RaiseOnLocationChangedEvent(debugSessionId);
                }

                nodeToRun = nextNode;
                
            }

            return justExecutedNodeId;
        }



        public Node TreeStep(int nodeToRunId) 
        {            
            Node node = this.TreeNodes[nodeToRunId];            
            node.engine = engine;

            this.RoutingEngine.CurrentNode = node;

            if (!(NodeExecutionResultedInError || ExecutionCompleted))
            {
                if (node.nodeType.Equals(NodeType.SubTree))
                {
                    #region subtree processing 
                    var currentNode = (SubTree)TreeNodes[nodeToRunId];

                    int subtreeVersion = Convert.ToInt32(currentNode.Version);

                    TreeProcessingContext subtreeContext;
                    if (generateFromDb)
                    {
                        subtreeContext = this.SubTreeContextManager.GetSubTreeDebugContext(currentNode.Name, subtreeVersion);
                        subtreeContext.ExecutionExceptionRaised += subtreeContext_ExecutionExceptionRaised;

                        object[] inputs_array = AssignSubtreeInputsFromParent(currentNode.subtreeParentVariablesMap, subtreeContext);
                        currentNode.RaiseExecutionStatusEvent(SubtreeExecutionStatus.Started, inputs_array, new Object[] { });
                    }
                    else
                    {
                        subtreeContext = this.SubTreeContextManager.GetSubtreeExecutionContext(currentNode.Name, subtreeVersion, DbConnectionString);
                        AssignSubtreeVariablesFromParent(currentNode.subtreeParentVariablesMap, Variables, subtreeContext.Variables);
                    }
                                        
                    var defaultStartNode = subtreeContext.TreeNodes.Single(s => s.Value.nodeType.Equals(NodeType.Start));
                    var nextSubtreeNode = subtreeContext.TreeStep(defaultStartNode.Value.id);
                    while (!(subtreeContext.NodeExecutionResultedInError || subtreeContext.ExecutionCompleted))
                    {
                        nextSubtreeNode = subtreeContext.TreeStep(nextSubtreeNode.id);
                    }

                    object[] subtree_messages_array = subtreeContext.ExtactMessagesArray();

                    if (generateFromDb)
                    {
                        object[] outputs_array = AssignParentOutputsFromSubtreeReturningOutputs(currentNode.subtreeParentVariablesMap, subtreeContext);
                        currentNode.RaiseExecutionStatusEvent(SubtreeExecutionStatus.Completed, outputs_array, subtree_messages_array);
                    }
                    else
                    {
                        ExpandoObject parentOutputVariableObj = DynamicExtensions.ToDynamic(this.Variables.outputs);
                        var subtreeOutputs = AssignParentOutputsFromSubtree(currentNode.subtreeParentVariablesMap, parentOutputVariableObj, subtreeContext.Variables);
                        this.Variables.outputs = DynamicExtensions.AssignDynamicValuesToObjectProperties(this.Variables.outputs, subtreeOutputs);
                    }
                    
                    ExposeSubtreeContextToParent(currentNode, subtreeContext);
                    #endregion subtree processing
                }
                else
                {
                    #region clear messages

                    if (node.nodeType.Equals(NodeType.ClearMessages))
                    {
                        var currentNode = node as ClearMessages;
                        string subtreeName = Utilities.StripInvalidChars(currentNode.SubtreeName);
                        SubtreeMessagesToClear.Add(string.Format("{0}_{1}", subtreeName, currentNode.SubtreeVersion));
                    }

                    #endregion clear messages

                    currentNodeExecutionResult = node.Process();
                }

                if (node.ExecutionResultedInError == false)
                {
                    if (node.nodeType == NodeType.End)
                    {
                        ExecutionCompleted = true;
                        MergeSubtreeMessagesIntoParentCollection();
                        nextNodeId = null;
                        node = null;   
                    }
                    else
                    {
                        node = this.RoutingEngine.MoveNext();
                        nextNodeId = node.id;
                    }
                }
            }
            return node;
        }

        public void ResetContext()
        {
            ExecutionCompleted = false;
            Paused = false;
            nodeExecutionResultedInError = false;
            nodeExecutionBreadcrumb.Clear();
            systemMessageCollection.Clear();
            SubtreeMessagesDictionary.Clear();
            SubtreeMessagesToClear.Clear();
        }
        
        public int? StepOverNode(Guid debugSessionId)
        {
            Node nextNode;
            nodeToRun = nextNodeId.HasValue ? TreeNodes[nextNodeId.Value]
                : TreeNodes.Single(n => n.Value.nodeType.Equals(NodeType.Start)).Value;

            if (!(ExecutionCompleted || Paused))
            {
                nextNode = TreeStep(nodeToRun.id);
                this.justExecutedNodeId = nodeToRun.id;
                nodeExecutionBreadcrumb.Add(new KeyValuePair<int, bool>(nodeToRun.id, currentNodeExecutionResult));

                if (BreakpointsContainNodeId(BreakPoints, this.justExecutedNodeId))
                {
                    Paused = true;
                }

                nodeToRun = nextNode;

                RaiseOnLocationChangedEvent(debugSessionId);

                Paused = true;
            }

            return justExecutedNodeId;
        }        

        public void PopulateTreeWithDebugValues(dynamic inputVariablesWithValues)
        {
            var inputsDict = (IDictionary<string, object>)this.Variables.inputs;
            inputsDict.Clear();
            var outputsDict = (IDictionary<string, object>)this.Variables.outputs;
            outputsDict.Clear();
            systemMessageCollection.Clear();

            dynamic inputsVariableObj = this.Variables.inputs;
            dynamic outputsVariableObj = this.Variables.outputs;

            var host = new HostObject(inputsVariableObj, outputsVariableObj, this.Enumerations);
            CSharpREPLCompilerService cSharpREPLCompilerService = new CSharpREPLCompilerService(host);

            foreach (var inputVariable in inputVariablesWithValues)
            {
                string variableName = Utilities.StripInvalidChars(Convert.ToString(inputVariable.name));
                dynamic propSpec = new ExpandoObject();
                propSpec.Name = variableName;
                propSpec.Type = GetVariableType(variableName, "input");
                var value = inputVariable.value;
                if (value.GetType() != inputVariable.value.Value.GetType())
                {
                    value = inputVariable.value.Value;
                }
                propSpec.Value = value;

                cSharpREPLCompilerService.AddPropertyWithUnboxedValue(propSpec);
            }
        }

        public object[] ExtractOutputsArray()
        {
            List<object> result = new List<object>();
            ExpandoObject outputVariableObj = this.Variables.outputs;
            ExpandoObject inputVariableObj = this.Variables.inputs;
            var outputVariablesDictionary = ((IDictionary<string, object>)outputVariableObj);
            var outputVariableNames = outputVariablesDictionary.Keys.ToArray();

            var host = new TreeProcessingContext.HostObject(inputVariableObj, outputVariableObj, this.Enumerations);
            CSharpREPLCompilerService cSharpREPLCompilerService = new CSharpREPLCompilerService(host);
            SetUnassignedOutputPropertiesToUserSpecifiedDefault(outputVariablesDictionary, cSharpREPLCompilerService);

            foreach (var variableName in outputVariableNames)
            {
                dynamic propSpec = new ExpandoObject();
                propSpec.Name = variableName;
                propSpec.Value = outputVariablesDictionary[variableName];

                if (propSpec.Name.Equals("NodeResult", StringComparison.InvariantCultureIgnoreCase))
                {
                    propSpec.Type = "bool";
                }
                else
                {
                    propSpec.Type = GetVariableType(variableName, "output");
                }

                cSharpREPLCompilerService.SetOutputPropertyWithUnboxedValue(propSpec);

                result.Add(new { name = variableName, value = outputVariablesDictionary[variableName] });
            }
            return result.ToArray();
        }

        public object[] ExtactMessagesArray()
        {
            List<object> result = new List<object>();
            foreach (var msg in systemMessageCollection.AllMessages)
            {
                result.Add(new { message = msg.Message, type = msg.Severity.ToString(), source = "Debug Context", line = "" });
            }
            return result.ToArray();
        }

        public void SetUnassignedOutputPropertiesToUserSpecifiedDefault(IDictionary<string, object> outputVariablesDictionary, CSharpREPLCompilerService cSharpREPLCompilerService)
        {
            foreach (var variableSpec in DecisionTreeJson.tree.variables)
            {
                var propertyName = Utilities.StripInvalidChars(Convert.ToString(variableSpec.name));
                var usage = Convert.ToString(variableSpec.usage);
                if (usage.Equals("output", StringComparison.InvariantCultureIgnoreCase)
                    && !outputVariablesDictionary.ContainsKey(propertyName)
                    && (variableSpec.defaultValue != null))
                {
                    dynamic propSpec = new ExpandoObject();
                    propSpec.Name = propertyName;
                    propSpec.Type = GetVariableType(propertyName, "output");
                    if (propSpec.Type.Equals("enum"))
                    {
                        propSpec.Value = Convert.ToString(variableSpec.defaultValue.value);
                    }
                    else
                    {
                        propSpec.Value = Convert.ToString(variableSpec.defaultValue);
                    }
                    
                    cSharpREPLCompilerService.SetOutputPropertyWithUnboxedValue(propSpec);
                }
            }
        }

        private string GetVariableType(string variableName, string usage)
        {
            // Getting type from custom strings not that straight fwd cause of namespace & assembly requirements.
            string variableType = string.Empty;
            foreach (var variable in DecisionTreeJson.tree.variables)
            {
                if (Convert.ToString(variable.usage).Equals(usage, StringComparison.InvariantCultureIgnoreCase)
                    && Utilities.StripInvalidChars(Convert.ToString(variable.name)).Equals(variableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    variableType = Convert.ToString(variable.type);
                    // Mapping for circumventing current issues with using strings for Enumerations.
                    // Proper Enums will not allow switching out versions based on current implementation
                    if (variableType.Contains("::"))
                    {
                        variableType = "enum";
                    }
                    break;
                }
            }

            if (variableType == "float")
            {
                variableType = "double";
            }
            return variableType;
        }
        
        public void CreateDynamicObjects(string code)
        {
            var assembly = Assembly.GetAssembly(typeof(IDecisionTree));
            var syntaxTree = SyntaxTree.ParseText(code);
            var compilation = Compilation.Create("Variables",
                                                  options: new CompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary))
                                                    .AddReferences(new MetadataReference[]
                                                    {
                                                        new MetadataFileReference(typeof (object).Assembly.Location),
                                                        new MetadataFileReference(typeof (System.Linq.Expressions.BinaryExpression).Assembly.Location),
                                                        new MetadataFileReference(typeof (SystemMessage).Assembly.Location)
                                                    })
                                                    .AddSyntaxTrees(syntaxTree);


            Assembly compiledAssembly;
            using (var stream = new MemoryStream())
            {
                EmitResult res = compilation.Emit(stream);
                compiledAssembly = Assembly.Load(stream.GetBuffer());
            }

            var messagesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Messages_{0}", GlobalsVersions.MessagesVersion);
            dynamic messages;
            messages = compiledAssembly.CreateInstance(messagesTypeName, false,
            BindingFlags.CreateInstance |
            BindingFlags.Public |
            BindingFlags.Instance |
            BindingFlags.OptionalParamBinding,
            null, new Object[] { systemMessageCollection }, null, null);

            Type msgType = assembly.GetType(messagesTypeName);

            this.Messages = messages;

            var enumerationsTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Enumerations_{0}", GlobalsVersions.EnumerationsVersion);
            var enumerations = compiledAssembly.CreateInstance(enumerationsTypeName, false,
             BindingFlags.CreateInstance |
             BindingFlags.Public |
             BindingFlags.Instance |
             BindingFlags.OptionalParamBinding,
             null, null, null, null);

            this.Enumerations = enumerations;

            var variablesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Variables_{0}", GlobalsVersions.VariablesVersion);
            Type varType = assembly.GetType(variablesTypeName);

            var variables = compiledAssembly.CreateInstance(variablesTypeName, false,
             BindingFlags.CreateInstance |
             BindingFlags.Public |
             BindingFlags.Instance |
             BindingFlags.OptionalParamBinding,
             null, new Object[] { this.Enumerations, this.Messages }, null, null);

            dynamic variablesObj = DynamicExtensions.ToDynamic(variables);
            variablesObj.inputs = new ExpandoObject();
            variablesObj.outputs = new ExpandoObject();
            variablesObj.outputs.NodeResult = false;

            this.Variables = variablesObj;
        }               

        private bool BreakpointsContainNodeId(dynamic breakpoints, int? nodeId)
        {
            bool found = false;
            if (nodeId.HasValue)
            {
                foreach (dynamic node in breakpoints)
                {
                    if (node.nodeId == nodeId)
                    {
                        found = true;
                        break;
                    }
                }
            }
            return found;
        }     

        private void MergeSubtreeMessagesIntoParentCollection()
        {
            foreach (var subtreeName in SubtreeMessagesDictionary.Keys)
            {
                if (!this.SubtreeMessagesToClear.Contains(subtreeName))
                {
                    foreach (var message in SubtreeMessagesDictionary[subtreeName].AllMessages)
                    {
                        if (systemMessageCollection.AllMessages.Any(m => m.Message.Equals(message) && m.Severity == message.Severity) )
                        {
                            continue;
                        }
                        systemMessageCollection.AddMessage(message);
                    }
                }
            }
        }

        private void ExposeSubtreeContextToParent(SubTree subtree, TreeProcessingContext subtreeDebugContext)
        {
            string subtreeName = Utilities.StripInvalidChars(subtree.Name);
            var variablesDictionary = (IDictionary<string, object>)this.Variables;
            if (!variablesDictionary.ContainsKey("subtrees"))
            {
                variablesDictionary.Add("subtrees", new ExpandoObject());
            }
            var subtreesDictionary = (IDictionary<string, object>)this.Variables.subtrees;
            string subtreeVariablesObjectName = Utilities.LowerFirstLetter(subtreeName);
            subtreesDictionary[subtreeVariablesObjectName] = subtreeDebugContext.Variables;

            var subtreeMessages = subtreeDebugContext.engine.GetVariable("Messages").SystemMessages as SystemMessageCollection;
            var subtreeMessageKey = String.Format("{0}_{1}", subtreeName, subtree.Version);
            if (SubtreeMessagesDictionary.ContainsKey(subtreeMessageKey))
            {
                SubtreeMessagesDictionary[subtreeMessageKey].AddMessages(subtreeMessages.AllMessages);
            }
            else
            {
                SubtreeMessagesDictionary.Add(subtreeMessageKey, subtreeMessages);
            }
        }

        private object[] AssignSubtreeInputsFromParent(dynamic subtreeParentVariablesMap, TreeProcessingContext subtreeDebugContext)
        {
            List<object> subtreeInputs = new List<object>();
            ExpandoObject subtreeInputVariableObj = subtreeDebugContext.Variables.inputs;
            var subtreeInputVariablesDictionary = ((IDictionary<string, object>)subtreeInputVariableObj);
            foreach (var map in subtreeParentVariablesMap)
            {
                if (Convert.ToString(map.usage).Equals("input", StringComparison.InvariantCultureIgnoreCase))
                {
                    string subtreeVariableName = Utilities.StripInvalidChars(map.name.ToString());
                    string parentVariablename = Utilities.StripInvalidChars(map.parentVariable.name.ToString());
                    string parentVariableUsage = Utilities.StripInvalidChars(map.parentVariable.usage.ToString());
                    var subtreeVariableValue = GetParentValue(parentVariablename, parentVariableUsage);
                    subtreeInputVariablesDictionary[subtreeVariableName] = subtreeVariableValue;
                    subtreeInputs.Add(new SubtreeVariable(subtreeVariableName, subtreeVariableValue));
                }
            }
            return subtreeInputs.ToArray();
        }

        private dynamic GetParentValue(string parentVariableName, string parentVariableUsage)
        {
            dynamic result = null;
            if (parentVariableUsage.Equals("input", StringComparison.InvariantCultureIgnoreCase))
            {
                ExpandoObject inputVariableObj = this.Variables.inputs;
                var parentInputVariablesDictionary = ((IDictionary<string, object>)inputVariableObj);
                result = parentInputVariablesDictionary[parentVariableName];
            }
            else if (parentVariableUsage.Equals("output", StringComparison.InvariantCultureIgnoreCase))
            {
                ExpandoObject outputVariableObj = this.Variables.outputs;
                var parentOutputVariablesDictionary = ((IDictionary<string, object>)outputVariableObj);
                result = parentOutputVariablesDictionary[parentVariableName];
            }
            return result;
        }

        private object[] AssignParentOutputsFromSubtreeReturningOutputs(dynamic subtreeParentVariablesMap, TreeProcessingContext subtreeDebugContext)
        {
            List<object> subtreeOutputs = new List<object>();
            ExpandoObject outputVariableObj = DynamicExtensions.ToDynamic(this.Variables.outputs);
            ExpandoObject subtreeOutputObj = DynamicExtensions.ToDynamic(subtreeDebugContext.Variables.outputs);
            ExpandoObject subtreeInputsObj = DynamicExtensions.ToDynamic(subtreeDebugContext.Variables.inputs);
            var parentOutputVariablesDictionary = ((IDictionary<string, object>)outputVariableObj);
            var subtreeOutputVariablesDictionary = ((IDictionary<string, object>)subtreeOutputObj);

            var host = new TreeProcessingContext.HostObject(subtreeInputsObj,subtreeOutputObj, this.Enumerations);
            CSharpREPLCompilerService cSharpREPLCompilerService = new CSharpREPLCompilerService(host);
            subtreeDebugContext.SetUnassignedOutputPropertiesToUserSpecifiedDefault(subtreeOutputVariablesDictionary, cSharpREPLCompilerService);

            foreach (var map in subtreeParentVariablesMap)
            {
                IDictionary<string, JToken> mapDictionary = JObject.Parse(map.ToString());
                if (Convert.ToString(mapDictionary["usage"]).Equals("output", StringComparison.InvariantCultureIgnoreCase)
                    && mapDictionary.ContainsKey("parentVariable"))
                {
                    IDictionary<string, JToken> parentVariableDictionary = JObject.Parse(map.parentVariable.ToString());
                    if (parentVariableDictionary.ContainsKey("name"))
                    {
                        string subtreeVariableName = Utilities.StripInvalidChars(Convert.ToString(map.name));
                        var subtreeVariableValue = subtreeOutputVariablesDictionary[subtreeVariableName];
                        subtreeOutputs.Add(new SubtreeVariable(subtreeVariableName, subtreeVariableValue));

                        string parentVariableName = Utilities.StripInvalidChars(Convert.ToString(map.parentVariable.name));
                        parentOutputVariablesDictionary[parentVariableName] = subtreeVariableValue;
                    }
                }
            }
            return subtreeOutputs.ToArray();
        }

        private class SubtreeVariable
        {
            public string name { get; set; }
            public object value { get; set; }

            public SubtreeVariable(string name, object value)
            {
                this.name = name;
                this.value = value != null && value.GetType().Name == "MutableString" ? Convert.ToString(value) : value;
            }
        };
        
        public class HostObject
        {
            public ExpandoObject InputsVariablesObject { get; protected set; }

            public ExpandoObject OutputsVariablesObject { get; protected set; }

            public dynamic EnumerationsObject { get; protected set; }

            public HostObject(ExpandoObject inputsVariablesObject, ExpandoObject outputsVariablesObject, dynamic enumerationsObject)
            {
                this.InputsVariablesObject = inputsVariablesObject;
                this.OutputsVariablesObject = outputsVariablesObject;
                this.EnumerationsObject = enumerationsObject;
            }
        }


        #region treeexecutionmanager methods
        private dynamic GetExecutionParentValue(dynamic parentVariable, dynamic parentVariablesCollection)
        {
            string parentVariableName = Utilities.StripInvalidChars(Convert.ToString(parentVariable.name));
            parentVariableName = Utilities.CapitaliseFirstLetter(parentVariableName);
            string parentVariableUsage = Convert.ToString(parentVariable.usage);
            object result = null;
            if (parentVariableUsage.Equals("input", StringComparison.InvariantCultureIgnoreCase))
            {
                ExpandoObject inputVariableObj = DynamicExtensions.ToDynamic(parentVariablesCollection.inputs);
                var parentInputVariablesDictionary = ((IDictionary<string, object>)inputVariableObj);
                result = parentInputVariablesDictionary[parentVariableName];
            }
            else if (parentVariableUsage.Equals("output", StringComparison.InvariantCultureIgnoreCase))
            {
                ExpandoObject outputVariableObj = DynamicExtensions.ToDynamic(parentVariablesCollection.outputs);
                var parentOutputVariablesDictionary = ((IDictionary<string, object>)outputVariableObj);
                result = parentOutputVariablesDictionary[parentVariableName];
            }
            return result;
        }

        public void AssignSubtreeVariablesFromParent(dynamic subtreeParentVariablesMap, dynamic parentVariables, dynamic subtreeVariables)
        {
            ExpandoObject subtreeInputVariableObj = DynamicExtensions.ToDynamic(Variables.inputs);
            var subtreeInputVariablesDictionary = ((IDictionary<string, object>)subtreeInputVariableObj);
            foreach (var map in subtreeParentVariablesMap)
            {
                if (Convert.ToString(map.usage).Equals("input", StringComparison.InvariantCultureIgnoreCase))
                {
                    string subtreeVariableName = Utilities.StripInvalidChars(map.name.ToString());
                    string parentVariablename = Utilities.StripInvalidChars(map.parentVariable.name.ToString());
                    subtreeInputVariablesDictionary[subtreeVariableName] = GetExecutionParentValue(map.parentVariable, parentVariables);
                }
            }
            subtreeVariables.inputs = DynamicExtensions.AssignDynamicValuesToObjectProperties(subtreeVariables.inputs, subtreeInputVariablesDictionary);
        }

        public dynamic AssignParentOutputsFromSubtree(dynamic subtreeParentVariablesMap, ExpandoObject parentOutputVariables, dynamic subtreeVariables)
        {
            var parentOutputVariablesDictionary = ((IDictionary<string, object>)parentOutputVariables);
            var subtreeOutputVariablesDictionary = ((IDictionary<string, object>)DynamicExtensions.ToDynamic(subtreeVariables.outputs));

            foreach (var map in subtreeParentVariablesMap)
            {
                if (Convert.ToString(map.usage).Equals("output", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (map.parentVariable.name != null)
                    {
                        string parentVariableName = Utilities.StripInvalidChars(Convert.ToString(map.parentVariable.name));
                        parentVariableName = Utilities.CapitaliseFirstLetter(parentVariableName);
                        string subtreeVariableName = Utilities.StripInvalidChars(Convert.ToString(map.name));
                        subtreeVariableName = Utilities.CapitaliseFirstLetter(subtreeVariableName);
                        parentOutputVariablesDictionary[parentVariableName] = subtreeOutputVariablesDictionary[subtreeVariableName];
                    }
                }
            }
            return parentOutputVariablesDictionary as ExpandoObject;
        }
      
        #endregion treeexecutionmanager methods
    }
}