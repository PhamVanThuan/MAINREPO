using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore
{
    public class XmlStreamPersistanceStore : IWorkflowPersistanceStore
    {
        private Stream xmlStream;
        private List<IVersionedXmlPersistanceStore> versionedStores;

        public XmlStreamPersistanceStore(Stream xmlStream)
        {
            this.xmlStream = xmlStream;
            this.versionedStores = new List<IVersionedXmlPersistanceStore>(new IVersionedXmlPersistanceStore[] { new ver0_3.ver0_3_XmlPersistanceStore() });
        }

        public void PersistProcess(Process processToPersist)
        {
            throw new NotImplementedException();
        }

        public Process LoadProcess()
        {
            XDocument xmlDocument = XDocument.Load(this.xmlStream);

            List<PostLoadRequirement> postLoadRequirements = new List<PostLoadRequirement>();

            // determine the xml version
            IVersionedXmlPersistanceStore persistanceStore = this.GetSupportedVersionedStore(xmlDocument);

            // load the process
            XElement processElement = xmlDocument.Element(persistanceStore.NodeNameManager.ProcessElementName);
            Process process = persistanceStore.ReadProcessFromXml(processElement);

            // get the usingstatements

            // add the extra weird usings that the designer automagically inserts
            UsingStatement statementExtra = new UsingStatement("System");
            if (!process.UsingStatements.Contains(statementExtra))
                process.AddUsingStatement(statementExtra);

            statementExtra = new UsingStatement("System.Collections.Generic");
            if (!process.UsingStatements.Contains(statementExtra))
                process.AddUsingStatement(statementExtra);

            statementExtra = new UsingStatement("System.Data");
            if (!process.UsingStatements.Contains(statementExtra))
                process.AddUsingStatement(statementExtra);

            statementExtra = new UsingStatement("System.Data.SqlClient");
            if (!process.UsingStatements.Contains(statementExtra))
                process.AddUsingStatement(statementExtra);

            XElement usingsElement = processElement.Element(persistanceStore.NodeNameManager.UsingStatementsElement);
            foreach (XElement usingElement in usingsElement.Elements())
            {
                UsingStatement statement = persistanceStore.ReadUsingStatementFromXml(usingElement);
                if (!process.UsingStatements.Contains(statement))
                    process.AddUsingStatement(statement);
            }

            // get the roles
            XElement rolesElement = processElement.Element(persistanceStore.NodeNameManager.RolesElement);
            foreach (XElement roleElement in rolesElement.Elements())
            {
                GlobalRole role = persistanceStore.ReadAbstractRoleFromXml(roleElement, postLoadRequirements) as GlobalRole;
                if (role != null)
                {
                    process.AddRole(role);
                }
            }

            // get the assembly references
            XElement assembliesElement = processElement.Element(persistanceStore.NodeNameManager.AssemblyReferencesElement);
            foreach (XElement assemblyElement in assembliesElement.Elements())
            {
                AssemblyReference reference = persistanceStore.ReadAssemblyReferenceFromXml(assemblyElement);
                process.AddAssemblyReference(reference);
            }

            // workflows
            XElement workflowsElement = processElement.Element(persistanceStore.NodeNameManager.WorkflowsElementName);
            foreach (XElement workflowElement in workflowsElement.Elements())
            {
                SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow = persistanceStore.ReadWorkflowFromXml(workflowElement);
                process.AddWorkflow(workflow);

                // add the fixed roles to the workflow
                workflow.AddRole(new FixedRole("Originator", "Originator Fixed Role", false));
                workflow.AddRole(new FixedRole("Everyone", "Everyone Fixed Role", false));
                workflow.AddRole(new FixedRole("WorkList", "WorkList Fixed Role", false));
                workflow.AddRole(new FixedRole("TrackList", "TrackList Fixed Role", false));

                // get the clapperboard
                XElement clapperBoardXml = workflowElement.Element(persistanceStore.NodeNameManager.ClapperBoardElementName);
                workflow.ClapperBoard = persistanceStore.ReadClapperBoardFromXml(clapperBoardXml, postLoadRequirements);

                // load the custom variables for the workflow
                XElement customVariableXmls = workflowElement.Element(persistanceStore.NodeNameManager.CustomVariablesElementName);
                foreach (XElement customVariableXml in customVariableXmls.Elements())
                {
                    AbstractCustomVariable variable = persistanceStore.ReadCustomVariableFromXml(customVariableXml);
                    workflow.AddCustomVariable(variable);
                }

                // load the external activity definitions
                XElement externalActivityXmls = workflowElement.Element(persistanceStore.NodeNameManager.ExternalActivitiesElementName);
                foreach (XElement externalActivityXml in externalActivityXmls.Elements())
                {
                    ExternalActivityDefinition externalActivityDef = persistanceStore.ReadExternalActivityDefinitionFromXml(externalActivityXml);
                    workflow.AddExternalActivityDefinition(externalActivityDef);
                }

                // load the custom forms
                XElement customFormsXmls = workflowElement.Element(persistanceStore.NodeNameManager.CustomFormsElementName);
                foreach (XElement customFormXml in customFormsXmls.Elements())
                {
                    CustomForm customForm = persistanceStore.ReadCustomFormFromXml(customFormXml);
                    workflow.AddCustomForm(customForm);
                }

                // load the workflow states
                XElement stateXmls = workflowElement.Element(persistanceStore.NodeNameManager.StatesElementName);
                foreach (XElement stateXml in stateXmls.Elements())
                {
                    AbstractNamedState state = persistanceStore.ReadStateFromXml(stateXml, postLoadRequirements);
                    workflow.AddState(state);
                    state.Workflow = workflow;
                }

                // load the workflow activities
                XElement activityXmls = workflowElement.Element(persistanceStore.NodeNameManager.ActivitiesElementName);
                foreach (XElement activityXml in activityXmls.Elements())
                {
                    AbstractActivity activity = persistanceStore.ReadAbstractActivityFromXml(activityXml, postLoadRequirements);
                    workflow.AddActivity(activity);
                    activity.Workflow = workflow;

                    XElement businessStageTransitionsXml = activityXml.Element(persistanceStore.NodeNameManager.BusinessStageTransitionsElement);
                    if (businessStageTransitionsXml != null)
                    {
                        foreach (XElement businessStageTransitionXml in businessStageTransitionsXml.Elements())
                        {
                            BusinessStageTransition transition = persistanceStore.ReadBusinessStageTransitionFromXml(businessStageTransitionXml);
                            activity.AddBusinessStageTransition(transition);
                        }
                    }
                }
            }

            // satisfy the process post load requirements
            foreach (PostLoadRequirement postLoadRequirement in postLoadRequirements)
            {
                if (postLoadRequirement.Element is WorkflowRole)
                {
                    WorkflowRole wfRole = postLoadRequirement.Element as WorkflowRole;

                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.WorkFlowAttribute)
                    {
                        // get the relevant workflow
                        SAHL.Tools.Workflow.Common.WorkflowElements.Workflow wf = process.Workflows.Where(x => x.Name == postLoadRequirement.RequirementValue).SingleOrDefault();
                        if (wf != null)
                        {
                            wfRole.UpdateApplicableWorkflow(wf);
                            wf.AddRole(wfRole);
                        }
                    }
                }
                else if (postLoadRequirement.Element is UserState)
                {
                    UserState state = postLoadRequirement.Element as UserState;
                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.WorklistItemAttribute)
                    {
                        // we need to add a worklist role
                        AbstractRole worklistRole = GetWorkListRoleFromName(postLoadRequirement.RequirementValue, state.Workflow, process);
                        // this check shouln't be required however tyhe designer is rubbish and leaves roles in the xml after they are removed.
                        if (worklistRole != null)
                        {
                            state.AddRoleToWorkList(worklistRole);
                        }
                    }
                    else if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.FormNameAttribute)
                    {
                        CustomForm form = GetFormFromName(postLoadRequirement.RequirementValue, state.Workflow, process);
                        if (form != null)
                        {
                            state.AddCustomForm(form);
                        }
                    }
                }
                else if (postLoadRequirement.Element is AbstractActivity)
                {
                    AbstractActivity activity = postLoadRequirement.Element as AbstractActivity;
                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.FromNodeAttribute)
                    {
                        activity.FromStateNode = activity.Workflow.States.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                    }
                    else if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ToNodeAttribute)
                    {
                        activity.ToStateNode = activity.Workflow.States.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                    }
                    else if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.RaiseExternalActivityAttribute)
                    {
                        activity.RaiseExternalActivity = activity.Workflow.ExternalActivityDefinitions.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                    }

                    if (postLoadRequirement.Element is UserActivity)
                    {
                        UserActivity userActivity = postLoadRequirement.Element as UserActivity;
                        if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ActivityAccessAttribute)
                        {
                            // we need to add an activity access role
                            AbstractRole activityAccessRole = GetWorkListRoleFromName(postLoadRequirement.RequirementValue, userActivity.Workflow, process);
                            // this check shouln't be required however tyhe designer is rubbish and leaves roles in the xml after they are removed.
                            if (activityAccessRole != null)
                            {
                                userActivity.AddRoleToSecurityAccess(activityAccessRole);
                            }
                        }
                        else
                            if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.CustomFormAttribute)
                            {
                                CustomForm customForm = userActivity.Workflow.CustomForms.Where(x => x.Name == postLoadRequirement.RequirementValue).SingleOrDefault();
                                userActivity.CustomForm = customForm;
                            }
                    }
                    else if (postLoadRequirement.Element is CallWorkflowActivity)
                    {
                        CallWorkflowActivity callWorkflowActivity = postLoadRequirement.Element as CallWorkflowActivity;
                        if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.WorkFlowToCallAttribute)
                        {
                            callWorkflowActivity.WorkflowToCall = process.Workflows.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                        }
                        else
                            if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ReturnActivityAttribute)
                            {
                                callWorkflowActivity.ReturnActivity = callWorkflowActivity.Workflow.Activities.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                            }
                            else
                                if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ActivityToCallAttribute)
                                {
                                    callWorkflowActivity.ActivityToCall = callWorkflowActivity.WorkflowToCall.Activities.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                                }
                    }
                    else if (postLoadRequirement.Element is ExternalActivity)
                    {
                        ExternalActivity externalActivity = postLoadRequirement.Element as ExternalActivity;
                        if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.InvokedByAttribute)
                        {
                            externalActivity.InvokedBy = activity.Workflow.ExternalActivityDefinitions.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                        }
                        else if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.InvokeTargetAttribute)
                        {
                            externalActivity.ExternalActivityTarget = postLoadRequirement.RequirementValue;
                        }
                    }
                }
                else if (postLoadRequirement.Element is ClapperBoard)
                {
                    ClapperBoard clapperBoard = postLoadRequirement.Element as ClapperBoard;
                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.LimitAccessToAttribute)
                    {
                        // we need to add a global role
                        GlobalRole accessRole = process.Roles.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                        clapperBoard.UpdateLimitAccessToRole(accessRole);
                    }
                }
                else if (postLoadRequirement.Element is CommonState)
                {
                    CommonState state = postLoadRequirement.Element as CommonState;
                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.AppliesToAttributeName)
                    {
                        AbstractNamedState appliesState = state.Workflow.States.Where(x => x.Name == postLoadRequirement.RequirementValue).SingleOrDefault();
                        state.ApplyToState(appliesState);
                    }
                }
                else if (postLoadRequirement.Element is ArchiveState)
                {
                    ArchiveState archiveState = postLoadRequirement.Element as ArchiveState;
                    if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ReturnWorkflowAttribute)
                    {
                        archiveState.ReturnToWorkflow = process.Workflows.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                    }
                    else
                        if (postLoadRequirement.RequirementName == persistanceStore.NodeNameManager.ReturnActivityAttribute)
                        {
                            archiveState.ReturnActivity = archiveState.ReturnToWorkflow.Activities.FirstOrDefault(x => x.Name == postLoadRequirement.RequirementValue);
                        }
                }
            }

            return process;
        }

        private CustomForm GetFormFromName(string roleName, SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow, Process process)
        {
            // loop through the process and workflow roles in order to find the role
            CustomForm form = workflow.CustomForms.FirstOrDefault(x => x.Name == roleName);
            return form;
        }

        private AbstractRole GetWorkListRoleFromName(string roleName, SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow, Process process)
        {
            // loop through the process and workflow roles in order to find the role
            AbstractRole role = workflow.Roles.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                role = process.Roles.FirstOrDefault(x => x.Name == roleName);
            }
            return role;
        }

        public void Dispose()
        {
            this.versionedStores.Clear();
            this.versionedStores = null;
        }

        private IVersionedXmlPersistanceStore GetSupportedVersionedStore(XDocument xmlDocument)
        {
            foreach (IVersionedXmlPersistanceStore verStore in this.versionedStores)
            {
                if (verStore.IsXMLDocumentSupported(xmlDocument))
                {
                    return verStore;
                }
            }

            return null;
        }
    }
}