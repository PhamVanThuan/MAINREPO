using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    ///  This is the Add Mode of ConditionsEdit
    /// </summary>
    public class ConditionsAddCondition : SAHLCommonBasePresenter<IConditionsEdit>
    {
        private IConditionsRepository conditionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsAddCondition(IConditionsEdit view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // This Data will be in the Cache becuse these views can only be accessed from the ConditionSet View
            //conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();
            conditionsRepository = GlobalCacheData[ViewConstants.ConditionSet] as IConditionsRepository;

            // Setup the View Event Handlers
            View.btnAddClicked += btnAddClicked;
            View.btnCancelClicked += btnCancelClicked;
            View.EnableAddButton = false;


        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            View.txtNotesAddAttributeforEditable(1);
            View.ShowAddButton = true;
            View.ShowCancelButton = true;
            View.SetPanel2GroupingText = "Loan Condition - Add Condition";
        }


        void btnAddClicked(object sender, EventArgs e)
        {
            string condition = View.SettxtNotesText;
            condition =  conditionsRepository.UserAddedConditionName + ") " + condition.Trim();
            if (condition.Length > 0)
            {
                // Add the new condition to the dataset
                DataRow dr = conditionsRepository.ConditionsSet.Tables[0].NewRow();
                dr[0] = conditionsRepository.ConditionsSet.Tables[0].Rows.Count + 1;
                dr[1] = conditionsRepository.UserEnteredConditionKey;
                dr[3] = false;
                dr[4] = conditionsRepository.ConvertStringForJavaScriptArray(condition); // User Added Text
                dr[5] = "";
                dr[6] = true;
                dr[7] = false;
                dr[8] = "Black";
                dr[9] = "White";
                dr[10] = 0;
                dr[11] = true;
                dr[12] = 4;
                dr[13] = View.SettxtNotesText; // User Added Text
                dr[14] = -1; // no key yet 
                conditionsRepository.ConditionsSet.Tables[0].Rows.Add(dr);

                // add a new item to the chosen array set

                conditionsRepository.ParseArrays();

                int ArrayCount = conditionsRepository.ChosenOfferConditionKeys.Length;
                conditionsRepository.ResizeChosenArrays(ArrayCount);
                conditionsRepository.ChosenStrings[ArrayCount] = conditionsRepository.ConvertStringForDBStorage(condition);
                conditionsRepository.ChosenID[ArrayCount] = "-1";
                conditionsRepository.ChosenCSSColor[ArrayCount] = "Black";
                conditionsRepository.ChosenCSSWeight[ArrayCount] = "#ffff99";  //"yellow";
                conditionsRepository.ChosenValue[ArrayCount] = conditionsRepository.ConvertStringForDBStorage(condition);
                conditionsRepository.ChosenEdited[ArrayCount] = "True";  // User Captured Condition = 4
                conditionsRepository.ChosenArrayUserConditionType[ArrayCount] = "4";  // User Captured Condition = 4
                conditionsRepository.ChosenOfferConditionKeys[ArrayCount] = conditionsRepository.UserEnteredConditionKey.ToString();  // Condition Key

                // update the javascript strings from the arrays (that have just been updated
                conditionsRepository.ParseStrings();
                conditionsRepository.EditableConditionValue = conditionsRepository.ConvertStringForDBStorage(condition);

                GlobalCacheData.Remove(ViewConstants.ConditionSet);
                GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());

                if (View.IsValid)
                    Navigator.Navigate(conditionsRepository.AddNavigateTo);
            }
            else
                View.Messages.Add(new Warning("Conditions cannot be blank. Add a valid condition or press cancel to return to the previous screen.", "Conditions cannot be blank. Add a valid condition or press cancel to return to the previous screen."));

        }

        void btnCancelClicked(object sender, EventArgs e)
        {
            if (View.IsValid)
                Navigator.Navigate(conditionsRepository.CancelNavigateTo);
        }

    }
}
