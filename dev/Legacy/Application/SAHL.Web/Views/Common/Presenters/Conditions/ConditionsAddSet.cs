using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.Conditions
{
    /// <summary>
    /// This View
    /// </summary>
    public class ConditionsAddSet : SAHLCommonBasePresenter<IConditionsSet>
    {

        private IConditionsRepository conditionsRepository;
        private bool MustBind;// = false;
        private CBONode cboNode;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ConditionsAddSet(IConditionsSet view, SAHLCommonBaseController controller)
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
            if (!View.ShouldRunPage) return;
            //Get the CBO Node   
            if (View.CurrentPrincipal != null) cboNode = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null) throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            //int GenericKey = 237288; //236097;//236097;
            //int GenericKeyTypeKey = 2;
            int GenericKey = cboNode.GenericKey;
            int GenericKeyTypeKey = cboNode.GenericKeyTypeKey;

           
            // Check the global cache to see if a condition object has been added
            conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();
            if (GlobalCacheData.ContainsKey(ViewConstants.ConditionSet))
            {
                if (View.IsMenuPostBack)
                    GlobalCacheData.Remove(ViewConstants.ConditionSet);
                else
                {
                    conditionsRepository = GlobalCacheData[ViewConstants.ConditionSet] as IConditionsRepository;
                    if (conditionsRepository != null)
                        if (!(conditionsRepository.GenericKey == cboNode.GenericKey))
                        {
                            GlobalCacheData.Remove(ViewConstants.ConditionSet);
                            conditionsRepository = RepositoryFactory.GetRepository<IConditionsRepository>();
                        }
                }
            }

            // this is an existing condition-set navigate to edit
            if (conditionsRepository != null)
                if (conditionsRepository.SavedConditionSetExists(GenericKey))
                    Navigator.Navigate("Update");
                else
                    if (!GlobalCacheData.ContainsKey(ViewConstants.ConditionSet))
                    {
                        conditionsRepository.Initialise(GenericKey);
                        // Populate _conditions and tokens
                        conditionsRepository.GetNewConditionSet(GenericKey, GenericKeyTypeKey);
                        conditionsRepository.GenericKey = GenericKey;
                        // Set up the Navigate Values
                        conditionsRepository.AddNavigateTo = "AddFromNew";
                        conditionsRepository.CancelNavigateTo = "CancelFromNew";
                        GlobalCacheData.Remove(ViewConstants.ConditionSet);
                        GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
                        MustBind = true;
                    }
                    else
                    {
                        MustBind = false;
                    }


            // Setup the View Interface
            View.btnAddConditionClicked += btnAddConditionClicked;
            View.btnEditConditionClicked += btnEditConditionClicked;
            View.btnCancelClicked += btnCancelClicked;
            View.btnSaveClicked += btnSaveClicked;
            View.ShowSaveButton = true;
            View.EnableSaveButton = true;
            View.ShowUpdateButton = false;
            View.ShowEditConditionButton = true;
            View.ShowAddButton = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            BindInitialData();
            RegisterClientScripts();

        }



        void btnAddConditionClicked(object sender, EventArgs e)
        {
            PopulateArrays();
            PopulateArraysforDBStorage();

            // Store the changes to The Session Object after view population
            GlobalCacheData.Remove(ViewConstants.ConditionSet);
            GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());

            if (_view.IsValid)
                Navigator.Navigate("Add");
        }



        void btnCancelClicked(object sender, EventArgs e)
        {
            if (_view.IsValid)
                Navigator.Navigate("Cancel");
        }


        void btnEditConditionClicked(object sender, EventArgs e)
        {

            PopulateArrays();
            PopulateArraysforDBStorage();

            //TODO - This needs to be re-Worked. If there are two identical conditions (should not be) - will edit the first
            string chosenstring = conditionsRepository.ChosenStrings[Convert.ToInt32(View.SettxttxtConditionTableKey)];
            for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; i++)
                if (chosenstring == conditionsRepository.ConditionsSet.Tables[0].Rows[i][4].ToString())
                {
                    conditionsRepository.EditableConditionKey = Convert.ToString(conditionsRepository.ConditionsSet.Tables[0].Rows[i][1]);
                    conditionsRepository.EditableConditionString = chosenstring;
                    conditionsRepository.EditableConditionValue = conditionsRepository.ConditionsSet.Tables[0].Rows[i][13].ToString();
                    conditionsRepository.EditableConditionIndex = Convert.ToInt32(View.SettxttxtConditionTableKey);
                    break;
                }
            // Store the changes to The Session Object after view population
            GlobalCacheData.Remove(ViewConstants.ConditionSet);
            GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());

            if (_view.IsValid)
                Navigator.Navigate("Edit");
        }


        void btnSaveClicked(object sender, EventArgs e)
        {
            PopulateArrays();
            PopulateArraysforDBStorage();

            string UnpopulatedTokenNames = AllTokensPopulated();
            if (UnpopulatedTokenNames.Length == 0)
            {
                conditionsRepository.SaveNewConditionSet();
                GlobalCacheData.Remove(ViewConstants.ConditionSet);
                //GlobalCacheData.Add(ViewConstants.ConditionSet, conditionsRepository, new List<ICacheObjectLifeTime>());
                if (_view.IsValid)
                    Navigator.Navigate("Save");
            }
            else
            {
                string warning = "The following token(s) need value(s) before you can save this condition set : " + UnpopulatedTokenNames;
                View.Messages.Add(new Error(warning, warning));
                View.ShowSaveButton = false;
            }
        }

        /// <summary>
        /// checks to make sure all tokens are populated before saving the condition set
        /// </summary>
        /// <returns></returns>
        string AllTokensPopulated()
        {
            string retval = "";
            for (int j = 0; j < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; j++)
                if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[j][3])) //Check if Selected
                    for (int i = 0; i < conditionsRepository.ConditionsSet.Tables[1].Rows.Count; i++)
                    {
                        int TokenKey = Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[1].Rows[i][1]);
                        int ConditionKey = Convert.ToInt32(conditionsRepository.ConditionsSet.Tables[0].Rows[j][1]);
                        if (TokenKey == ConditionKey)
                        {
                            string TokenValue = conditionsRepository.ConditionsSet.Tables[1].Rows[i][5].ToString();
                            if (TokenValue.Length == 0)
                            {
                                retval += "[" +  conditionsRepository.ConditionsSet.Tables[1].Rows[i][2] + "] " ;
                            }
                        }
                    }
            return retval;
        }

        /// <summary>
        /// Populate the Comma delimited strings with array data - for passing to Javascript
        /// </summary>
        void PopulateArrays()
        {
            conditionsRepository.SelectArrayValue = View.SettxtSelectArrayValue.Replace("??", "'");
            conditionsRepository.SelectArrayStrings = View.SettxtSelectArrayStrings.Replace("??", "'");
            conditionsRepository.SelectArrayCSSColor = View.SettxtSelectArrayCSSColor.Replace("??", "'");
            conditionsRepository.SelectArrayCSSWeight = View.SettxtSelectArrayCSSWeight.Replace("??", "'");
            conditionsRepository.SelectArrayID = View.SettxtSelectArrayID.Replace("??", "'");
            conditionsRepository.SelectUserEdited = View.SettxtSelectArrayUserEdited.Replace("??", "'");
            conditionsRepository.SelectUserConditionType = View.SettxtSelectUserConditionType.Replace("??", "'");
            conditionsRepository.SelectedArrayOfferConditionKeys = View.SettxtSelectedOfferConditionKeys.Replace("??", "'");
            conditionsRepository.SelectedArrayOfferConditionSetKeys = View.SettxtSelectedOfferConditionSetKeys.Replace("??", "'");

            conditionsRepository.ChosenArrayValue = View.SettxtChosenArrayValue.Replace("??", "'");
            conditionsRepository.ChosenArrayStrings = View.SettxtChosenArrayStrings.Replace("??", "'");
            conditionsRepository.ChosenArrayCSSColor = View.SettxtChosenArrayCSSColor.Replace("??", "'");
            conditionsRepository.ChosenArrayCSSWeight = View.SettxtChosenArrayCSSWeight.Replace("??", "'");
            conditionsRepository.ChosenArrayID = View.SettxtChosenArrayID.Replace("??", "'");
            conditionsRepository.ChosenUserEdited = View.SettxtChosenArrayUserEdited.Replace("??", "'");
            conditionsRepository.ChosenUserConditionType = View.SettxtChosenUserConditionType.Replace("??", "'");
            conditionsRepository.ChosenArrayOfferConditionKeys = View.SettxtChosenOfferConditionKeys.Replace("??", "'");
            conditionsRepository.ChosenArrayOfferConditionSetKeys = View.SettxtChosenOfferConditionSetKeys.Replace("??", "'");

            // Clear and re-Populate the arrays
            conditionsRepository.ParseArrays();
            // Clear and Re-populate the consitionSet.Tables[0]
            conditionsRepository.PopulateConditionsTableFromArrays();
        }

        /// <summary>
        /// Modify the Exisiting String Arrays to accomodate DB storage changes
        /// </summary>
        void PopulateArraysforDBStorage()
        {
            conditionsRepository.ChosenArrayStrings = "";
            conditionsRepository.ChosenArrayValue = "";
            conditionsRepository.SelectArrayStrings = "";
            conditionsRepository.SelectArrayValue = "";
            for (int x = 0; x < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; x++)
            {
                string ConditionTemplate = conditionsRepository.ConditionsSet.Tables[0].Rows[x][4].ToString();
                string ConditionPhrase = conditionsRepository.ConditionsSet.Tables[0].Rows[x][13].ToString();

                if (!Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[x][11]))
                {
                    conditionsRepository.ChosenArrayStrings += "'" + conditionsRepository.ConvertStringForDBStorage(ConditionTemplate) + "',";
                    conditionsRepository.ChosenArrayValue += "'" + conditionsRepository.ConvertStringForDBStorage(ConditionPhrase) + "',";
                }
                else
                {
                    conditionsRepository.SelectArrayStrings += "'" + conditionsRepository.ConvertStringForDBStorage(ConditionTemplate) + "',";
                    conditionsRepository.SelectArrayValue += "'" + conditionsRepository.ConvertStringForDBStorage(ConditionPhrase) + "',";
                }
            }
        }


        /// <summary>
        /// Populate the Data the first time this is run
        /// </summary>
        void BindInitialData()
        {

            if (MustBind)
                for (int x = 0; x < conditionsRepository.ConditionsSet.Tables[0].Rows.Count; x++)
                {
                    //TODO RE-EVALUATE THIS PROCESS BECAUSE CONDITION TEMPLATES CAN CONTAIN MULTIPLE TOKENS WITH DIFFERENT PROPERTIES. THIS IS FINE AT THE MOMENT BUT LOGICALLY FLAWED. ALSO AS CONDITIONS ARE NO LONGER STORED IN THE CONDITION TABLE, THE DEFINITION IS NOT STORED NEXT TO THE APPLICATION CONDITIONS.

                    /*      ConditionTypes
                    *	0	No Tokens
                    *	1	Tokenised String
                    *	2	Run time Session Token
                    *	3	Runtime executed SQL String
                    *	4	User captured Condition
                    *	5	Standard Condition edited by User
                    *	6	Tokenised String edited by User
                    */

                    string CanEdit = conditionsRepository.ConditionsSet.Tables[0].Rows[x][6].ToString();
                    string ConditionKey = conditionsRepository.ConditionsSet.Tables[0].Rows[x][1].ToString();
                    string ConditionType = conditionsRepository.ConditionsSet.Tables[0].Rows[x][12].ToString();
                    string ArrayWeight = conditionsRepository.ConditionsSet.Tables[0].Rows[x][8].ToString();
                    string CSSColor = conditionsRepository.ConditionsSet.Tables[0].Rows[x][9].ToString();
                    string ConditionSetKey = conditionsRepository.ConditionsSet.Tables[0].Rows[x][15].ToString();
                    string ConditionPhrase = conditionsRepository.ConditionsSet.Tables[0].Rows[x][4].ToString();
                    string ParsedPhrase = ConditionPhrase;
                    conditionsRepository.PopulateConditionsWithTokens((int)conditionsRepository.ConditionsSet.Tables[0].Rows[x][1], ref ParsedPhrase, false);
                    ConditionPhrase = conditionsRepository.ConvertStringForJavaScriptArray(ConditionPhrase);
                    ParsedPhrase = conditionsRepository.ConvertStringForJavaScriptArray(ParsedPhrase);

                    if (Convert.ToBoolean(conditionsRepository.ConditionsSet.Tables[0].Rows[x][11])) // is selected
                    {
                        conditionsRepository.ChosenArrayStrings += "'" + ConditionPhrase + "',"; // Tokenised
                        conditionsRepository.ChosenArrayCSSColor += "'" + CSSColor + "'" + ",";
                        conditionsRepository.ChosenArrayCSSWeight += "'" + ArrayWeight + "'" + ",";
                        conditionsRepository.ChosenArrayValue += "'" + ParsedPhrase + "',";
                        conditionsRepository.ChosenArrayID += "'" + ConditionKey + "',";
                        conditionsRepository.ChosenUserEdited += "'" + CanEdit + "',";
                        conditionsRepository.ChosenUserConditionType += "'" + ConditionType + "',";
                        conditionsRepository.ChosenArrayOfferConditionKeys += "'" + ConditionKey + "',";
                        conditionsRepository.ChosenArrayOfferConditionSetKeys += "'" + ConditionSetKey + "',";
                    }
                    else
                    {
                        conditionsRepository.SelectArrayStrings += "'" + ConditionPhrase + "',";
                        conditionsRepository.SelectArrayCSSColor += "'" + CSSColor + "'" + ",";
                        conditionsRepository.SelectArrayCSSWeight += "'" + ArrayWeight + "'" + ",";
                        conditionsRepository.SelectArrayValue += "'" + ParsedPhrase + "',";
                        conditionsRepository.SelectArrayID += "'" + ConditionKey + "',";
                        conditionsRepository.SelectUserEdited += "'" + CanEdit + "',";
                        conditionsRepository.SelectUserConditionType += "'" + ConditionType + "',";
                        conditionsRepository.SelectedArrayOfferConditionKeys += "'" + ConditionKey + "',";
                        conditionsRepository.SelectedArrayOfferConditionSetKeys += "'" + ConditionSetKey + "',";
                    }

                }

        }

        /// <summary>
        /// Create and Register the Views Javascript Model
        /// </summary>
        void RegisterClientScripts()
        {
            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
            mBuilder.AppendLine("var ArrayCount=0; ");

            // declare the javascript global variables for page
            mBuilder.AppendLine("var SelectArrayStrings =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.SelectArrayStrings) + "'');");
            mBuilder.AppendLine("var SelectArrayValue =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.SelectArrayValue) + "'');");
            mBuilder.AppendLine("var SelectArrayCSSColor =  new Array(" + conditionsRepository.SelectArrayCSSColor + "'');");
            mBuilder.AppendLine("var SelectArrayCSSWeight =  new Array(" + conditionsRepository.SelectArrayCSSWeight + "'');");
            mBuilder.AppendLine("var SelectArrayID =  new Array(" + conditionsRepository.SelectArrayID + "'');");
            mBuilder.AppendLine("var SelectArrayUserEdited =  new Array(" + conditionsRepository.SelectUserEdited + "'');");
            mBuilder.AppendLine("var SelectArrayUserConditionType =  new Array(" + conditionsRepository.SelectUserConditionType + "'');");
            mBuilder.AppendLine("var selectedArrayOfferConditionKeys =  new Array(" + conditionsRepository.SelectedArrayOfferConditionKeys + "'');");
            mBuilder.AppendLine("var selectedArrayOfferConditionSetKeys =  new Array(" + conditionsRepository.SelectedArrayOfferConditionSetKeys + "'');");

            mBuilder.AppendLine("var ChosenArrayStrings =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.ChosenArrayStrings) + "'');");
            mBuilder.AppendLine("var ChosenArrayValue =  new Array(" + conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.ChosenArrayValue) + "'');");
            mBuilder.AppendLine("var ChosenArrayCSSColor =  new Array(" + conditionsRepository.ChosenArrayCSSColor + "'');");
            mBuilder.AppendLine("var ChosenArrayCSSWeight =  new Array(" + conditionsRepository.ChosenArrayCSSWeight + "'');");
            mBuilder.AppendLine("var ChosenArrayID =  new Array(" + conditionsRepository.ChosenArrayID + "'');");
            mBuilder.AppendLine("var ChosenArrayUserEdited =  new Array(" + conditionsRepository.ChosenUserEdited + "'');");
            mBuilder.AppendLine("var ChosenArrayUserConditionType =  new Array(" + conditionsRepository.ChosenUserConditionType + "'');");
            mBuilder.AppendLine("var chosenArrayOfferConditionKeys =  new Array(" + conditionsRepository.ChosenArrayOfferConditionKeys + "'');");
            mBuilder.AppendLine("var chosenArrayOfferConditionSetKeys =  new Array(" + conditionsRepository.ChosenArrayOfferConditionSetKeys + "'');");
            //set up page initialisation for javascript environment
            //        mBuilder.AppendLine("window.attachEvent('onload',initIt);");
            mBuilder.AppendLine("if (window.addEventListener) ");//DOM method for binding an event
            mBuilder.AppendLine("window.addEventListener('load', initIt, false);");
            mBuilder.AppendLine("else if (window.attachEvent)"); //IE exclusive method for binding an event
            mBuilder.AppendLine("window.attachEvent('onload', initIt);");
            mBuilder.AppendLine("else if (document.getElementById) ");//support older modern browsers
            mBuilder.AppendLine("window.onload=initIt;");

            // basic page initialisation function
            mBuilder.AppendLine("function initIt()");
            mBuilder.AppendLine("{");

            mBuilder.AppendLine("document.getElementById('btnRemove').disabled = true;");
            mBuilder.AppendLine("document.getElementById('btnAdd').disabled = true;");

            mBuilder.AppendLine("popped = ChosenArrayStrings.pop();");
            mBuilder.AppendLine("popped = ChosenArrayCSSColor.pop();");
            mBuilder.AppendLine("popped = ChosenArrayCSSWeight.pop();");
            mBuilder.AppendLine("popped = ChosenArrayValue.pop();");
            mBuilder.AppendLine("popped = ChosenArrayID.pop();");
            mBuilder.AppendLine("popped = ChosenArrayUserEdited.pop();");
            mBuilder.AppendLine("popped = ChosenArrayUserConditionType.pop();");
            mBuilder.AppendLine("popped = chosenArrayOfferConditionKeys.pop();");
            mBuilder.AppendLine("popped = chosenArrayOfferConditionSetKeys.pop();");

            mBuilder.AppendLine("popped = SelectArrayStrings.pop();");
            mBuilder.AppendLine("popped = SelectArrayCSSColor.pop();");
            mBuilder.AppendLine("popped = SelectArrayCSSWeight.pop();");
            mBuilder.AppendLine("popped = SelectArrayValue.pop();");
            mBuilder.AppendLine("popped = SelectArrayID.pop();");
            mBuilder.AppendLine("popped = SelectArrayUserEdited.pop();");
            mBuilder.AppendLine("popped = SelectArrayUserConditionType.pop();");
            mBuilder.AppendLine("popped = selectedArrayOfferConditionKeys.pop();");
            mBuilder.AppendLine("popped = selectedArrayOfferConditionSetKeys.pop();");

            mBuilder.AppendLine("var chosenList = document.getElementById('ctl00_Main_listSelectedConditions');");
            mBuilder.AppendLine("var chosenOptions = chosenList.options;");
            mBuilder.AppendLine("var selectList = document.getElementById('ctl00_Main_listGenericConditions');");
            mBuilder.AppendLine("var selectOptions = selectList.options;");

            // populate the select lists from the arrays
            mBuilder.AppendLine(" ArrayCount = 0 ; ");
            mBuilder.AppendLine(" while (ArrayCount < SelectArrayStrings.length)");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  selectOptions[ArrayCount] =  new Option(SelectArrayStrings[ArrayCount].replace(/(\\r\\n|\\n|\\r)/gm, '   '));");
            mBuilder.AppendLine("  selectOptions[ArrayCount].value = SelectArrayValue[ArrayCount];");
            mBuilder.AppendLine("  selectOptions[ArrayCount].style.color= SelectArrayCSSColor[ArrayCount] ;");
            mBuilder.AppendLine("  selectOptions[ArrayCount].style.backgroundColor =  SelectArrayCSSWeight[ArrayCount];");
            mBuilder.AppendLine("  selectOptions[ArrayCount].id =  SelectArrayID[ArrayCount];");
            mBuilder.AppendLine("  ArrayCount ++; ");
            mBuilder.AppendLine("}");

            mBuilder.AppendLine(" ArrayCount = 0 ; ");
            mBuilder.AppendLine(" while (ArrayCount < ChosenArrayStrings.length)");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  chosenOptions[ArrayCount] =   new Option(ChosenArrayStrings[ArrayCount].replace(/(\\r\\n|\\n|\\r)/gm, '   '));");
            mBuilder.AppendLine("  chosenOptions[ArrayCount].value = ChosenArrayValue[ArrayCount];");
            mBuilder.AppendLine("  chosenOptions[ArrayCount].style.color= ChosenArrayCSSColor[ArrayCount] ;");
            mBuilder.AppendLine("  chosenOptions[ArrayCount].style.backgroundColor =  ChosenArrayCSSWeight[ArrayCount];");
            mBuilder.AppendLine("  chosenOptions[ArrayCount].id =  ChosenArrayID[ArrayCount];");
            mBuilder.AppendLine("  ArrayCount ++; ");
            mBuilder.AppendLine("}");

            // first time loading or return from an edit - reset selection

            if (conditionsRepository.EditableConditionIndex > -1) // edit
            {
                mBuilder.AppendLine("  document.getElementById('" + View.GettxtDisplayClientID + "').value = '" +  conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.EditableConditionValue) + "' ;");
                mBuilder.AppendLine("  chosenList.focus();");
                mBuilder.AppendLine("  chosenList.selectedIndex = '" + Convert.ToString(conditionsRepository.EditableConditionIndex) + "';");
            }
            else // added new condition
            {
                mBuilder.AppendLine("  document.getElementById('" + View.GettxtDisplayClientID + "').value = '" + conditionsRepository.ConvertStringForJavaScriptArray(conditionsRepository.EditableConditionValue) + "';");
                mBuilder.AppendLine("  chosenList.focus();");
                mBuilder.AppendLine("  chosenList.selectedIndex = chosenList.length;");
            }

            mBuilder.AppendLine("selectList.focus();");
            mBuilder.AppendLine("document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;");
            mBuilder.AppendLine("}");

            // Clear and set the text display box with the selected text 
            mBuilder.AppendLine("function cleartext(TextToView)");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine(" document.getElementById('" + View.GettxtDisplayClientID + "').value = TextToView;");
            mBuilder.AppendLine("  var chosenList = document.getElementById('ctl00_Main_listSelectedConditions');");
            mBuilder.AppendLine("  var chosenIndex = chosenList.selectedIndex;");

            // ADD THE INDEX FOR CHOSEN TO RETIREVE THE EDITABLE VALUE FROM THE ARRAY
            mBuilder.AppendLine("  document.getElementById('" + View.GettxtConditionTableKeyClientID + "').value = chosenIndex");

            mBuilder.AppendLine("  document.getElementById('" + View.GettxtSelectedIndexClientID + "').value = chosenIndex;");
            mBuilder.AppendLine(" StoreConditionsState();");
            mBuilder.AppendLine("}");

            // Add item from select to chosen
            mBuilder.AppendLine("function addIt()");
            mBuilder.AppendLine("{");



            mBuilder.AppendLine("  var selectList = document.getElementById('ctl00_Main_listGenericConditions');");
            mBuilder.AppendLine("  var selectOptions = selectList.options;");
            mBuilder.AppendLine("  var selectIndex = selectList.selectedIndex ;");
            mBuilder.AppendLine("  var selectLength = selectOptions.length;");
            mBuilder.AppendLine("  var chosenList = document.getElementById('ctl00_Main_listSelectedConditions');");
            mBuilder.AppendLine("  var chosenOptions = chosenList.options;");
            mBuilder.AppendLine("  var chosenIndex = chosenList.selectedIndex;");
            mBuilder.AppendLine("  var chosenLength = chosenOptions.length;");

            //mBuilder.AppendLine("  alert(SelectArrayValue[selectIndex]);");
            mBuilder.AppendLine("  if (SelectArrayValue[selectIndex].length >= 0)");
            mBuilder.AppendLine("  {");

            // process the lists
            mBuilder.AppendLine("  chosenOptions[chosenLength] = new Option(SelectArrayStrings[selectIndex].replace(/(\\r\\n|\\n|\\r)/gm, '   '));");
            mBuilder.AppendLine("  chosenOptions[chosenLength].value = SelectArrayValue[selectIndex];");
            mBuilder.AppendLine("  chosenOptions[chosenLength].style.color= SelectArrayCSSColor[selectIndex];");
            mBuilder.AppendLine("  chosenOptions[chosenLength].style.backgroundColor = SelectArrayCSSWeight[selectIndex];");
            mBuilder.AppendLine("  chosenOptions[chosenLength].id = SelectArrayID[selectIndex];");

            // process the arrays
            // add the item to the chosen arrays
            mBuilder.AppendLine("  ChosenArrayStrings.push(SelectArrayStrings[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayCSSColor.push(SelectArrayCSSColor[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayCSSWeight.push(SelectArrayCSSWeight[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayValue.push(SelectArrayValue[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayID.push(SelectArrayID[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayUserEdited.push(SelectArrayUserEdited[selectIndex]);");
            mBuilder.AppendLine("  ChosenArrayUserConditionType.push(SelectArrayUserConditionType[selectIndex]);");
            mBuilder.AppendLine("  chosenArrayOfferConditionKeys.push(selectedArrayOfferConditionKeys[selectIndex]);");
            mBuilder.AppendLine("  chosenArrayOfferConditionSetKeys.push(selectedArrayOfferConditionSetKeys[selectIndex]);");


            // delete the selected item from the select arrays
            mBuilder.AppendLine("  SelectArrayStrings.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayCSSColor.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayCSSWeight.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayValue.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayID.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayUserEdited.splice(selectIndex,1);");
            mBuilder.AppendLine("  SelectArrayUserConditionType.splice(selectIndex,1);");
            mBuilder.AppendLine("  selectedArrayOfferConditionKeys.splice(selectIndex,1);");
            mBuilder.AppendLine("  selectedArrayOfferConditionSetKeys.splice(selectIndex,1);");

            // deletethe item in the selected list
            mBuilder.AppendLine("  selectOptions[selectIndex] = null;");
            mBuilder.AppendLine("  selectList.focus();");
            mBuilder.AppendLine("  selectList.selectedIndex = selectIndex-1;");


            mBuilder.AppendLine("  }");
            mBuilder.AppendLine("}");

            // remove item from chosen and move to select
            mBuilder.AppendLine("function delIt()");
            mBuilder.AppendLine("{");

            mBuilder.AppendLine("  var selectList = document.getElementById('ctl00_Main_listGenericConditions');");
            mBuilder.AppendLine("  var selectOptions = selectList.options;");
            mBuilder.AppendLine("  var selectIndex = selectList.selectedIndex;");
            mBuilder.AppendLine("  var selectLength = selectOptions.length;");
            mBuilder.AppendLine("  var chosenList = document.getElementById('ctl00_Main_listSelectedConditions');");
            mBuilder.AppendLine("  var chosenOptions = chosenList.options;");
            mBuilder.AppendLine("  var chosenIndex = chosenList.selectedIndex;");
            mBuilder.AppendLine("  var chosenLength = chosenOptions.length;");

            mBuilder.AppendLine(" document.getElementById('" + View.GettxtDisplayClientID + "').value = '';");

            //mBuilder.AppendLine("  alert(ChosenArrayValue[chosenIndex]);");
            mBuilder.AppendLine("  if (ChosenArrayValue[chosenIndex].length >= 0)");
            mBuilder.AppendLine("  {");

            // process the lists
            mBuilder.AppendLine("  selectOptions[selectLength] = new Option(ChosenArrayStrings[chosenIndex].replace(/(\\r\\n|\\n|\\r)/gm, '   '));");
            mBuilder.AppendLine("  selectOptions[selectLength].value = ChosenArrayValue[chosenIndex];");
            mBuilder.AppendLine("  selectOptions[selectLength].style.color= ChosenArrayCSSColor[chosenIndex];");
            mBuilder.AppendLine("  selectOptions[selectLength].style.backgroundColor =  ChosenArrayCSSWeight[chosenIndex];");
            mBuilder.AppendLine("  selectOptions[selectLength].id =  ChosenArrayID[chosenIndex];");

            // add the item to the select array
            mBuilder.AppendLine("  SelectArrayStrings.push(ChosenArrayStrings[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayCSSColor.push(ChosenArrayCSSColor[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayCSSWeight.push(ChosenArrayCSSWeight[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayValue.push(ChosenArrayValue[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayID.push(ChosenArrayID[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayUserEdited.push(ChosenArrayUserEdited[chosenIndex]);");
            mBuilder.AppendLine("  SelectArrayUserConditionType.push(ChosenArrayUserConditionType[chosenIndex]);");
            mBuilder.AppendLine("  selectedArrayOfferConditionKeys.push(chosenArrayOfferConditionKeys[chosenIndex]);");
            mBuilder.AppendLine("  selectedArrayOfferConditionSetKeys.push(chosenArrayOfferConditionSetKeys[chosenIndex]);");


            // delete the selected item from the chosen array
            mBuilder.AppendLine("  ChosenArrayStrings.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayCSSColor.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayCSSWeight.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayValue.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayID.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayUserEdited.splice(chosenIndex,1);");
            mBuilder.AppendLine("  ChosenArrayUserConditionType.splice(chosenIndex,1);");
            mBuilder.AppendLine("  chosenArrayOfferConditionKeys.splice(chosenIndex,1);");
            mBuilder.AppendLine("  chosenArrayOfferConditionSetKeys.splice(chosenIndex,1);");

            // deletethe item in the selected list
            mBuilder.AppendLine("  chosenOptions[chosenIndex] = null;");
            mBuilder.AppendLine("  chosenList.focus();");
            mBuilder.AppendLine("  chosenList.selectedIndex = chosenIndex-1;");

            // reset the delete status of the selected condition
            mBuilder.AppendLine("  CanEdit();");
            mBuilder.AppendLine("  }");
            mBuilder.AppendLine("}");

            // function to check whether the condition is editable and make the edit button enabled or not
            mBuilder.AppendLine("function CanEdit()");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("  var chosenList = document.getElementById('ctl00_Main_listSelectedConditions');");
            mBuilder.AppendLine("  var chosenOptions = chosenList.options;");
            mBuilder.AppendLine("  var chosenIndex = chosenList.selectedIndex;");
            mBuilder.AppendLine("  var chosenLength = chosenOptions.length;");
            mBuilder.AppendLine("if (ChosenArrayUserEdited[chosenIndex] == 'True')");
            mBuilder.AppendLine("  {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;} ");
            mBuilder.AppendLine("else  ");
            mBuilder.AppendLine("  {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = false;} ");
            mBuilder.AppendLine("if (ChosenArrayCSSColor[chosenIndex] == 'Red')");
            mBuilder.AppendLine("  {document.getElementById('btnRemove').disabled = true;} ");
            mBuilder.AppendLine("else  ");
            mBuilder.AppendLine("  {document.getElementById('btnRemove').disabled = false;} ");

            /*      ConditionTypes
            *	0	No Tokens // User cannot Edit
            *	1	Tokenised String // User cannot Edit
            *	2	Run time Session Token // User cannot Edit
            *	3	Runtime executed SQL String // User cannot Edit
            *	4	User captured Condition // User can Edit
            *	5	Standard Condition edited by User // User can Edit
            *	6	Tokenised String edited by User // User can Edit Tokens Only
            */

            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '0')");
            mBuilder.AppendLine("   { document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;} ");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '1')");
            mBuilder.AppendLine("    {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;} ");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '2')");
            mBuilder.AppendLine("    {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;} ");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '3')");
            mBuilder.AppendLine("    {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = true;} ");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '4')");
            mBuilder.AppendLine("   { document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = false;} ");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '5')");
            mBuilder.AppendLine("   {document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = false; }");
            mBuilder.AppendLine("if (ChosenArrayUserConditionType[chosenIndex] == '6')");
            mBuilder.AppendLine("   { document.getElementById('" + View.GetbtnEditConditionClientID + "').disabled = false; }");
            mBuilder.AppendLine("}");

            mBuilder.AppendLine(" ");

            mBuilder.AppendLine("function StoreConditionsState() ");
            mBuilder.AppendLine("{");
            mBuilder.AppendLine("   var ArrayLength = 0;");
            mBuilder.AppendLine("   var StringBuild = '';");
            string Quote = "??";
            string QuoteComma = "??,";
            //mBuilder.AppendLine("   var CommaChar = '" + Quote + "';");

            mBuilder.AppendLine("   var count = 0;");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayStrings.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayStrings[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayStringsClientID + "').value = StringBuild;");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayCSSColor.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayCSSColor[count] + '" + QuoteComma + "';	");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayCSSColorClientID + "').value  = StringBuild;");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayCSSWeight.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayCSSWeight[count] + '" + QuoteComma + "';	");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayCSSWeightClientID + "').value = StringBuild;");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayValue.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayValue[count] + '" + QuoteComma + "';	");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayValueClientID + "').value = StringBuild;  ");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayID.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayID[count] + '" + QuoteComma + "'; ");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayIDClientID + "').value  = StringBuild;  ");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayUserEdited.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayUserEdited[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectArrayUserEditedClientID + "').value = StringBuild;  	");

            mBuilder.AppendLine("   ");
            mBuilder.AppendLine("   ArrayLength = SelectArrayUserConditionType.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + SelectArrayUserConditionType[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectUserConditionType + "').value = StringBuild;  	");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayStrings.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayStrings[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayStringsClientID + "').value  = StringBuild;  	");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayCSSColor.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayCSSColor[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayCSSColorClientID + "').value = StringBuild;  	");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayCSSWeight.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayCSSWeight[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayCSSWeightClientID + "').value = StringBuild;  	");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayValue.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayValue[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayValueClientID + "').value = StringBuild;  ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayID.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayID[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayIDClientID + "').value = StringBuild;  ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayUserEdited.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayUserEdited[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenArrayUserEditedClientID + "').value = StringBuild; ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = ChosenArrayUserConditionType.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + ChosenArrayUserConditionType[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenUserConditionType + "').value = StringBuild; ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = selectedArrayOfferConditionKeys.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + selectedArrayOfferConditionKeys[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectedOfferConditionKeys + "').value = StringBuild; ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = chosenArrayOfferConditionKeys.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + chosenArrayOfferConditionKeys[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenOfferConditionKeys + "').value = StringBuild; ");

            
            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = selectedArrayOfferConditionSetKeys.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + selectedArrayOfferConditionSetKeys[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtSelectedOfferConditionSetKeys + "').value = StringBuild; ");

            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   ArrayLength = chosenArrayOfferConditionSetKeys.length ;");
            mBuilder.AppendLine("   StringBuild = '';");
            mBuilder.AppendLine("   count = 0;");
            mBuilder.AppendLine("   	while (count < ArrayLength)");
            mBuilder.AppendLine("   	{");
            mBuilder.AppendLine("   	StringBuild += '" + Quote + "' + chosenArrayOfferConditionSetKeys[count] + '" + QuoteComma + "';");
            mBuilder.AppendLine("   	count++;");
            mBuilder.AppendLine("   	}");
            mBuilder.AppendLine("   document.getElementById('" + View.GettxtChosenOfferConditionSetKeys + "').value = StringBuild; ");


            mBuilder.AppendLine(" }");
            View.RegisterClientScripts(mBuilder);
        }
    }
}
