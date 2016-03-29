using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionsTranslate : SAHLCommonBasePresenter<IConditionsTranslate>
    {

        private IConditionsRepository conditionsRepository;
        private CBONode cboNode; // = null;
        //strings to use for populating the interface
        private string EnglishArray = "",
                       AfrikaansArray = "",
                       ConditionKeyArray = "",
                       MustEditArray = "";

        /// <summary>
        /// List of conditions with translatable items
        /// </summary>
        protected SAHL.Common.Collections.EventList<ICondition> conditions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsTranslate(IConditionsTranslate view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // Get the CBO Node   
            if (View.CurrentPrincipal != null) cboNode = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null) throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            // Check the global cache to see if a condition object has been added
            //if (GlobalCacheData.ContainsKey(ViewConstants.ConditionSet))
            //    conditionsRepository = GlobalCacheData[ViewConstants.ConditionSet] as IConditionsRepository;
            //else
            conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();


            View.btnCancelClicked += btnCancelClicked;
            View.btnUpdateClicked += btnUpdateClicked;
            View.btnEditClicked += btnEditClicked;

            View.ShowbtnCancel = true;
            View.ShowbtnUpdate = true;
            View.ShowbtnEdit = true;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            if (!GlobalCacheData.ContainsKey(ViewConstants.ConditionSet))
            {
                int GenericKey = cboNode.GenericKey;
                int GenericKeyTypeKey = cboNode.GenericKeyTypeKey;
                //int GenericKey = 236097;
                //int GenericKeyTypeKey = 2;

                //int OfferKey = cboNode.GenericKey;
                conditionsRepository.Initialise(GenericKey);

                conditionsRepository.GetExistingConditionSetForEditing(GenericKey, GenericKeyTypeKey);

                // Set up the Edit Navigation
                conditionsRepository.AddNavigateTo = "AddFromTranslate";
                conditionsRepository.CancelNavigateTo = "CancelFromTranslate";
                // Store the changes to The Session Object after view population
                GlobalCacheData.Remove(ViewConstants.ConditionSet);
                GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
            }

            BindInitialData();
            RegisterClientScripts();
        }




        void btnCancelClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ConditionsSummary");
        }

        void btnEditClicked(object sender, EventArgs e)
        {
            // Get the Edit Keys from the client to pass to the edit Presenter

            for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                if (conditionsRepository.ConditionsSet.Tables[0].Rows[i][0].ToString() == View.GettxtConditionsKey) // conditionset table key - not the actual condition key
                {
                    conditionsRepository.EditableConditionString = conditionsRepository.ConditionsSet.Tables[0].Rows[i][4].ToString();
                    conditionsRepository.EditableTranslationString = conditionsRepository.ConditionsSet.Tables[0].Rows[i][5].ToString();
                }

            conditionsRepository.EditableConditionKey = View.GettxtConditionsKey;

            // Store the changes to The Session Object after translation updates have been made
            GlobalCacheData.Remove(ViewConstants.ConditionSet);
            GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
            Navigator.Navigate("ConditionsEditTranslation");

        }

        void btnUpdateClicked(object sender, EventArgs e)
        {
            if (AllTokensPopulated())
            {
                TransactionScope txn = new TransactionScope();

                try
                {
                    // Save all of the objects to the Database
                    conditionsRepository.SaveExistingConditionSet(conditionsRepository.LegalEntityLanguageKey);
                    txn.VoteCommit();
                }

                catch (Exception)
                {
                    txn.VoteRollBack();
                    if (_view.IsValid)
                        throw;
                }

                finally
                {
                    txn.Dispose();
                }
            }
            else
                View.Messages.Add(new Warning("You have included conditions within your condition set that contain [Tokens] that you need to edit before saving.", "You have included conditions within your condition set that contain [Tokens] that you need to edit before saving."));

            // Store the changes to The Session Object after translation updates have been made
            GlobalCacheData.Remove(ViewConstants.ConditionSet);
            GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
            Navigator.Navigate("ConditionsSummary");
        }

        /// <summary>
        /// Set up the data stuff.
        /// </summary>
        public void BindInitialData()
        {
            for (int x = 0; x < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; x++)
            {
                if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[x][3])) // is selected
                {
                    string EnglishCondition = conditionsRepository.ConditionsSet.Tables[0].Rows[x][4].ToString();
                    conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[x][1], ref EnglishCondition, false);
                    string AfrikaansCondition = conditionsRepository.ConditionsSet.Tables[0].Rows[x][5].ToString();
                    conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[x][1], ref AfrikaansCondition, true);
                    string ConditionKey = conditionsRepository.ConditionsSet.Tables[0].Rows[x][0].ToString();

                    string MustEdit = conditionsRepository.ConditionsSet.Tables[0].Rows[x][6].ToString();
                    EnglishArray += "'" + EnglishCondition + "',"; // Tokenised English
                    AfrikaansArray += "'" + AfrikaansCondition + "'" + ","; // Tokenised Afrikaans
                    ConditionKeyArray += "'" + ConditionKey + "'" + ",";
                    MustEditArray += "'" + MustEdit + "'" + ","; // true or false - to set the CSS colors
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterClientScripts()
        {
            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
            // Clear and set the text display box with the selected text 

            mBuilder.AppendLine("var EnglishArray =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(EnglishArray) + "'');");
            mBuilder.AppendLine("var AfrikaansArray =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(AfrikaansArray) + "'');");
            mBuilder.AppendLine("var ConditionKeyArray =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(ConditionKeyArray) + "'');");
            mBuilder.AppendLine("var MustEditArray =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(MustEditArray) + "'');");

            //set up page initialisation for javascript environment
            mBuilder.AppendLine("if (window.addEventListener) ");//DOM method for binding an event
            mBuilder.AppendLine("window.addEventListener('load', initIt, false);");
            mBuilder.AppendLine("else if (window.attachEvent)"); //IE exclusive method for binding an event
            mBuilder.AppendLine("window.attachEvent('onload', initIt);");
            mBuilder.AppendLine("else if (document.getElementById) ");//support older modern browsers
            mBuilder.AppendLine("window.onload=initIt;");

            mBuilder.AppendLine("var currentIndex = 0;");
            // basic page initialisation function
            mBuilder.AppendLine("function initIt()");
            mBuilder.AppendLine("{");

            //mBuilder.AppendLine("alert('starting the init.');");
            // Delete the last row in the arrays (cleanup)
            mBuilder.AppendLine(" document.getElementById('" + View.GetbtnEditClientID + "').disabled = true;");
            mBuilder.AppendLine("EnglishArray.pop();");
            mBuilder.AppendLine("AfrikaansArray.pop();");
            mBuilder.AppendLine("ConditionKeyArray.pop();");
            mBuilder.AppendLine("MustEditArray.pop();");

            // set up the javascript app variables
            mBuilder.AppendLine("var selectList = document.getElementById('" + View.GetlistGenericConditionsClientID + "');");
            mBuilder.AppendLine("var selectOptions = selectList.options;");

            // populate the select list from the English Display Array
            mBuilder.AppendLine(" ArrayCount = 0 ; ");
            mBuilder.AppendLine(" while (ArrayCount < EnglishArray.length)");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  selectOptions[ArrayCount] =  new Option(EnglishArray[ArrayCount]);");
            mBuilder.AppendLine("  selectOptions[ArrayCount].value = EnglishArray[ArrayCount];");
            mBuilder.AppendLine("  if (MustEditArray[ArrayCount] == 'True') ");
            mBuilder.AppendLine("{");
            //mBuilder.AppendLine("  selectOptions[ArrayCount].style.color= ChosenArrayCSSColor[ArrayCount] ;");
            mBuilder.AppendLine("  selectOptions[ArrayCount].style.backgroundColor = 'Silver';");
            mBuilder.AppendLine("}");
            mBuilder.AppendLine("  ArrayCount ++; ");
            mBuilder.AppendLine("}");
            mBuilder.AppendLine("}");

            // Clear and set the text display box with the selected text 
            mBuilder.AppendLine("function cleartext()");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  var chosenList = document.getElementById('" + View.GetlistGenericConditionsClientID + "');");
            mBuilder.AppendLine("  var chosenIndex = chosenList.selectedIndex;");
            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("  if (MustEditArray[chosenIndex] == 'True') ");
            mBuilder.AppendLine("  {document.getElementById('" + View.GetbtnEditClientID + "').disabled = false;} ");
            mBuilder.AppendLine("else  ");
            mBuilder.AppendLine("  {document.getElementById('" + View.GetbtnEditClientID + "').disabled = true;} ");
            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("  document.getElementById('" + View.GettxtDisplayClientID + "').value = EnglishArray[chosenIndex];");
            mBuilder.AppendLine("  document.getElementById('" + View.GettxtTranslateClientID + "').value = AfrikaansArray[chosenIndex];");
            mBuilder.AppendLine("}");

            // This function parses the Afrikaans Javascript Array back to Comma delimited text strings and writes them into the hidden fields.
            mBuilder.AppendLine("function StoreConditionsState() ");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("   var ArrayLength = 0;");
            mBuilder.AppendLine("   var StringBuild = '';");
            mBuilder.AppendLine("   var chosenList = document.getElementById('" + View.GetlistGenericConditionsClientID + "');");
            mBuilder.AppendLine("   var chosenIndex = chosenList.selectedIndex;");

            //mBuilder.AppendLine("   alert(chosenIndex);");
            //mBuilder.AppendLine("   alert(ConditionKeyArray[chosenIndex]);");

            mBuilder.AppendLine("   document.getElementById('" + View.GettxtConditionsKeyClientID + "').value = ConditionKeyArray[chosenIndex] ;");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtTranslateClientID + "').value = AfrikaansArray[chosenIndex] ;");

            mBuilder.AppendLine("}");
            View.RegisterClientScripts(mBuilder);

        }

        /// <summary>
        /// checks to make sure all thr translated tokens are populated before saving the condition set
        /// </summary>
        /// <returns></returns>
        bool AllTokensPopulated()
        {
            bool retval = true;
            for (int j = 0; j < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; j++)
                if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[j][3])) //Check if Selected
                    for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[1].Rows.Count; i++)
                    {
                        int TokenKey = Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[1].Rows[i][1]);
                        int ConditionKey = Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[j][1]);
                        if (TokenKey == ConditionKey)
                        {
                            string TokenValue = conditionsRepository.ConditionsSet.Tables[1].Rows[i][10].ToString();
                            if (TokenValue.Length == 0)
                            {
                                retval = false;
                                break;
                            }
                        }
                    }
            return retval;
        }
    }
}
