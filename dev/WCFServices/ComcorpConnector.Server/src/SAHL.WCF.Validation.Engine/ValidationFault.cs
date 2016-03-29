using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SAHL.WCF.Validation.Engine
{
    [DataContract]
    public class ValidationFault
    {
        public ValidationFault(IEnumerable<ModelValidationResult> result)
        {
            var error = new StringBuilder();
            foreach (var e in result)
            {
                error.AppendLine();
                error.Append(string.Format("Validation error on {0}: {1}", e.MemberName, e.Message));
                error.AppendLine();
                error.AppendLine();
            }
            ErrorCode = 24;
            ErrorMessage = error.ToString();
        }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
