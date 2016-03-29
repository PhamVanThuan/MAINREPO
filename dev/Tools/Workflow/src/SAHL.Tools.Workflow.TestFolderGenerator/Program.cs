using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CommandLine;
using SAHL.Tools.Workflow.Builder.CommandLine;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.TestFolderGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.ReadKey();
            CommandLineArguments arguments = new CommandLineArguments();
            Parser parser = new Parser();
            bool parserResult = parser.ParseArguments(args, arguments);
            if (parserResult)
            {
                if (File.Exists(arguments.X2WorkflowMap))
                {
                    string rootDirectory = arguments.RootDirectory;
                    if (string.IsNullOrEmpty(rootDirectory))
                    {
                        string workflowMapsBuildToolsDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        DirectoryInfo up1 = Directory.GetParent(workflowMapsBuildToolsDir);
                        DirectoryInfo up2 = Directory.GetParent(up1.FullName);
                        rootDirectory = up2.FullName;
                    }

                    string outputDirectory = arguments.OutputDirectory;
                    if (string.IsNullOrEmpty(outputDirectory))
                    {
                        string workflowMapDirectory = Path.GetDirectoryName(arguments.X2WorkflowMap);
                        if (!Path.IsPathRooted(workflowMapDirectory))
                        {
                            string currentDir = Directory.GetCurrentDirectory();
                            workflowMapDirectory = Path.Combine(currentDir, workflowMapDirectory);
                        }

                        // find the specs folder under workflowMapdirectory
                        string[] dirs = Directory.GetDirectories(workflowMapDirectory, "*.Specs");
                        if (dirs.Length == 1)
                        {
                            outputDirectory = dirs[0];

                            using (FileStream fs = new FileStream(arguments.X2WorkflowMap, FileMode.Open, FileAccess.Read))
                            {
                                XmlStreamPersistanceStore store = new XmlStreamPersistanceStore(fs);
                                Process process = store.LoadProcess();

                                // is there more than one workflow
                                if (process.Workflows.Count > 1)
                                {
                                    GenerateWorkflowsForProcess(process, outputDirectory, true);
                                }
                                else if (process.Workflows.Count == 1)
                                {
                                    GenerateWorkflowsForProcess(process, outputDirectory, false);
                                }
                            }
                        }
                        else
                        {
                            Environment.Exit(-1);
                        }
                    }
                }
            }
        }

        private static void GenerateWorkflowsForProcess(Process process, string directory, bool createWorkflowDirs)
        {
            // check if there is a workflows directory
            string newPath = Path.Combine(directory, "Workflows");

            if (!Directory.Exists(newPath) && createWorkflowDirs)
            {
                Directory.CreateDirectory(newPath);
            }

            foreach (SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow in process.Workflows)
            {
                string workflowPath = Path.Combine(newPath, workflow.Name);
                if (!Directory.Exists(workflowPath) && createWorkflowDirs)
                {
                    Directory.CreateDirectory(workflowPath);
                }

                if (!createWorkflowDirs)
                {
                    workflowPath = directory;
                }
                GenerateActivitiesForWorkflow(workflow, workflowPath);

                GenerateStatesForWorkflow(workflow, workflowPath);
            }
        }

        private static void GenerateStatesForWorkflow(SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow, string directory)
        {
            string newPath = Path.Combine(directory, "States");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            foreach (AbstractNamedState state in workflow.States)
            {
                string statePath = Path.Combine(newPath, state.Name);
                statePath = statePath.Replace("?", "");
                if (!Directory.Exists(statePath))
                {
                    Directory.CreateDirectory(statePath);
                }

                // On Enter
                CheckAndCreateDirectory(statePath, "OnEnter");
                GenerateBoolTest(statePath, state.SafeName, GetTestWorkflowName(workflow.Name), "States", "State", state.OnEnterStateCode, "OnEnter", "OnEnter");

                // On Exit
                CheckAndCreateDirectory(statePath, "OnExit");
                GenerateBoolTest(statePath, state.SafeName, GetTestWorkflowName(workflow.Name), "States", "State", state.OnExitStateCode, "OnExit", "OnExit");

                // if its an archive state
                if (state is ArchiveState)
                {
                    CheckAndCreateDirectory(statePath, "OnReturn");
                    GenerateBoolTest(statePath, state.SafeName, GetTestWorkflowName(workflow.Name), "States", "State", ((ArchiveState)state).OnReturnCode, "OnReturn", "OnReturn");
                }

                // if its a system state
                if (state is SystemState)
                {
                    SystemState sys = state as SystemState;
                    if (sys.UseAutoForward)
                    {
                        CheckAndCreateDirectory(statePath, "OnAutoForward");
                        if (((SystemState)state).AutoForwardCode != null)
                        {
                            GenerateStringTest(statePath, state.SafeName, GetTestWorkflowName(workflow.Name), "States", "State", ((SystemState)state).AutoForwardCode, "OnAutoForward", "GetForwardStateName");
                        }
                    }
                }
            }
        }

        private static void GenerateActivitiesForWorkflow(SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow, string directory)
        {
            string newPath = Path.Combine(directory, "Activities");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            foreach (AbstractActivity activity in workflow.Activities)
            {
                string activityPath = Path.Combine(newPath, activity.Name);
                activityPath = activityPath.Replace("?", "");

                // On Start
                CheckAndCreateDirectory(activityPath, "OnStart");
                GenerateBoolTest(activityPath, activity.SafeName, GetTestWorkflowName(workflow.Name), "Activities", "Activity", activity.OnStartActivityCode, "OnStart", "OnStartActivity");

                // On Complete
                CheckAndCreateDirectory(activityPath, "OnComplete");
                GenerateBoolTest(activityPath, activity.SafeName, GetTestWorkflowName(workflow.Name), "Activities", "Activity", activity.OnCompleteActivityCode, "OnComplete", "OnCompleteActivity");

                // On Stage Activity
                CheckAndCreateDirectory(activityPath, "OnGetStageTransition");
                GenerateStringTest(activityPath, activity.SafeName, GetTestWorkflowName(workflow.Name), "Activities", "Activity", activity.GetStageTransitionCode, "OnGetStageTransition", "GetStageTransition");

                // if its a user activity
                if (activity is UserActivity)
                {
                    CheckAndCreateDirectory(activityPath, "OnGetActivityMessage");
                    GenerateStringTest(activityPath, activity.SafeName, GetTestWorkflowName(workflow.Name), "Activities", "Activity", ((UserActivity)activity).GetActivityMesssageCode, "OnGetActivityMessage", "GetActivityMessage");
                }

                // if its a timer
                if (activity is TimedActivity)
                {
                    CheckAndCreateDirectory(activityPath, "OnGetActivityTime");
                }
            }
        }

        private static void CheckAndCreateDirectory(string path, string folder)
        {
            string newPath = Path.Combine(path, folder);
            newPath = newPath.Replace("?", "");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
        }

        private static void GenerateBoolTest(string path, string elementSafeName, string workflowName, string parentFolderName, string parentFolderType, CodeSection codeSection, string codeSectionName, string codeSectionPrefix)
        {
            if (!RequiresGenericBoolTest(codeSection))
            {
                return;
            }

            bool? result = GetBoolCodeSectionResult(codeSection);
            if (result != null)
            {
                GenericBooleanTest test = new GenericBooleanTest(result.Value, elementSafeName, codeSectionName, codeSectionPrefix, ActualWorkflowName(workflowName), parentFolderName, parentFolderType);
                string testString = test.TransformText();
                path = Path.Combine(path, codeSectionName);
                string fileName = string.Format("when_{0}.cs", elementSafeName).ToLower();
                fileName = Path.Combine(path, fileName);
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        sw.Write(testString);
                        sw.Flush();
                    }
                }
            }
        }

        private static void GenerateStringTest(string path, string elementSafeName, string workflowName, string parentFolderName, string parentFolderType, CodeSection codeSection, string codeSectionName, string codeSectionPrefix)
        {
            if (!RequiresGenericStringTest(codeSection))
            {
                return;
            }

            GenericStringTest test = new GenericStringTest(elementSafeName, codeSectionName, codeSectionPrefix, ActualWorkflowName(workflowName), parentFolderName, parentFolderType);
            string testString = test.TransformText();
            path = Path.Combine(path, codeSectionName);
            string fileName = string.Format("when_{0}.cs", elementSafeName).ToLower();
            fileName = Path.Combine(path, fileName);
            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.Write(testString);
                    sw.Flush();
                }
            }
        }

        private static bool RequiresGenericStringTest(CodeSection section)
        {
            Regex regex = new Regex("(\\r\\n)*\\s*public string");
            if (regex.Match(section.Code).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool RequiresGenericBoolTest(CodeSection section)
        {
            Regex regex = new Regex("(\\r\\n)*\\s*public bool");
            if (regex.Match(section.Code).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool? GetBoolCodeSectionResult(CodeSection section)
        {
            Regex regex = new Regex("(?<={(\\r\\n)*\\s*return\\s*)(true|false)(?=;\\s*(\\r\\n)*})");
            Match m = regex.Match(section.Code);
            if (m.Success)
            {
                bool result = false;
                bool parsed = bool.TryParse(m.Value, out result);
                if (parsed)
                {
                    return result;
                }
            }
            return null;
        }

        private static string GetTestWorkflowName(string workflowName)
        {
            if (workflowName == "CAP2 Offers")
                return "Cap2";
            return workflowName.Replace(" ", "");
        }

        private static string ActualWorkflowName(string workflowName)
        {
            if (workflowName == "LifeOrigination")
                return "Life";

            return workflowName;
        }

        private static void GenerateStateOnEnterTest(string path, string workflowName, List<string> stateNames)
        {
            OnEnter test = new OnEnter(stateNames, workflowName);//result.Value, elementSafeName, codeSectionName, codeSectionPrefix, ActualWorkflowName(workflowName), parentFolderName, parentFolderType);
            string testString = test.TransformText();
            path = Path.Combine(path, "Internal");
            string fileName = "when_enter.cs";
            fileName = Path.Combine(path, fileName);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(testString);
                sw.Flush();
            }
        }
    }
}