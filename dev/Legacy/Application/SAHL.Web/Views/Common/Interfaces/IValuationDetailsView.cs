using System;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IValuationDetailsView : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnViewDetailClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnSubmitClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler grdValuationsGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnPropertyClicked;


        ///// <summary>
        ///// 
        ///// </summary>
        //event EventHandler PropertiesGrid_RowDataBound;

        ///// <summary>
        ///// 
        ///// </summary>
        //event EventHandler PropertiesGrid_GridDoubleClick;

        ///// <summary>
        ///// 
        ///// </summary>
        //event EventHandler PropertiesGrid_SelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdValuations(DataTable DT);

        /// <summary>
        /// Bind the Address DataTable to the Grid
        /// </summary>
        /// <param name="DT"></param>
        void BindPropertyGrid(DataTable DT);

        /// <summary>
        /// Gets or sets the Proeprty Item index
        /// </summary>
        int PropertyItemIndex  { get; set;}

        //***********************************************************************************
        // SET UP THE PROPERTY CHANGE METHODS


        /// <summary>
        /// Property to show/hide the Valudation Message
        /// </summary>
        bool ShowlblErrorMessage { get; set;}

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        bool ShowbtnCancel { set;}


        /// <summary>
        /// Property to show/hide the 'btnProperty' button
        /// </summary>
        bool ShowbtnProperty { set;}

                /// <summary>
        /// Property to show/hide the 'btnViewDetail' button
        /// </summary>
        bool ShowbtnViewDetail { set;}

        /// <summary>
        /// Property to show/hide the 'btnSubmit' button
        /// </summary>
        bool ShowbtnSubmit { set;}


        /// <summary>
        /// Property to show/hide the 'pnlValuations' panel
        /// </summary>
        bool ShowpnlValuations { set;}




        /// <summary>
        /// Property to show/hide the 'grdValuations' grid
        /// </summary>
        bool ShowgrdValuations { set;}



        /// <summary>
        /// Property to show/hide the 'pnlInspectionContactDetails' panel including subcomponents
        /// </summary>
        bool ShowpnlInspectionContactDetails { set;}

        // SET UP pnlInspectionContactDetails VALUES


        /// <summary>
        ///  Set or Get the Valuations Grid Postbacktype
        /// </summary>
        GridPostBackType SetPostBackTypegrdValuations { set; get;}




        /// <summary>
        ///  enable or disable grdValuations
        /// </summary>
        bool EnablegrdValuations { set; get;}

        /// <summary>
        ///  Set The error Messages Message
        /// </summary>
        string SetlblErrorMessage { set; get;}

        /// <summary>
        /// 
        /// </summary>
        string SettxtContact1Name { set; get;}
 

        /// <summary>
        /// 
        /// </summary>
        string SettxtContact1Phone { set; get;}


        /// <summary>
        /// 
        /// </summary>
        string SettxtContact1WorkPhone { set; get;}


        /// <summary>
        /// 
        /// </summary>
        string SettxtContact1MobilePhone { set; get;}


        /// <summary>
        /// 
        /// </summary>
        string SettxtContact2Name { set; get;}


        /// <summary>
        /// 
        /// </summary>
        string SettxtContact2Phone { set; get;}

        /// <summary>
        /// This is the Valuation Key that has been selected
        /// </summary>
        int ValuationItemIndex{set; get;}

    }
}
