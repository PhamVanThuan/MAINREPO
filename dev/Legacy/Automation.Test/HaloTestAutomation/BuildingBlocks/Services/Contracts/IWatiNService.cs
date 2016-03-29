using System.Collections.Generic;
using WatiN.Core;

namespace BuildingBlocks.Services.Contracts
{
    public interface IWatiNService
    {
        List<Table> GetTables(object objectMapsPresenterClass);

        void GenericCheckAllCheckBoxes(TestBrowser browser, params CheckBox[] checkBoxExclusions);

        void GenericUncheckAllCheckBoxes(TestBrowser browser);

        void SetWatiNTimeouts(int waitTimeOut);

        void CloseAllOpenIEBrowsers();

        void KillAllIEProcesses();

        string HandleFileDownload(TestBrowser browser, Element element);

        List<TableRow> FindRows(string cellTextExpression, Table table);

        void CheckTableCheckboxes(bool isChecked, Table table);

        void GenericCheckAllCheckBoxes(DomContainer domContainer, params CheckBox[] checkBoxExclusions);

        void PopulateRandCentsFields(TextField randsField, TextField centsField, decimal value);

        void HandleConfirmationPopup(Element element);

        void CancelConfirmationPopup(Element button);

        string HandleConfirmationPopupAndReturnConfirmationMessage(Element button);
    }
}