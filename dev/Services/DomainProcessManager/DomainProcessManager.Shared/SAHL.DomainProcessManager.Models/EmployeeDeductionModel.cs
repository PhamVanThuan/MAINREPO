using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class EmployeeDeductionModel : IDataModel
    {
        public EmployeeDeductionModel(EmployeeDeductionTypeEnum type, double value)
        {
            this.Type = type;
            this.Value = value;
        }

        [DataMember]
        public EmployeeDeductionTypeEnum Type { get; set; }

        [DataMember]
        public double Value { get; set; }
    }
}