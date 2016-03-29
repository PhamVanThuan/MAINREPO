using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.PersonalLoan.Interfaces
{
    public interface IPersonalLoanImportLeads : IViewBase
    {

        event KeyChangedEventHandler OnDownLoadClicked;

        event EventHandler OnImportClicked;

        HttpPostedFile File { get; }

        void SetUpLeadSummaryGrid();

        void BindLeadSummaryGrid(List<BatchServiceResult> results);

        bool EditMode { get; set; }

        string ImportFileName { get; }

        void FileReadyForDownload(IBatchService batchService);
    }

}