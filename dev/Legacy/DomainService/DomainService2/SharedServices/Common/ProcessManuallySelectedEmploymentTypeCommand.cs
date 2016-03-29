using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.SharedServices.Common
{
    public class ProcessApplicationForManuallySelectedEmploymentTypeCommand : StandardDomainServiceCommand
    {
        public bool IsBondExceptionAction
        {
            get;
            protected set;
        }

        public int ApplicationKey
        {
            get;
            protected set;
        }
        public int SelectedEmploymentTypeKey 
        { 
            get; 
            protected set; 
        }

        public ProcessApplicationForManuallySelectedEmploymentTypeCommand(int applicationKey, bool isBondExceptionAction, int selectedEmploymentTypeKey)
        {
            this.ApplicationKey = applicationKey;
            this.IsBondExceptionAction = isBondExceptionAction;
            this.SelectedEmploymentTypeKey = selectedEmploymentTypeKey;
        }
    }
}
