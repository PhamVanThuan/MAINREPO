using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3
{
    public class ver0_3_XmlPersistanceStore : IVersionedXmlPersistanceStore
    {
        private ver0_3_XmlNodeNameManager nameManager;

        public ver0_3_XmlPersistanceStore()
        {
            this.nameManager = new ver0_3_XmlNodeNameManager();
        }

        public XmlPersistanceVersionEnum VersionSupported
        {
            get { return XmlPersistanceVersionEnum.ver0_3; }
        }

        public IVersionedNodeNameManager NodeNameManager { get { return this.nameManager; } }

        public bool IsXMLDocumentSupported(XDocument xmlDocument)
        {
            try
            {
                if (xmlDocument.Elements(this.NodeNameManager.ProcessElementName).Attributes(this.NodeNameManager.ProductVersionAttributeName).Single().Value == GetStringFromVersion(this.VersionSupported))
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public virtual Process ReadProcessFromXml(XElement xmlElement)
        {
            Process result = null;
            try
            {
                result = new Process(xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value,
                                             xmlElement.Attribute(this.NodeNameManager.ProductVersionAttributeName).Value,
                                             xmlElement.Attribute(this.NodeNameManager.MapVersionAttributeName).Value,
                                             xmlElement.Attribute(this.NodeNameManager.RetrievedAttributeName).Value,
                                             xmlElement.Attribute(this.NodeNameManager.Legacy) == null ? "true" : xmlElement.Attribute(this.NodeNameManager.Legacy).Value,
                                             xmlElement.Attribute(this.NodeNameManager.ViewableOnUserInterfaceVersion) == null ? "2" : xmlElement.Attribute(this.NodeNameManager.ViewableOnUserInterfaceVersion).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }
            return result;
        }

        public virtual XElement WriteProcessToXml(Process element)
        {
            throw new NotImplementedException();
        }

        public virtual SAHL.Tools.Workflow.Common.WorkflowElements.Workflow ReadWorkflowFromXml(XElement xmlElement)
        {
            SAHL.Tools.Workflow.Common.WorkflowElements.Workflow result = null;
            try
            {
                result = new SAHL.Tools.Workflow.Common.WorkflowElements.Workflow(xmlElement.Attribute(this.NodeNameManager.WorkflowNameAttribute).Value,
                                             Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationXAttribute).Value),
                                             Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationYAttribute).Value),
                                             Convert.ToInt32(xmlElement.Attribute(this.NodeNameManager.GenericKeyTypeKey).Value));
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public virtual XElement WriteWorkflowToXml(SAHL.Tools.Workflow.Common.WorkflowElements.Workflow element)
        {
            throw new NotImplementedException();
        }

        public AbstractCustomVariable ReadCustomVariableFromXml(XElement xmlElement)
        {
            try
            {
                CustomVariableTypeEnum varType = ConvertFromString(xmlElement.Attribute(this.NodeNameManager.TypeAttributeName).Value);
                string name = xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value;
                int length = Convert.ToInt32(xmlElement.Attribute(this.NodeNameManager.LengthAttributeName).Value);

                switch (varType)
                {
                    case CustomVariableTypeEnum.BigInteger:
                        return new BigIntegerVariable(name);
                    case CustomVariableTypeEnum.Bool:
                        return new BoolVariable(name);
                    case CustomVariableTypeEnum.DateTime:
                        return new DateTimeVariable(name);
                    case CustomVariableTypeEnum.Decimal:
                        return new DecimalVariable(name);
                    case CustomVariableTypeEnum.Double:
                        return new DoubleVariable(name);
                    case CustomVariableTypeEnum.Integer:
                        return new IntegerVariable(name);
                    case CustomVariableTypeEnum.Single:
                        return new SingleVariable(name);
                    case CustomVariableTypeEnum.String:
                        return new StringVariable(name, length);
                    case CustomVariableTypeEnum.Text:
                        return new TextVariable(name, length);
                    default:
                        throw new Exception("Cannot convert Type");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }
        }

        private CustomVariableTypeEnum ConvertFromString(string variableType)
        {
            if (variableType == this.NodeNameManager.Bool)
                return CustomVariableTypeEnum.Bool;
            if (variableType == this.NodeNameManager.BigInteger)
                return CustomVariableTypeEnum.BigInteger;
            if (variableType == this.NodeNameManager.DateTime)
                return CustomVariableTypeEnum.DateTime;
            if (variableType == this.NodeNameManager.Decimal)
                return CustomVariableTypeEnum.Decimal;
            if (variableType == this.NodeNameManager.Double)
                return CustomVariableTypeEnum.Double;
            if (variableType == this.NodeNameManager.Integer)
                return CustomVariableTypeEnum.Integer;
            if (variableType == this.NodeNameManager.Single)
                return CustomVariableTypeEnum.Single;
            if (variableType == this.NodeNameManager.String)
                return CustomVariableTypeEnum.String;
            if (variableType == this.NodeNameManager.Text)
                return CustomVariableTypeEnum.Text;

            throw new Exception("Could not convert type from string");
        }

        private string GetStringFromVersion(XmlPersistanceVersionEnum version)
        {
            string enumString = version.ToString();
            return enumString.Remove(0, 3).Replace("_", ".");
        }

        public XElement WriteCustomVariableToXml(AbstractCustomVariable element)
        {
            throw new NotImplementedException();
        }

        public ExternalActivityDefinition ReadExternalActivityDefinitionFromXml(XElement xmlElement)
        {
            ExternalActivityDefinition result = null;
            try
            {
                result = new ExternalActivityDefinition(xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value,
                                             xmlElement.Attribute(this.NodeNameManager.DescriptionAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteExternalActivityDefinitionToXml(ExternalActivityDefinition element)
        {
            throw new NotImplementedException();
        }

        public CustomForm ReadCustomFormFromXml(XElement xmlElement)
        {
            CustomForm result = null;
            try
            {
                result = new CustomForm(xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value,
                                             xmlElement.Attribute(this.NodeNameManager.DescriptionAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteCustomFormToXml(CustomForm element)
        {
            throw new NotImplementedException();
        }

        public ClapperBoard ReadClapperBoardFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements)
        {
            ClapperBoard result = null;
            try
            {
                string roleName = xmlElement.Attribute(this.NodeNameManager.LimitAccessToAttribute).Value;

                result = new ClapperBoard(xmlElement.Attribute(this.NodeNameManager.KeyVariableAttribute).Value,
                                             xmlElement.Attribute(this.NodeNameManager.SubjectAttribute).Value,
                                             null,
                                             Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationXAttribute).Value),
                                             Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationYAttribute).Value));
                postLoadRequirements.Add(new PostLoadRequirement(result, this.NodeNameManager.LimitAccessToAttribute, roleName));
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }
            return result;
        }

        public XElement WriteClapperBoardToXml(ClapperBoard element)
        {
            throw new NotImplementedException();
        }

        public AbstractNamedState ReadStateFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements)
        {
            AbstractNamedState result = null;
            try
            {
                string stateName = xmlElement.Attribute(this.NodeNameManager.StateNameAttribute).Value;
                string stateType = xmlElement.Attribute(this.NodeNameManager.TypeAttributeName).Value;
                
                Guid x2ID = Guid.Empty;
                XAttribute X2IDAttribute = xmlElement.Attribute(this.NodeNameManager.X2ID);
                if (X2IDAttribute != null)
                {
                    x2ID = Guid.Parse(X2IDAttribute.Value);
                }

                Single locationX = Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationXAttribute).Value);
                Single locationY = Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationYAttribute).Value);

                CodeSection onEnter = null;
                CodeSection onExit = null;
                CodeSection onAutoForward = null;
                CodeSection onReturn = null;

                // get the code sections
                XElement codeSectionsXml = xmlElement.Element(NodeNameManager.CodeSectionsElementName);
                foreach (XElement codeSectionXml in codeSectionsXml.Elements())
                {
                    CodeSection codeSection = this.ReadCodeSectionFromXml(codeSectionXml);
                    if (codeSection.Name == this.NodeNameManager.OnEnterCodeSectionName)
                    {
                        onEnter = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.OnExitCodeSectionName)
                    {
                        onExit = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.AutoForwardToCodeSection)
                    {
                        onAutoForward = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.OnReturnCodeSectionName)
                    {
                        onReturn = codeSection;
                    }
                }

                if (stateType == this.NodeNameManager.UserStateType)
                {
                    UserState userState = new UserState(stateName, locationX, locationY, onEnter, onExit, x2ID);

                    // load the worklist
                    XElement workListXml = xmlElement.Element(NodeNameManager.WorkListElement);
                    foreach (XElement workListItem in workListXml.Elements())
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(userState, this.NodeNameManager.WorklistItemAttribute, workListItem.Attribute(this.NodeNameManager.WorklistItemAttribute).Value));
                    }

                    XElement customForms = xmlElement.Element(NodeNameManager.CustomFormsElementName);
                    foreach (XElement customFormCollectionElement in customForms.Elements())
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(userState, this.NodeNameManager.FormNameAttribute, customFormCollectionElement.Attribute(this.NodeNameManager.FormNameAttribute).Value));
                    }

                    return userState;
                }
                if (stateType == this.NodeNameManager.SystemStateType)
                    return new SystemState(stateName, locationX, locationY, Convert.ToBoolean(xmlElement.Attribute(this.NodeNameManager.UseAutoForwardAttribute).Value), onEnter, onExit, onAutoForward, x2ID);
                if (stateType == this.NodeNameManager.CommonStateType)
                {
                    CommonState state = new CommonState(stateName, locationX, locationY, onEnter, onExit, x2ID);
                    XElement appliesToElement = xmlElement.Element(NodeNameManager.AppliesToElementName);
                    foreach (XElement appliesToItem in appliesToElement.Elements())
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(state, this.NodeNameManager.AppliesToAttributeName, appliesToItem.Attribute(this.NodeNameManager.AppliesToAttributeName).Value));
                    }
                    return state;
                }
                if (stateType == this.NodeNameManager.HoldStateType)
                    return new HoldState(stateName, locationX, locationY, onEnter, onExit, x2ID);
                if (stateType == this.NodeNameManager.ArchiveStateType)
                {
                    ArchiveState state = new ArchiveState(stateName, locationX, locationY, onEnter, onExit, onReturn, x2ID);
                    if (!string.IsNullOrEmpty(xmlElement.Attribute(this.NodeNameManager.ReturnWorkflowAttribute).Value))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(state, this.NodeNameManager.ReturnWorkflowAttribute, xmlElement.Attribute(this.NodeNameManager.ReturnWorkflowAttribute).Value));
                        postLoadRequirements.Add(new PostLoadRequirement(state, this.NodeNameManager.ReturnActivityAttribute, xmlElement.Attribute(this.NodeNameManager.ReturnActivityAttribute).Value));
                    }
                    return state;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteStateToXml(CustomForm element)
        {
            throw new NotImplementedException();
        }

        public CodeSection ReadCodeSectionFromXml(XElement xmlElement)
        {
            CodeSection result = null;
            try
            {
                result = new CodeSection(xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value, xmlElement.Attribute(this.NodeNameManager.CodeAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteCodeSectionToXml(CodeSection element)
        {
            throw new NotImplementedException();
        }

        public AbstractActivity ReadAbstractActivityFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements)
        {
            try
            {
                string activityName = xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value;
                string activityType = xmlElement.Attribute(this.NodeNameManager.TypeAttributeName).Value;
                Single locationX = Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationXAttribute).Value);
                Single locationY = Convert.ToSingle(xmlElement.Attribute(this.NodeNameManager.LocationYAttribute).Value);
                string message = xmlElement.Attribute(this.NodeNameManager.MessageAttribute).Value;
                int priority = Convert.ToInt32(xmlElement.Attribute(this.NodeNameManager.PriorityAttribute).Value);
                string raiseExternalActivity = xmlElement.Attribute(this.NodeNameManager.RaiseExternalActivityAttribute).Value;
                bool splitWorkflow = Convert.ToBoolean(xmlElement.Attribute(this.NodeNameManager.SplitWorkFlowAttribute).Value);

                CodeSection onStartCode = null;
                CodeSection onCompleteCode = null;
                CodeSection onGetMessageCode = null;
                CodeSection onGetStageTransitionCode = null;
                CodeSection onGetActivityTimeCode = null;

                Guid x2ID = Guid.Empty;
                XAttribute X2IDAttribute = xmlElement.Attribute(this.NodeNameManager.X2ID);
                if (X2IDAttribute != null)
                {
                    x2ID = Guid.Parse(X2IDAttribute.Value);
                }

                // get the code sections
                XElement codeSectionsXml = xmlElement.Element(NodeNameManager.CodeSectionsElementName);
                foreach (XElement codeSectionXml in codeSectionsXml.Elements())
                {
                    CodeSection codeSection = this.ReadCodeSectionFromXml(codeSectionXml);
                    if (codeSection.Name == this.NodeNameManager.OnStartCodeSectionName)
                    {
                        onStartCode = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.OnCompleteCodeSectionName)
                    {
                        onCompleteCode = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.GetActivityMessageCodeSection)
                    {
                        onGetMessageCode = codeSection;
                    }
                    // this is a temporary hack
                    if (codeSection.Name == "GetActivityMessage")
                    {
                        onGetMessageCode = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.OnStageActivityCodeSectionName)
                    {
                        onGetStageTransitionCode = codeSection;
                    }
                    if (codeSection.Name == this.NodeNameManager.OnTimedActivityCodeSection)
                    {
                        onGetActivityTimeCode = codeSection;
                    }
                }

                string fromStateName = xmlElement.Attribute(this.NodeNameManager.FromNodeAttribute).Value;
                string toStateName = xmlElement.Attribute(this.NodeNameManager.ToNodeAttribute).Value;

                if (activityType == this.NodeNameManager.UserActivityType)
                {
                    UserActivity activity = new UserActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetMessageCode, onGetStageTransitionCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;
                    string customFormName = xmlElement.Attribute(this.NodeNameManager.CustomFormAttribute).Value;

                    if (!string.IsNullOrEmpty(customFormName))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.CustomFormAttribute, customFormName));
                    }
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));

                    if (!string.IsNullOrEmpty(raiseExternalActivity))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.RaiseExternalActivityAttribute, raiseExternalActivity));
                    }

                    // read the activity security access
                    XElement accessXml = xmlElement.Element(NodeNameManager.ActivityAccessElement);
                    foreach (XElement accessItem in accessXml.Elements())
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ActivityAccessAttribute, accessItem.Attribute(this.NodeNameManager.ActivityAccessAttribute).Value));
                    }
                    return activity;
                }
                if (activityType == this.NodeNameManager.TimedActivityType)
                {
                    TimedActivity activity = new TimedActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetStageTransitionCode, onGetActivityTimeCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));

                    if (!string.IsNullOrEmpty(raiseExternalActivity))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.RaiseExternalActivityAttribute, raiseExternalActivity));
                    }

                    return activity;
                }
                if (activityType == this.NodeNameManager.ConditionalActivityType)
                {
                    ConditionalActivity activity = new ConditionalActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetStageTransitionCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));

                    if (!string.IsNullOrEmpty(raiseExternalActivity))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.RaiseExternalActivityAttribute, raiseExternalActivity));
                    }
                    return activity;
                }
                if (activityType == this.NodeNameManager.ExternalActivityType)
                {
                    ExternalActivity activity = new ExternalActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetStageTransitionCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));

                    if (!string.IsNullOrEmpty(raiseExternalActivity))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.RaiseExternalActivityAttribute, raiseExternalActivity));
                    }

                    string invokedByActivity = xmlElement.Attribute(this.NodeNameManager.InvokedByAttribute).Value;
                    string invokeTarget = xmlElement.Attribute(this.NodeNameManager.InvokeTargetAttribute).Value;

                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.InvokedByAttribute, invokedByActivity));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.InvokeTargetAttribute, invokeTarget));
                    return activity;
                }
                if (activityType == this.NodeNameManager.CallWorkflowActivityType)
                {
                    CallWorkflowActivity activity = new CallWorkflowActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetStageTransitionCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;

                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));

                    if (!string.IsNullOrEmpty(xmlElement.Attribute(this.NodeNameManager.WorkFlowToCallAttribute).Value))
                    {
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.WorkFlowToCallAttribute, xmlElement.Attribute(this.NodeNameManager.WorkFlowToCallAttribute).Value));
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ReturnActivityAttribute, xmlElement.Attribute(this.NodeNameManager.ReturnActivityAttribute).Value));
                        postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ActivityToCallAttribute, xmlElement.Attribute(this.NodeNameManager.ActivityToCallAttribute).Value));
                    }

                    return activity;
                }
                if (activityType == this.NodeNameManager.ReturnWorkflowActivityType)
                {
                    ReturnWorkflowActivity activity = new ReturnWorkflowActivity(activityName, locationX, locationY, onStartCode, onCompleteCode, onGetStageTransitionCode, x2ID);
                    activity.Message = message;
                    activity.Priority = priority;
                    activity.SplitWorkflow = splitWorkflow;
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.FromNodeAttribute, fromStateName));
                    postLoadRequirements.Add(new PostLoadRequirement(activity, this.NodeNameManager.ToNodeAttribute, toStateName));
                    return activity;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return null;
        }

        public XElement WriteAbstractActivityToXml(AbstractActivity element)
        {
            throw new NotImplementedException();
        }

        public AbstractRole ReadAbstractRoleFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements)
        {
            AbstractRole result = null;
            try
            {
                string roleName = xmlElement.Attribute(this.NodeNameManager.RoleAttribute).Value;
                string roleType = xmlElement.Attribute(this.NodeNameManager.RoleTypeAttribute).Value;
                string roleDescription = xmlElement.Attribute(this.NodeNameManager.DescriptionAttribute).Value;
                bool isDynamic = Convert.ToBoolean(xmlElement.Attribute(this.NodeNameManager.IsDynamicAttribute).Value); ;

                CodeSection onGetRoleCode = null;

                // get the code sections
                XElement codeSectionsXml = xmlElement.Element(NodeNameManager.CodeSectionsElementName);
                if (codeSectionsXml != null)
                {
                    foreach (XElement codeSectionXml in codeSectionsXml.Elements())
                    {
                        CodeSection codeSection = this.ReadCodeSectionFromXml(codeSectionXml);
                        if (codeSection.Name == this.NodeNameManager.OnGetRoleCodeSectionName)
                        {
                            onGetRoleCode = codeSection;
                        }
                    }
                }

                if (roleType == this.NodeNameManager.GlobalRoleType)
                {
                    GlobalRole globalRole = new GlobalRole(roleName, isDynamic, onGetRoleCode);
                    globalRole.Description = roleDescription;
                    return globalRole;
                }
                if (roleType == this.NodeNameManager.WorkflowRoleType)
                {
                    WorkflowRole workflowRole = new WorkflowRole(roleName, isDynamic, onGetRoleCode);
                    workflowRole.Description = roleDescription;
                    postLoadRequirements.Add(new PostLoadRequirement(workflowRole, this.NodeNameManager.WorkFlowAttribute, xmlElement.Attribute(this.NodeNameManager.WorkFlowAttribute).Value));
                    return workflowRole;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteAbstractRoleToXml(AbstractRole element)
        {
            throw new NotImplementedException();
        }

        public UsingStatement ReadUsingStatementFromXml(XElement xmlElement)
        {
            UsingStatement result = null;
            try
            {
                result = new UsingStatement(xmlElement.Attribute(this.NodeNameManager.StatementAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteUsingStatementToXml(UsingStatement element)
        {
            throw new NotImplementedException();
        }

        public AssemblyReference ReadAssemblyReferenceFromXml(XElement xmlElement)
        {
            AssemblyReference result = null;
            try
            {
                result = new AssemblyReference(xmlElement.Attribute(this.NodeNameManager.NameAttribute).Value,
                                                xmlElement.Attribute(this.NodeNameManager.FullNameAttribute).Value,
                                                xmlElement.Attribute(this.NodeNameManager.PathAttribute).Value,
                                                xmlElement.Attribute(this.NodeNameManager.VersionAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteAssemblyReferenceToXml(AssemblyReference element)
        {
            throw new NotImplementedException();
        }

        public BusinessStageTransition ReadBusinessStageTransitionFromXml(XElement xmlElement)
        {
            BusinessStageTransition result = null;
            try
            {
                result = new BusinessStageTransition(Convert.ToInt32(xmlElement.Attribute(this.NodeNameManager.StageDefinitionKeyAttribute).Value),
                                                    xmlElement.Attribute(this.NodeNameManager.DefinitionGroupDescriptionAttribute).Value,
                                                    xmlElement.Attribute(this.NodeNameManager.DefinitionDescriptionAttribute).Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading from xml.", ex);
            }

            return result;
        }

        public XElement WriteBusinessStageTransitionToXml(BusinessStageTransition element)
        {
            throw new NotImplementedException();
        }
    }
}