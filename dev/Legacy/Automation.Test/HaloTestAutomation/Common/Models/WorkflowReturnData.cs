using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class WorkflowReturnData
    {
        public Int64 InstanceID { get; set; }

        public bool ActivityCompleted { get; set; }

        public string Error { get; set; }

        public List<string> X2Messages
        {
            get
            {
                var message = Error;
                var split = Error.Split(new string[] { "X2 Message:" }, StringSplitOptions.None);
                if (split.Length > 1)
                    message = split[1].Trim();
                return new List<string>() { message };
            }
        }
    }
}