using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Mandates.QC
{
    [MandateInfo()]
    public class QCSwitch : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("Switch expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 6)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class QCRefinance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("Refinance expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 8)
                return true;
            return false;

        }
    }

    [MandateInfo()]
    public class QCFurtherLoan : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            IApplicationMortgageLoan app = args[0] as IApplicationMortgageLoan;
            if (null == app) throw new InvalidCastException("FurtherLoan expects and IApplicationMortgageLoan");
            if (app.ApplicationType.Key == 4 || app.ApplicationType.Key == 3 || app.ApplicationType.Key == 2)
                return true;
            return false;

        }
    }
}
