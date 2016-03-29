using System;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.ReleaseAndVariations.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReleaseAndVariationsERFDetails : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnUpdateClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler btnCancelClicked;


        /// <summary>
        /// Set the Text value of 'txtExistingSecurity'
        /// </summary>
         string SettxtExistingSecurity { set; get;}

        /// <summary>
        /// Set the Text value of 'txtExistingSecurityEXT'
        /// </summary>
        string SettxtExistingSecurityEXT { set; get;}


        /// <summary>
        /// Set the Text value of 'txtExistingSecurityValuation'
        /// </summary>
        string SettxtExistingSecurityValuation { set; get;}


        /// <summary>
        /// Set the Text value of 'txtToBeReleased'
        /// </summary>
        string SettxtToBeReleased { set; get;}


        /// <summary>
        /// Set the Text value of 'txtToBeReleasedExt'
        /// </summary>
        string SettxtToBeReleasedExt { set; get;}


        /// <summary>
        /// Set the Text value of 'txtToBeReleasedValuation'
        /// </summary>
        string SettxtToBeReleasedValuation { set; get;}


        /// <summary>
        /// Set the Text value of 'txtRemainingSecurity'
        /// </summary>
        string SettxtRemainingSecurity { set; get;}


        /// <summary>
        /// Set the Text value of 'txtRemainingSecurityExt'
        /// </summary>
        string SettxtRemainingSecurityExt { set; get;}


        /// <summary>
        /// Set the Text value of 'txtRemainingSecurityValuation'
        /// </summary>
        string SettxtRemainingSecurityValuation { set; get;}


        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        bool ShowbtnUpdate { set;}

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        bool ShowbtnCancel { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurity'
        /// </summary>
        bool SetReadOnlytxtExistingSecurity { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurityEXT'
        /// </summary>
        bool SetReadOnlytxtExistingSecurityEXT { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtExistingSecurityValuation'
        /// </summary>
        bool SetReadOnlytxtExistingSecurityValuation { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtToBeReleased'
        /// </summary>
        bool SetReadOnlytxtToBeReleased { set;}

        /// <summary>
        ///Set the readonly attribute of 'txtToBeReleasedExt'
        /// </summary>
        bool SetReadOnlytxtToBeReleasedExt { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtToBeReleasedValuation'
        /// </summary>
        bool SetReadOnlytxtToBeReleasedValuation { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurity'
        /// </summary>
        bool SetReadOnlytxtRemainingSecurity { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurityExt'
        /// </summary>
        bool SetReadOnlytxtRemainingSecurityExt { set;}

        /// <summary>
        /// Set the readonly attribute of 'txtRemainingSecurityValuation'
        /// </summary>
        bool SetReadOnlytxtRemainingSecurityValuation { set;}
    }
}
