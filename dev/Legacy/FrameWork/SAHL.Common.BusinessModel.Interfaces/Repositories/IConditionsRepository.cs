using System.Collections.Generic;
using System.Data;
using SAHL.Common.Collections;
using SAHL.Common.DataAccess;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    /// <summary>
    ///
    /// </summary>
    public interface IConditionsRepository
    {
        /// <summary>
        ///
        /// </summary>
        DataSet ConditionsSet { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenArrayUserConditionType { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenStrings { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenID { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenCSSColor { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenCSSWeight { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenEdited { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenOfferConditionKeys { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] ChosenOfferConditionSetKeys { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedStrings { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedID { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedCSSColor { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedCSSWeight { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedEdited { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedArrayUserConditionType { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedOfferConditionKeys { get; set; }

        /// <summary>
        ///
        /// </summary>
        string[] SelectedOfferConditionSetKeys { get; set; }

        ///////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets or Sets the CompareToken Array
        /// </summary>
        string[] CompareTokens { get; set; }

        /// <summary>
        /// Gets or Sets the Offer Key value
        /// </summary>
        int OfferKey { get; set; }

        /// <summary>
        /// String array of Chosen offer keys
        /// </summary>
        string ChosenArrayOfferConditionKeys { get; set; }

        /// <summary>
        /// Gets or Sets the ChosenArrayOfferConditionSetKeys
        /// </summary>
        string ChosenArrayOfferConditionSetKeys { get; set; }

        /// <summary>
        /// Gets or Sets the SelectedArrayOfferConditionKeys
        /// </summary>
        string SelectedArrayOfferConditionKeys { get; set; }

        /// <summary>
        /// Gets or Sets the SelectedArrayOfferConditionSetKeys
        /// </summary>
        string SelectedArrayOfferConditionSetKeys { get; set; }

        /// <summary>
        /// Gets or Sets the User entered condition Key Value
        /// </summary>
        int UserEnteredConditionKey { get; set; }

        /// <summary>
        /// Gets or Sets the Generic Key value
        /// </summary>
        int GenericKey { get; set; }

        /// <summary>
        /// Gets or Sets the Account Key value
        /// </summary>
        int AccountKey { get; set; }

        /// <summary>
        /// Gets or Sets the Legal Entity Language Key
        /// </summary>
        int LegalEntityLanguageKey { get; set; }

        /// <summary>
        /// Gets or Sets whether the Legal Entity Language is Translatable
        /// </summary>
        bool LegalEntityLanguageTranslatable { get; set; }

        /// <summary>
        /// Gets or Sets whether the repository is being used in translate mode (for data retrieval and storage)
        /// </summary>
        bool Translating { get; set; }

        /// <summary>
        /// Gets or Sets the condition Name - Used for display purposes
        /// </summary>
        string UserAddedConditionName { get; set; }

        /// <summary>
        /// Gets or Sets the Selected Array Strings
        /// </summary>
        string SelectArrayStrings { get; set; }

        /// <summary>
        /// Gets or Sets the Selected Array ID
        /// </summary>
        string SelectArrayID { get; set; }

        /// <summary>
        /// Gets or Sets the Select Array CSS Color
        /// </summary>
        string SelectArrayCSSColor { get; set; }

        /// <summary>
        /// Gets or Sets the Selec Array CSS Weight
        /// </summary>
        string SelectArrayCSSWeight { get; set; }

        /// <summary>
        /// Gets or Sets the Select Array Value
        /// </summary>
        string SelectArrayValue { get; set; }

        /// <summary>
        /// Gets or Sets the Select User Edited value
        /// </summary>
        string SelectUserEdited { get; set; }

        /// <summary>
        /// Gets or Sets the Selected conditions condition Type
        /// </summary>
        string SelectUserConditionType { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen Array Strings
        /// </summary>
        string ChosenArrayStrings { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen Array ID value
        /// </summary>
        string ChosenArrayID { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen Array CSS Color
        /// </summary>
        string ChosenArrayCSSColor { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen Array CSS Weight
        /// </summary>
        string ChosenArrayCSSWeight { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen Array Value
        /// </summary>
        string ChosenArrayValue { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen User edited value
        /// </summary>
        string ChosenUserEdited { get; set; }

        /// <summary>
        /// Gets or Sets the Chosen conditions condition Type
        /// </summary>
        string ChosenUserConditionType { get; set; }

        /// <summary>
        /// Gets or Sets the Editable Condition Key
        /// </summary>
        string EditableConditionKey { get; set; }

        /// <summary>
        /// Gets or Sets the Editable Condition String value
        /// </summary>
        string EditableConditionString { get; set; }

        /// <summary>
        /// Gets or Sets the Editable Translation Condition String value
        /// </summary>
        string EditableTranslationString { get; set; }

        /// <summary>
        /// Gets or Sets the Editable Condition Value
        /// </summary>
        string EditableConditionValue { get; set; }

        /// <summary>
        /// Gets or Sets the Editable Condition Index
        /// </summary>
        int EditableConditionIndex { get; set; }

        /// <summary>
        /// Gets or Sets the Editable condition token Array
        /// </summary>
        string[] EditableConditionTokens { get; set; }

        /// <summary>
        /// Gets or Sets a list of condition objects used primarily for the translatiosn screen
        /// </summary>
        EventList<ICondition> LstConditions { get; set; }

        /// <summary>
        /// Gets or Sets the UpdateNavigateTo value
        /// </summary>
        string CancelNavigateTo { get; set; }

        /// <summary>
        /// Gets or Sets the AddNavigateTo value
        /// </summary>
        string AddNavigateTo { get; set; }

        //STRING FUNCTIONS///////////////////////////////////////////////////////////////////////
        // Done
        /// <summary>
        /// Right Trims a String
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="StringLength">The Lenth if the required string</param>
        /// <returns>The Trimmed String</returns>
        string TrimString(string Input, int StringLength);

        /// <summary>
        /// Splits a ConditionString into its condition name and condition.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string[0] = conditionName , string[1] = Condition </returns>
        string[] SplitConditionAndName(string input);

        //Done
        /// <summary>
        /// Populates the selected condition with Tokens that have been automatically populated
        /// This method is for display purposes only
        /// This Method
        /// </summary>
        /// <param name="ConditionKey"></param>
        /// <param name="ConditionPhrase"></param>
        /// <param name="TranslatedCondition"></param>
        void PopulateConditionsWithTokens(int ConditionKey, ref string ConditionPhrase, bool TranslatedCondition);

        // Done
        /// <summary>
        /// Convert a display string for storage between Javascript linebreak and ASCII syntax linebreak
        /// </summary>
        /// <param name="original">String to be parsed for the linebreaks</param>
        /// <returns>A String in the correct format for SQL storage</returns>
        string ConvertStringForDBStorage(string original);

        /// <summary>
        /// This method must only be run on a single condition, and not an array
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        string NormaliseParenthesis(string original);

        // Done
        /// <summary>
        /// Convert a string to contain linbreaks compatible for Javascript display from ASCII syntax Line Breaks
        /// </summary>
        /// <param name="original">Original string containing ASCII Line Breaks</param>
        /// <returns>String containing Javascript Linebreaks</returns>
        string ConvertStringForJavaScriptArray(string original);

        // Done
        /// <summary>
        /// convert ASCII linebreaks to display in HTML
        /// </summary>
        /// <param name="original">Origininal string containing ASCII linebreaks</param>
        /// <returns>String formatted for HTML linebreaks</returns>
        string ConvertStringForHTMLDisplay(string original);

        /// <summary>
        /// Parse all display arrays (for Javascript) from  Comma delimited strings
        /// </summary>
        void ParseArrays();

        /// <summary>
        /// Parse all display comma delimted strings (for Javascript) from Arrays
        /// </summary>
        void ParseStrings();

        /// <summary>
        /// Save The variables from The Arrays back into the Conditions Table for Saving.
        /// </summary>
        void PopulateConditionsTableFromArrays();

        /// <summary>
        /// Parse an array from a comma delited string
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns>Array of Strings</returns>
        string[] ParseStringToArray(string InputString);

        /// <summary>
        /// Parses a comma delimited string from an array
        /// </summary>
        /// <param name="InputArray">String Array</param>
        /// <returns>Comma delimted string</returns>
        string ParseArrayToString(string[] InputArray);

        /// <summary>
        /// Compares two strings to see if they are identical
        /// </summary>
        /// <param name="One">Main string</param>
        /// <param name="Two">Comparison string</param>
        /// <returns>True if String has changed else false</returns>
        bool StringHasChanged(string One, string Two);

        /// <summary>
        /// Adds/Updates a Translatable Item object
        /// </summary>
        /// <param name="TI">The Translatable Item entity.</param>
        void SaveTranslatableItem(ITranslatableItem TI);

        /// <summary>
        /// Adds/Updates  Translatable Item Translated Trxt object
        /// </summary>
        /// <param name="TIT">The Translatable Translated Text entity.</param>
        void SaveTranslatableItemText(ITranslatedText TIT);

        /// <summary>
        ///Saves an Existing Condition and Token Set for an Application
        ///</summary>
        void SaveExistingConditionSet(int LanguageKey);

        /// <summary>
        ///Saves a New Condition and Token Set for an Application
        ///</summary>
        void SaveNewConditionSet();

        // ADD DATA FUNCTIONS /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This Method s called AFTER a Language Key has been Changed.
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        /// <returns>true if new translated conditions added, false if none added</returns>
        bool CheckForAndAddTranslatedConditions(int GenericKey, int GenericKeyTypeKey);

        /// <summary>
        /// Adds an Offer Condition Token Record. Returns the new token's key.
        /// </summary>
        /// <param name="tokenkey"></param>
        /// <param name="OfferConditionKey"></param>
        /// <param name="tokenvalue"></param>
        /// <returns></returns>
        int AddOfferConditionTokenNoTranslation(int tokenkey, int OfferConditionKey, string tokenvalue);

        /// <summary>
        /// Adds an Offer Condition Token, That needs to be  Record. Returns the new token's key.
        /// </summary>
        /// <param name="tokenkey"></param>
        /// <param name="OfferConditionKey"></param>
        /// <param name="TranslatableItemKey"></param>
        /// <param name="tokenvalue"></param>
        /// <returns></returns>
        int AddOfferConditionToken(int tokenkey, int OfferConditionKey, int TranslatableItemKey, string tokenvalue);

        /// <summary>
        /// Add an Offer Condition Record to the OfferCondition Table
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="ConditionKey"></param>
        /// <param name="TranslatableItemKey"></param>
        /// <returns></returns>
        int AddOfferCondition(int offerkey, int ConditionKey, int TranslatableItemKey);

        /// <summary>
        /// Adds a new OfferCondition record with no translation required.
        /// </summary>
        /// <param name="offerkey"></param>
        /// <param name="conditionkey"></param>
        /// <returns></returns>
        int AddOfferConditionNoTranslation(int offerkey, int conditionkey);

        /// <summary>
        /// Adds a new OfferCondition record that requires no translation
        /// </summary>
        /// <param name="ConditionTypeKey"></param>
        /// <param name="ConditionPhrase"></param>
        /// <param name="TokenDescriptions"></param>
        /// <param name="ConditionName"></param>
        /// <returns></returns>
        int AddConditionNoTranslation(int ConditionTypeKey, string ConditionPhrase, string TokenDescriptions, string ConditionName);

        // LOAD DATA FUNCTIONS /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This function adds a token_dao to the tokens list if a matching token name  is found
        /// </summary>
        /// <param name="Description"></param>
        void TokensByDescriptionGet(string Description);

        /// <summary>
        /// Get the conditions from the last disbursed application.
        /// Readvance applications are ignored.
        /// </summary>
        /// <param name="OfferKey"></param>
        /// <returns>Returns a populated Loan Condition Set</returns>
        List<string> GetLastDisbursedApplicationConditions(int OfferKey);

        /// <summary>
        /// This method is called to update the loan condition and tokens in process
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTypeKey"></param>
        void UpdateLoanConditions(int GenericKey, int GenericKeyTypeKey);

        /// <summary>
        /// Returns A Populated Loan Conditions Set
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        List<string> GetLoanConditions(int offerkey);

        /// <summary>
        /// Checks if a Condition Set has been saved for this offer
        /// </summary>
        /// <param name="offerkey"></param>
        /// <returns></returns>
        bool SavedConditionSetExists(int offerkey);

        /// <summary>
        /// Retrieves an Existing ConditionSet for editing
        /// </summary>
        /// <param name="_generickey"></param>
        /// <param name="_generickeytypekey"></param>
        void GetExistingConditionSetForEditing(int _generickey, int _generickeytypekey);

        /// <summary>
        /// Gets the Conditions for the Selected Condition Matrix - For Creating Condition Set for the Application
        /// </summary>
        /// <param name="GenericKey"></param>
        /// <param name="GenericKeyTpyeKey"></param>
        void GetNewConditionSet(int GenericKey, int GenericKeyTpyeKey);

        /// <summary>
        /// Loops through an integer array and checks if a value is present
        /// </summary>
        /// <param name="IntArray"></param>
        /// <param name="ValueToCheckFor"></param>
        /// <returns>True if present, False if not present.</returns>
        bool CheckArrayForValue(int[] IntArray, int ValueToCheckFor);

        // DATA VALUE RETRIEVAL  //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This function parses a list of tokens from the selected conditions
        /// </summary>
        void ParseTokensFromConditions();

        /// <summary>
        /// Retrive the records from the OfferConditionToken Table
        /// </summary>
        void ParseTokensFromExistingConditions(int offerkey);

        ///// <summary>
        ///// Gets the Translatable Item Key from the Conditions Table
        ///// </summary>
        ///// <param name="ConditionKey"></param>
        ///// <returns></returns>
        /////int GetTranslatableItemKeyFromConditions(int ConditionKey);

        /// <summary>
        /// Initialises the Conditions Repository Factory
        /// </summary>
        /// <param name="OK">Offer Key</param>
        void Initialise(int OK);

        /// <summary>
        /// Resizes the chosen Arrays
        /// </summary>
        /// <param name="ArrayCount"></param>
        void ResizeChosenArrays(int ArrayCount);

        /// <summary>
        /// Returns a token value from the Statement Defininition system
        /// </summary>
        /// <param name="StatementDefinitionKey"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        object ExecuteConditionTokenStatement(int StatementDefinitionKey, ParameterCollection Parameters);
    }
}