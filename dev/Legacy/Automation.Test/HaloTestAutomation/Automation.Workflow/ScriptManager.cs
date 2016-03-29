using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Common.Enums;

namespace WorkflowAutomation.Harness
{
    public static class ScriptManager
    {
        public static void CreateAutomationScripts()
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<Type> automationScripts = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.IsPublic &&
                            typeof(IAutomationScript).IsAssignableFrom(x)
                            ).ToList();
            foreach (var script in automationScripts)
            {
                var scriptInstance = Activator.CreateInstance(script) as IAutomationScript;
                string scriptPath = CreateScriptDirectory(scriptInstance.Workflow);
                var workflowScript = scriptInstance.Create();
                System.Xaml.XamlServices.Save(scriptPath, workflowScript);
            }
        }
        public static string CreateScriptDirectory(WorkflowEnum workflow)
        {
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string scriptsFolder = System.IO.Path.Combine(startupPath, "Scripts");
            var scriptPath = System.IO.Path.Combine(scriptsFolder, string.Format("{0}.xaml", workflow.ToString()));
            if (!System.IO.Directory.Exists(scriptsFolder))
            {
                System.IO.Directory.CreateDirectory(scriptsFolder);
            }
            return scriptPath;
        }
    }
}