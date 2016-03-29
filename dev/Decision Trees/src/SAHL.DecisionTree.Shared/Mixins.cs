using Microsoft.Scripting.Hosting;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DecisionTree.Shared
{
    public static class Mixins
    {
        public static void MixinRun(this ITreeProcess runnableTree, ScriptScope runningScope, int startNodeId)
        {
            if (runnableTree == null) throw new NullReferenceException();

            runnableTree.Scope = runningScope;
            runnableTree.CurrentNodeId = startNodeId;
            Node node = runnableTree.Nodes[runnableTree.CurrentNodeId];
            node.scope = runningScope;
            runnableTree.NodeExecutionResultedInError = false;

            while (!(runnableTree.NodeExecutionResultedInError || runnableTree.ExecutionCompleted ))
            {
                if (node.nodeType.Equals(NodeType.SubTree))
                {
                    var currentNode = (SubTree)runnableTree.Nodes[runnableTree.CurrentNodeId];
                    int subtreeVersion = Convert.ToInt32(currentNode.Version);

                    var subtreeExecutionManager = GetSubtreeExecutionManager(runnableTree, currentNode.Name, subtreeVersion);
                    subtreeExecutionManager.AssignSubtreeVariablesFromParent(currentNode.subtreeParentVariablesMap, runnableTree.VariablesCollection);
                    node.scope = subtreeExecutionManager.runningScope;

                    subtreeExecutionManager.Process();

                    ExpandoObject parentOutputVariableObj = DynamicExtensions.ToDynamic(runnableTree.VariablesCollection.outputs);
                    var subtreeOutputs = subtreeExecutionManager.AssignParentOutputsFromSubtree(currentNode.subtreeParentVariablesMap, parentOutputVariableObj);
                    runnableTree.VariablesCollection.outputs = DynamicExtensions.AssignDynamicValuesToObjectProperties(runnableTree.VariablesCollection.outputs, subtreeOutputs);

                    ExposeSubtreeContextToParent(runnableTree, currentNode, subtreeExecutionManager);
                }
                else
                {
                    if (node.nodeType.Equals(NodeType.ClearMessages))
                    {
                        var currentNode = node as ClearMessages;
                        string subtreeName = Utilities.StripInvalidChars(currentNode.SubtreeName);
                        runnableTree.SubtreeMessagesToClear.Add(string.Format("{0}_{1}", subtreeName, currentNode.SubtreeVersion));
                    }
                    runnableTree.CurrentResult = node.Process();
                    runnableTree.NodeExecutionResultedInError = node.ExecutionResultedInError;
                }
                node = GetNextNode(runnableTree);
            }
        }

        public static Node MixinStep(this ITreeProcess stepableTree, ScriptScope runningScope, int nodeToRunId)
        {
            if (stepableTree == null) throw new NullReferenceException();

            stepableTree.Scope = runningScope;
            stepableTree.CurrentNodeId = nodeToRunId;
            Node node = stepableTree.Nodes[stepableTree.CurrentNodeId];
            node.scope = runningScope;
            stepableTree.NodeExecutionResultedInError = false;
            if (!(stepableTree.NodeExecutionResultedInError || stepableTree.ExecutionCompleted))
            {
                if (node.nodeType.Equals(NodeType.SubTree))
                {
                    var currentNode = (SubTree)stepableTree.Nodes[stepableTree.CurrentNodeId];
                    int subtreeVersion = Convert.ToInt32(currentNode.Version);

                    var subtreeExecutionManager = GetSubtreeExecutionManager(stepableTree, currentNode.Name, subtreeVersion);
                    
                    subtreeExecutionManager.AssignSubtreeVariablesFromParent(currentNode.subtreeParentVariablesMap, stepableTree.VariablesCollection);
                    node.scope = subtreeExecutionManager.runningScope;

                    subtreeExecutionManager.Process();

                    ExpandoObject parentOutputVariableObj = DynamicExtensions.ToDynamic(stepableTree.VariablesCollection.outputs);
                    var subtreeOutputs = subtreeExecutionManager.AssignParentOutputsFromSubtree(currentNode.subtreeParentVariablesMap, parentOutputVariableObj);
                    stepableTree.VariablesCollection.outputs = DynamicExtensions.AssignDynamicValuesToObjectProperties(stepableTree.VariablesCollection.outputs, subtreeOutputs);


                    ExposeSubtreeContextToParent(stepableTree, currentNode, subtreeExecutionManager);
                }
                else
                {
                    if (node.nodeType.Equals(NodeType.ClearMessages))
                    {
                        var currentNode = node as ClearMessages;
                        string subtreeName = Utilities.StripInvalidChars(currentNode.SubtreeName);
                        stepableTree.SubtreeMessagesToClear.Add(string.Format("{0}_{1}", subtreeName, currentNode.SubtreeVersion));
                    }
                    stepableTree.CurrentResult = node.Process();
                    stepableTree.NodeExecutionResultedInError = node.ExecutionResultedInError;
                }
                node = GetNextNode(stepableTree);
            }
            return node;
        }

        private static void ExposeSubtreeContextToParent(ITreeProcess treeToProcess, SubTree subtree, TreeExecutionManager subtreeExecutionManager)
        {
            var variablesDictionary = (IDictionary<string, object>)treeToProcess.VariablesCollection;
            if (!variablesDictionary.ContainsKey("subtrees"))
            {
                variablesDictionary.Add("subtrees", new ExpandoObject());
            }
            var subtreesDictionary = (IDictionary<string, object>)treeToProcess.VariablesCollection.subtrees;
            var formattedSubtreeName = Utilities.StripInvalidChars(subtree.Name);
            formattedSubtreeName = Utilities.LowerFirstLetter(formattedSubtreeName).Replace(" ", String.Empty);
            subtreesDictionary[formattedSubtreeName] = subtreeExecutionManager.VariablesCollection;

            dynamic parentTreeMessages = treeToProcess.Scope.Engine.Runtime.Globals.GetVariable("Messages");
            dynamic subtreeMessages = subtreeExecutionManager.runningScope.Engine.Runtime.Globals.GetVariable("Messages");
        }

        private static TreeExecutionManager GetSubtreeExecutionManager(ITreeProcess processableTree, string subtreeName, int subtreeVersion)
        {
            ISystemMessageCollection systemMessageCollection = new SystemMessageCollection();

            var assembly = Assembly.GetAssembly(typeof(IDecisionTree));
            //// Check if tree  variables object is already cached
            //// If not instantiate and add to cache
            //// Get the tree variables object from cache
            var messagesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Messages_{0}", processableTree.GlobalsVersion.MessagesVersion);
            Type msgType = assembly.GetType(messagesTypeName);
            var boxedMessages = Activator.CreateInstance(msgType, systemMessageCollection);
            var messages = Convert.ChangeType(boxedMessages, msgType);

            var enumerations = assembly.CreateInstance(string.Format("SAHL.DecisionTree.Shared.Globals.Enumerations_{0}", processableTree.GlobalsVersion.EnumerationsVersion));

            // Globals and mixin with treeSpecific
            var variablesTypeName = string.Format("SAHL.DecisionTree.Shared.Globals.Variables_{0}", processableTree.GlobalsVersion.VariablesVersion);
            Type varType = assembly.GetType(variablesTypeName);
            var boxedVariables = Activator.CreateInstance(varType, new object[] { enumerations, messages });
            var unboxedVariables = Convert.ChangeType(boxedVariables, varType);

            var variablesObj = DynamicExtensions.ToDynamic(unboxedVariables);

            var formattedSubtreeName = Utilities.StripInvalidChars(subtreeName);
            formattedSubtreeName = Utilities.CapitaliseFirstLetter(formattedSubtreeName);

            var subtreeSpecificVariablesTypeName = string.Format("SAHL.DecisionTree.Shared.{0}_{1}Variables", formattedSubtreeName, subtreeVersion);
            Type subtreeSpecificVariablesType = assembly.GetType(subtreeSpecificVariablesTypeName);
            var boxedSubtreeSpecificVariables = Activator.CreateInstance(subtreeSpecificVariablesType, new object[] { enumerations });
            dynamic subtreeSpecificVariables = Convert.ChangeType(boxedSubtreeSpecificVariables, subtreeSpecificVariablesType);
            variablesObj.inputs = subtreeSpecificVariables.inputs;
            variablesObj.outputs = subtreeSpecificVariables.outputs;
            variablesObj.outputs.NodeResult = false;

            processableTree.SubtreeMessagesDictionary.Add(string.Format("{0}_{1}", Utilities.StripInvalidChars(subtreeName), subtreeVersion), systemMessageCollection);
            var subtreeExecutionManager = new TreeExecutionManager(formattedSubtreeName, subtreeVersion, processableTree.GlobalsVersion, variablesObj, messages, enumerations);

            return subtreeExecutionManager;
        }

        private static Node GetNextNode(ITreeProcess processableTree)
        {
            Node nextNode = null;
            var switchOnNodeType = processableTree.Nodes[processableTree.CurrentNodeId].nodeType;
            switch (switchOnNodeType)
            {
                case NodeType.Start:
                case NodeType.Process:
                case NodeType.SubTree:
                case NodeType.ClearMessages:
                    {
                        var link = processableTree.NodeLinks.SingleOrDefault(nl => nl.FromNodeID == processableTree.CurrentNodeId);
                        nextNode = processableTree.Nodes[link.ToNodeID];
                        break;
                    }
                case NodeType.Decision:
                    {
                        if (processableTree.CurrentResult)
                        {
                            var link = processableTree.NodeLinks.SingleOrDefault(nl => nl.FromNodeID == processableTree.CurrentNodeId && nl.Type == LinkType.DecisionYes);
                            nextNode = processableTree.Nodes[link.ToNodeID];
                        }
                        else
                        {
                            var link = processableTree.NodeLinks.SingleOrDefault(nl => nl.FromNodeID == processableTree.CurrentNodeId && nl.Type == LinkType.DecisionNo);
                            nextNode = processableTree.Nodes[link.ToNodeID];
                        }
                        break;
                    }
                case NodeType.End:
                    {
                        MergeSubtreeMessagesIntoParentCollection(processableTree);
                        processableTree.ExecutionCompleted = true;
                        break;
                    }
                default:
                    {
                        var systemMessages = processableTree.Scope.Engine.Runtime.Globals.GetVariable("Messages");
                        systemMessages.AddMessage(new SystemMessage("Node type is unknown on this tree", SystemMessageSeverityEnum.Error));
                        processableTree.NodeExecutionResultedInError = true;
                        break;
                    }
            }
            if (nextNode != null)
            {
                nextNode.scope = processableTree.Scope;
                processableTree.CurrentNodeId = nextNode.id;
            }
            return nextNode;
        }

        private static void MergeSubtreeMessagesIntoParentCollection(ITreeProcess processableTree)
        {
            foreach (var subtreeName in processableTree.SubtreeMessagesDictionary.Keys)
            {
                if (!processableTree.SubtreeMessagesToClear.Contains(subtreeName))
                {
                    foreach (var message in processableTree.SubtreeMessagesDictionary[subtreeName].AllMessages)
                    {
                        if (processableTree.SystemMessages.AllMessages.Any(m => m.Severity == message.Severity && m.Message.Equals(message.Message)))
                        {
                            continue;
                        }
                        processableTree.SystemMessages.AddMessage(message);
                    }
                }
            }
        }
    }
}
