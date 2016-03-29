using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Templates
{
    partial class X2WorkflowData
    {
        private SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow;

        public X2WorkflowData(SAHL.Tools.Workflow.Common.WorkflowElements.Workflow workflow)
        {
            this.workflow = workflow;
        }

        public bool RequiresNullCheck(CustomVariableTypeEnum customVariableType)
        {
            if (customVariableType == CustomVariableTypeEnum.Text || customVariableType == CustomVariableTypeEnum.String)
            {
                return true;
            }

            return false;
        }

        public string SqlDataTypeFromCustomVariableType(CustomVariableTypeEnum customVariableType)
        {
            switch (customVariableType)
            {
                case CustomVariableTypeEnum.Bool:
                    return "SqlDbType.Bit";
                case CustomVariableTypeEnum.DateTime:
                    return "SqlDbType.DateTime";
                case CustomVariableTypeEnum.Double:
                    return "SqlDbType.Float";
                case CustomVariableTypeEnum.Single:
                    return "SqlDbType.Real";
                case CustomVariableTypeEnum.Decimal:
                    return "SqlDbType.Decimal";
                case CustomVariableTypeEnum.Integer:
                    return "SqlDbType.Int";
                case CustomVariableTypeEnum.BigInteger:
                    return "SqlDbType.BigInt";
                case CustomVariableTypeEnum.String:
                    return "SqlDbType.VarChar";
                case CustomVariableTypeEnum.Text:
                    return "SqlDbType.Text";
                default:
                    return "";
            }
        }
    }
}