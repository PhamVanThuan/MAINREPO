using Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class WatiNAssertions
    {
        /// <summary>
        /// Assert that the InnerHTML contents of the SelectList matches the expected content of the list provided.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expectedContents"></param>
        /// <param name="performCountAssertion">By default the assertion will check that the count matches between the 2 lists.</param>
        public static void AssertSelectListContents(SelectList list, List<string> expectedContents, bool performCountAssertion = true)
        {
            List<string> actualContents = new List<string>();
            var contents = (from l in list.Options
                            where
                            !l.Text.ToLower().Contains("select")
                            select l);
            foreach (var item in contents)
            {
                actualContents.Add(item.Text.RemoveWhiteSpace());
            }
            var b = (from e in expectedContents where actualContents.Contains(e.RemoveWhiteSpace()) == false select e).ToList<string>();
            //now we have our actual list

            //Concatenate names of elements that do not exist for reporting
            string options = string.Empty;
            foreach (string option in b)
                options += string.Format(@"{0}, ", option);

            if (options.Contains(','))
                options = options.Remove(options.LastIndexOf(','));

            Logger.LogAction("Asserting the options in the specified Select List ");
            Assert.That(b.Count == 0, string.Format(@"Select List Element/s '{0}' were not found", options));

            if (performCountAssertion)
            {
                Logger.LogAction("Asserting no unexpected options exist in the specified Select List");
                Assert.AreEqual(expectedContents.Count, actualContents.Count, "There are unexpected items in the Select List");
            }
        }

        /// <summary>
        /// Asserts that a set of elements are enabled on a screen
        /// </summary>
        /// <param name="elementList"></param>
        public static void AssertFieldsAreEnabled(List<Element> elementList)
        {
            Logger.LogAction("Asserting that the elements in the given list are enabled");
            var results = (from ele in elementList where ele.Enabled == false select ele.IdOrName).FirstOrDefault();
            Assert.That(results == null, "Element {0} is not enabled", results);
        }

        /// <summary>
        /// Asserts that a set of elements exist on a screen
        /// </summary>
        /// <param name="elementList"></param>
        public static void AssertFieldsExist(List<Element> elementList)
        {
            Logger.LogAction("Asserting that the elements in the given list exist on screen");
            var results = (from ele in elementList where ele.Exists == false select ele).FirstOrDefault();
            var elementDescription = String.Empty;
            if (results != null)
                elementDescription = results.Description;
            Assert.That((results == null), "Element {0} does not exist", elementDescription);
        }

        /// <summary>
        /// Asserts that a set of elements are disabled on a screen
        /// </summary>
        /// <param name="elementList"></param>
        public static void AssertFieldsAreDisabled(List<Element> elementList)
        {
            Logger.LogAction("Asserting that the elements in the given list are disabled");
            var results = (from ele in elementList where ele.Enabled select ele.IdOrName).FirstOrDefault();
            Assert.That(results == null, "Element {0} is enabled, expected to be disabled.", results);
        }

        /// <summary>
        /// Asserts that a set of elements do not exist on a screen
        /// </summary>
        /// <param name="elementList"></param>
        public static void AssertFieldsDoNotExist(List<Element> elementList)
        {
            string fields = string.Empty;

            //Create a secondary list of elements that exist from the given list of elements to check
            List<string> results = (from ele in elementList where ele.Exists select ele.IdOrName).ToList<string>();

            //Concatenate names of elements that exist for reporting
            foreach (string element in results)
                fields += string.Format(@"{0}, ", element);

            if (!string.IsNullOrEmpty(fields) && fields.Contains(','))
                fields = fields.Remove(fields.LastIndexOf(','));

            //Check if any elements exist.  If elements exist fail test and report list of elements that exist
            Logger.LogAction("Asserting the elements in the given list do not exist");
            Assert.That(results.Count == 0, "Element/s '{0}' exist", fields);
        }

        /// <summary>
        /// Assert the expected option is selected in the dropdown list
        /// </summary>
        /// <param name="expectedOption">string</param>
        /// <param name="selectList">SelectList</param>
        public static void AssertSelectedOption(string expectedOption, SelectList selectList)
        {
            Logger.LogAction("Asserting the {0} option is selected in the {1} dropdown list", expectedOption, selectList.Description);
            string actualOption = selectList.SelectedItem;
            StringAssert.AreEqualIgnoringCase(expectedOption, actualOption, "The expected {0} option is not slelected in the {1} dropdown list", expectedOption, selectList.Description);
        }

        public static void AssertFieldText(string expectedText, Element element)
        {
            Logger.LogAction("Asserting that the {0} field contains the text {1}", element.Description, expectedText);
            var actualText = element.Text;
            StringAssert.AreEqualIgnoringCase(expectedText, actualText, "The {0} field does not contain the text {1}", element.Description, expectedText);
        }

        public static void AssertCheckboxValue(bool expectedValue, CheckBox checkBox)
        {
            Logger.LogAction("Asserting that the {0} field Checked value is {1}", checkBox.Description, expectedValue);
            var actualText = checkBox.Checked;
            Assert.AreEqual(expectedValue, actualText, "The {0} field does not Checked value is {1}", checkBox.Description, expectedValue);
        }

        /// <summary>
        /// Assert that the Value contents of the SelectList matches the expected values of the list provided.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="expectedValues"></param>
        /// <param name="performCountAssertion">By default the assertion will check that the count matches between the 2 lists.</param>
        public static void AssertSelectListContents(SelectList list, List<int> expectedValues, bool performCountAssertion = true)
        {
            expectedValues = expectedValues.Distinct().ToList();
            List<int> screenValues = new List<int>();
            var contents = (from l in list.Options where l.Value != "-select-" select l);
            foreach (var item in contents)
            {
                screenValues.Add(int.Parse(item.Value));
            }

            var notOnScreen = expectedValues.Except(screenValues);
            var onScreenNotInExpected = screenValues.Except(expectedValues);

            Assert.That(notOnScreen.Count() == 0, "An expected select list value was not found in the select list");
            if (performCountAssertion)
            {
                Assert.That(onScreenNotInExpected.Count() == 0, "The select list contained an unexpected value");
            }
        }

        /// <summary>
        /// Asserts that the value of a currency label matches the database value provided to the assertion.
        /// </summary>
        /// <param name="span">Label text to use</param>
        /// <param name="databaseValue">Database value</param>
        internal static void AssertCurrencyLabel(Span span, decimal databaseValue)
        {
            var screenValue = Convert.ToDecimal(span.Text.CleanCurrencyString(false).Trim());
            var dbValue = Math.Round(Math.Abs(databaseValue), 2, MidpointRounding.AwayFromZero);
            Assert.AreEqual(screenValue, dbValue);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expectedText"></param>
        /// <param name="element"></param>
        internal static void AssertFieldTextContains(string expectedText, Element element)
        {
            Logger.LogAction("Asserting that the {0} field contains the text {1}", element.Description, expectedText);
            var actualText = element.Text;
            Assert.That(expectedText.Contains(actualText), string.Format(@"The {0} field does not contain the text {1}", element.Description, expectedText));
        }

        /// <summary>
        /// Assert the expected option is selected in the dropdown list
        /// </summary>
        /// <param name="expectedValue">string</param>
        /// <param name="selectList">SelectList</param>
        public static void AssertSelectedValue(string expectedValue, SelectList selectList)
        {
            Logger.LogAction("Asserting the {0} option is selected in the {1} dropdown list", expectedValue, selectList.Description);
            string actualValue = selectList.SelectedOption.Value;
            StringAssert.AreEqualIgnoringCase(expectedValue, actualValue, "The expected {0} option is not slelected in the {1} dropdown list", expectedValue, selectList.Description);
        }

        /// <summary>
        /// Assert that any rows within a table contains text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool AssertTableRowsContainsText(string text, Table table)
        {
            var rows = table.OwnTableRows.AsEnumerable<TableRow>();

            return (from r in rows
                    where r.Text.ToUpper().Contains(text.ToUpper())
                    select r != null).FirstOrDefault();
        }

        public static void AssertCheckboxExistsInCollectionByNextSiblingValue(IEnumerable<WatiN.Core.CheckBox> checkboxCollection, string labelDescription)
        {
            var checkbox = (from d in checkboxCollection where d.NextSibling.Text == labelDescription select d).FirstOrDefault();
            Assert.That(checkbox != null, string.Format(@"Could not find a checkbox with a label description of {0} in the collection provided.", labelDescription));
        }
    }
}