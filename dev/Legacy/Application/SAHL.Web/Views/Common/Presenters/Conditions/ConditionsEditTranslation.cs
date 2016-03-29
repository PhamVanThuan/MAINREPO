using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    /// This is the Edit Mode of ConditionsEdit
    /// </summary>
    public class ConditionsEditTranslation : SAHLCommonBasePresenter<IConditionsEdit>
    {
        private IConditionsRepository conditionsRepository;
        private int ConditionEditKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsEditTranslation(IConditionsEdit view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            ConditionEditKey = -1;
            // This Data will be in the Cache becuse these views can only be accessed from the ConditionSet View
            //conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();
            conditionsRepository = GlobalCacheData[ViewConstants.ConditionSet] as IConditionsRepository;

            // Setup the View Interface
            View.btnUpdateClicked += btnUpdateClicked;
            View.btnCancelClicked += btnCancelClicked;
            
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            View.txtNotesAddAttributeforEditable(3);
            View.ShowUpdateButton = true;
            View.ShowCancelButton = true;
            View.SetPanel2GroupingText = "Loan Condition - Edit Translated Condition";
            View.ShowTranslatePanel = true;
            View.ShowtxtTranslation = true;

            // Set up the Data
            int actualconditionkey = -1;
            ConditionEditKey = Convert.ToInt32(conditionsRepository.EditableConditionKey);

            for (int i = 0; i <conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][0]) == ConditionEditKey)
                {
                    actualconditionkey = Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]); 
                    break;
                }

            View.SettxtNotesText = conditionsRepository.ConvertStringForHTMLDisplay(conditionsRepository.EditableConditionString);
            View.SettxtTranslation = conditionsRepository.ConvertStringForHTMLDisplay(conditionsRepository.EditableTranslationString);

            View.SettxtTranslationReadonly = true;
            View.SettxtNotesReadOnly = true;
            // only user added conditions are text editable - check for the key
            for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][0]) == Convert.ToInt32(conditionsRepository.EditableConditionKey))
                    if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]) == conditionsRepository.UserEnteredConditionKey) // user editable
                        View.SettxtTranslationReadonly = false;


            // Set up the Tokens for editing
            if (conditionsRepository.ConditionsSet.Tables[1] != null)
                View.ConfigureTokenCaptureFields(conditionsRepository.ConditionsSet.Tables[1], actualconditionkey, true);
        }


        void btnUpdateClicked(object sender, EventArgs e)
        {
            UpdateData();
            if (View.IsValid)
                Navigator.Navigate(conditionsRepository.AddNavigateTo);
        }

        void btnCancelClicked(object sender, EventArgs e)
        {
            if (View.IsValid)
                Navigator.Navigate(conditionsRepository.CancelNavigateTo);
        }


        /// <summary>
        /// Update the data in the relevant tables
        /// </summary>
        /// <returns></returns>
        void UpdateData()
        {
            if (View.SettxtTokenIDs.Length > 0)
            {
                // This is a Read only condition - save the token values only
                string RetVals = View.SettxtTokenValues.Replace("??", "'");
                string[] tokenArray = conditionsRepository.ParseStringToArray(RetVals);

                string[] TokenIDArray = conditionsRepository.ParseStringToArray(View.SettxtTokenIDs);

                // add the values to the tokens table from the array retrieved from the view
                for (int i = 0; i < TokenIDArray.Length; i++)
                    for (int j = 0; j < conditionsRepository.ConditionsSet.Tables[1].Rows.Count; j++)
                        if (Convert.ToString(conditionsRepository.ConditionsSet.Tables[1].Rows[j][0]) == Convert.ToString(TokenIDArray[i]))
                            conditionsRepository.ConditionsSet.Tables[1].Rows[j][10] = tokenArray[i];
            }
            else
                // This is a user added condition - save the change to the condition only
                for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                    if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][0]) == Convert.ToInt32(conditionsRepository.EditableConditionKey))
                    {
                        string ParsedPhrase = conditionsRepository.ConvertStringForDBStorage(View.SettxtNotesText);
                        conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[i][1], ref ParsedPhrase, false);
                        if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[i][6]))
                            conditionsRepository.ConditionsSet.Tables[0].Rows[i][5] = ParsedPhrase;
                        
                        break;
                    }

            GlobalCacheData.Remove(ViewConstants.ConditionSet);
            GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());

        }

    }
}
