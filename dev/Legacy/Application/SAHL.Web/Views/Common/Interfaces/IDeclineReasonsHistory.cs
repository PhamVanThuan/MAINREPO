using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Decline Reasons History
    /// </summary>
    public interface IDeclineReasonsHistory : IViewBase
    {
        #region eventhandlers
        /// <summary>
        /// The Event Handler for clicking on the
        /// </summary>
        event KeyChangedEventHandler OngrdRevisionHistoryIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdRevisionHistory(DataTable DT);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        void BindgrdDeclineReasons(DataTable DT);

        #endregion 
        
        #region Methods
        /// <summary>
        /// Property to set  the 'lblTotalLoanRequired' text
        /// </summary>
        string SetlblTotalLoanRequired { set;}
        /// <summary>
        /// Property to set  the 'lblTerm' text
        /// </summary>
        string SetlblTerm { set;}
        /// <summary>
        /// Property to set  the 'lblLinkRate' text
        /// </summary>
        string SetlblLinkRate { set;}
        /// <summary>
        /// Property to set  the 'lblEffectiveRate' text
        /// </summary>
        string SetlblEffectiveRate { set;}
        /// <summary>
        /// Property to set  the 'lblBondToRegister' text
        /// </summary>
        string SetlblBondToRegister { set;}
        /// <summary>
        /// Property to set  the 'lblPTI' text
        /// </summary>
        string SetlblPTI { set;}
        /// <summary>
        /// Property to set  the 'lblLTV' text
        /// </summary>
        string SetlblLTV { set;}
        /// <summary>
        /// Property to set  the 'lblTotalInstallment' text
        /// </summary>
        string SetlblTotalInstallment { set;}
        /// <summary>
        /// Property to set  the 'lblHouseHoldIncome' text
        /// </summary>
        string SetlblHouseHoldIncome { set;}
        /// <summary>
        /// Property to set  the 'lblCategory' text
        /// </summary>
        string SetlblCategory { set;}
        /// <summary>
        /// Property to set  the 'lblSPVName' text
        /// </summary>
        string SetlblSPVName { set;}

        /// <summary>
        ///  Gets and Sets the Selected DataKey for the Revision HistoryGrid
        /// </summary>
        int grdRevisionHistoryKey { get; }

        /// <summary>
        ///  Gets and Sets the Selected Index for the Revision HistoryGrid
        /// </summary>
        int grdRevisionHistorySelectedIndex { get; set;}
        

        #endregion


    }
}
