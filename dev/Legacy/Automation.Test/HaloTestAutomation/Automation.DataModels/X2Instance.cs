using Common.Enums;
using System;

namespace Automation.DataModels
{
    public sealed class X2Instance : IDataModel
    {
        public Int64 InstanceId { get; set; }

        public bool IsParentInstance { get; set; }

        public bool IsSourceInstance { get; set; }

        public int StateID { get; set; }

        public bool RequireValuation { get; set; }

        public int GenericKey { get; set; }

        public GenericKeyTypeEnum GenericKeyTypeKey { get; set; }

        public X2Workflow Workflow { get; set; }

        public string WorkflowName { get; set; }

        public X2State State { get; set; }
    }
}