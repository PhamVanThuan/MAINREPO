using System;
using System.Collections.Generic;
using Automation.Framework;
using Common.Extensions;
using Common.Models;
using WorkflowAutomation.Harness;

public class X2ScriptEngine : WorkflowAutomation.Harness.IX2ScriptEngine
{
    public X2ScriptEngine()
    {
        ScriptManager.CreateAutomationScripts();
    }

    public Dictionary<int, Common.Models.WorkflowReturnData> ExecuteScript(Common.Enums.WorkflowEnum _workflow, string scriptToRun, int keyValue, string identity = "")
    {
        try
        {
            string fileName = System.IO.Path.GetFullPath(GetWorkflowFile(_workflow));
            ScriptEngine engine = new ScriptEngine();
            WorkflowScript workflow = engine.LoadWorkflow(fileName);
            Dictionary<int, WorkflowReturnData> results = engine.RunScript(workflow, scriptToRun, keyValue, identity);
            if (!results.LastActivitySucceeded())
                throw new Exception(string.Format(@"Workflow Script Execution failed. Running script: {0}, Error: {1}", scriptToRun, results.LastStep().Error));
            return results;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static string GetWorkflowFile(Common.Enums.WorkflowEnum workflow)
    {
        string fileName = string.Empty;
        return string.Format(@"Scripts\{0}.xaml", workflow.ToString());
    }

    public bool ClearCacheFor(string processName, string workflowName, Common.Enums.CacheTypes cacheToClear)
    {
        var scriptEngine = new ScriptEngine();

        switch (cacheToClear)
        {
            case Common.Enums.CacheTypes.DomainService:
                return scriptEngine.ClearDomainServiceCache(processName, workflowName);
            case Common.Enums.CacheTypes.Rules:
                return scriptEngine.ClearRuleCache(processName, workflowName);
        }
        return false;
    }

    public bool ClearDomainServiceCacheForAllWorkflows()
    {
        var workflows = GetAllWorkflows();
        bool allWereSuccessful = true;
        foreach (var workflow in workflows)
        {
            var success = ClearCacheFor(workflow.ProcessName, workflow.WorkflowName, Common.Enums.CacheTypes.DomainService);
            if (!success)
                allWereSuccessful = false;
        }
        return allWereSuccessful;
    }

    private List<Workflow> GetAllWorkflows()
    {
        return new List<Workflow> {
            new Workflow { ProcessName = "CAP2 Offers", WorkflowName = "CAP2 Offers"},
            new Workflow { ProcessName = "Debt Counselling", WorkflowName = "Debt Counselling"},
            new Workflow { ProcessName = "Help Desk", WorkflowName = "Help Desk"},
            new Workflow { ProcessName = "Life", WorkflowName = "LifeOrigination"},
            new Workflow { ProcessName = "Loan Adjustments", WorkflowName = "Loan Adjustments"},
            new Workflow { ProcessName = "Origination", WorkflowName = "Application Capture"},
            new Workflow { ProcessName = "Personal Loan", WorkflowName = "Personal Loans"},
            new Workflow { ProcessName = "LifeClaims", WorkflowName = "Disability Claim"}
        };
    }

    private class Workflow
    {
        public string ProcessName { get; set; }

        public string WorkflowName { get; set; }
    }
}