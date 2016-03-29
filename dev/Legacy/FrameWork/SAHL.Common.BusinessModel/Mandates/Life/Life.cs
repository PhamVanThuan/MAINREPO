using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Mandates.Life
{
    [MandateInfo()]
    public class LifeAllOfferTypes : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // always return true here because we dont care what the mortgageloanpurpose is. 
            // this mandate will enable us to round-robin the leads thru the consultants regardless of the mortgage loan purpose 
            return true;
        }
    }

    [MandateInfo()]
    public class LifeNewPurchase : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeNewPurchase Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.NewPurchaseLoan)
                return true;

            return false;
        }
    }

    [MandateInfo()]
    public class LifeSwitch : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeSwitch Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.SwitchLoan)
                return true;

            return false;

        }
    }

    [MandateInfo()]
    public class LifeRefinance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeRefinance Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.RefinanceLoan)
                return true;

            return false;

        }
    }

    [MandateInfo()]
    public class LifeFurtherLoan : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeFurtherLoan Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.FurtherLoan)
                return true;

            return false;

        }
    }

    [MandateInfo()]
    public class LifeReAdvance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeReAdvance Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.ReAdvance)
                return true;

            return false;

        }
    }

    [MandateInfo()]
    public class LifeFurtherAdvance : BaseMandate
    {
        public override bool ExecuteMandate(params object[] args)
        {
            // Get the Loan Offer
            IApplication application = args[0] as IApplication;
            // Validate the Parameters
            if (null == application)
                throw new InvalidCastException("LifeFurtherAdvance Mandate expects an IApplication");
            // Check the Offer Type
            if (application.ApplicationType.Key == (int)Globals.OfferTypes.FurtherAdvance)
                return true;

            return false;

        }
    }
}
