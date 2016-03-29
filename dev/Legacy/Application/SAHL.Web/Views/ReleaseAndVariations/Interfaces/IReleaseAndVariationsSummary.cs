using System;
using System.Data;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReleaseAndVariationsSummary : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnPrintRequestClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateConditionsClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnERFInformationClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateSummaryClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnSubmitClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnConfirmClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;


        //event EventHandler gridConditionsGridSelectedIndexChanged;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgridBondDetails(DataTable DT);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgridConditions(DataTable DT);


        /// <summary>
        /// bind the object list to the 'ddlRequestType' drop down list
        /// </summary>
        void bindddlRequestType(DataTable DT);


        /// <summary>
        /// bind the object list to the 'ddlRequestType' drop down list
        /// </summary>
        void bindddlApplyChangeTo(DataTable DT);

        // SET THE OBJECTS VISIBILITY *****************************************************************

        /// <summary>
        /// Property to show/hide the 'lblAccountName' 
        /// </summary>
        bool ShowlblAccountName { set;}


        /// <summary>
        /// Property to show/hide the 'lblAccountNumber'  object
        /// </summary>
        bool ShowlblAccountNumber { set;}

        /// <summary>
        /// Property to show/hide the 'lblLinkedToOffer'  object
        /// </summary>
        bool ShowlblLinkedToOffer { set;}


        /// <summary>
        /// Property to show/hide the 'ddlRequestType'  object
        /// </summary>
        bool ShowddlRequestType { set;}

        /// <summary>
        /// Property to show/hide the 'lblRequestType'  object
        /// </summary>
        bool ShowlblRequestType { set;}


        /// <summary>
        /// Property to show/hide the 'ddlApplyChangeTo'  object
        /// </summary>
        bool ShowddlApplyChangeTo { set;}

        /// <summary>
        /// Property to show/hide the 'lblApplyChangeTo'  object
        /// </summary>
        bool ShowlblApplyChangeTo { set;}

        /// <summary>
        /// Property to show/hide the 'lblLoanBalance'  object
        /// </summary>
        bool ShowlblLoanBalance { set;}

        /// <summary>
        /// Property to show/hide the 'lblArrears'  object
        /// </summary>
        bool ShowlblArrears { set;}

        /// <summary>
        /// Property to show/hide the 'lblSPV'  object
        /// </summary>
        bool ShowlblSPV { set;}


        /// <summary>
        /// Property to show/hide the 'txtNotes'  object
        /// </summary>
        bool ShowtxtNotes { set;}

        /// <summary>
        /// Property to set The readonly attribure of the 'txtNotes'  object
        /// </summary>
        bool SetReadOnlytxtNotes { set;}

        /// <summary>
        /// Property to show/hide the 'lblCurrentLTV'  object
        /// </summary>
        bool ShowlblCurrentLTV { set;}

        /// <summary>
        /// Property to show/hide the 'lblCurrentPTI'  object
        /// </summary>
        bool ShowlblCurrentPTI { set;}

        /// <summary>
        /// Property to show/hide the 'lblCurrentLoan'  object
        /// </summary>
        bool ShowlblCurrentLoan { set;}


        /// <summary>
        /// Property to show/hide the 'lblProducts'  object
        /// </summary>
        bool ShowlblProducts { set;}


        /// <summary>
        /// Property to show/hide the 'gridBondDetails'  object
        /// </summary>
        bool ShowgridBondDetails { set;}

        /// <summary>
        /// Property to show/hide the 'gridConditions'  object
        /// </summary>
        bool ShowgridConditions { set;}


        /// <summary>
        /// Property to show/hide the 'lblCaption'  object
        /// </summary>
        bool ShowlblCaption { set;}

        /// <summary>
        /// Property to show/hide the 'btnConfirm'  object
        /// </summary>
        bool ShowbtnConfirm { set;}

        /// <summary>
        /// Property to show/hide the 'btnCancel'  object
        /// </summary>
        bool ShowbtnCancel { set;}

        /// <summary>
        /// Property to show/hide the 'btnSubmit'  object
        /// </summary>
        bool ShowbtnSubmit { set;}

        /// <summary>
        /// Property to show/hide the 'btnPrintRequest'  object
        /// </summary>
        bool ShowbtnPrintRequest { set;}

        /// <summary>
        /// Property to show/hide the 'btnUpdateConditions'  object
        /// </summary>
        bool ShowbtnUpdateConditions { set;}
        /// <summary>
        /// Property to show/hide the 'btnERFInformation'  object
        /// </summary>
        bool ShowbtnERFInformation { set;}
        /// <summary>
        /// Property to show/hide the 'btnUpdateSummary'  object
        /// </summary>
        bool ShowbtnUpdateSummary { set;}

        /// <summary>
        /// Property to show/hide the 'pnlContact'  object
        /// </summary>
        bool ShowpnlContact { set;}
        /// <summary>
        /// Property to show/hide the 'pnlMemo'  object
        /// </summary>
        bool ShowpnlMemo { set;}
        /// <summary>
        /// Property to show/hide the 'pnlLoanDetails'  object
        /// </summary>
        bool ShowpnlLoanDetails { set;}
        /// <summary>
        /// Property to show/hide the 'pnlBondDetails'  object
        /// </summary>
        bool ShowpnlBondDetails { set;}
        /// <summary>
        /// Property to show/hide the 'pnlConditions'  object
        /// </summary>
        bool ShowpnlConditions { set;}

        // DO THE GET SET VALUES FOR THE SCREEN OBJECTS


        /// <summary>
        ///  Set or Get 'lblAccountName' value
        /// </summary>
        string GetSetlblAccountName  { set; get;}


        /// <summary>
        ///  Set or Get 'lblAccountNumber' value
        /// </summary>
        string GetSetlblAccountNumber { set; get;}


        /// <summary>
        ///  Set or Get 'lblLinkedToOffer' value
        /// </summary>
        string GetSetlblLinkedToOffer { set; get;}

        /// <summary>
        ///  Set or Get 'ddlRequestType' value
        /// </summary>
        string GetSetddlRequestType { set; get;}


        /// <summary>
        ///  Set or Get 'ddlRequestType' SelectedIndex
        /// </summary>
        int GetSetddlRequestTypeSelectedIndex { set; get;}

        /// <summary>
        ///  Set or Get 'GetSetddlApplyChangeTo' SelectedIndex
        /// </summary>
        int GetSetddlApplyChangeToSelectedIndex { set; get;}

        /// <summary>
        ///  Set or Get 'lblRequestType' value
        /// </summary>
        string GetSetlblRequestType { set; get;}

        /// <summary>
        ///  Set or Get 'ddlApplyChangeTo' value
        /// </summary>
        string GetSetddlApplyChangeTo { set; get;}

        /// <summary>
        ///  Set or Get 'lblApplyChangeTo' value
        /// </summary>
        string GetSetlblApplyChangeTo { set; get;}

        /// <summary>
        ///  Set or Get 'lblLoanBalance' value
        /// </summary>
        string GetSetlblLoanBalance { set; get;}

        /// <summary>
        ///  Set or Get 'lblArrears' value
        /// </summary>
        string GetSetlblArrears { set; get;}

        /// <summary>
        ///  Set or Get 'lblSPV' value
        /// </summary>
        string GetSetlblSPV { set; get;}

        /// <summary>
        ///  Set or Get 'txtNotes' value
        /// </summary>
        string GetSettxtNotes { set; get;}

        /// <summary>
        ///  Set or Get 'lblCurrentLTV' value
        /// </summary>
        string GetSetlblCurrentLTV { set; get;}

        /// <summary>
        ///  Set or Get 'lblCurrentPTI' value
        /// </summary>
        string GetSetlblCurrentPTI { set; get;}

        /// <summary>
        ///  Set or Get 'lblCurrentLoan' value
        /// </summary>
        string GetSetlblCurrentLoan { set; get;}
        /// <summary>
        ///  Set or Get 'lblProducts' value
        /// </summary>
        string GetSetlblProducts { set; get;}


        /// <summary>
        ///  Set or Get 'lblCaption' value
        /// </summary>
        string GetSetlblCaption { set; get;}

    }
}
