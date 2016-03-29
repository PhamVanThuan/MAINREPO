using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IConditionsRepository))]
    public class ConditionsRepository : AbstractRepositoryBase, IConditionsRepository
    {
        public ConditionsRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public ConditionsRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        /* The following strings are used for handling the the data in CSV strings
        * for parsing to and from the Javascript front end in the views
        */

        private DataSet conditionsSet = new DataSet();

        public DataSet ConditionsSet
        {
            get { return conditionsSet; }
            set { conditionsSet = value; }
        }

        /// <summary>
        /// Creates the ConditionSet Table
        /// </summary>
        private static DataSet CreateConditionDataSet()
        {
            DataSet CS = new DataSet();
            CS.Tables.Add(CreateConditionsTable());
            CS.Tables.Add(CreateTokensTable());
            return CS;
        }

        private static DataTable CreateConditionsTable()
        {
            /*      ConditionTypes  - 12
            *	0	No Tokens
            *	1	Tokenised String
            *	2	Run time Session Token
            *	3	Runtime executed SQL String
            *	4	User captured Condition
            *	5	Standard Condition edited by User
            *	6	Tokenised String edited by User
            */

            DataTable conditions = new DataTable();
            conditions.Columns.Add("Key", Type.GetType("System.Int32")); //0

            //DataColumn myDataColumn = new DataColumn();
            //myDataColumn.DataType = Type.GetType("System.Int32");
            //myDataColumn.ColumnName = "ConditionKey";
            //myDataColumn.ReadOnly = false;
            //myDataColumn.Unique = false;
            //conditions.Columns.Add(myDataColumn);//1

            //DataColumn[] myPrimaryKeyColumns = new DataColumn[1];
            //myPrimaryKeyColumns[0] = conditions.Columns["ConditionKey"];

            conditions.Columns.Add("ConditionKey", Type.GetType("System.Int32")); //1

            conditions.Columns.Add("TranslatableItemKey", Type.GetType("System.Int32")); //2
            conditions.Columns.Add("Selected", Type.GetType("System.Boolean")); //3
            conditions.Columns.Add("Template", Type.GetType("System.String")); //4
            conditions.Columns.Add("TranslatedTemplate", Type.GetType("System.String")); //5
            conditions.Columns.Add("UserEditable", Type.GetType("System.Boolean")); //6
            conditions.Columns.Add("Required", Type.GetType("System.Boolean")); //7
            conditions.Columns.Add("CSSColor", Type.GetType("System.String")); //8
            conditions.Columns.Add("CSSWeight", Type.GetType("System.String")); //9
            conditions.Columns.Add("DisplayIndex", Type.GetType("System.Int32")); //10
            conditions.Columns.Add("IsSelectTokenValued", Type.GetType("System.Boolean")); //11
            conditions.Columns.Add("ConditionType", Type.GetType("System.Int32")); //12
            conditions.Columns.Add("ConditionValue", Type.GetType("System.String")); //13
            conditions.Columns.Add("ApplicationConditionKey", Type.GetType("System.Int32")); //14
            conditions.Columns.Add("CondtionSetKey", Type.GetType("System.Int32")); //15

            //conditions.Columns.Add("ConditionName", Type.GetType("System.String")); //16

            conditions.TableName = "Conditions";
            return conditions;
        }

        private static DataTable CreateTokensTable()
        {
            DataTable tokenstable = new DataTable();
            tokenstable.Columns.Add("Key", Type.GetType("System.Int32")); //0
            tokenstable.Columns.Add("ConditionKey", Type.GetType("System.Int32")); //1
            tokenstable.Columns.Add("tokenName", Type.GetType("System.String")); //2
            tokenstable.Columns.Add("tokenDescription", Type.GetType("System.String")); //3
            tokenstable.Columns.Add("tokenTypeKey", Type.GetType("System.Int32")); //4
            tokenstable.Columns.Add("tokenValue", Type.GetType("System.String")); //5
            tokenstable.Columns.Add("tokenStatementDefinitionKey", Type.GetType("System.Int32")); //6
            tokenstable.Columns.Add("tokenMustTranslate", Type.GetType("System.Boolean")); //7
            tokenstable.Columns.Add("tokenParameterTypeKey", Type.GetType("System.Int32")); //8
            tokenstable.Columns.Add("TokenKey", Type.GetType("System.Int32")); //9
            tokenstable.Columns.Add("TranslatedToken", Type.GetType("System.String")); //10
            tokenstable.TableName = "Tokens";
            return tokenstable;
        }

        private string[] compareTokens;

        ///////////////////////////////////////////////////////////////////
        // The Basic Parameters for the Offer
        private int offerKey = -1;

        private int accountKey;
        private int genericKey;
        private int legalEntityLanguageKey;
        private bool legalEntityLanguageTranslatable;
        private int userEnteredConditionKey = -1;
        private bool translating;
        private string userAddedConditionName;

        ///////////////////////////////////////////////////////////////
        private bool ReadOnly;

        ///////////////////////////////////////////////////////////////
        private string[] chosenStrings;

        private string[] chosenID;
        private string[] chosenCSSColor;
        private string[] chosenCSSWeight;
        private string[] chosenValue;
        private string[] chosenEdited;
        private string[] chosenArrayUserConditionType;
        private string[] chosenOfferConditionKeys;
        private string[] chosenOfferConditionSetKeys;

        private string chosenArrayStrings;
        private string chosenArrayID;
        private string chosenArrayCSSColor;
        private string chosenArrayCSSWeight;
        private string chosenArrayValue;
        private string chosenUserEdited;
        private string chosenUserConditionType;
        private string chosenArrayOfferConditionKeys;
        private string chosenArrayOfferConditionSetKeys;

        private string[] selectedStrings;
        private string[] selectedID;
        private string[] selectedCSSColor;
        private string[] selectedCSSWeight;
        private string[] selectedValue;
        private string[] selectedEdited;
        private string[] selectedArrayUserConditionType;
        private string[] selectedOfferConditionKeys;
        private string[] selectedOfferConditionSetKeys;

        private string selectArrayStrings;
        private string selectArrayID;
        private string selectArrayCSSColor;
        private string selectArrayCSSWeight;
        private string selectArrayValue;
        private string selectUserEdited;
        private string selectUserConditionType;
        private string selectedArrayOfferConditionKeys;
        private string selectedArrayOfferConditionSetKeys;

        ////////////////////////////////////////////////////////////

        public string[] ChosenArrayUserConditionType
        {
            get { return chosenArrayUserConditionType; }
            set { chosenArrayUserConditionType = value; }
        }

        public string[] ChosenStrings
        {
            get { return chosenStrings; }
            set { chosenStrings = value; }
        }

        public string[] ChosenID
        {
            get { return chosenID; }
            set { chosenID = value; }
        }

        public string[] ChosenCSSColor
        {
            get { return chosenCSSColor; }
            set { chosenCSSColor = value; }
        }

        public string[] ChosenCSSWeight
        {
            get { return chosenCSSWeight; }
            set { chosenCSSWeight = value; }
        }

        public string[] ChosenValue
        {
            get { return chosenValue; }
            set { chosenValue = value; }
        }

        public string[] ChosenEdited
        {
            get { return chosenEdited; }
            set { chosenEdited = value; }
        }

        public string[] ChosenOfferConditionKeys
        {
            get { return chosenOfferConditionKeys; }
            set { chosenOfferConditionKeys = value; }
        }

        public string[] ChosenOfferConditionSetKeys
        {
            get { return chosenOfferConditionSetKeys; }
            set { chosenOfferConditionSetKeys = value; }
        }

        //public string ChosenArrayOfferConditionKeys
        //{
        //    get { return chosenArrayOfferConditionKeys; }
        //    set { chosenArrayOfferConditionKeys = value; }
        //}

        public string[] SelectedStrings
        {
            get { return selectedStrings; }
            set { selectedStrings = value; }
        }

        public string[] SelectedID
        {
            get { return selectedID; }
            set { selectedID = value; }
        }

        public string[] SelectedCSSColor
        {
            get { return selectedCSSColor; }
            set { selectedCSSColor = value; }
        }

        public string[] SelectedCSSWeight
        {
            get { return selectedCSSWeight; }
            set { selectedCSSWeight = value; }
        }

        public string[] SelectedValue
        {
            get { return selectedValue; }
            set { selectedValue = value; }
        }

        public string[] SelectedEdited
        {
            get { return selectedEdited; }
            set { selectedEdited = value; }
        }

        public string[] SelectedArrayUserConditionType
        {
            get { return selectedArrayUserConditionType; }
            set { selectedArrayUserConditionType = value; }
        }

        public string[] SelectedOfferConditionKeys
        {
            get { return selectedOfferConditionKeys; }
            set { selectedOfferConditionKeys = value; }
        }

        public string[] SelectedOfferConditionSetKeys
        {
            get { return selectedOfferConditionSetKeys; }
            set { selectedOfferConditionSetKeys = value; }
        }

        // The following variables contain information about the text being edited
        private string editableConditionKey;

        private string editableConditionString;
        private string editableTranslationString;
        private string editableConditionValue;
        private int editableConditionIndex = -1;
        private string[] editableConditionTokens;

        // DATASETS //////////////////////////////////////////////
        //DAO Objects below formed the Original Conditions Dataset
        private IList<IConditionSetCondition> conditionList;

        private Token_DAO[] tokens;

        ///////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or Sets the CompareToken Array
        /// </summary>
        public string[] CompareTokens
        {
            get { return compareTokens; }
            set { compareTokens = value; }
        }

        /// <summary>
        /// Gets or Sets the Offer Key value
        /// </summary>
        public int OfferKey
        {
            get { return offerKey; }
            set { offerKey = value; }
        }

        /// <summary>
        /// Gets or Sets the ChosenArrayOfferConditionKeys
        /// </summary>
        public string ChosenArrayOfferConditionKeys
        {
            get { return chosenArrayOfferConditionKeys; }
            set { chosenArrayOfferConditionKeys = value; }
        }

        /// <summary>
        /// Gets or Sets the ChosenArrayOfferConditionSetKeys
        /// </summary>
        public string ChosenArrayOfferConditionSetKeys
        {
            get { return chosenArrayOfferConditionSetKeys; }
            set { chosenArrayOfferConditionSetKeys = value; }
        }

        /// <summary>
        /// Gets or Sets the SelectedArrayOfferConditionKeys
        /// </summary>
        public string SelectedArrayOfferConditionKeys
        {
            get { return selectedArrayOfferConditionKeys; }
            set { selectedArrayOfferConditionKeys = value; }
        }

        /// <summary>
        /// Gets or Sets the SelectedArrayOfferConditionSetKeys
        /// </summary>
        public string SelectedArrayOfferConditionSetKeys
        {
            get { return selectedArrayOfferConditionSetKeys; }
            set { selectedArrayOfferConditionSetKeys = value; }
        }

        /// <summary>
        /// Gets or Sets the User entered condition Key Value
        /// </summary>
        public int UserEnteredConditionKey
        {
            get { return userEnteredConditionKey; }
            set { userEnteredConditionKey = value; }
        }

        /// <summary>
        /// Gets or Sets the Generic Key value
        /// </summary>
        public int GenericKey
        {
            get { return genericKey; }
            set { genericKey = value; }
        }

        /// <summary>
        /// Gets or Sets the Account Key value
        /// </summary>
        public int AccountKey
        {
            get { return accountKey; }
            set { accountKey = value; }
        }

        /// <summary>
        /// Gets or Sets the Legal Entity Language Key
        /// </summary>
        public int LegalEntityLanguageKey
        {
            get { return legalEntityLanguageKey; }
            set { legalEntityLanguageKey = value; }
        }

        /// <summary>
        /// Gets or Sets whether the Legal Entity Language is Translatable
        /// </summary>
        public bool LegalEntityLanguageTranslatable
        {
            get { return legalEntityLanguageTranslatable; }
            set { legalEntityLanguageTranslatable = value; }
        }

        /// <summary>
        /// Gets or Sets whether the repository is being used in translate mode (for data retrieval and storage)
        /// </summary>
        public bool Translating
        {
            get { return translating; }
            set { translating = value; }
        }

        /// <summary>
        /// Gets or Sets the condition Name - Used for display purposes
        /// </summary>
        public string UserAddedConditionName
        {
            get { return userAddedConditionName; }
            set { userAddedConditionName = value; }
        }

        /// <summary>
        /// Gets or Sets the Selected Array Strings
        /// </summary>
        public string SelectArrayStrings
        {
            get { return selectArrayStrings; }
            set { selectArrayStrings = value; }
        }

        /// <summary>
        /// Gets or Sets the Selected Array ID
        /// </summary>
        public string SelectArrayID
        {
            get { return selectArrayID; }
            set { selectArrayID = value; }
        }

        /// <summary>
        /// Gets or Sets the Select Array CSS Color
        /// </summary>
        public string SelectArrayCSSColor
        {
            get { return selectArrayCSSColor; }
            set { selectArrayCSSColor = value; }
        }

        /// <summary>
        /// Gets or Sets the Selec Array CSS Weight
        /// </summary>
        public string SelectArrayCSSWeight
        {
            get { return selectArrayCSSWeight; }
            set { selectArrayCSSWeight = value; }
        }

        /// <summary>
        /// Gets or Sets the Select Array Value
        /// </summary>
        public string SelectArrayValue
        {
            get { return selectArrayValue; }
            set { selectArrayValue = value; }
        }

        /// <summary>
        /// Gets or Sets the Select User Edited value
        /// </summary>
        public string SelectUserEdited
        {
            get { return selectUserEdited; }
            set { selectUserEdited = value; }
        }

        /// <summary>
        /// Gets or Sets the Selected conditions condition Type
        /// </summary>
        public string SelectUserConditionType
        {
            get { return selectUserConditionType; }
            set { selectUserConditionType = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen Array Strings
        /// </summary>
        public string ChosenArrayStrings
        {
            get { return chosenArrayStrings; }
            set { chosenArrayStrings = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen Array ID value
        /// </summary>
        public string ChosenArrayID
        {
            get { return chosenArrayID; }
            set { chosenArrayID = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen Array CSS Color
        /// </summary>
        public string ChosenArrayCSSColor
        {
            get { return chosenArrayCSSColor; }
            set { chosenArrayCSSColor = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen Array CSS Weight
        /// </summary>
        public string ChosenArrayCSSWeight
        {
            get { return chosenArrayCSSWeight; }
            set { chosenArrayCSSWeight = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen Array Value
        /// </summary>
        public string ChosenArrayValue
        {
            get { return chosenArrayValue; }
            set { chosenArrayValue = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen User edited value
        /// </summary>
        public string ChosenUserEdited
        {
            get { return chosenUserEdited; }
            set { chosenUserEdited = value; }
        }

        /// <summary>
        /// Gets or Sets the Chosen conditions condition Type
        /// </summary>
        public string ChosenUserConditionType
        {
            get { return chosenUserConditionType; }
            set { chosenUserConditionType = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable Condition Key
        /// </summary>
        public string EditableConditionKey
        {
            get { return editableConditionKey; }
            set { editableConditionKey = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable Condition String value
        /// </summary>
        public string EditableConditionString
        {
            get { return editableConditionString; }
            set { editableConditionString = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable Translation Condition String value
        /// </summary>
        public string EditableTranslationString
        {
            get { return editableTranslationString; }
            set { editableTranslationString = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable Condition Value
        /// </summary>
        public string EditableConditionValue
        {
            get { return editableConditionValue; }
            set { editableConditionValue = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable Condition Index
        /// </summary>
        public int EditableConditionIndex
        {
            get { return editableConditionIndex; }
            set { editableConditionIndex = value; }
        }

        /// <summary>
        /// Gets or Sets the Editable condition token Array
        /// </summary>
        public string[] EditableConditionTokens
        {
            get { return editableConditionTokens; }
            set { editableConditionTokens = value; }
        }

        private EventList<ICondition> lstConditions;

        /// <summary>
        /// Gets or Sets a list of condition objects used primarily for the translatiosn screen
        /// </summary>
        public EventList<ICondition> LstConditions
        {
            get { return lstConditions; }
            set { lstConditions = value; }
        }

        ///////////////////////////////////////////////////////////////
        /// Edit/Add Navigate values
        private string addNavigateTo = "";

        private string cancelNavigateTo = "";

        /// <summary>
        /// Gets or Sets the UpdateNavigateTo value
        /// </summary>
        public string CancelNavigateTo
        {
            get { return cancelNavigateTo; }
            set { cancelNavigateTo = value; }
        }

        /// <summary>
        /// Gets or Sets the AddNavigateTo value
        /// </summary>
        public string AddNavigateTo
        {
            get { return addNavigateTo; }
            set { addNavigateTo = value; }
        }

        //STRING FUNCTIONS///////////////////////////////////////////////////////////////////////
        // Done
        /// <summary>
        /// Right Trims a String
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="StringLength">The Lenth if the required string</param>
        /// <returns>The Trimmed String</returns>
        public string TrimString(string Input, int StringLength)
        {
            if (Input.Length > StringLength)
            {
                //string RetString = "";
                StringBuilder RetString = new StringBuilder();
                for (int x = 0; x < StringLength; x++)
                {
                    //RetString += Input[x];
                    RetString.Append(Input[x]);
                }
                return RetString.ToString();
            }
            return Input;
        }

        /// <summary>
        /// Splits a ConditionString into its condition name and condition.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string[0] = conditionName , string[1] = Condition </returns>
        public string[] SplitConditionAndName(string input)
        {
            string[] returnArray = new string[] { "", "" };
            char bracket = Convert.ToChar(")");
            int bracketIndex = input.IndexOf(bracket, 0);
            returnArray[1] = input.Substring(bracketIndex + 2);
            returnArray[0] = input.Remove(bracketIndex);

            return returnArray;
        }

        //Done
        /// <summary>
        /// Populates the selected condition with Tokens that have been automatically populated
        /// This method is for display purposes only
        /// This Method
        /// </summary>
        /// <param name="ConditionKey"></param>
        /// <param name="ConditionPhrase"></param>
        /// <param name="TranslatedCondition"></param>
        public void PopulateConditionsWithTokens(int ConditionKey, ref string ConditionPhrase, bool TranslatedCondition)
        {
            for (int i = 0; i < conditionsSet.Tables[1].Rows.Count; i++)
                if ((int)conditionsSet.Tables[1].Rows[i][1] == ConditionKey)
                    if (!TranslatedCondition)
                    {
                        if (conditionsSet.Tables[1].Rows[i][5].ToString().Length > 0)
                        {
                            string oldvalue = "[" + conditionsSet.Tables[1].Rows[i][2] + "]";
                            string newvalue = conditionsSet.Tables[1].Rows[i][5].ToString();
                            ConditionPhrase = ConditionPhrase.Replace(oldvalue, newvalue);
                        }
                    }
                    else
                    {
                        if (conditionsSet.Tables[1].Rows[i][10].ToString().Length > 0)
                        {
                            string oldvalue = "[" + conditionsSet.Tables[1].Rows[i][2] + "]";
                            string newvalue = conditionsSet.Tables[1].Rows[i][10].ToString();
                            ConditionPhrase = ConditionPhrase.Replace(oldvalue, newvalue);
                        }
                    }
        }

        // Done
        /// <summary>
        /// Convert a display string for storage between Javascript linebreak and ASCII syntax linebreak
        /// </summary>
        /// <param name="original">String to be parsed for the linebreaks</param>
        /// <returns>A String in the correct format for SQL storage</returns>
        public string ConvertStringForDBStorage(string original)
        {
            if (original != null)
            {
                string pattern = "  ";
                string replacement = " ";
                original = original.Replace(pattern, replacement);

                pattern = "+ +";
                replacement = "+";
                original = original.Replace(pattern, replacement);

                pattern = "\\r\\n";
                replacement = "\r\n";
                original = original.Replace(pattern, replacement);

                pattern = " + char(13) + char(10) + ";
                replacement = "\r\n";
                original = original.Replace(pattern, replacement);
            }
            return original;
        }

        /// <summary>
        /// This method must only be run on a single condition, and not an array
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public string NormaliseParenthesis(string original)
        {
            if (original != null)
            {
                ConvertStringForJavaScriptArray(original);

                string pattern = "'";
                string replacement = "\\'";
                original = original.Replace(pattern, replacement);

                pattern = "  ";
                replacement = " ";
                original = original.Replace(pattern, replacement);

                pattern = "+ +";
                replacement = "+";
                original = original.Replace(pattern, replacement);

                pattern = "+ char(13) + char(10) +";
                replacement = "\\r\\n";
                original = original.Replace(pattern, replacement);
            }

            return original;
        }

        /// <summary>
        /// Convert a string to contain linbreaks compatible for Javascript display from ASCII syntax Line Breaks
        /// </summary>
        /// <param name="original">Original string containing ASCII Line Breaks</param>
        /// <returns>String containing Javascript Linebreaks</returns>
        public string ConvertStringForJavaScriptArray(string original)
        {
            if (original != null)
            {
                // Test for ' and replace with \'
                //string RetString = "";
                StringBuilder RetString = new StringBuilder();
                for (int x = 0; x < original.Length; x++)
                {
                    if (x != 0 && x != original.Length - 1)
                        if (original[x].ToString() == "'")
                        {
                            if (original[x - 1].ToString() == "\\" | original[x - 1].ToString() == "," | original[x + 1].ToString() == ",")
                            {
                            }
                            else
                            {
                                // This is the one
                                //RetString += "\\'";
                                RetString.Append("\\'");
                                x++;
                            }
                        }

                    //RetString += original[x];
                    RetString.Append(original[x]);
                }

                original = RetString.ToString();

                string pattern = "  ";
                string replacement = " ";
                original = original.Replace(pattern, replacement);

                pattern = "+ +";
                replacement = "+";
                original = original.Replace(pattern, replacement);

                pattern = "+ char(13) + char(10) +";
                replacement = "\\r\\n";
                original = original.Replace(pattern, replacement);

                pattern = "\r\n";
                replacement = "\\r\\n";
                original = original.Replace(pattern, replacement);
            }

            return original;
        }

        // Done
        /// <summary>
        /// convert ASCII linebreaks to display in HTML
        /// </summary>
        /// <param name="original">Origininal string containing ASCII linebreaks</param>
        /// <returns>String formatted for HTML linebreaks</returns>
        public string ConvertStringForHTMLDisplay(string original)
        {
            if (original != null)
            {
                string pattern = "\'";
                string replacement = "'";
                original = original.Replace(pattern, replacement);

                pattern = "+ char(13) + char(10) +";
                replacement = "\r\n";
                original = original.Replace(pattern, replacement);

                pattern = "  ";
                replacement = " ";
                original = original.Replace(pattern, replacement);
            }
            return original;
        }

        /// <summary>
        /// Parse all display arrays (for Javascript) from  Comma delimited strings
        /// </summary>
        public void ParseArrays()
        {
            chosenStrings = ParseStringToArray(chosenArrayStrings);
            chosenID = ParseStringToArray(chosenArrayID);
            chosenCSSColor = ParseStringToArray(chosenArrayCSSColor);
            chosenCSSWeight = ParseStringToArray(chosenArrayCSSWeight);
            chosenValue = ParseStringToArray(chosenArrayValue);
            chosenEdited = ParseStringToArray(chosenUserEdited);
            chosenArrayUserConditionType = ParseStringToArray(chosenUserConditionType);
            chosenOfferConditionKeys = ParseStringToArray(chosenArrayOfferConditionKeys);
            chosenOfferConditionSetKeys = ParseStringToArray(chosenArrayOfferConditionSetKeys);

            selectedStrings = ParseStringToArray(selectArrayStrings);
            selectedID = ParseStringToArray(selectArrayID);
            selectedCSSColor = ParseStringToArray(selectArrayCSSColor);
            selectedCSSWeight = ParseStringToArray(selectArrayCSSWeight);
            selectedValue = ParseStringToArray(selectArrayValue);
            selectedEdited = ParseStringToArray(selectUserEdited);
            selectedArrayUserConditionType = ParseStringToArray(selectUserConditionType);
            selectedOfferConditionKeys = ParseStringToArray(selectedArrayOfferConditionKeys);
            selectedOfferConditionSetKeys = ParseStringToArray(selectedArrayOfferConditionSetKeys);

            // Resize ChosenOfferConditionKeys to the correct array size
            Array.Resize(ref chosenOfferConditionKeys, chosenStrings.Length);
            Array.Resize(ref chosenOfferConditionSetKeys, chosenStrings.Length);
        }

        /// <summary>
        /// Parse all display comma delimted strings (for Javascript) from Arrays
        /// </summary>
        public void ParseStrings()
        {
            chosenArrayStrings = ParseArrayToString(chosenStrings);
            chosenArrayID = ParseArrayToString(chosenID);
            chosenArrayCSSColor = ParseArrayToString(chosenCSSColor);
            chosenArrayCSSWeight = ParseArrayToString(chosenCSSWeight);
            chosenArrayValue = ParseArrayToString(chosenValue);
            chosenUserEdited = ParseArrayToString(chosenEdited);
            chosenUserConditionType = ParseArrayToString(chosenArrayUserConditionType);
            chosenArrayOfferConditionKeys = ParseArrayToString(chosenOfferConditionKeys);
            chosenArrayOfferConditionSetKeys = ParseArrayToString(chosenOfferConditionSetKeys);

            selectArrayStrings = ParseArrayToString(selectedStrings);
            selectArrayID = ParseArrayToString(selectedID);
            selectArrayCSSColor = ParseArrayToString(selectedCSSColor);
            selectArrayCSSWeight = ParseArrayToString(selectedCSSWeight);
            selectArrayValue = ParseArrayToString(selectedValue);
            selectUserEdited = ParseArrayToString(selectedEdited);
            selectUserConditionType = ParseArrayToString(selectedArrayUserConditionType);
            selectedArrayOfferConditionKeys = ParseArrayToString(selectedOfferConditionKeys);
            selectedArrayOfferConditionSetKeys = ParseArrayToString(selectedOfferConditionSetKeys);
        }

        /// <summary>
        /// Save The variables from The Arrays back into the Conditions Table for Saving.
        /// </summary>
        public void PopulateConditionsTableFromArrays()
        {
            // clear the table data
            conditionsSet.Tables[0].Clear();
            int count = 0;

            // add the chosen records to the table

            if (selectedStrings[0] != null)
                for (int i = 0; i < selectedStrings.Length; i++)
                {
                    DataRow dr = conditionsSet.Tables[0].NewRow();
                    dr[0] = count;  //Key
                    dr[1] = selectedOfferConditionKeys[i];//ConditionKey
                    dr[2] = selectedOfferConditionKeys[i];//ConditionKey
                    dr[3] = false; //IsSelected

                    // if (Convert.ToBoolean(selectedEdited[i])) //UserEditable

                    //  else
                    dr[4] = selectedStrings[i]; //Template

                    //dr[4] = selectedValue[i]; //Template
                    dr[5] = "";
                    dr[6] = Convert.ToBoolean(selectedEdited[i]); //UserEditable
                    dr[7] = Convert.ToInt32(selectedArrayUserConditionType[i]) < 4;
                    dr[8] = selectedCSSColor[i];//CSSColor
                    dr[9] = selectedCSSWeight[i]; //CSSWeight
                    dr[10] = i; //DisplayIndex
                    dr[11] = true; //Selected
                    dr[12] = selectedOfferConditionKeys[i];// condition type key
                    dr[13] = selectedValue[i]; // Edited Value
                    dr[14] = Convert.ToInt32(selectedID[i]); //Offer Condition Key or -1
                    if (!string.IsNullOrEmpty(selectedOfferConditionSetKeys[i]))
                        dr[15] = Convert.ToInt32(selectedOfferConditionSetKeys[i]); //Offer Condition Set Key or -1
                    count++;
                    conditionsSet.Tables[0].Rows.Add(dr);
                }

            // add the non-selected records to the table
            if (chosenStrings[0] != null)
                for (int i = 0; i < chosenStrings.Length; i++)
                {
                    DataRow dr = conditionsSet.Tables[0].NewRow();
                    dr[0] = count;  //Key
                    dr[1] = chosenOfferConditionKeys[i];//ConditionKey
                    dr[2] = chosenOfferConditionKeys[i];//ConditionKey
                    dr[3] = true; //IsSelected

                    //if (Convert.ToBoolean(chosenEdited[i])) //UserEditable

                    //else
                    dr[4] = chosenStrings[i]; //Template
                    dr[5] = "";
                    dr[6] = Convert.ToBoolean(chosenEdited[i]); //UserEditable
                    dr[7] = Convert.ToInt32(chosenArrayUserConditionType[i]) < 4;
                    dr[8] = chosenCSSColor[i];//CSSColor
                    dr[9] = chosenCSSWeight[i]; //CSSWeight
                    dr[10] = i; //DisplayIndex
                    dr[11] = false; //Selected
                    dr[12] = chosenArrayUserConditionType[i];// condition type key
                    dr[13] = chosenValue[i]; //Edited Value
                    dr[14] = Convert.ToInt32(chosenID[i]); // OfferConditionKey or -1
                    if (!string.IsNullOrEmpty(chosenOfferConditionSetKeys[i]))
                        dr[15] = Convert.ToInt32(chosenOfferConditionSetKeys[i]); // OfferConditionSetKey or -1

                    count++;
                    conditionsSet.Tables[0].Rows.Add(dr);
                }
        }

        /// <summary>
        /// Parse an array from a comma delited string
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns>Array of Strings</returns>
        public string[] ParseStringToArray(string InputString)
        {
            int tokencount = 0;
            string ParseString = InputString;
            string[] ParsedArray = new string[1];

            //char[] chars = new char[ParseString.Length - 1];
            // walk through the string
            for (int y = 0; y < (ParseString.Length); y++)
            {
                if (ParseString[y].ToString() == "'")
                {
                    y++;
                    string TempToken = "";
                    while ((Convert.ToString(ParseString[y]) + Convert.ToString(ParseString[y + 1])) != "',")
                    {
                        if ((ParseString[y - 1].ToString() + ParseString[y]) != ",'")
                            TempToken += ParseString[y];
                        y++;
                    }
                    ParsedArray[tokencount] = TempToken;
                    tokencount++;
                    if (y < (ParseString.Length - 2))
                        Array.Resize(ref ParsedArray, tokencount + 1);
                }
            }
            return ParsedArray;
        }

        /// <summary>
        /// Parses a comma delimited string from an array
        /// </summary>
        /// <param name="InputArray">String Array</param>
        /// <returns>Comma delimted string</returns>
        public string ParseArrayToString(string[] InputArray)
        {
            //string ParsedString = "";
            StringBuilder ParsedString = new StringBuilder();

            if (InputArray.Length == 1)
            {
                if (InputArray[0] != null)
                    for (int y = 0; y < (InputArray.Length); y++)

                        //ParsedString += "'" + InputArray[y] + "',";
                        ParsedString.AppendFormat("'{0}',", InputArray[y]);
            }
            else
            {
                for (int y = 0; y < (InputArray.Length); y++)
                {
                    //ParsedString += "'" + InputArray[y] + "',";
                    ParsedString.AppendFormat("'{0}',", InputArray[y]);
                }
            }
            return ParsedString.ToString();
        }

        /// <summary>
        /// Compares two strings to see if they are identical
        /// </summary>
        /// <param name="One">Main string</param>
        /// <param name="Two">Comparison string</param>
        /// <returns>True if String has changed else false</returns>
        public bool StringHasChanged(string One, string Two)
        {
            bool ReturnVal = false;
            if (One.Length != Two.Length)
                ReturnVal = true;
            else
            {
                for (int y = 0; y < (One.Length); y++)
                    if (One[y] != Two[y])
                    {
                        ReturnVal = true;
                        break;
                    }
            }

            return ReturnVal;
        }

        // ADD DATA FUNCTIONS /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Adds/Updates a Translatable Item object
        /// </summary>
        /// <param name="TI">The Translatable Item entity.</param>
        public void SaveTranslatableItem(ITranslatableItem TI)
        {
            Save<ITranslatableItem, TranslatableItem_DAO>(TI);
        }

        /// <summary>
        /// Adds/Updates  Translatable Item Translated Trxt object
        /// </summary>
        /// <param name="TIT">The Translatable Translated Text entity.</param>
        public void SaveTranslatableItemText(ITranslatedText TIT)
        {
            Save<ITranslatedText, TranslatedText_DAO>(TIT);
        }

        /// <summary>
        /// This method is used to remove an OfferCondition and orphans in the translatableitem, translatedtext and OfferConditionToken tables
        /// </summary>
        /// <param name="OfferConditionKey"></param>
        private static void DeleteCondition(int OfferConditionKey)
        {
            IDbConnection con = Helper.GetSQLDBConnection();
            ParameterCollection parameters = new ParameterCollection();

            // DELETE THE OFFER CONDITIONS NEXT
            const string CQL = "from ApplicationCondition_DAO c where c.Key = ?";
            SimpleQuery<ApplicationCondition_DAO> qry = new SimpleQuery<ApplicationCondition_DAO>(CQL, OfferConditionKey);
            ApplicationCondition_DAO[] AC = qry.Execute();

            int TranslatableItemKey = AC[0].TranslatableItem.Key;

            parameters.Clear();
            Helper.AddIntParameter(parameters, "@OfferConditionKey", OfferConditionKey);
            Helper.AddIntParameter(parameters, "@TranslatableItemKey", TranslatableItemKey);
            const string query = "DELETE FROM [2AM].[dbo].[OfferConditionToken] WHERE OfferConditionKey = @OfferConditionKey; "
                                 + "DELETE FROM [2AM].[dbo].[OfferCondition] WHERE OfferConditionKey = @OfferConditionKey; "
                                 + "DELETE FROM [2AM].[dbo].[TranslatedText] WHERE TranslatableItemKey = @TranslatableItemKey; "
                                 + "DELETE FROM [2AM].[dbo].[TranslatableItem] WHERE TranslatableItemKey = @TranslatableItemKey;";

            Helper.ExecuteNonQuery(con, query, parameters);
        }

        /// <summary>
        ///Deletes the old and Saves a New Condition and Token Set for an Application
        ///</summary>
        public void SaveExistingConditionSet(int LanguageKey)
        {
            /*
             * Delete existing conditionset
             * Save a new one
             */

            // check if there is a record and update it, else add it.

            const string CQL = "from ApplicationCondition_DAO c where c.Application.Key = ?";
            SimpleQuery<ApplicationCondition_DAO> query = new SimpleQuery<ApplicationCondition_DAO>(CQL, offerKey);
            ApplicationCondition_DAO[] offerconditions = query.Execute();

            // Delete all of the currently saved conditions (for replacement with a new set)
            for (int i = 0; i < offerconditions.Length; i++)
                DeleteCondition(offerconditions[i].Key);

            SaveNewConditionSet();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///Saves a New Condition and Token Set for an Application
        ///</summary>
        public void SaveNewConditionSet()
        {
            //Add the Offer Conditions and Tokens
            for (int i = 0; i < conditionsSet.Tables[0].Rows.Count; i++)

                // check if the condition has been selected and if it is required. if it is ignore it
                if ((Convert.ToBoolean(conditionsSet.Tables[0].Rows[i][3])))
                    SaveNewCondition(conditionsSet.Tables[0].Rows[i]);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// This Method s called AFTER a Language Key has been Changed.
        /// </summary>
        /// <param name="_generickey"></param>
        /// <param name="_generickeytypekey"></param>
        /// <returns>true if new translated conditions added, false if none added</returns>
        public bool CheckForAndAddTranslatedConditions(int _generickey, int _generickeytypekey)
        {
            /*
             * This method should only be run in edit mode, not reference mode
             * All edit Mode methods
             *
             */

            Initialise(_generickey);
            GetExistingConditionSetForEditing(_generickey, _generickeytypekey);

            // Check if there is a condition set -if not skip because not needed
            if (ConditionsSet.Tables[0].Rows.Count > 0)

                // if its not english & translatable - add a set
                if ((legalEntityLanguageKey > 2) && LegalEntityLanguageTranslatable)
                {
                    const string CQL = "from ApplicationCondition_DAO c where c.Application.Key = ?";
                    SimpleQuery<ApplicationCondition_DAO> query = new SimpleQuery<ApplicationCondition_DAO>(CQL, OfferKey);
                    ApplicationCondition_DAO[] keyVal = query.Execute();

                    for (int i = 0; i < keyVal.Length; i++)
                    {
                        // check if there are translated conditions yet if there are of the same language, they dont need adding
                        for (int j = 0; j < keyVal[i].TranslatableItem.TranslatedTexts.Count; j++)
                            if (keyVal[i].TranslatableItem.TranslatedTexts[j].Language.Key == legalEntityLanguageKey)
                                return false;
                    }

                    for (int i = 0; i < keyVal.Length; i++)
                    {
                        // find the condition row in the array and pass both through
                        for (int j = 0; j < ConditionsSet.Tables[0].Rows.Count; j++)
                            if (keyVal[i].Condition.Key == (int)ConditionsSet.Tables[0].Rows[j][1])
                                SaveNewTranslatedCondition(ConditionsSet.Tables[0].Rows[j], keyVal[i]);
                    }
                    return true;
                }

            return false;
        }

        /// <summary>
        ///Saves a New Translated Condition if the language is changed later in the application process
        ///</summary>
        private void SaveNewTranslatedCondition(DataRow ConditionDataRow, ApplicationCondition_DAO applicationcondition)
        {
            if ((LegalEntityLanguageKey > (int)Languages.English) && LegalEntityLanguageTranslatable)
            {
                IApplication IA = GetByKey<IApplication, Application_DAO>(offerKey);
                ICondition C = GetByKey<ICondition, Condition_DAO>((int)ConditionDataRow[1]);
                ILanguage TranslatedLanguage = GetByKey<ILanguage, Language_DAO>(LegalEntityLanguageKey);

                ApplicationCondition offerconditions = GetByKey<ApplicationCondition, ApplicationCondition_DAO>(applicationcondition.Key);

                //ApplicationCondition_DAO AC = (ApplicationCondition_DAO)offerconditions.GetDAOObject();

                offerconditions.Application = IA;
                offerconditions.Condition = C;

                //// Get the TranslatableItem
                TranslatableItem translatableitem = GetByKey<TranslatableItem, TranslatableItem_DAO>(offerconditions.TranslatableItem.Key);
                TranslatableItem_DAO TI = (TranslatableItem_DAO)translatableitem.GetDAOObject();
                ITranslatableItem ITI = GetByKey<ITranslatableItem, TranslatableItem_DAO>(TI.Key);

                //// add the translatedtexts
                TranslatedText translatedtexts = new TranslatedText(new TranslatedText_DAO());
                TranslatedText_DAO TT = (TranslatedText_DAO)translatedtexts.GetDAOObject();
                translatedtexts.TranslatableItem = ITI;
                translatedtexts.Language = TranslatedLanguage; // These are our working english set;

                translatedtexts.Text = ConditionDataRow[5].ToString(); // TranslatedText
                TT.CreateAndFlush();

                // TOKEN HANDLING
                // Loop through and store any tokens' values for the condition
                for (int k = 0; k < applicationcondition.ApplicationConditionTokens.Count; k++)
                {
                    ApplicationConditionToken applicationtoken = GetByKey<ApplicationConditionToken, ApplicationConditionToken_DAO>(applicationcondition.ApplicationConditionTokens[k].Key);

                    // This should edist already - get it
                    TranslatableItem tokentranslatableitem = GetByKey<TranslatableItem, TranslatableItem_DAO>(applicationtoken.TranslatableItem.Key);
                    TranslatableItem_DAO TTI = (TranslatableItem_DAO)tokentranslatableitem.GetDAOObject();
                    ITranslatableItem ITTI = GetByKey<ITranslatableItem, TranslatableItem_DAO>(TTI.Key);

                    // STORE TRANSLATED TOKENS
                    if ((LegalEntityLanguageKey > (int)Languages.English))
                    {
                        // add the token_translatedtexts
                        TranslatedText token_actualtranslatedtexts = new TranslatedText(new TranslatedText_DAO());
                        TranslatedText_DAO TATT = (TranslatedText_DAO)token_actualtranslatedtexts.GetDAOObject();

                        token_actualtranslatedtexts.TranslatableItem = ITTI;
                        token_actualtranslatedtexts.Language = TranslatedLanguage; // These are our working english set;
                        token_actualtranslatedtexts.Text = "";

                        // find the token value from the Arrays and store it
                        for (int j = 0; j < conditionsSet.Tables[1].Rows.Count; j++)
                            if ((int)conditionsSet.Tables[1].Rows[j][1] == (int)ConditionDataRow[1])
                            {
                                token_actualtranslatedtexts.Text = conditionsSet.Tables[1].Rows[j][10].ToString();
                                break;
                            }

                        TATT.CreateAndFlush();
                    }
                }
            }

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///Saves a New Condition
        ///</summary>
        private void SaveNewCondition(DataRow ConditionDataRow)
        {
            IApplication IA = GetByKey<IApplication, Application_DAO>(offerKey);
            ICondition C = GetByKey<ICondition, Condition_DAO>((int)ConditionDataRow[1]);
            ILanguage L = GetByKey<ILanguage, Language_DAO>(2);
            ILanguage TranslatedLanguage = GetByKey<ILanguage, Language_DAO>(LegalEntityLanguageKey);

            ApplicationCondition offerconditions = new ApplicationCondition(new ApplicationCondition_DAO());
            ApplicationCondition_DAO AC = (ApplicationCondition_DAO)offerconditions.GetDAOObject();

            offerconditions.Application = IA;
            offerconditions.Condition = C;

            // If it belongs to a condition set and not a user added condition
            if (ConditionDataRow[15].ToString().Length > 0)
            {
                IConditionSet conditionset = GetByKey<IConditionSet, ConditionSet_DAO>((int)ConditionDataRow[15]);
                offerconditions.ConditionSet = conditionset;
            }

            // add the TranslatableItem
            TranslatableItem translatableitem = new TranslatableItem(new TranslatableItem_DAO());
            TranslatableItem_DAO TI = (TranslatableItem_DAO)translatableitem.GetDAOObject();

            // Split the display condition into name and text
            string[] conditionName = SplitConditionAndName(ConditionDataRow[4].ToString());

            string translatedtext = ConvertStringForDBStorage(conditionName[1]);
            translatableitem.Description = TrimString(translatedtext, 200);
            TI.CreateAndFlush();

            ITranslatableItem ITI = GetByKey<ITranslatableItem, TranslatableItem_DAO>(TI.Key);

            // add the translatedtexts
            TranslatedText translatedtexts = new TranslatedText(new TranslatedText_DAO());
            TranslatedText_DAO TT = (TranslatedText_DAO)translatedtexts.GetDAOObject();

            translatedtexts.TranslatableItem = ITI;

            translatedtexts.Language = L; // These are our working english set;
            translatedtexts.Text = ConvertStringForDBStorage(conditionName[1]);
            TT.CreateAndFlush();

            //Save the Offer Condition
            offerconditions.TranslatableItem = ITI;
            AC.CreateAndFlush();

            ConditionDataRow[14] = AC.Key;

            IApplicationCondition IAC = GetByKey<IApplicationCondition, ApplicationCondition_DAO>(AC.Key);

            // STORE TRANSLATED CONDITION TEMPLATES
            if (LegalEntityLanguageKey > (int)Languages.English)
            {
                // add the translatedtexts - for the condition
                TranslatedText propertranslatedtexts = new TranslatedText(new TranslatedText_DAO());
                TranslatedText_DAO PTT = (TranslatedText_DAO)propertranslatedtexts.GetDAOObject();

                propertranslatedtexts.TranslatableItem = ITI;
                propertranslatedtexts.Language = TranslatedLanguage; // These are our working english set;

                // Get the translated text template
                Condition_DAO condition = ActiveRecordBase<Condition_DAO>.Find((int)ConditionDataRow[1]);

                propertranslatedtexts.Text = ""; // set as user entered. update below if it is template based
                if (userEnteredConditionKey == condition.Key)
                {
                    ConditionDataRow[5] = "";
                    propertranslatedtexts.Text = "";
                }
                else
                {
                    for (int l = 0; l < condition.TranslatableItem.TranslatedTexts.Count; l++)
                        if (condition.TranslatableItem.TranslatedTexts[l].Language.Key == LegalEntityLanguageKey)
                        {
                            //string[] conditionNameTranslated = SplitConditionAndName(condition.TranslatableItem.TranslatedTexts[l].Text);
                            //string[] conditionNameTranslated = SplitConditionAndName(condition.TranslatableItem.TranslatedTexts[l].Text);
                            ConditionDataRow[5] = condition.TranslatableItem.TranslatedTexts[l].Text;

                            //ConditionDataRow[5] = conditionNameTranslated[1];
                            propertranslatedtexts.Text = condition.TranslatableItem.TranslatedTexts[l].Text;

                            //propertranslatedtexts.Text = conditionNameTranslated[1];
                            break;
                        }
                }
                PTT.SaveAndFlush();
            }

            // TOKEN HANDLING
            // Loop through and store any tokens' values for the condition
            for (int j = 0; j < conditionsSet.Tables[1].Rows.Count; j++)
            {
                // find out if the token belons to the condition - and then store it
                if ((int)conditionsSet.Tables[1].Rows[j][1] == (int)ConditionDataRow[1])
                {
                    int tokenKey = (int)conditionsSet.Tables[1].Rows[j][9];
                    IToken T = GetByKey<IToken, Token_DAO>(tokenKey);

                    ApplicationConditionToken applicationtokens = new ApplicationConditionToken(new ApplicationConditionToken_DAO());
                    ApplicationConditionToken_DAO ACT = (ApplicationConditionToken_DAO)applicationtokens.GetDAOObject();

                    applicationtokens.Token = T;
                    applicationtokens.ApplicationCondition = IAC;

                    // Get a new token_translatableitem
                    TranslatableItem tokentranslatableitem = new TranslatableItem(new TranslatableItem_DAO());
                    TranslatableItem_DAO TTI = (TranslatableItem_DAO)tokentranslatableitem.GetDAOObject();

                    //store tokens as values where values have not been included
                    if (conditionsSet.Tables[1].Rows[j][5].ToString().Length == 0)
                        tokentranslatableitem.Description = "[" + conditionsSet.Tables[1].Rows[j][2] + "]";
                    else
                        tokentranslatableitem.Description = TrimString(conditionsSet.Tables[1].Rows[j][5].ToString(), 200);

                    TTI.CreateAndFlush();

                    ITranslatableItem ITTI = GetByKey<ITranslatableItem, TranslatableItem_DAO>(TTI.Key);

                    // add the token_translatedtexts
                    TranslatedText token_translatedtexts = new TranslatedText(new TranslatedText_DAO());
                    TranslatedText_DAO TTT = (TranslatedText_DAO)token_translatedtexts.GetDAOObject();

                    token_translatedtexts.TranslatableItem = ITTI;
                    token_translatedtexts.Language = L; // These are our working english set;

                    if (conditionsSet.Tables[1].Rows[j][5].ToString().Length == 0)
                        token_translatedtexts.Text = "[" + conditionsSet.Tables[1].Rows[j][2] + "]";
                    else
                        token_translatedtexts.Text = conditionsSet.Tables[1].Rows[j][5].ToString();

                    TTT.CreateAndFlush();

                    // Save the Application tokens
                    applicationtokens.TranslatableItem = ITTI;
                    ACT.CreateAndFlush();

                    // STORE TRANSLATED TOKENS
                    if (LegalEntityLanguageKey > (int)Languages.English)
                    {
                        // add the token_translatedtexts
                        TranslatedText token_actualtranslatedtexts = new TranslatedText(new TranslatedText_DAO());
                        TranslatedText_DAO TATT = (TranslatedText_DAO)token_actualtranslatedtexts.GetDAOObject();

                        token_actualtranslatedtexts.TranslatableItem = ITTI;
                        token_actualtranslatedtexts.Language = TranslatedLanguage; // These are our working english set;
                        token_actualtranslatedtexts.Text = conditionsSet.Tables[1].Rows[j][5].ToString();
                        TATT.CreateAndFlush();
                    }
                }
            }

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Adds an Offer Condition Token Record. Returns the new token's key.
        /// </summary>
        /// <param name="tokenkey"></param>
        /// <param name="OfferConditionKey"></param>
        /// <param name="tokenvalue"></param>
        /// <returns></returns>
        public int AddOfferConditionTokenNoTranslation(int tokenkey, int OfferConditionKey, string tokenvalue)
        {
            IApplicationConditionToken iACT = RepositoryFactory.GetRepository<IApplicationConditionToken>();
            iACT.Key = tokenkey;
            iACT.ApplicationCondition.Key = OfferConditionKey;
            iACT.TokenValue = tokenvalue;

            ApplicationConditionToken_DAO m_ACT = (ApplicationConditionToken_DAO)((IDAOObject)iACT).GetDAOObject();

            m_ACT.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return m_ACT.Key;
        }

        /// <summary>
        /// Adds an Offer Condition Token, That needs to be  Record. Returns the new token's key.
        /// </summary>
        /// <param name="tokenkey"></param>
        /// <param name="OfferConditionKey"></param>
        /// <param name="TranslatableItemKey"></param>
        /// <param name="tokenvalue"></param>
        /// <returns></returns>
        public int AddOfferConditionToken(int tokenkey, int OfferConditionKey, int TranslatableItemKey, string tokenvalue)
        {
            IApplicationConditionToken iACT = RepositoryFactory.GetRepository<IApplicationConditionToken>();
            iACT.Key = tokenkey;
            iACT.ApplicationCondition.Key = OfferConditionKey;
            iACT.TokenValue = tokenvalue;
            iACT.TranslatableItem.Key = TranslatableItemKey;

            ApplicationConditionToken_DAO m_ACT = (ApplicationConditionToken_DAO)((IDAOObject)iACT).GetDAOObject();
            m_ACT.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return m_ACT.Key;
        }

        /// <summary>
        /// Add an Offer Condition Record to the OfferCondition Table
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="ConditionKey"></param>
        /// <param name="TranslatableItemKey"></param>
        /// <returns></returns>
        public int AddOfferCondition(int offerkey, int ConditionKey, int TranslatableItemKey)
        {
            IApplicationConditionToken iACT = RepositoryFactory.GetRepository<IApplicationConditionToken>();
            iACT.ApplicationCondition.Application.Key = offerkey;
            iACT.TranslatableItem.Key = TranslatableItemKey;
            iACT.ApplicationCondition.Condition.Key = ConditionKey;

            ApplicationCondition_DAO m_ACT = (ApplicationCondition_DAO)((IDAOObject)iACT).GetDAOObject();
            m_ACT.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return m_ACT.Key;
        }

        /// <summary>
        /// Adds a new OfferCondition record with no translation required.
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="conditionkey"></param>
        /// <returns></returns>
        public int AddOfferConditionNoTranslation(int offerkey, int conditionkey)
        {
            IApplicationCondition iACT = RepositoryFactory.GetRepository<IApplicationCondition>();
            iACT.Application.Key = offerkey;
            iACT.Condition.Key = conditionkey;

            ApplicationCondition_DAO m_ACT = (ApplicationCondition_DAO)((IDAOObject)iACT).GetDAOObject();
            m_ACT.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return m_ACT.Key;
        }

        /// <summary>
        /// Adds a new OfferCondition record that requires no translation
        /// </summary>
        /// <param name="ConditionTypeKey"></param>
        /// <param name="ConditionPhrase"></param>
        /// <param name="TokenDescriptions"></param>
        /// <param name="ConditionName"></param>
        /// <returns></returns>
        public int AddConditionNoTranslation(int ConditionTypeKey, string ConditionPhrase, string TokenDescriptions, string ConditionName)
        {
            ICondition iACT = RepositoryFactory.GetRepository<ICondition>();
            iACT.ConditionType.Key = ConditionTypeKey;
            iACT.ConditionPhrase = ConditionPhrase;
            iACT.TokenDescriptions = TokenDescriptions;
            iACT.ConditionName = ConditionName;

            Condition_DAO m_ACT = (Condition_DAO)((IDAOObject)iACT).GetDAOObject();

            m_ACT.SaveAndFlush();
            return m_ACT.Key;
        }

        // LOAD DATA FUNCTIONS /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method adds a token_dao to the tokens list if a matching token name  is found
        /// </summary>
        /// <param name="Description"></param>
        public void TokensByDescriptionGet(string Description)
        {
            const string HQL = "from Token_DAO c where c.Description = ?";
            SimpleQuery<Token_DAO> q = new SimpleQuery<Token_DAO>(HQL, Description);

            Token_DAO[] res = q.Execute();
            if (res.Length > 0)
            {
                Array.Resize(ref tokens, res.Length);
                tokens[0] = res[0];

                //........
            }
        }

        /// <summary>
        /// Get the conditions from the last disbursed application.
        /// Readvance applications are ignored.
        /// </summary>
        /// <param name="_accountkey"></param>
        /// <returns>Returns a populated Loan Condition Set</returns>
        public List<string> GetLastDisbursedApplicationConditions(int _accountkey)
        {
            //get the latest disbursed offer
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = appRepo.GetLastDisbursedApplicationByAccountKey(_accountkey);

            //get the conditions for the offer
            if (app != null)
                return GetLoanConditions(app.Key);

            return null;
        }

        /// <summary>
        /// This method is called to update the loan condition and tokens in process
        /// </summary>
        /// <param name="_generickey"></param>
        /// <param name="_generickeytypekey"></param>
        public void UpdateLoanConditions(int _generickey, int _generickeytypekey)
        {
            int offerkey = 0;

            // check the _generickey. if its an account, pass through the application key
            if ((_generickeytypekey == (int)GenericKeyTypes.Account) || (_generickeytypekey == (int)GenericKeyTypes.ParentAccount))
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                IAccount account = accRepo.GetAccountByKey(_generickey);
                if (account != null)
                {
                    List<IApplication> applicationsSorted = new List<IApplication>(account.Applications);
                    if (applicationsSorted.Count > 0)
                    {
                        applicationsSorted.Sort(delegate(IApplication c1, IApplication c2) { return c2.Key.CompareTo(c1.Key); });
                        offerkey = applicationsSorted[0].Key;
                    }
                }
            }
            else if (_generickeytypekey == (int)GenericKeyTypes.Offer)
                offerkey = _generickey;

            if (offerkey == 0)
                return;

            Initialise(offerkey);
            UpdateTokens(offerkey);
        }

        private void UpdateTokens(int OK)
        {
            // Check that the Offer is not in read-only mode
            if (!ReadOnly)
            {
                const string CQL =
                    "FROM ApplicationConditionToken_DAO ACT WHERE ACT.ApplicationCondition.Application.Key = ?";
                SimpleQuery<ApplicationConditionToken_DAO> query = new SimpleQuery<ApplicationConditionToken_DAO>(CQL, OK);
                ApplicationConditionToken_DAO[] tokens_dao = query.Execute();

                // Loop through the existing token set
                if (tokens_dao != null)
                    for (int r = 0; r < tokens_dao.Length; r++)
                    {
                        // check for system updated Token
                        if (tokens_dao[r].Token.TokenType.Key == 4)
                        {
                            string TokenValue = ReturnTokenValue(tokens_dao[r].Token.StatementDefinition);

                            //Update the token value here in the DB as well - for future read-only access
                            if (TokenValue.Length > 0)
                            {
                                ITranslatableItem TITI =
                                    GetByKey<ITranslatableItem, TranslatableItem_DAO>(tokens_dao[r].TranslatableItem.Key);
                                TITI.Description = TrimString(TokenValue, 255);
                                SaveTranslatableItem(TITI);

                                // Update the TranslatedText
                                for (int m = 0; m < tokens_dao[r].TranslatableItem.TranslatedTexts.Count; m++)
                                {
                                    ITranslatedText TITT = GetByKey<ITranslatedText, TranslatedText_DAO>(tokens_dao[r].TranslatableItem.TranslatedTexts[m].Key);
                                    TITT.Text = TokenValue;
                                    SaveTranslatableItemText(TITT);
                                }
                            }
                        }
                    }
            }
        }

        /// <summary>
        /// Returns a  read-only populated Loan Condition Set
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        public List<string> GetLoanConditions(int offerkey)
        {
            List<string> loanConditions = new List<string>();
            Initialise(offerkey);

            if (offerkey != 0)
            {
                if (SavedConditionSetExists(offerkey))
                {
                    UpdateTokens(offerkey);
                    DataTable dtConditions = new DataTable();
                    using (IDbConnection con = Helper.GetSQLDBConnection())
                    {
                        string qry = UIStatementRepository.GetStatement("COMMON", "GetLoanConditions");

                        // Create a collection
                        ParameterCollection Parameters = new ParameterCollection();

                        //Add the required parameters
                        Helper.AddIntParameter(Parameters, "@OfferKey", offerkey);
                        Helper.FillFromQuery(dtConditions, qry, con, Parameters);
                    }

                    for (int i = 0; i < dtConditions.Rows.Count; i++)
                        loanConditions.Add(dtConditions.Rows[i]["condition"].ToString());
                }
                else

                    // Tis is a new condition set so there is no summary yet
                    loanConditions.Add("Loan conditions for this loan have not been defined yet.");
            }
            else

                // Tis is a new condition set so there is no summary yet
                loanConditions.Add("Loan conditions for this loan have not been defined yet.");

            return loanConditions;
        }

        /// <summary>
        /// Checks if a Condition Set has been saved for this offer
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        public bool SavedConditionSetExists(int offerkey)
        {
            // Get the Stored condition Set for the Application/Account
            // Get the Conditions either via the application or account
            const string CQL = "from ApplicationCondition_DAO c where c.Application.Key = ?";
            SimpleQuery<ApplicationCondition_DAO> query = new SimpleQuery<ApplicationCondition_DAO>(CQL, offerkey);
            ApplicationCondition_DAO[] keyVal = query.Execute();

            return keyVal.Length > 0;
        }

        /// <summary>
        /// Retrieves an Existing ConditionSet for editing
        /// </summary>
        /// <param name="_generickey"></param>
        /// <param name="_generickeytypekey"></param>
        public void GetExistingConditionSetForEditing(int _generickey, int _generickeytypekey)
        {
            /*
             * This method gets the existing stored conditions.
             * It first gets a new condition set based on the loan attributes
             * Then loads the stored condition set
             * then deletes all stored required conditions (because the attributes may have changed)
             * (its cheaper to delete them than to test for them)
             * and adds user added conditions to the conditionset - for storing later
             */

            // Get a fresh conditionset
            GetConditionSet(_generickey, _generickeytypekey); // <- can be called for edit

            // Get the Stored condition Set for the Application/Account
            // Get the Conditions either via the application or account
            const string CQL = "from ApplicationCondition_DAO c where c.Application.Key = ?";
            SimpleQuery<ApplicationCondition_DAO> query = new SimpleQuery<ApplicationCondition_DAO>(CQL, _generickey);
            ApplicationCondition_DAO[] offerconditions = query.Execute();

            // Delete the requied conditions from the database first.

            // Add all user entered conditions to the conditionset
            int count = conditionsSet.Tables[0].Rows.Count;
            for (int y = 0; y < offerconditions.Length; y++)
                if (offerconditions[y].Condition.Key == userEnteredConditionKey)
                {
                    DataRow dr = conditionsSet.Tables[0].NewRow();
                    dr[0] = count;  //Key
                    dr[1] = offerconditions[y].Condition.Key;//ConditionKey
                    dr[2] = offerconditions[y].TranslatableItem.TranslatedTexts[0].Key; //TranslatableItemKey
                    dr[3] = true; // User Added - on the selected list.
                    for (int x = 0; x < offerconditions[y].TranslatableItem.TranslatedTexts.Count; x++)
                    {
                        if (offerconditions[y].TranslatableItem.TranslatedTexts[x].Language.Key == (int)Languages.English)
                            dr[4] = UserAddedConditionName + ") " + offerconditions[y].TranslatableItem.TranslatedTexts[x].Text;
                        else
                            dr[5] = offerconditions[y].TranslatableItem.TranslatedTexts[x].Text;
                    }

                    if (dr[5].ToString().Length == 0) dr[5] = "";

                    //dr[5] = offerconditions[y].TranslatableItem.TranslatedTexts.Count > 1 ? offerconditions[y].TranslatableItem.TranslatedTexts[1].Text : "";

                    //(*Red = required condition :  *Silver Background = user must edit selected condition: // "#ffff99" = user has edited//"yellow";
                    dr[6] = true; //UserEditable
                    dr[7] = false; // Required
                    dr[8] = "#ffff99";//CSSColor
                    dr[9] = "Black"; // CSSWeight

                    // ignore dr[10] - its the display index
                    dr[11] = true; //Is Selected initially
                    dr[12] = offerconditions[y].Condition.ConditionType.Key; // Condition Type

                    // These are only used in Update Mode.
                    dr[14] = offerconditions[y].Key; // Application Condition Key - will only be set for user caputed records

                    // populate the translated items...
                    conditionsSet.Tables[0].Rows.Add(dr);
                    count++;
                }
                else // Add selected conditions
                    for (int z = 0; z < conditionsSet.Tables[0].Rows.Count; z++)
                        if (offerconditions[y].Condition.Key == (int)conditionsSet.Tables[0].Rows[z][1])
                        {
                            //If the condition in the set is a genral - check if the saved one was too
                            if ((int)conditionsSet.Tables[0].Rows[z][15] == (int)ConditionSets.General)
                            {
                                if ((offerconditions[y].ConditionSet != null) && (offerconditions[y].ConditionSet.Key == (int)ConditionSets.General))
                                {
                                    conditionsSet.Tables[0].Rows[z][3] = true;
                                    conditionsSet.Tables[0].Rows[z][11] = true;
                                }
                                else
                                {
                                    conditionsSet.Tables[0].Rows[z][3] = false;
                                    conditionsSet.Tables[0].Rows[z][11] = false;
                                }
                            }
                            else
                            {
                                conditionsSet.Tables[0].Rows[z][3] = true;
                                conditionsSet.Tables[0].Rows[z][11] = true;
                            }
                            conditionsSet.Tables[0].Rows[z][14] = offerconditions[y].Key;
                            if (offerconditions[y].ConditionSet != null) conditionsSet.Tables[0].Rows[z][15] = offerconditions[y].ConditionSet.Key;
                        }

            //               {
            //                    if (offerconditions[y].Condition.ConditionSetConditions[0].ConditionSet = (int)SAHL.Common)

            //                }

            ParseTokensFromExistingConditions(_generickey);
        }

        /// <summary>
        /// Gets the Conditions for the Selected A New application
        /// </summary>
        /// <param name="_generickey"></param>
        /// <param name="_generickeytypekey"></param>
        public void GetNewConditionSet(int _generickey, int _generickeytypekey)
        {
            GetConditionSet(_generickey, _generickeytypekey);

            // Populate the Tokens as well
            ParseTokensFromConditions();
        }

        /// <summary>
        /// Gets the Conditions for the Selected Condition Matrix - For Creating Condition Set for the Application
        /// </summary>
        /// <param name="generickey"></param>
        /// <param name="genericKeyTypeKey"></param>
        [SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals")]
        private void GetConditionSet(int generickey, int genericKeyTypeKey)
        {
            /*
             * This Method is only Valid in Edit Mode.
             * It must to be called in reference mode
             *
             */

            /*
             * Overview of this Method:
             *
             * 1. Get all possible ConditionConfigurations for the GenericKeyType
             * 2. Filter the list of ConditionConfigurations against the Offer
             *      i.e.: The offer data must match the ConditionConfig
             * 3. Using the ConditionConfigurations from 2 Get a distinct list of
             *      all the possible ConditionSets linked to the ConditionConfig
             * 4. For each possible ConditionSet, make sure that all the necessary
             *      ConditionConfigs exist for the set in the list from 2
             * 5. Ensure a distinct list of conditions from all the sets
             *      And some other setup/display stuff
             *      And some other stuff I dont understand
             * 6. Sort the list
             */

            #region 1. GetConfigsForGenericKeyType

            /*
             Get all possible conditions for the GenericKeyType from the DB,
             currently this is only for Offers
             */
            const string CQL = "from ConditionConfiguration_DAO c where c.GenericKeyType.Key = ?";
            SimpleQuery<ConditionConfiguration_DAO> qry = new SimpleQuery<ConditionConfiguration_DAO>(CQL, genericKeyTypeKey);
            ConditionConfiguration_DAO[] ConfigsDAO = qry.Execute();

            //Get a list of items to work with that are not DAO's
            IEventList<IConditionConfiguration> Configs = new DAOEventList<ConditionConfiguration_DAO, IConditionConfiguration, ConditionConfiguration>(ConfigsDAO);
            IList<IConditionConfiguration> ApplicableConfigs = new List<IConditionConfiguration>();

            #endregion 1. GetConfigsForGenericKeyType

            #region 2. Filter ConfigsForOffer

            IPropertyRepository propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();

            // get offer
            if (genericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

                //We want to build a seperate list of ConditionConfigurations's that match the offer data.

                //Loop through all the Condition Configurations
                //and check that the offer data matches the Cond Config
                //If we find a match, add the CC to the Applicable list.

                //i.e.: if the Condition applies to the Offer Origination Source
                //      make sure the offers OrigSource = (one of) the Cond Config OrigSource's
                //          the Condition must be configured for each applicable OS

                IApplication app = appRepo.GetApplicationByKey(generickey);

                //loop through and evaluate given offer
                foreach (IConditionConfiguration cc in Configs)

                    switch (cc.GenericColumnDefinition.Description)
                    {
                        case "Offer Origination Source":
                            if (app.OriginationSource != null
                                && app.OriginationSource.Key == cc.GenericColumnDefinitionValue)
                                ApplicableConfigs.Add(cc);
                            break;

                        case "OfferType":
                            if (app.ApplicationType.Key == cc.GenericColumnDefinitionValue)
                                ApplicableConfigs.Add(cc);
                            break;

                        case "OfferAttributeType":
                            foreach (IApplicationAttribute att in app.ApplicationAttributes)
                            {
                                if (att.ApplicationAttributeType.Key == cc.GenericColumnDefinitionValue)
                                {
                                    ApplicableConfigs.Add(cc);
                                    break;
                                }
                            }
                            break;

                        case "Legal entity citizen type":
                            foreach (IApplicationRole ar in app.ApplicationRoles)
                            {
                                //only for client roles
                                //NB this should be a seperate ConditionConfiguration, but
                                //assuming that conditions only apply to clients and the performance of this method
                                //this check has been left in
                                if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                                {
                                    ILegalEntityNaturalPerson lenp = ar.LegalEntity as ILegalEntityNaturalPerson;
                                    if (lenp != null && lenp.CitizenType != null
                                        && (lenp.CitizenType.Key == cc.GenericColumnDefinitionValue))
                                    {
                                        ApplicableConfigs.Add(cc);
                                        break;
                                    }
                                }
                            }
                            break;

                        case "Legal entity type":
                            foreach (IApplicationRole ar in app.ApplicationRoles)
                            {
                                //only for client roles
                                //NB this should be a seperate ConditionConfiguration, but
                                //assuming that conditions only apply to clients and the performance of this method
                                //this check has been left in
                                if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                                    if (cc.GenericColumnDefinitionValue == ar.LegalEntity.LegalEntityType.Key)
                                    {
                                        ApplicableConfigs.Add(cc);
                                        break;
                                    }
                            }
                            break;

                        case "Offer role type":
                            foreach (IApplicationRole ar in app.ApplicationRoles)
                            {
                                if (cc.GenericColumnDefinitionValue == ar.ApplicationRoleType.Key)
                                {
                                    ApplicableConfigs.Add(cc);
                                    break;
                                }
                            }
                            break;

                        case "Title Type":
                            IProperty property = propRepo.GetPropertyByApplicationKey(app.Key);
                            if (property != null && property.TitleType != null)
                                if (cc.GenericColumnDefinitionValue == property.TitleType.Key)
                                    ApplicableConfigs.Add(cc);
                            break;

                        case "Employment Type Key":

                            // Check if the legal entity is a client then add the subsidy conditions
                            foreach (IApplicationRole ar in app.ApplicationRoles)
                            {
                                //only for client roles
                                //NB this should be a seperate ConditionConfiguration, but
                                //assuming that conditions only apply to clients and the performance of this method
                                //this check has been left in
                                if (ar.ApplicationRoleType.ApplicationRoleTypeGroup.Key == (int)OfferRoleTypeGroups.Client)
                                {
                                    foreach (IEmployment emp in ar.LegalEntity.Employment)
                                        if ((emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                                            && (cc.GenericColumnDefinitionValue == emp.EmploymentType.Key))
                                        {
                                            ApplicableConfigs.Add(cc); // = true;
                                            break;
                                        }
                                }
                            }
                            break;

                        case "Detail":
                            if (app.Account != null)
                            {
                                if (app.Account.Details != null)
                                {
                                    if (app.Account.Details.Where(x => x.DetailType.Key == cc.GenericColumnDefinitionValue).Count() > 0)
                                    {
                                        ApplicableConfigs.Add(cc); // = true;
                                    }
                                }
                            }
                            break;

                        case "Category":
                            int applicationInformationKey = app.GetLatestApplicationInformation().Key;
                            IApplicationInformationVariableLoan appInfo = appRepo.GetApplicationInformationVariableLoan(applicationInformationKey);
                            if (appInfo != null)
                            {
                                if (appInfo.Category.Key == cc.GenericColumnDefinitionValue)
                                {
                                    ApplicableConfigs.Add(cc); // = true;
                                }
                            }
                            break;

                        case "Market Value":

                            bool ignoreConfiguration = false;
                            IApplicationMortgageLoan mortgageLoan = app as IApplicationMortgageLoan;
                            if (mortgageLoan != null && mortgageLoan.Property != null)
                            {
                                var lastestCompleteValuation = mortgageLoan.Property.LatestCompleteValuation;

                                if (lastestCompleteValuation != null)
                                {
                                    if(lastestCompleteValuation.ValuationAmount > cc.GenericColumnDefinitionValue)
                                    {
                                        ignoreConfiguration = true; // Ignore this configuration if the actual valuation is greater than GenericColumnDefinitionValue (R600 000)
                                    }
                                }
                            }

                            if (!ignoreConfiguration)
                            {
                                ApplicableConfigs.Add(cc);
                            }

                            break;                            
                    }
            }
            else // From Account
            {
                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                IAccount account = accRepo.GetAccountByKey(generickey);
                int LastApplication = account.Applications.Count;
                int appTypeKey = account.Applications[LastApplication].ApplicationType.Key;
                IApplication lastApp = account.Applications[LastApplication];

                //loop through and evaluate given offer
                foreach (IConditionConfiguration cc in Configs)
                    switch (cc.GenericColumnDefinition.Description)
                    {
                        case "Offer Origination Source":
                            if (lastApp.ApplicationSource != null)
                                if (lastApp.ApplicationSource.Key == cc.GenericColumnDefinitionValue)
                                    ApplicableConfigs.Add(cc);
                            break;

                        case "OfferType":
                            if (lastApp.ApplicationType.Key == cc.GenericColumnDefinitionValue)
                                ApplicableConfigs.Add(cc); // = true;
                            break;

                        case "OfferAttributeType":
                            foreach (IApplicationAttribute att in lastApp.ApplicationAttributes)
                                if (att.ApplicationAttributeType.Key == cc.GenericColumnDefinitionValue)
                                {
                                    if (att.ApplicationAttributeType.Key == (int)OfferAttributeTypes.CapitalizeFees)
                                        if (appTypeKey == (int)OfferTypes.SwitchLoan || appTypeKey == (int)OfferTypes.NewPurchaseLoan || appTypeKey == (int)OfferTypes.RefinanceLoan)
                                            ApplicableConfigs.Add(cc);
                                    break;
                                }
                            break;

                        case "Legal entity citizen type":
                            foreach (IApplicationRole ar in lastApp.ApplicationRoles)
                            {
                                ILegalEntityNaturalPerson le = ar.LegalEntity as ILegalEntityNaturalPerson;
                                if (le != null)
                                {
                                    if ((le.CitizenType != null) && (le.CitizenType.Key == cc.GenericColumnDefinitionValue))
                                        ApplicableConfigs.Add(cc); // = true;
                                }
                            }
                            break;

                        case "Legal entity type":
                            foreach (IApplicationRole ar in lastApp.ApplicationRoles)
                            {
                                ILegalEntity le = ar.LegalEntity;
                                if (le != null)
                                    if (cc.GenericColumnDefinitionValue == le.LegalEntityType.Key)
                                        ApplicableConfigs.Add(cc); // = true;
                            }
                            break;

                        case "Offer role type":
                            foreach (IApplicationRole ar in lastApp.ApplicationRoles)
                            {
                                if (ar.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor || ar.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
                                {
                                    ApplicableConfigs.Add(cc);
                                }
                            }
                            break;

                        case "Title Type":
                            IProperty prop = propRepo.GetPropertyByAccountKey(account.Key);

                            if (prop != null && prop.TitleType.Key == (int)TitleTypes.SectionalTitle)
                                if (appTypeKey == (int)OfferTypes.SwitchLoan || appTypeKey == (int)OfferTypes.NewPurchaseLoan || appTypeKey == (int)OfferTypes.RefinanceLoan)
                                    ApplicableConfigs.Add(cc);

                            break;

                        case "Employment Type Key":

                            // Check if the legal entity is a client then add the subsidy conditions
                            foreach (IApplicationRole ar in lastApp.ApplicationRoles)
                            {
                                foreach (IEmployment emp in ar.LegalEntity.Employment)
                                    if ((emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                                        && (emp.EmploymentType.Key == (int)EmploymentTypes.SalariedwithDeduction))
                                    {
                                        ApplicableConfigs.Add(cc); // = true;
                                        break;
                                    }
                            }
                            break;
                    }
            }

            #endregion 2. Filter ConfigsForOffer

            #region 3. GetDistinctPossibleConditionSets

            // From the Applicable Configs,
            // populate a new distinct list of Condition sets to evaluate for this offer.
            IList<IConditionSet> PossibleConditionSets = new List<IConditionSet>();
            foreach (IConditionConfiguration CC in ApplicableConfigs)
            {
                foreach (IConditionSet CurrentSet in CC.ConditionSets)
                    if (!PossibleConditionSets.Contains(CurrentSet))
                        PossibleConditionSets.Add(CurrentSet);
            }

            #endregion 3. GetDistinctPossibleConditionSets

            #region 4. GetApplicableConditionSets

            /*
             * Here we want to evaluate the possible condition sets to find the ones that apply to this offer

             * According to convention we apply OR logic to CondConfigs of the same type (ie. GenericColumnDefinitions).
             * After that we apply AND logic to evaluate condition config's of different types
             * i.e.: An offer has an origSourceKey = 1
             *      Our ConditionSet has configs for OSKey's 1 & 2
             *      We apply OR logic to the 1 & 2 resulting in TRUE
             *          setting both HasOriginationSource & OriginationSourceFound = true
             *      if our condition Set has offertype config's for OfferTypeKey values for 6, 7, 8
             *          but our offer has an offertypekey = 4 we will set HasOfferType = true and OfferTypeFound = false.
             *          In this case we will not add the condition set to the PossibleConditionSets collection.
             *          If, however, our Offer had an OffertypeKey of 7 and  an OSKey of 1,
             *              we would have added the ConditionSet to the PossibleConditionSets Collection.
             *
             *      i.e.: OSKey (1 || 2) && OfferTypeKey (6 || 7 || 8)
             *
             *      Any Condition Configuration type that is evaluated in the above case statement, has to be
             *      evaluated in this section as well, for this algorithm to function properly.
             */
            IList<IConditionSet> ApplicableConditionSets = new List<IConditionSet>();
            foreach (IConditionSet currentConditionSet in PossibleConditionSets)
            {
                //Declare and reset the possible comparison variables for all supported configuration types
                bool HasOriginationSource = false,  // OriginationSource
                     OriginationSourceFound = false,
                     HasOfferType = false,      // Offertype
                     OfferTypeFound = false,
                     HasOfferAttributeType = false, // Offer Attribute
                     OfferAttributeTypeFound = false,
                     HasCitizenType = false,        // CitizenType
                     CitizenTypeFound = false,
                     HasLegalEntityType = false,    // LegalEntity Type
                     LegalEntityTypeFound = false,
                     HasOfferRoleType = false,      // Offer Role Type
                     OfferRoleTypeFound = false,
                     HasTitleType = false,      // Title Type
                     TitleTypeFound = false,
                     HasEmploymentTypeKey = false,  // Employment Type
                     EmploymentTypeKeyFound = false,
                     HasDetailKey = false,
                     DetailKeyFound = false,
                     HasCategoryKey = false,
                     CategoryKeyFound = false,
                     HasMarketValue = false,
                     MarketValueApplicable = false,

                     CanAdd = true;     // our ALL HAS PASSSED, ADD this MOFO variable.

                //Evaluate each condition configuration for the Current Condition Set
                foreach (IConditionConfiguration CurrentConfiguration in currentConditionSet.ConditionConfigurations)
                    switch (CurrentConfiguration.GenericColumnDefinition.Description)
                    {
                        case "Offer Origination Source":
                            HasOriginationSource = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                OriginationSourceFound = true;

                            break;

                        case "OfferType":
                            HasOfferType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                OfferTypeFound = true;

                            break;

                        case "OfferAttributeType":
                            HasOfferAttributeType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                OfferAttributeTypeFound = true;

                            break;

                        case "Legal entity citizen type":
                            HasCitizenType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                CitizenTypeFound = true;

                            break;

                        case "Legal entity type":
                            HasLegalEntityType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                LegalEntityTypeFound = true;

                            break;

                        case "Offer role type":
                            HasOfferRoleType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                OfferRoleTypeFound = true;

                            break;

                        case "Title Type":
                            HasTitleType = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                TitleTypeFound = true;

                            break;

                        case "Employment Type Key":
                            HasEmploymentTypeKey = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                EmploymentTypeKeyFound = true;

                            break;

                        case "Detail":
                            HasDetailKey = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                DetailKeyFound = true;

                            break;

                        case "Category":
                            HasCategoryKey = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                CategoryKeyFound = true;

                            break;
                        case "Market Value":
                            HasMarketValue = true;
                            if (ApplicableConfigs.Contains(CurrentConfiguration))
                                MarketValueApplicable = true;
                            break;
                    }

                // here we do the logic evaluation for the current conditionset.
                if (HasOriginationSource && !OriginationSourceFound)
                    CanAdd = false;
                if (HasOfferType && !OfferTypeFound)
                    CanAdd = false;
                if (HasOfferAttributeType && !OfferAttributeTypeFound)
                    CanAdd = false;
                if (HasCitizenType && !CitizenTypeFound)
                    CanAdd = false;
                if (HasLegalEntityType && !LegalEntityTypeFound)
                    CanAdd = false;
                if (HasOfferRoleType && !OfferRoleTypeFound)
                    CanAdd = false;
                if (HasTitleType && !TitleTypeFound)
                    CanAdd = false;
                if (HasEmploymentTypeKey && !EmploymentTypeKeyFound)
                    CanAdd = false;
                if (HasDetailKey && !DetailKeyFound)
                    CanAdd = false;
                if (HasCategoryKey && !CategoryKeyFound)
                    CanAdd = false;
                if (HasMarketValue && !MarketValueApplicable)
                    CanAdd = false;

                /* Nasty hardcoded hack that has to be refactored properly
                 * for ruleset 366, only add the rule set if an additional surety exists
                 * on the application that is not on the account */
                if (currentConditionSet.Key == 366 && CanAdd == true)
                {
                    string query = UIStatementRepository.GetStatement("COMMON", "TokenGetNewSuretorsNames");

                    ParameterCollection prms = new ParameterCollection();
                    Helper.AddBigIntParameter(prms, "@OfferKey", generickey);

                    object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

                    if (obj == null || String.IsNullOrEmpty(obj.ToString()))

                        CanAdd = false;
                }

                // this conditionset configured to be available for this offer.  ADD IT!!
                if (CanAdd && ExecuteConfigurationUIStatements(currentConditionSet, generickey))
                {
                    ApplicableConditionSets.Add(currentConditionSet);
                }

                    
            }

            #endregion 4. GetApplicableConditionSets

            #region 5. GetDistinctConditionListAndSetup

            if (conditionsSet != null) conditionsSet.Clear();
            ConditionsSet = CreateConditionDataSet();
            conditionList = new List<IConditionSetCondition>();

            // ApplicableConditionSets now contains all of the ApplicableConditionsets
            int count = 0;
            int requiredcount = 0;
            int optionalcount = 0;

            foreach (IConditionSet cs in ApplicableConditionSets)
            {
                foreach (IConditionSetCondition csc in cs.ConditionSetConditions)
                {
                    // check the conditionList for Duplicate Conditions
                    // only add the condition if it does not already exist in the list to output.
                    bool Duplicate = false;
                    int savedconditionSet = 0;
                    int potentialconditionset = 0;
                    int rowkey = 0;

                    // Loop through the conditiontable and check if a duplicate value exists
                    foreach (DataRow dr in conditionsSet.Tables[0].Rows)
                    {
                        int conditionkey = (int)dr[1];
                        if (conditionkey == csc.Condition.Key)
                        {
                            rowkey = (int)dr[0]; //rowkey = z;
                            savedconditionSet = (int)dr[15];
                            potentialconditionset = cs.Key;
                            Duplicate = true;
                            break;
                        }
                    }

                    // if no duplicate was found, add the condition to the condition set
                    if (!Duplicate)
                    {
                        // add non duplicate conditions to the list...
                        conditionList.Add(csc);

                        DataRow dr = conditionsSet.Tables[0].NewRow();
                        dr[0] = count;  //Key
                        dr[1] = csc.Condition.Key;//ConditionKey

                        /*
                         *  (*Red = required condition :  *Silver Background = user must edit selected condition:
                         */
                        switch (csc.Condition.ConditionType.Key)
                        {
                            case 0: //No Tokens // User cannot Edit
                                dr[6] = false; //UserEditable
                                dr[8] = "White";//CSSColor
                                break;

                            case 1: //Tokenised String // User cannot Edit
                                dr[6] = false; //UserEditable
                                dr[8] = "White";//CSSColor
                                break;

                            case 2: //Run time Session Token // User cannot Edit
                                dr[6] = false; //UserEditable
                                dr[8] = "White";//CSSColor
                                break;

                            case 3: //Runtime executed SQL String // User cannot Edit
                                dr[6] = false; //UserEditable
                                dr[8] = "White";//CSSColor
                                break;

                            case 4: //User captured Condition // User can Edit
                                dr[6] = true; //UserEditable
                                dr[8] = "Silver";//CSSColor
                                break;

                            case 5: //Standard Condition edited by User // User can Edit
                                dr[6] = true; //UserEditable
                                dr[8] = "Silver";//CSSColor
                                break;

                            case 6: //Tokenised String edited by User // User can Edit Tokens Only
                                dr[6] = true; //UserEditable
                                dr[8] = "Silver";//CSSColor
                                break;
                        }

                        if (csc.RequiredCondition)
                        {
                            dr[7] = true; //Required
                            dr[3] = true; //Selected
                            dr[9] = "Red"; //CSSWeight
                            dr[10] = requiredcount; //DisplayIndex
                            dr[11] = true; //Selected ??WTF
                            requiredcount++;
                        }
                        else
                        {
                            dr[7] = false; //Required
                            dr[3] = false; //Selected
                            dr[9] = "Black"; //CSSWeight
                            dr[10] = optionalcount; //DisplayIndex
                            dr[11] = false; //Selected
                            optionalcount++;
                        }

                        dr[12] = csc.Condition.ConditionType.Key;

                        dr[4] = csc.Condition.ConditionName + ") " + csc.Condition.ConditionPhrase; //Template

                        //Translated Template
                        dr[5] = csc.Condition.TranslatableItem.TranslatedTexts[0].Text;
                        dr[13] = csc.Condition.ConditionPhrase; //Load Value from Template

                        dr[14] = -1; // Application Condition Key  - not saved so no value yet

                        if (csc.ConditionSet != null) dr[15] = csc.ConditionSet.Key;

                        // populate the translated items...
                        conditionsSet.Tables[0].Rows.Add(dr);
                        count++;
                    }
                    else
                    {
                        if (savedconditionSet == (int)ConditionSets.General)
                        {
                            if ((potentialconditionset != (int)ConditionSets.General) && (csc.RequiredCondition))
                            {
                                conditionsSet.Tables[0].Rows[rowkey][15] = potentialconditionset;
                                conditionsSet.Tables[0].Rows[rowkey][7] = true; //Required
                                conditionsSet.Tables[0].Rows[rowkey][3] = true; //Selected
                                conditionsSet.Tables[0].Rows[rowkey][9] = "Red"; //CSSWeight
                                conditionsSet.Tables[0].Rows[rowkey][10] = requiredcount; //DisplayIndex
                                conditionsSet.Tables[0].Rows[rowkey][11] = true; //Selected ??WTF
                            }
                        }
                        else
                        {
                            if (csc.RequiredCondition)
                            {
                                conditionsSet.Tables[0].Rows[rowkey][15] = potentialconditionset;
                                conditionsSet.Tables[0].Rows[rowkey][7] = true; //Required
                                conditionsSet.Tables[0].Rows[rowkey][3] = true; //Selected
                                conditionsSet.Tables[0].Rows[rowkey][9] = "Red"; //CSSWeight
                                conditionsSet.Tables[0].Rows[rowkey][10] = requiredcount; //DisplayIndex
                                conditionsSet.Tables[0].Rows[rowkey][11] = true; //Selected ??WTF
                            }
                        }
                    }
                }
            }

            #endregion 5. GetDistinctConditionListAndSetup

            #region 6. SortTheList

            //Sort the Conditionset by ConditionKey - This may need to be changed to conditionName later
            DataSet tempSet = conditionsSet.Copy();
            const string strSort = "ConditionKey ASC";
            DataRow[] sortedRows = tempSet.Tables[0].Select("", strSort);
            conditionsSet.Tables[0].Clear();
            foreach (DataRow dr in sortedRows)
                conditionsSet.Tables[0].ImportRow(dr);

            #endregion 6. SortTheList
        }

        /// <summary>
        /// If there are ConditionSet UIStatements execute them. If all their return values equal 1, the apply the condition set.
        /// </summary>
        /// <param name="conditionSet"></param>
        /// <param name="genericKey"></param>
        /// <returns>True when there are no UIStatements to execute or when all the UIStatements return 1. Otherwise False</returns>
        private bool ExecuteConfigurationUIStatements(IConditionSet conditionSet, int genericKey)
        {
            /* calling .All() on an empty list returns true. */
            return conditionSet.ConditionSetUIStatements.All(c =>
                                    {
                                        var query = UIStatementRepository.GetStatement("Conditions", c.UIStatementName);
                                        ParameterCollection prms = new ParameterCollection();
                                        Helper.AddBigIntParameter(prms, "@GenericKey", genericKey);

                                        object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

                                        return Convert.ToInt32(obj) == 1;
                                    }); 
               
        }

        //****************************************************************

        public bool CheckArrayForValue(int[] IntArray, int ValueToCheckFor)
        {
            bool retval = false;
            for (int x = 0; x < IntArray.Length; x++)
                if (ValueToCheckFor == IntArray[x]) retval = true;

            return retval;
        }

        // DATA VALUE RETRIEVAL  //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Parse a token set from an existing condition set
        /// </summary>
        public void ParseTokensFromExistingConditions(int offerkey)
        {
            // Get the full token set for the existing conditionset
            ParseTokensFromConditions();

            // Populate the Table with existing token values

            // Get the Stored condition Set for the Application
            const string CQL = "from ApplicationCondition_DAO c where c.Application.Key = ?";
            SimpleQuery<ApplicationCondition_DAO> query = new SimpleQuery<ApplicationCondition_DAO>(CQL, offerkey);

            ApplicationCondition_DAO[] keyVal = query.Execute();

            //TokensTable.Initialize();
            if (keyVal != null)
                for (int r = 0; r < keyVal.Length; r++)
                {
                    // Loop through the ApplicationConditiontokens Dataset and populate
                    if (!(keyVal[r].ApplicationConditionTokens == null))
                        for (int q = 0; q < keyVal[r].ApplicationConditionTokens.Count; q++)
                        {
                            // Loop through the existin token set
                            for (int s = 0; s < conditionsSet.Tables[1].Rows.Count; s++)
                            {
                                int tableTokenKey = Convert.ToInt32(conditionsSet.Tables[1].Rows[s][9]);
                                int templatetokenkey = keyVal[r].ApplicationConditionTokens[q].Token.Key;
                                if (tableTokenKey == templatetokenkey)
                                {
                                    // Check the ReadOnly flag here - if it is set dont parse token values again
                                    // Check if this is an automated token and populate it if it is...
                                    if ((keyVal[r].ApplicationConditionTokens[q].Token.TokenType.Key == 4) && !ReadOnly)
                                    {
                                        string TokenValue = ReturnTokenValue(keyVal[r].ApplicationConditionTokens[q].Token.StatementDefinition);
                                        conditionsSet.Tables[1].Rows[s][5] = TokenValue;
                                        conditionsSet.Tables[1].Rows[s][6] = keyVal[r].ApplicationConditionTokens[q].Token.StatementDefinition.Key;

                                        //conditionsSet.Tables[1].Rows[s][10] = conditionsSet.Tables[1].Rows[s][5];

                                        //*****************************************************************************
                                        //Update the token value here in the DB as well - for future read-only access
                                        if (TokenValue.Length > 0)
                                        {
                                            ITranslatableItem TITI =
                                                GetByKey<ITranslatableItem, TranslatableItem_DAO>(
                                                    keyVal[r].ApplicationConditionTokens[q].TranslatableItem.Key);
                                            TITI.Description = TrimString(
                                                conditionsSet.Tables[1].Rows[s][5].ToString(), 255);

                                            //SaveTranslatableItem(TITI);

                                            ITranslatedText TITT = null;

                                            // Assign the correct record for the language
                                            for (int m = 0; m < keyVal[r].TranslatableItem.TranslatedTexts.Count; m++)
                                                if (keyVal[r].TranslatableItem.TranslatedTexts[m].Language.Key ==
                                                    LegalEntityLanguageKey)
                                                    TITT =
                                                        GetByKey<ITranslatedText, TranslatedText_DAO>(
                                                            keyVal[r].ApplicationConditionTokens[q].TranslatableItem.
                                                                TranslatedTexts[m].Key);

                                            if ((LegalEntityLanguageKey == (int)Languages.English))
                                            {
                                                if (TITT != null)
                                                    TITT.Text = TrimString(
                                                        conditionsSet.Tables[1].Rows[s][5].ToString(),
                                                        255);
                                            }
                                            else if (TITT != null)
                                                TITT.Text = TrimString(conditionsSet.Tables[1].Rows[s][10].ToString(),
                                                                       255);

                                            if (TITT != null) SaveTranslatableItemText(TITT);
                                        }
                                    }
                                    else
                                    {
                                        conditionsSet.Tables[1].Rows[s][5] = keyVal[r].ApplicationConditionTokens[q].TranslatableItem.TranslatedTexts[0].Text;
                                        if (keyVal[r].ApplicationConditionTokens[q].TranslatableItem.TranslatedTexts.Count > 1)
                                            conditionsSet.Tables[1].Rows[s][10] = keyVal[r].ApplicationConditionTokens[q].TranslatableItem.TranslatedTexts[1].Text;
                                    }

                                    // there are tokens - set the edited status of thecondition to edited.
                                    for (int t = 0; t < conditionsSet.Tables[0].Rows.Count; t++)
                                    {
                                        if (Convert.ToInt32(conditionsSet.Tables[0].Rows[t][1]) == keyVal[r].Condition.Key)

                                            conditionsSet.Tables[0].Rows[t][8] = "#ffff99"; //"yellow";
                                    }
                                }
                            }
                        }
                }
        }

        /// <summary>
        /// Parse a token set from a new condition set
        /// </summary>
        public void ParseTokensFromConditions()
        {
            //Tokens.Initialize();
            int TokenKey = 0;

            //DataTable tokensstable = conditionsSet.Tables[1];

            for (int x = 0; x < conditionList.Count; x++)
            {
                for (int z = 0; z < conditionList[x].Condition.ConditionTokens.Count; z++)
                {
                    DataRow dr = conditionsSet.Tables[1].NewRow();
                    dr[0] = TokenKey; //Key
                    dr[1] = conditionList[x].Condition.Key; //ConditionKey
                    dr[2] = conditionList[x].Condition.ConditionTokens[z].Token.Description; //tokenName
                    dr[3] = conditionList[x].Condition.ConditionTokens[z].Token.Description; //tokenDescription
                    dr[4] = conditionList[x].Condition.ConditionTokens[z].Token.TokenType.Key; //tokenTypeKey

                    // Check if this is an automated token and populate it if it is...
                    if (conditionList[x].Condition.ConditionTokens[z].Token.TokenType.Key == 4)
                    {
                        string tokenValue = ReturnTokenValue(conditionList[x].Condition.ConditionTokens[z].Token.StatementDefinition);
                        dr[5] = tokenValue;
                        dr[10] = tokenValue;
                        dr[6] = conditionList[x].Condition.ConditionTokens[z].Token.StatementDefinition.Key;
                    }
                    else
                        dr[5] = "";  //tokenValue  <- no value yet because its a new tokenset

                    //tokenStatementDefinitionKey
                    dr[7] = conditionList[x].Condition.ConditionTokens[z].Token.MustTranslate;

                    //tokenMustTranslate
                    dr[8] = conditionList[x].Condition.ConditionTokens[z].Token.ParameterType.Key;

                    // Token Key
                    dr[9] = conditionList[x].Condition.ConditionTokens[z].Token.Key;

                    //tokenParameterTypeKey

                    conditionsSet.Tables[1].Rows.Add(dr);
                    TokenKey++;
                }
            }
        }

        /// <summary>
        /// Returns the value from a UIStatement's parsed Token (for DB stored values)
        /// </summary>
        /// <param name="statementDefinition"></param>
        /// <returns></returns>
        [Obsolete("Use business model objects, not DAO's")]
        private string ReturnTokenValue(StatementDefinition_DAO statementDefinition)
        {
            IStatementDefinition sd = BMTM.GetMappedType<IStatementDefinition>(statementDefinition);

            return ReturnTokenValue(sd);
        }

        private IBusinessModelTypeMapper _bmtm;

        public IBusinessModelTypeMapper BMTM
        {
            get
            {
                if (_bmtm == null)
                    _bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return _bmtm;
            }
        }

        /// <summary>
        /// Returns the value from a UIStatement's parsed Token (for DB stored values)
        /// </summary>
        /// <param name="statementDefinition"></param>
        /// <returns></returns>
        private string ReturnTokenValue(IStatementDefinition statementDefinition)
        {
            string TokenName = statementDefinition.Tokens[0].Description;
            string StatementName = statementDefinition.StatementName;
            string Application = statementDefinition.ApplicationName;

            if (Application == "COMMON")
            {
                // use StatementName and ApplicationName to get the Statement Definition Key from StatementDefinition_DAO
                const string HQL = "from StatementDefinition_DAO c where c.ApplicationName = ? AND c.StatementName = ?";
                SimpleQuery<StatementDefinition_DAO> qry = new SimpleQuery<StatementDefinition_DAO>(HQL, Application, StatementName);
                StatementDefinition_DAO[] std = qry.Execute();

                ParameterCollection parameters = new ParameterCollection();

                // get the parameter names

                for (int x = 0; x < std[0].StatementParameters.Count; x++)
                {
                    System.Data.SqlClient.SqlParameter newparam = new System.Data.SqlClient.SqlParameter();
                    newparam.ParameterName = "@" + std[0].StatementParameters[x].ParameterName;
                    newparam.SqlDbType = (SqlDbType)std[0].StatementParameters[x].ParameterType.Key;

                    if (newparam.ParameterName == "@OfferKey")
                        newparam.SqlValue = Convert.ToString(OfferKey);

                    parameters.Add(newparam);
                }

                // This traps if there are conditions that may not be in the correct sets
                if (parameters.Count == 0)
                    return "[" + TokenName + "]";

                Object o = ExecuteConditionTokenStatement(statementDefinition.Key, parameters);

                //call the generic uistatement with the parameter values
                return Convert.ToString(o);
            }

            return "[" + TokenName + "]";
        }

        /// <summary>
        /// Populates the basic variable set used throughout the conditions Repository
        /// </summary>
        /// <param name="OK">Offer Key</param>
        public void Initialise(int OK)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = appRepo.GetApplicationByKey(OK);

            //Check whether the condtions are read only & Set up the readonly flag here (defaults to false)
            // latest offerinformation  -> offerinformationstatus = approved - logic fomr Ivor
            ReadOnly = false;
            if (application.ApplicationInformations != null)
            {
                IApplicationInformation appInfo = application.ApplicationInformations[application.ApplicationInformations.Count - 1];
                if (appInfo.ApplicationInformationType.Key == (int)OfferInformationTypes.AcceptedOffer)
                    ReadOnly = true;
            }

            AccountKey = application.ReservedAccount.Key;

            // check if there is an existing account
            if (application.Account != null)//AccountStatus.Key != (int)AccountStatuses.Application)
            {
                // Trap for the possability of a language not being selected for the mailing address
                if (application.Account.MailingAddresses.Count == 0)
                {
                    legalEntityLanguageKey = (int)Languages.English;
                    LegalEntityLanguageTranslatable = false;
                }
                else
                {
                    legalEntityLanguageKey = application.Account.MailingAddresses[0].Language.Key;
                    LegalEntityLanguageTranslatable = application.Account.MailingAddresses[0].Language.Translatable;
                }
            }

            //legalEntities = application.Account.GetLegalEntitiesByRoleType(null, accountRoleTypes);
            else
            {
                // Trap for the possability of a language not being selected for the mailing address
                if (application.ApplicationMailingAddresses.Count == 0)
                {
                    legalEntityLanguageKey = (int)Languages.English;
                    LegalEntityLanguageTranslatable = false;
                }
                else
                {
                    legalEntityLanguageKey = application.ApplicationMailingAddresses[0].Language.Key;
                    LegalEntityLanguageTranslatable = application.ApplicationMailingAddresses[0].Language.Translatable;
                }
            }

            //legalEntities = application.GetLegalEntitiesByRoleType(applicationRoleTypes);
            offerKey = OK;

            // Get the Default Blank condition conditionkey used for user added conditions
            const string HQL = "from Condition_DAO ac where ac.ConditionPhrase = '' and ac.ConditionType.Key = 5";
            SimpleQuery<Condition_DAO> conditionQry = new SimpleQuery<Condition_DAO>(HQL);
            Condition_DAO[] conditions = conditionQry.Execute();
            if (conditions != null && conditions.Length > 0)
            {
                userEnteredConditionKey = conditions[0].Key;
                userAddedConditionName = conditions[0].ConditionName;
            }
        }

        /// <summary>
        /// Resizes the chosen Arrays
        /// </summary>
        /// <param name="ArrayCount"></param>
        public void ResizeChosenArrays(int ArrayCount)
        {
            Array.Resize(ref chosenStrings, (ArrayCount + 1));
            Array.Resize(ref chosenID, (ArrayCount + 1));
            Array.Resize(ref chosenCSSColor, (ArrayCount + 1));
            Array.Resize(ref chosenCSSWeight, (ArrayCount + 1));
            Array.Resize(ref chosenValue, (ArrayCount + 1));
            Array.Resize(ref chosenEdited, (ArrayCount + 1));
            Array.Resize(ref chosenArrayUserConditionType, (ArrayCount + 1));
            Array.Resize(ref chosenOfferConditionKeys, (ArrayCount + 1));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="StatementDefinitionKey"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public object ExecuteConditionTokenStatement(int StatementDefinitionKey, ParameterCollection Parameters)
        {
            StatementDefinition_DAO def = ActiveRecordBase<StatementDefinition_DAO>.Find(StatementDefinitionKey);
            string query = UIStatementRepository.GetStatement(def.ApplicationName, def.StatementName);
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                return Helper.ExecuteScalar(con, query, Parameters);
            }
        }
    }
}