using SAHL.Common.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IITCCheck : IViewBase
    {
        event EventHandler OnDoEnquiryButtonClicked;

        event EventHandler OnViewHistoryButtonClicked;

        void BindITCGrid(List<SAHL.Web.Views.Common.Interfaces.BindableITC> listITC);

        Int32 LegalEntityKeyForHistory
        {
            get;
        }

        bool DoEnquiryColumnVisible
        {
            set;
        }
    }
}