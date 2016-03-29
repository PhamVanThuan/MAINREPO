using Common.Extensions;
using NUnit.Framework;
using ObjectMaps.Pages;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Exceptions;
using WatiN.Core.Logging;

public class BasePageAssertions : BasePageControls
{
    /// <summary>
    /// This assertion will check the IE frame for a text string
    /// </summary>
    /// <param name="expectedResult">The string that you expect to see on the screen</param>
    public void AssertBrowserWindowContainsText(string expectedResult)
    {
        Logger.LogAction("Asserting that the following text exists in the Browser Window: {0}", expectedResult);
        bool textfound = false;
        textfound = base.Document.ContainsText(expectedResult);
        Assert.That(textfound, string.Format(@"String: '{0}' was not found", expectedResult));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="attributeValue"></param>
    /// <param name="attributeName"></param>
    public void AssertFrameDoesNotContainLink(string attributeValue, string attributeName)
    {
        Assert.IsFalse(base.Document.Link(Find.By(attributeName, attributeValue)).Exists);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeValue"></param>
    /// <param name="attributeName"></param>
    public void AssertFrameContainsLink(string attributeValue, string attributeName)
    {
        Assert.IsTrue(base.Document.Link(Find.By(attributeName, attributeValue)).Exists);
    }

    /// <summary>
    ///
    /// </summary>
    public void AssertNotificationDoesNotExist()
    {
        Logger.LogAction("Asserting that the notification text is not displayed on the screen.");
        Assert.That(!base.Notification.Exists);
    }

    /// <summary>
    /// This assertion will check that a given Rule Message is displayed on the screen, searching for an exact match.
    /// </summary>
    /// <param name="expectedResult">The Expected Domain Validation Warning</param>
    public void AssertValidationMessageExists(string expectedResult)
    {
        bool found = false;
        string actualResult = string.Empty;
        try
        {
            base.divValidationSummaryBody.WaitUntilExists();
            foreach (Element item in base.listErrorMessages)
            {
                actualResult = actualResult + " | " + item.Text;
                if (item.Text.RemoveWhiteSpace().Replace(",", "").ToUpper() == expectedResult.RemoveWhiteSpace().Replace(",", "").ToUpper())
                {
                    found = true;
                    item.Flash(2);
                    break;
                }
            }
            Logger.LogAction("Asserting that Validation Message: '{0}' is returned and displayed", expectedResult);
            Assert.True(found, string.Format(@"Expected validation message: '{0}' vs actual validation message: '{1}'", expectedResult, actualResult));
        }
        catch (TimeoutException)
        {
            Assert.Fail(string.Format(@"No validation summary was found. Expected a summary with message {0}", expectedResult));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expectedResults"></param>
    public void AssertValidationMessageExists(params string[] expectedResults)
    {
        foreach (string expectedResult in expectedResults)
            AssertValidationMessageExists(expectedResult);
    }

    /// <summary>
    /// This assertion will check that a given Rule Message is not displayed on the screen.
    /// </summary>
    /// <param name="expectedResult">The Domain Validation Warning that should not exist</param>
    public void AssertValidationMessageDoesNotExist(string expectedResult)
    {
        bool resultMatch = true;
        bool valSummaryExists = base.divValidationSummaryBody.Exists;
        if (valSummaryExists)
        {
            resultMatch = base.listErrorMessages.Exists(Find.ByText(expectedResult));
            Logger.LogAction(string.Format(@"Asserting that Validation Message: {0} is not returned", expectedResult));
            Assert.False(resultMatch, string.Format(@"Asserting that Validation Message: {0} is not returned", expectedResult));
        }
        else
        {
            Logger.LogAction(string.Format(@"No validation summary was found"));
            Assert.Pass("No validation summary was found");
        }
    }

    /// <summary>
    /// This assertion will check that a given Rule Message is displayed on the screen, using a regular expression to find
    /// the Domain Validation Warning.
    /// </summary>
    /// <param name="expectedResult">The Expected Domain Validation Warning</param>
    public void AssertValidationMessageExists(System.Text.RegularExpressions.Regex expectedResult)
    {
        bool resultMatch = false;

        base.divValidationSummaryBody.WaitUntilExists();
        resultMatch = base.listErrorMessages.Exists(Find.ByText(expectedResult));

        Logger.LogAction("Asserting that Validation Message: '{0}' is returned and displayed", expectedResult);

        Assert.That(resultMatch, "The Validation Message: '" + expectedResult + "' was not found");
    }

    /// <summary>
    /// This assertion will check that the Domain Validation Warning message box does not appear on the screen.
    /// </summary>
    public void AssertNoValidationMessageExists()
    {
        Logger.LogAction("Asserting that no Validation Messages were returned");
        base.Document.DomContainer.WaitForComplete();
        Assert.False(base.divValidationSummaryBody.Exists, "ValidationSummaryBody Div was found");
    }

    /// <summary>
    /// This assertion will check the IE frame for a text string
    /// </summary>
    /// <param name="expectedResult">The string that you expect to see on the screen</param>
    public void AssertFrameContainsText(string expectedResult)
    {
        Logger.LogAction("Asserting that the following text exists in the Frame: {0}", expectedResult);
        bool textfound = false;
        textfound = base.Document.ContainsText(expectedResult);
        Assert.That(textfound, string.Format(@"String: '{0}' was not found", expectedResult));
    }

    /// <summary>
    /// This assertion will check the IE frame does not contain a text string
    /// </summary>
    /// <param name="expectedResult">The string that you expect to see on the screen</param>
    public void AssertFrameNotContainsText(string expectedResult)
    {
        bool textfound = false;
        textfound = base.Document.ContainsText(expectedResult);
        Assert.False(textfound, string.Format(@"String: '{0}' was found", expectedResult));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="notification"></param>
    public void AssertNotification(string notification)
    {
        Logger.LogAction("Asserting the Notification matches the regular expression pattern: {0}", notification);
        StringAssert.IsMatch(notification, base.Notification.Text);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expectedView"></param>
    public void AssertViewLoaded(string expectedView)
    {
        if (base.ViewName.Exists)
        {
            Assert.That(base.ViewName.Text == expectedView,
                string.Format(@"Expected View {0} was not loaded. Actual view was: {1}", expectedView, base.ViewName.Text));
        }
        else
        {
            string stackTrace = base.StackTrace.Exists ? base.StackTrace.Text : string.Empty;
            string errorMessage = base.ErrorLabel.Exists ? base.ErrorLabel.Text : string.Empty;
            Assert.Fail(string.Format("No valid view was loaded. Error was: {0} - {1}", stackTrace, errorMessage));
        }
    }

    /// <summary>
    /// Asserts that the list of validation messages contains a message with the string provided.
    /// </summary>
    /// <param name="expectedString"></param>
    public void AssertValidationMessagesContains(string expectedString)
    {
        bool found = false;
        base.divValidationSummaryBody.WaitUntilExists();
        foreach (var item in base.listErrorMessages)
        {
            if (item.Text.Contains(expectedString))
            {
                found = true;
                break;
            }
        }
        Assert.IsTrue(found, string.Format(@"No validation message containing: '{0}' was found in the list of domain messages.", expectedString));
    }

    public bool ValidationSummaryExists()
    {
        return base.divValidationSummaryBody.Exists;
    }

    public void AssertNode(string nodeName, string messageOnFail)
    {
        var link = (from l in base.GetAllLinks()
                    where l.Text.Equals(nodeName)
                    select l != null).FirstOrDefault();
        Assert.IsTrue(link, messageOnFail);
    }

    public void AssertValidationIsWarning()
    {
        Logger.LogAction(string.Format(@"Asserting that the validation message is a warning"));
        Assert.True(base.divValidationSummaryBody.Buttons.Count > 0, string.Format(@"This is not a warning as no buttons exist to continue processing"));
    }

    public void AssertValidationIsError()
    {
        Logger.LogAction(string.Format(@"Asserting that the validation message is an error message"));
        Assert.True(base.divValidationSummaryBody.Buttons.Count == 0, string.Format(@"This is not an error message as buttons exist to continue processing"));
    }
}