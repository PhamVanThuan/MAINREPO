using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    /// This is the Edit Mode of ConditionsEdit
    /// </summary>
    public class ConditionsEditCondition : SAHLCommonBasePresenter<IConditionsEdit>
    {
        private IConditionsRepository conditionsRepository;
        private int ConditionEditKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsEditCondition(IConditionsEdit view, SAHLCommonBaseController controller)
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

            View.txtNotesAddAttributeforEditable(2);
            View.ShowUpdateButton = true;
            View.EnableUpdateButton = false;
            View.ShowCancelButton = true;
            View.SetPanel2GroupingText = "Loan Condition - Edit Condition";

            // Set up the Data
            ConditionEditKey = Convert.ToInt32(conditionsRepository.EditableConditionKey);
            string[] conditionData = conditionsRepository.SplitConditionAndName(conditionsRepository.EditableConditionString);
            PrivateCacheData.Add("ConditionName", conditionData[0]);
            View.SettxtNotesText = conditionsRepository.ConvertStringForHTMLDisplay(conditionData[1]);

            View.SettxtNotesReadOnly = true;
            // only user added conditions are text editable - check for the key
            for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]) == Convert.ToInt32(conditionsRepository.EditableConditionKey))
                    if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[i][6])) // user editable
                        View.SettxtNotesReadOnly = false;


            // Set up the Tokens for editing
            // check if there are tokens for editing...

            if (conditionsRepository.ConditionsSet.Tables[1] != null)
                View.ConfigureTokenCaptureFields(conditionsRepository.ConditionsSet.Tables[1], ConditionEditKey, false);
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
        private bool UpdateData()
        {
            //edit here 
            string conditionName = Convert.ToString(PrivateCacheData["ConditionName"]);

            if (View.SettxtTokenIDs.Length > 0)
            {

                // This is a Read only condition - save the token values only
                string RetVals = View.SettxtTokenValues.Replace("??", "'");
                string[] tokenArray = conditionsRepository.ParseStringToArray(RetVals);

                string[] TokenIDArray = conditionsRepository.ParseStringToArray(View.SettxtTokenIDs);

                // add the values to the tokens table from the array retrieved from the view
                bool throwError = false;
                string errorString = "The following tokens contain incorrect data. Please rectify this before saving the condition: ";
                for (int i = 0; i < TokenIDArray.Length; i++)
                    for (int j = 0; j < conditionsRepository.ConditionsSet.Tables[1].Rows.Count; j++)
                        if (Convert.ToString(conditionsRepository.ConditionsSet.Tables[1].Rows[j][0]) == Convert.ToString(TokenIDArray[i]))
                        {

                            // validate the tokens - 
                            string tokenValue = tokenArray[i];
                            // Make sure that the token is not blank
                            tokenValue = tokenValue.Trim();
                            if (tokenValue.Length == 0)
                            {
                                errorString += "[" + conditionsRepository.ConditionsSet.Tables[1].Rows[j][2] + "] must have a value, ";
                                throwError = true;
                            }
                            else
                                switch (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[1].Rows[j][8]))
                                {
                                    case 5: // datetime
                                        DateTime inputdate;
                                        if (!DateTime.TryParse(tokenValue, out inputdate))
                                        {
                                            throwError = true;
                                            errorString += "[" + conditionsRepository.ConditionsSet.Tables[1].Rows[j][2] + "] must contain a valid date, ";
                                        }
                                            
                                        break;
                                    case 6: // decimal
                                        decimal inputdecimal;
                                        if (!decimal.TryParse(tokenValue, out inputdecimal))
                                        {
                                            throwError = true;
                                            errorString += "[" + conditionsRepository.ConditionsSet.Tables[1].Rows[j][2] + "] must contain a valid decimal, ";
                                        }
                                            
                                        break;
                                    case 9: // integer
                                        int inputinteger;
                                        if (!int.TryParse(tokenValue, out inputinteger))
                                        {
                                            throwError = true;
                                            errorString += "[" + conditionsRepository.ConditionsSet.Tables[1].Rows[j][2] + "] must contain a valid integer, ";
                                        }
                                        
                                        break;
                                    case 10: // money
                                        float num;
                                        bool isValid = float.TryParse(tokenValue,
                                            NumberStyles.Float,
                                            CultureInfo.GetCultureInfo("en-ZA"), // cached
                                            out num);


                                        //Regex regex = new Regex(@"/^\d+\.\d{2}\/");
                                        //if (!regex.IsMatch(tokenValue))
                                        if (!isValid)
                                        {
                                            throwError = true;
                                            errorString += "[" + conditionsRepository.ConditionsSet.Tables[1].Rows[j][2] + "] must contain a valid currency value, ";
                                        }
                                        
                                        break;
                                }
                         
                            conditionsRepository.ConditionsSet.Tables[1].Rows[j][5] = tokenArray[i];
                        }

                if (throwError)
                {
                    View.Messages.Add(new Warning(errorString, errorString));
                    return false;
                }


                // Parse the token values into the display arrays
                conditionsRepository.ParseArrays();
                for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                    if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]) == Convert.ToInt32(conditionsRepository.EditableConditionKey))
                    {
                        if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[i][6]))
                            conditionsRepository.ConditionsSet.Tables[0].Rows[i][4] = conditionName + ") " + View.SettxtNotesText;
                        // update the array value and the String Value
                        string ParsedPhrase = conditionsRepository.NormaliseParenthesis(conditionName + ") " + View.SettxtNotesText);
                        conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[i][1], ref ParsedPhrase, false);
                        conditionsRepository.ChosenStrings[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = conditionName + ") " + View.SettxtNotesText;
                        conditionsRepository.ChosenValue[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = ParsedPhrase;
                        conditionsRepository.ChosenCSSWeight[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = "#ffff99";  //"yellow";
                        conditionsRepository.EditableConditionValue = ParsedPhrase;
                        break;
                    }

                conditionsRepository.ParseStrings();
                GlobalCacheData.Remove(ViewConstants.ConditionSet);
                GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
                return true;
            }
            else
            {
                string condition = conditionName + ") " + View.SettxtNotesText;
                condition = condition.Trim();
                if (condition.Length > 0)
                {

                    // This is a user added condition - save the change to the condition only
                    for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                        if (Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]) == Convert.ToInt32(conditionsRepository.EditableConditionKey))
                        {
                            // Strip out any carriage returns on a user edited condition
                            string ParsedPhrase = conditionsRepository.NormaliseParenthesis(condition);

                            conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[i][1], ref ParsedPhrase, false);
                            if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[i][6]))
                                conditionsRepository.ConditionsSet.Tables[0].Rows[i][4] = condition;
                            conditionsRepository.ParseArrays();
                            // update the array value and the String Value
                            conditionsRepository.ChosenStrings[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = condition;
                            conditionsRepository.ChosenValue[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = ParsedPhrase;
                            conditionsRepository.ChosenCSSWeight[Convert.ToInt32(conditionsRepository.EditableConditionIndex)] = "#ffff99"; //"yellow";
                            conditionsRepository.EditableConditionValue = ParsedPhrase;
                            conditionsRepository.ParseStrings();
                            break;
                        }

                    GlobalCacheData.Remove(ViewConstants.ConditionSet);
                    GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
                    return true;
                }
                else
                {
                    View.Messages.Add(
                        new Warning(
                            "Conditions cannot be blank. Add a valid condition or press cancel to return to the previous screen.",
                            "Conditions cannot be blank. Add a valid condition or press cancel to return to the previous screen."));
                    return false;
                }

            }

        }




    }
}
