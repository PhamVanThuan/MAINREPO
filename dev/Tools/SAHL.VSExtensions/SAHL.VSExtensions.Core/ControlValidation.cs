using SAHL.VSExtensions.Interfaces.Validation;
using System;

namespace SAHL.VSExtensions.Core
{
    public class ControlValidation : IControlValidation
    {
        public bool Result
        {
            get;
            protected set;
        }

        public string ResultMessage
        {
            get;
            protected set;
        }

        public ControlValidation()
        {
            this.Result = true;
            this.ResultMessage = string.Empty;
        }

        public void ValidateControl(string message, Func<bool> isValid)
        {
            if (Result)
            {
                bool tempResult = isValid();
                if (!tempResult)
                {
                    this.Result = false;
                    this.ResultMessage = message;
                }
            }
        }
    }
}