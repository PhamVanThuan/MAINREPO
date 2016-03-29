using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using xmlElements = SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Database.Templates
{
    partial class CustomX2Data
    {
        private xmlElements.Workflow workflow;

        public CustomX2Data(xmlElements.Workflow workflow)
        {
            this.workflow = workflow;
        }

        public string GetSqlDataType(xmlElements.AbstractCustomVariable customVariable)
        {
            string type = "";
            switch (customVariable.VariableType)
            {
                case xmlElements.CustomVariableTypeEnum.BigInteger:
                    type = "BIGINT NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.Bool:
                    type = "BIT NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.DateTime:
                    type = string.Format("DATETIME NOT NULL DEFAULT('{0}')", SqlDateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    break;
                case xmlElements.CustomVariableTypeEnum.Decimal:
                    type = "DECIMAL NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.Double:
                    type = "FLOAT NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.Integer:
                    type = "INT NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.Single:
                    type = "REAL NOT NULL DEFAULT(0)";
                    break;
                case xmlElements.CustomVariableTypeEnum.String:
                    type = string.Format("VARCHAR({0})",((xmlElements.AbstractLengthBasedVariable)customVariable).Length);
                    break;
                case xmlElements.CustomVariableTypeEnum.Text:
                    type = "VARCHAR(MAX)";
                    break;
                default:
                    break;
            }
            return type;
        }
    }
}
