using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dbElements = SAHL.Tools.Workflow.Common.Database.WorkflowElements;
using xmlElements = SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class ProcessFromXmlGenerator
    {
        public dbElements.Process GenerateFromXml(xmlElements.Process xmlProcess, List<dbElements.SecurityGroup> fixedRoles)
        {
            dbElements.Process dbProcess = new dbElements.Process();
            dbProcess.Name = xmlProcess.Name;
            dbProcess.MapVersion = xmlProcess.MapVersion;
            dbProcess.CreateDate = DateTime.Now;
            dbProcess.Version = string.Format("CmdLine Publisher: {0}", DateTime.Now);
            dbProcess.IsLegacy = xmlProcess.Legacy.Equals("true", StringComparison.OrdinalIgnoreCase);
            dbProcess.ViewableOnUserInterfaceVersion = String.IsNullOrWhiteSpace(xmlProcess.ViewableOnUserInterfaceVersion) ? "2" : xmlProcess.ViewableOnUserInterfaceVersion;

            // add the global roles
            foreach (xmlElements.GlobalRole xmlGlobalRole in xmlProcess.Roles)
            {
                dbProcess.SecurityGroups.Add(new dbElements.SecurityGroup()
                {
                    Name = xmlGlobalRole.Name,
                    Description = xmlGlobalRole.Description,
                    IsDynamic = xmlGlobalRole.IsDynamic,
                    Process = dbProcess
                });
            }

            // place holder for archived states that we will need to fix up after ALL workflows and activities have been created
            Dictionary<dbElements.State, xmlElements.ArchiveState> archiveStates = new Dictionary<dbElements.State, xmlElements.ArchiveState>();
            Dictionary<dbElements.WorkFlowActivity, xmlElements.CallWorkflowActivity> workflowActivities = new Dictionary<dbElements.WorkFlowActivity, xmlElements.CallWorkflowActivity>();

            // add workflows
            foreach (xmlElements.Workflow xmlWorkflow in xmlProcess.Workflows)
            {
                dbElements.WorkFlow dbWorkflow = new dbElements.WorkFlow()
                {
                    Name = xmlWorkflow.Name,
                    Process = dbProcess,
                    StorageKey = xmlWorkflow.ClapperBoard.KeyVariable,
                    StorageTable = xmlWorkflow.SafeName,
                    GenericKeyTypeKey = xmlWorkflow.GenericKeyType,
                    DefaultSubject = xmlWorkflow.ClapperBoard.Subject,
                    WorkFlowIconId = 1,
                    CreateDate = DateTime.Now
                };

                dbProcess.WorkFlows.Add(dbWorkflow);

                // add the global roles first, for some reason they are mapped to the workflow level in the old designer
                foreach (dbElements.SecurityGroup globalRole in dbProcess.SecurityGroups)
                {
                    dbWorkflow.SecurityGroups.Add(new dbElements.SecurityGroup()
                    {
                        Name = globalRole.Name,
                        Description = globalRole.Description,
                        IsDynamic = globalRole.IsDynamic,
                        Process = dbProcess,
                        WorkFlow = dbWorkflow
                    });
                }

                // add the workflow roles
                foreach (xmlElements.WorkflowRole xmlWorkflowRole in xmlWorkflow.Roles)
                {
                    dbWorkflow.SecurityGroups.Add(new dbElements.SecurityGroup()
                    {
                        Name = xmlWorkflowRole.Name,
                        Description = xmlWorkflowRole.Description,
                        IsDynamic = xmlWorkflowRole.IsDynamic,
                        Process = dbProcess,
                        WorkFlow = dbWorkflow
                    });
                }

                // add the forms for the workflow
                foreach (xmlElements.CustomForm xmlForm in xmlWorkflow.CustomForms)
                {
                    dbWorkflow.Forms.Add(new dbElements.Form()
                    {
                        Name = xmlForm.Name,
                        Description = xmlForm.Description,
                        WorkFlow = dbWorkflow
                    });
                }

                // add the external activities to the workflow
                foreach (xmlElements.ExternalActivityDefinition xmlExtDef in xmlWorkflow.ExternalActivityDefinitions)
                {
                    dbElements.ExternalActivity dbExternalActivity = new dbElements.ExternalActivity()
                    {
                        Name = xmlExtDef.Name,
                        Description = xmlExtDef.Description,
                        WorkFlow = dbWorkflow
                    };

                    dbWorkflow.ExternalActivities.Add(dbExternalActivity);
                }

                // add the states for the workflow, we will have to come back and fix up the returnworkflow activities that do not yet exist
                foreach (xmlElements.AbstractNamedState xmlState in xmlWorkflow.States)
                {
                    // CommonStates have some special needs, we will handle them after all the standard activties have been added
                    if (xmlState is xmlElements.CommonState)
                    {
                        continue;
                    }

                    dbElements.State dbState = new dbElements.State()
                    {
                        Name = xmlState.Name,
                        WorkFlow = dbWorkflow,
                        ForwardState = xmlState is xmlElements.SystemState ? ((xmlElements.SystemState)xmlState).UseAutoForward : false,
                        StateType = GetStateTypeFromXmlState(xmlState),
                        X2ID = xmlState.X2ID
                    };

                    dbWorkflow.States.Add(dbState);

                    // archive state specific info to come back to after the activities are defined
                    xmlElements.ArchiveState xmlArchivedState = xmlState as xmlElements.ArchiveState;
                    if (xmlArchivedState != null)
                    {
                        archiveStates.Add(dbState, xmlArchivedState);
                    }

                    // setup the state worklists and security for user states
                    xmlElements.UserState xmlUserState = xmlState as xmlElements.UserState;
                    if (xmlUserState != null)
                    {
                        foreach (xmlElements.AbstractRole xmlRole in xmlUserState.WorkList)
                        {
                            dbState.WorkList.Add(new dbElements.StateWorkList()
                            {
                                SecurityGroup = GetSecurityGroupFromXmlRole(fixedRoles, xmlRole, dbProcess, dbWorkflow),
                                State = dbState
                            });
                        }
                        int i = 1;
                        foreach (xmlElements.CustomForm xmlForm in xmlUserState.Forms)
                        {
                            dbElements.Form form = GetFormFromWorkflow(dbWorkflow,xmlForm.Name);
                            dbState.Forms.Add(new dbElements.StateForm()
                            {
                                Form = form,
                                State = dbState,
                                FormOrder = i
                            });
                            i += 1;
                        }
                    }
                }

                // add the activities for the workflow
                foreach (xmlElements.AbstractActivity xmlActivity in xmlWorkflow.Activities)
                {
                    // Call Workflow activity has some special needs, its not really an activity,
                    // we can only complete configuring it once all workflows states and activities are completed
                    if (xmlActivity is xmlElements.CallWorkflowActivity)
                    {
                        dbElements.WorkFlowActivity dbCallWorkflowActivity = new dbElements.WorkFlowActivity()
                        {
                            Name = xmlActivity.Name,
                            WorkFlow = dbWorkflow,
                            State = GetStateFromXmlState(xmlActivity.FromStateNode, dbWorkflow),
                        };
                        workflowActivities.Add(dbCallWorkflowActivity, xmlActivity as xmlElements.CallWorkflowActivity);
                        dbWorkflow.CallWorkFlowActivities.Add(dbCallWorkflowActivity);
                        continue;
                    }

                    // what we do depends on whether the activity has been applied from a common state or not
                    if (xmlActivity.FromStateNode is xmlElements.CommonState)
                    {
                        xmlElements.CommonState xmlFromState = xmlActivity.FromStateNode as xmlElements.CommonState;

                        // get all the states that this activity is applied to
                        xmlElements.AbstractState oldFromState = null;
                        xmlElements.AbstractNamedState oldToState = null;
                        foreach (xmlElements.AbstractNamedState appliedState in xmlFromState.AppliesTo)
                        {
                            int maxPriority = GetMaxPriorityFromXmlState(xmlWorkflow, appliedState);
                            oldFromState = xmlActivity.FromStateNode;
                            xmlActivity.FromStateNode = appliedState;

                            // the tostate may also be common in a loopback situation
                            if (xmlActivity.ToStateNode is xmlElements.CommonState)
                            {
                                oldToState = xmlActivity.ToStateNode;
                                xmlActivity.ToStateNode = appliedState;
                            }

                            // set the activity to have the next priority
                            xmlActivity.Priority = maxPriority + 1;

                            CreateDBActivity(dbProcess, dbWorkflow, xmlActivity, fixedRoles, xmlActivity.X2ID);

                            if (oldFromState != null)
                            {
                                xmlActivity.FromStateNode = oldFromState;
                                oldFromState = null;
                            }
                            if (oldToState != null)
                            {
                                xmlActivity.ToStateNode = oldToState;
                                oldToState = null;
                            }
                        }
                    }
                    else
                    {
                        CreateDBActivity(dbProcess, dbWorkflow, xmlActivity, fixedRoles, xmlActivity.X2ID);
                    }
                }
            }

            // now go back and sort out the archive states
            foreach (KeyValuePair<dbElements.State, xmlElements.ArchiveState> kvp in archiveStates)
            {
                dbElements.State dbArchiveState = kvp.Key;
                xmlElements.ArchiveState xmlArchiveState = kvp.Value;

                if (xmlArchiveState.ReturnToWorkflow != null)
                {
                    dbArchiveState.ReturnWorkflow = GetWorkflowFromXmlWorkflow(dbProcess, xmlArchiveState.ReturnToWorkflow);
                    dbArchiveState.ReturnActivity = GetActivityFromXmlActivity(dbArchiveState.ReturnWorkflow, xmlArchiveState.ReturnActivity);
                }
            }

            // now go back and sort out the call workflow activities
            foreach (KeyValuePair<dbElements.WorkFlowActivity, xmlElements.CallWorkflowActivity> kvp in workflowActivities)
            {
                dbElements.WorkFlowActivity dbWorkflowActivity = kvp.Key;
                xmlElements.CallWorkflowActivity xmlWorkflowActivity = kvp.Value;

                if (xmlWorkflowActivity.WorkflowToCall != null)
                {
                    dbWorkflowActivity.NextWorkFlow = GetWorkflowFromXmlWorkflow(dbProcess, xmlWorkflowActivity.WorkflowToCall);
                    dbWorkflowActivity.NextActivity = GetActivityFromXmlActivity(dbWorkflowActivity.NextWorkFlow, xmlWorkflowActivity.ActivityToCall);
                    dbWorkflowActivity.ReturnActivity = GetActivityFromXmlActivity(dbWorkflowActivity.WorkFlow, xmlWorkflowActivity.ReturnActivity);
                }
            }

            // setup the process assemblies

            return dbProcess;
        }

        private dbElements.Activity GetActivityFromXmlActivity(dbElements.WorkFlow dbWorkFlow, xmlElements.AbstractActivity xmlAbstractActivity)
        {
            if (xmlAbstractActivity == null)
            {
                return null;
            }
            return dbWorkFlow.Activities.FirstOrDefault(x => x.Name == xmlAbstractActivity.Name);
        }

        private dbElements.WorkFlow GetWorkflowFromXmlWorkflow(dbElements.Process dbProcess, xmlElements.Workflow xmlWorkflow)
        {
            return dbProcess.WorkFlows.FirstOrDefault(x => x.Name == xmlWorkflow.Name);
        }

        private int GetMaxPriorityFromXmlState(xmlElements.Workflow xmlWorkflow, xmlElements.AbstractNamedState appliedState)
        {
            if (xmlWorkflow.Activities.Where(x => x.FromStateNode == appliedState).Any())
            {
                return xmlWorkflow.Activities.Where(x => x.FromStateNode == appliedState).Max(y => y.Priority);
            }
            else
                return 0;
        }

        private void CreateDBActivity(dbElements.Process dbProcess, dbElements.WorkFlow dbWorkflow, xmlElements.AbstractActivity xmlActivity, List<dbElements.SecurityGroup> fixedRoles,Guid x2ID)
        {
            dbElements.Activity dbActivity = new dbElements.Activity()
            {
                Name = xmlActivity.Name,
                WorkFlow = dbWorkflow,
                ActivityType = GetActivityTypeFromXmlActivity(xmlActivity),
                FromState = GetStateFromXmlState(xmlActivity.FromStateNode, dbWorkflow),
                ToState = GetStateFromXmlState(xmlActivity.ToStateNode, dbWorkflow),
                ActivityMessage = xmlActivity.Message,
                Priority = xmlActivity.Priority,
                SplitWorkFlow = xmlActivity.SplitWorkflow,
                RaiseExternalActivity = GetExternalActivityFromXmlExternalActivityDefinition(xmlActivity.RaiseExternalActivity, dbWorkflow),
                X2ID = x2ID
            };
            FillActivities(dbActivity, xmlActivity);
            xmlElements.ReturnWorkflowActivity xmlReturnWorkflowActivity = xmlActivity as xmlElements.ReturnWorkflowActivity;
            if (xmlReturnWorkflowActivity != null)
            {
                dbActivity.ToState = dbActivity.FromState;
            }

            // User Activity specific properties
            xmlElements.UserActivity xmlUserActivity = xmlActivity as xmlElements.UserActivity;
            if (xmlUserActivity != null)
            {
                dbActivity.Form = GetFormFromXmlForm(xmlUserActivity.CustomForm, dbWorkflow);

                // User Activity Security Groups
                foreach (xmlElements.AbstractRole xmlRole in xmlUserActivity.SecurityAccess)
                    dbActivity.Security.Add(new dbElements.ActivitySecurity()
                    {
                        Activity = dbActivity,
                        SecurityGroup = GetSecurityGroupFromXmlRole(fixedRoles, xmlRole, dbProcess, dbWorkflow)
                    });
            }

            // External Activity specific properties
            xmlElements.ExternalActivity xmlExtActivity = xmlActivity as xmlElements.ExternalActivity;
            if (xmlExtActivity != null)
            {
                dbActivity.ExternalActivityTarget = GetExtActTargetFromXmlExtActivity(xmlExtActivity.ExternalActivityTarget);
                dbActivity.ActivatedByExternalActivity = GetExternalActivityFromXmlExternalActivityDefinition(xmlExtActivity.InvokedBy, dbWorkflow);
            }

            dbWorkflow.Activities.Add(dbActivity);
        }

        private dbElements.SecurityGroup GetSecurityGroupFromXmlRole(List<dbElements.SecurityGroup> fixedRoles, xmlElements.AbstractRole xmlRole, dbElements.Process dbProcess, dbElements.WorkFlow dbWorkflow)
        {
            dbElements.SecurityGroup SG = null;
            // try the fixed roles first
            SG = fixedRoles.FirstOrDefault(x => x.Name == xmlRole.Name);
            if (SG == null)
            {
                // loop through process then the workflow roles
                SG = dbProcess.SecurityGroups.FirstOrDefault(x => x.Name == xmlRole.Name);
                if (SG == null)
                {
                    SG = dbWorkflow.SecurityGroups.FirstOrDefault(x => x.Name == xmlRole.Name);
                }
            }
            return SG;
        }

        private dbElements.Form GetFormFromWorkflow(dbElements.WorkFlow workflow,string name)
        {
            dbElements.Form form = (from f in workflow.Forms
                     where f.Name == name
                     select f).SingleOrDefault();
            return form;
        }

        private dbElements.ExternalActivityTarget GetExtActTargetFromXmlExtActivity(string externalActivityTargetString)
        {
            dbElements.ExternalActivityTarget result;
            dbElements.ExternalActivityTarget.TryParse(externalActivityTargetString, out result);
            return result;
        }

        private dbElements.ExternalActivity GetExternalActivityFromXmlExternalActivityDefinition(xmlElements.ExternalActivityDefinition xmlExtActivity, dbElements.WorkFlow dbWorkflow)
        {
            if (xmlExtActivity == null)
            {
                return null;
            }

            return dbWorkflow.ExternalActivities.FirstOrDefault(x => x.Name == xmlExtActivity.Name);
        }

        private dbElements.Form GetFormFromXmlForm(xmlElements.CustomForm xmlCustomForm, dbElements.WorkFlow dbWorkflow)
        {
            if (xmlCustomForm == null)
            {
                return null;
            }
            return dbWorkflow.Forms.FirstOrDefault(x => x.Name == xmlCustomForm.Name);
        }

        private dbElements.State GetStateFromXmlState(xmlElements.AbstractState xmlState, dbElements.WorkFlow dbWorkflow)
        {
            xmlElements.AbstractNamedState namedState = xmlState as xmlElements.AbstractNamedState;
            if (namedState != null)
            {
                return dbWorkflow.States.FirstOrDefault(x => x.Name == namedState.Name);
            }
            return null;
        }

        private dbElements.ActivityType GetActivityTypeFromXmlActivity(xmlElements.AbstractActivity xmlActivity)
        {
            if (xmlActivity is xmlElements.UserActivity)
            {
                return dbElements.ActivityType.User;
            }
            else
                if (xmlActivity is xmlElements.TimedActivity)
                {
                    return dbElements.ActivityType.Timed;
                }
                else
                    if (xmlActivity is xmlElements.ConditionalActivity)
                    {
                        return dbElements.ActivityType.Decision;
                    }
                    else
                        if (xmlActivity is xmlElements.ExternalActivity)
                        {
                            return dbElements.ActivityType.External;
                        }
                        else
                            if (xmlActivity is xmlElements.ReturnWorkflowActivity)
                            {
                                return dbElements.ActivityType.ReturnWorkflow;
                            }
                            else
                                if (xmlActivity is xmlElements.CallWorkflowActivity)
                                {
                                    return dbElements.ActivityType.CallWorkFlow;
                                }
                                else
                                    throw new Exception("ActivityType Not Found");
        }

        private dbElements.StateType GetStateTypeFromXmlState(xmlElements.AbstractNamedState xmlState)
        {
            if (xmlState is xmlElements.UserState)
            {
                return dbElements.StateType.User;
            }
            else
                if (xmlState is xmlElements.SystemState)
                {
                    return dbElements.StateType.System;
                }
                else
                    if (xmlState is xmlElements.ArchiveState)
                    {
                        return dbElements.StateType.Archive;
                    }
                    else
                        if (xmlState is xmlElements.HoldState)
                        {
                            return dbElements.StateType.Hold;
                        }
            throw new Exception("StateType Not Found");
        }

        public void FillActivities(dbElements.Activity activity,xmlElements.AbstractActivity xmlActivity)
        {
            foreach (xmlElements.BusinessStageTransition businessStageTransition in xmlActivity.BusinessStageTransitions)
            {
                dbElements.StageActivity stageActivity = new dbElements.StageActivity();
                stageActivity.Activity = activity;
                stageActivity.StageDefinitionStageDefinitionGroupKey = businessStageTransition.StageDefinitionStageDefinitionGroupKey;
                activity.StageActivities.Add(stageActivity);
            }
        }

        public void GetFileData(ref Byte[] data, string pathToFile)
        {
            using (FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                data = new byte[(int)fs.Length];

                fs.Read(data, 0, (int)fs.Length);
            }
        }
    }
}