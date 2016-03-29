using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Templates
{
    partial class X2ProcessLegacy
    {
        private Process process;

        public X2ProcessLegacy(Process process)
        {
            this.process = process;
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

        public string ConvertStringFromCustomVariableType(CustomVariableTypeEnum customVariableType)
        {
            switch (customVariableType)
            {
                case CustomVariableTypeEnum.Bool:
                    return "ToBoolean";
                case CustomVariableTypeEnum.DateTime:
                    return "ToDateTime";
                case CustomVariableTypeEnum.Double:
                    return "ToDouble";
                case CustomVariableTypeEnum.Single:
                    return "ToSingle";
                case CustomVariableTypeEnum.Decimal:
                    return "ToDecimal";
                case CustomVariableTypeEnum.Integer:
                    return "ToInt32";
                case CustomVariableTypeEnum.BigInteger:
                    return "ToInt64";
                case CustomVariableTypeEnum.String:
                    return "ToString";
                case CustomVariableTypeEnum.Text:
                    return "ToString";
                default:
                    return "";
            }
        }
    }
}