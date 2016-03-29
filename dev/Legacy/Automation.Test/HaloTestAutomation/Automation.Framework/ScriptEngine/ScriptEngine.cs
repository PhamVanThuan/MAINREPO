using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xaml;
using Automation.Framework.DataAccess;
using Common.Constants;
using Common.Models;

namespace Automation.Framework
{
    /// <summary>
    /// Automation
    /// </summary>
    public class ScriptEngine
    {
        public WorkflowScript Workflow { get; set; }

        private static object obj = new object();
        private string codePrefix = "code:";
        private string scriptPrefix = "script:";

        /// <summary>
        /// Load Workflow
        /// </summary>
        /// <param name="workflowLocation"></param>
        public WorkflowScript LoadWorkflow(string workflowLocation)
        {
            var scriptsDir = new DirectoryInfo("Scripts");
            if (!scriptsDir.Exists)
                scriptsDir.Create();

            var scriptFile = scriptsDir.GetFiles().Where(x => x.FullName.Contains(workflowLocation)).FirstOrDefault();
            if (scriptFile == null)
            {
                throw new ArgumentException(String.Format("The Workflow ({0}) does not exist", workflowLocation));
            }
            lock (obj)
            {
                Workflow = (WorkflowScript)XamlServices.Load(scriptFile.FullName);
            };
            if (Workflow == null)
            {
                throw new NullReferenceException("The Workflow could not be parsed, please check your workflow file");
            }
            return Workflow;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="scriptToRun"></param>
        /// <param name="keyValue"></param>
        public Dictionary<int, WorkflowReturnData> ExecuteScript(string fileName, string scriptToRun, int keyValue)
        {
            var workflow = this.LoadWorkflow(fileName);
            return this.RunScript(workflow, scriptToRun, keyValue);
        }

        /// <summary>
        /// Run the specified Script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public Dictionary<int, WorkflowReturnData> RunScript(WorkflowScript workflow, string scriptName, int keyValue, string identity = "")
        {
            var query = (from script in workflow.Scripts
                         where script.Name == scriptName
                         select script).FirstOrDefault();
            if (!string.IsNullOrEmpty(identity) && query != null)
            {
                foreach (var step in query.Steps)
                {
                    step.Identity = identity;
                }
            }
            if (query == null)
            {
                throw new EntryPointNotFoundException(String.Format("The script could not be found : {0}", scriptName));
            }
            return RunScript(query, keyValue);
        }

        /// <summary>
        /// Run the specified Script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public Dictionary<int, WorkflowReturnData> RunScript(Script script, int keyValue)
        {
            if (script == null)
            {
                throw new ArgumentNullException("script");
            }
            //run the script setup
            var setupResult = Setup(script.Setup, keyValue);
            //stores the results of our script
            var workflowResults = new Dictionary<int, WorkflowReturnData>();
            foreach (var step in script.Steps.OrderBy((i) => i.Order))
            {
                var stepPriorToStartResult = Setup(step.PriorToStart, keyValue);
                var stepWorkflowActivityResult = ExecuteWorkflowActivity(step, keyValue, Workflow);
                //check to see if the activity completed if successful then add the results
                workflowResults.Add(step.Order, stepWorkflowActivityResult);
                if (stepWorkflowActivityResult.ActivityCompleted == false)
                    return workflowResults;
                var stepPostCompleteResult = Complete(step.PostComplete, keyValue);
            }

            var completeResult = Complete(script.Complete, keyValue);
            return workflowResults;
        }

        /// <summary>
        /// Run Setup Script
        /// </summary>
        /// <param name="setupScript"></param>
        /// <returns></returns>
        private bool Setup(string setupScript, int keyValue)
        {
            if (String.IsNullOrEmpty(setupScript))
                return false;
            if (setupScript.Contains(codePrefix))
            {
                //this is a code method that we need to run
                var codeToRun = setupScript.Replace(codePrefix, String.Empty);
                var methodResult = ExecuteMethod(codeToRun, keyValue);
            }
            else if (setupScript.Contains(scriptPrefix))
            {
                var procToRun = setupScript.Replace(scriptPrefix, String.Empty);
                var param = new Dictionary<string, string> { { Workflow.PrimaryKey, keyValue.ToString() } };
                var procResult = DataHelper.ExecuteProcedure(procToRun, param);
            }
            return true;
        }

        /// <summary>
        /// Execute Workflow Activity
        /// </summary>
        /// <param name="workflowActivity"></param>
        /// <returns></returns>
        private WorkflowReturnData ExecuteWorkflowActivity(Step step, int keyValue, WorkflowScript workflow)
        {
            WorkflowReturnData result = null;
            if (String.IsNullOrEmpty(step.WorkflowActivity))
            {
                return new WorkflowReturnData { ActivityCompleted = false, InstanceID = 0 };
            }
            var engine = new X2Helper();
            if (step.WorkflowActivity == "Create")
            {
                result = engine.CreateWorkflowInstance(workflow, keyValue, step.Identity);
            }
            else
            {
                result = engine.PerformActivity(keyValue, step, workflow);
            }
            return result;
        }

        /// <summary>
        /// Complete
        /// </summary>
        /// <param name="completeScript"></param>
        /// <returns></returns>
        private bool Complete(string completeScript, int keyValue)
        {
            if (String.IsNullOrEmpty(completeScript))
                return false;

            if (completeScript.Contains(codePrefix))
            {
                //this is a code method that we need to run
                var codeToRun = completeScript.Replace(codePrefix, String.Empty);
                var methodResult = ExecuteMethod(codeToRun, keyValue);
            }
            else if (completeScript.Contains(scriptPrefix))
            {
                var procToRun = completeScript.Replace(scriptPrefix, String.Empty);
                var param = new Dictionary<string, string> { { Workflow.PrimaryKey, keyValue.ToString() } };
                var procResult = DataHelper.ExecuteProcedure(procToRun, param);
            }
            return true;
        }

        /// <summary>
        /// Get Assembly
        /// </summary>
        /// <returns></returns>
        private Assembly GetCurrentAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        /// <summary>
        /// Execute Method
        /// </summary>
        /// <param name="method"></param>
        private bool ExecuteMethod(string method, int keyValue)
        {
            string typeName = String.Empty;
            string methodName = String.Empty;
            if (method.Contains("."))
            {
                typeName = method.Split('.')[0];
                methodName = method.Split('.')[1];
                methodName.Trim();
            }
            else
            {
                methodName = method.Trim();
            }

            //if we don't pass the assembly, we assume that the method is in the current assembly
            var assembly = GetCurrentAssembly();
            var typeToExecute = (from type in assembly.GetTypes()
                                 where type.Name.Contains(typeName)
                                 select type).FirstOrDefault();

            if (typeToExecute != null)
            {
                var instanceOfType = Activator.CreateInstance(typeToExecute);
                var methodToExecute = (from m in typeToExecute.GetMethods()
                                       where m.Name.Contains(methodName)
                                       select m).FirstOrDefault();

                if (methodToExecute != null)
                {
                    methodToExecute.Invoke(instanceOfType, new object[] { keyValue });
                    return true;
                }
            }
            return false;
        }

        public bool ClearDomainServiceCache(string processName, string workflowName)
        {
            return ClearCache(processName, workflowName, Common.Enums.CacheTypes.DomainService);
        }

        public bool ClearRuleCache(string processName, string workflowName)
        {
            return ClearCache(processName, workflowName, Common.Enums.CacheTypes.Rules);
        }

        private static bool ClearCache(string processName, string workflowName, Common.Enums.CacheTypes cacheType)
        {
            var x2Helper = new X2Helper();
            object data = cacheType.ToString();
            string activityName = "Clear Cache";

            if (cacheType == Common.Enums.CacheTypes.Rules)
            {
                activityName = "ClearRuleCache";
                data = null;
            }
            var workflowReturnData = x2Helper.CreateCase(TestUsers.ClintonS, processName, "-1", workflowName, activityName, null, false, data);
            return workflowReturnData.ActivityCompleted;
        }
    }
}