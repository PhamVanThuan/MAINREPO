using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SAHL.Tools.Workflow.Common.Compiler;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Datasets;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.Tools.Workflow.Common.Database.Publishing;
using NHibernate;
using SAHL.Tools.Workflow.Common.Database;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Publishing
{
    public class X2Publisher
    {
        public static int cmdTimeout = 90;
        public static string ConnectionString2AM = "";
        public static string ConnectionStringX2 = "";

        public static bool Publish(ProcessDocument pDocument, string ExecutablePath, out List<string> compilerErrorMessages, out List<string> publisherErrorMessages, out int noOfInstancesRecalculated)
        {
            compilerErrorMessages = new List<string>();
            publisherErrorMessages = new List<string>();
            noOfInstancesRecalculated = 0;

            List<CallWorkFlowActivity> lstCallWorkFlowActivitiesNotSet = CheckIfCallWorkFlowActivityPropertiesSet(pDocument);
            if (lstCallWorkFlowActivitiesNotSet.Count > 0)
            {
                frmCallWorkFlowActivitiesNotSet mFrm = new frmCallWorkFlowActivitiesNotSet(lstCallWorkFlowActivitiesNotSet);
                mFrm.ShowDialog();
                return false;
            }

            // compile it first and see if there were errors
            using (Compiler compiler = new Compiler())
            {
                // get the x2map name              
                string x2Map = MainForm.App.GetCurrentView().Document.Location;
                // get the folder where the nuget binaries located
                string binariesDirectory = Path.Combine(Path.GetDirectoryName(x2Map), "Binaries");
                // set the output directory where the compiled dll with be stored
                string outputDirectory = binariesDirectory;

                #region compile the map
                CompilerResults compilerResults = compiler.Compile(MainForm.App.GetCurrentView().Document.Location, new CompilerOptions(outputDirectory, binariesDirectory, true));
                X2Generator.CurrentCode = compiler.LastCompiledSourceCode;

                if (compilerResults.Errors.HasErrors)
                {
                    // we cannot continue, the compiler errors will already have been shown in the code error view
                    // just show an error that publishing could not continue
                    //MessageBox.Show("Unable to compile the WorkFlow! Publishing cannot continue!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    foreach (CompilerError e in compilerResults.Errors)
                    {
                        compilerErrorMessages.Add("Line: " + e.Line + "  Column: " + e.Column + "  Error: " + e.ErrorText);
                    }
                    return false;
                }

                //My Code
                //---------------------
                bool bDuplicates = false;
                ArrayList arrActivityNodes = new ArrayList();

                //Loop through each workflow map
                foreach (WorkFlow wflow in MainForm.App.GetCurrentView().Document.WorkFlows)
                {
                    //Loop through each state in the workflow map
                    foreach (BaseState bState in wflow.States)
                    {
                        if (!(bState is CommonState))
                        {
                            arrActivityNodes.Clear();
                            foreach (CustomLink lnk in bState.Links)
                            {
                                BaseActivity myParent = lnk.ToPort.Node as BaseActivity;
                                if (myParent != null)
                                {
                                    arrActivityNodes.Add(myParent);
                                }
                            }

                            //Store all duplicate activities in an arraylist for checking so that
                            //it wont be added again
                            ArrayList arrDuplicates = new ArrayList();
                            //find any duplicate priorities
                            foreach (BaseActivity bActivity1 in arrActivityNodes)
                            {
                                foreach (BaseActivity bActivity2 in arrActivityNodes)
                                {
                                    if (bActivity1 != bActivity2 && (bActivity1.Priority == bActivity2.Priority) && !(bActivity1 is ReturnWorkflowActivity) && !(bActivity2 is ReturnWorkflowActivity))
                                    {
                                        //Loop through arraylist to check if the activities have aready been added
                                        //If not, then add them to the list
                                        bool bFound = false;
                                        foreach (BaseActivity bact in arrDuplicates)
                                        {
                                            if (bActivity1 == bact || bActivity2 == bact)
                                            {
                                                bFound = true;
                                                break;
                                            }
                                        }
                                        if (!bFound)
                                        {
                                            arrDuplicates.Add(bActivity1);
                                            arrDuplicates.Add(bActivity2);
                                            bDuplicates = true;
                                            compilerErrorMessages.Add("Duplicate Priority Error(s) - Workflow: " + wflow.WorkFlowName + "|Activities:" + bActivity1.Name + " and " + bActivity2.Name + " have duplicate priorities.");

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (bDuplicates) return false;
                //-------------------------------------------

                // if we have no compiler errors then continue
                if (ConnectionStringX2.Length < 1)
                {
                    ConnectionForm CF = new ConnectionForm();
                    if (Helpers.ShowX2ConnForm(CF, true))
                        ConnectionStringX2 = CF.ConnectionString;
                    else
                        return false;
                }

                // Check BusinessStateTransition validity for all relevent activites
                DialogResult res = MainForm.App.GetBusinessStageItems(ConnectionString2AM);
                if (res == DialogResult.Cancel)
                {
                    publisherErrorMessages.Add("Publishing Cancelled!");
                    return false;
                }

                #region Publish the Process
                PrePublishChecker checker = new PrePublishChecker();
                ProcessFromXmlGenerator procGen = new ProcessFromXmlGenerator();
                Publisher p = new Publisher(checker, procGen);

                Console.WriteLine("Starting {0}", Path.GetFileNameWithoutExtension(x2Map));
                Console.WriteLine("Publish " + x2Map + ": binaryFolder :" + binariesDirectory);
                p.PublishProcess(x2Map, binariesDirectory, ConnectionStringX2, out noOfInstancesRecalculated);
                Console.WriteLine("Done");

                #endregion


            }
            return true;
        }


        internal static List<CallWorkFlowActivity> CheckIfCallWorkFlowActivityPropertiesSet(ProcessDocument pDocument)
        {
            List<CallWorkFlowActivity> lstCallWorkFlowActivitiesNotSet = new List<CallWorkFlowActivity>();
            foreach (WorkFlow w in pDocument.WorkFlows)
            {
                foreach (BaseActivity a in w.Activities)
                {
                    if (a is CallWorkFlowActivity)
                    {
                        CallWorkFlowActivity mCallWorkFlowActivity = a as CallWorkFlowActivity;
                        if (mCallWorkFlowActivity.ActivityToCall == null || mCallWorkFlowActivity.WorkFlowToCall == null)
                        {
                            lstCallWorkFlowActivitiesNotSet.Add(mCallWorkFlowActivity);
                        }
                    }
                }
            }
            return lstCallWorkFlowActivitiesNotSet;
        }

     }

    public class MatchVariableItem
    {
        bool m_FoundMatch = false;
        string m_VariableName = "";
        string m_VariableType = "";

        public bool FoundMatch
        {
            get
            {
                return m_FoundMatch;
            }
            set
            {
                m_FoundMatch = value;
            }
        }

        public string VariableName
        {
            get
            {
                return m_VariableName;
            }
            set
            {
                m_VariableName = value;
            }
        }

        public string VariableType
        {
            get
            {
                return m_VariableType;
            }
            set
            {
                m_VariableType = value;
            }
        }
    }
}