using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Services.Interfaces.LifeDomain.Models;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface IDisabilityClaimHistoryDetails : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="disabilityClaim"></param>
        void BindDisabilityClaim(DisabilityClaimDetailModel disabilityClaim);

        /// <summary>
        ///
        /// </summary>
        /// <param name="listDisabilityClaimsHistory"></param>
        void BindDisabilityClaimsHistoryGrid(IEnumerable<DisabilityClaimDetailModel> listDisabilityClaimsHistory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decisionDate"></param>
        /// <param name="decisionUser"></param>
        void BindRepudiatedDecision(System.DateTime decisionDate, string decisionUser, IEnumerable<IReason> repudiatedReasons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decisionDate"></param>
        /// <param name="decisionUser"></param>
        void BindApprovedDecision(System.DateTime decisionDate, string decisionUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decisionDate"></param>
        /// <param name="decisionUser"></param>
        void BindTerminatedDecision(System.DateTime decisionDate, string decisionUser, IEnumerable<IReason> terminatedReasons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decisionDate"></param>
        /// <param name="decisionUser"></param>
        void BindSettledDecision(System.DateTime decisionDate, string decisionUser);
        
        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler DisabilityClaims_OnSelectedIndexChanged;

        /// <summary>
        ///
        /// </summary>
        int SelectedIndex { get; }

        /// <summary>
        ///
        /// </summary>
        List<DisabilityClaimDetailModel> ListDisabilityClaimsHistory { get; }
    }
}