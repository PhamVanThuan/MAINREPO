using mshtml;
using System;
using System.Reflection;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Logging;
using WatiN.Core.Native;
using WatiN.Core.Native.InternetExplorer;

namespace ObjectMaps.Pages
{
    public abstract class BasePageControls : Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
            var watinProperties
                = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var watinProperty in watinProperties)
            {
                if (watinProperty.PropertyType.BaseType != null
                        && watinProperty.PropertyType.BaseType.Name.Contains(typeof(Element).Name)
                        && !watinProperty.PropertyType.BaseType.Name.Contains("Collection")
                        && watinProperty.Name != "WarningSubmit")
                {
                    var element = (Element)watinProperty.GetValue(this, null);
                    if (element == null)
                    {
                        throw new WatiN.Core.Exceptions.WatiNException
                            (
                                String.Format("Property:{0} in the {1} ObjectMap class returned null. Check if the FindBy attribute was applied.", watinProperty.Name, this.GetType().Name)
                            );
                    }
                    element.Description = watinProperty.Name;
                }
            }
        }

        [FindBy(Id = "ctl00_valSummary_Body")]
        protected Div divValidationSummaryBody { get; set; }

        protected ElementCollection listErrorMessages
        {
            get
            {
                Div validationSummaryBody = divValidationSummaryBody;
                return validationSummaryBody.ElementsWithTag("LI");
            }
        }

        [FindBy(Id = "ctl00_Main_lblNotification")]
        protected Span Notification { get; set; }

        protected Element bodyErrorPage
        {
            get
            {
                return base.Document.ElementWithTag("BODY", Find.ByClass("Error"));
            }
        }

        [FindBy(Id = "lblError")]
        protected Span ErrorLabel { get; set; }

        [FindBy(Id = "content")]
        protected Div divErrorMessage { get; set; }

        [FindBy(Id = "HomePageButton")]
        protected Button HomePageButton { get; set; }

        /// <summary>
        /// Will use regular expression to find SubmitButton"
        /// </summary>
        [FindBy(IdRegex = "Submit")]
        protected Button SubmitButton { get; set; }

        /// <summary>
        /// YWill use regular expression to find CancelButton"
        /// </summary>
        [FindBy(IdRegex = "Cancel")]
        protected Button CancelButton { get; set; }

        [FindBy(Id = "ctl00_lblCurrentView")]
        protected Span ViewName { get; set; }

        [FindBy(Id = "lblStackTrace")]
        protected Span StackTrace { get; set; }

        protected DivCollection SAHLAutoComplete_DefaultItem_Collection()
        {
            return base.Document.Div("SAHLAutoCompleteDiv").Divs;
        }

        protected LinkCollection GetAllLinks()
        {
            return base.Document.Links;
        }

        public Button WarningSubmit
        {
            get
            {
                if (divValidationSummaryBody.Exists)
                    return divValidationSummaryBody.Buttons[0];
                return null;
            }
        }

        protected IHTMLElement ReturnNativeHtmlElementByClass(INativeDocument document, string className)
        {
            try
            {
                Thread.Sleep(3000);
                var iedoc = (IEDocument)document;
                Logger.Log(LogMessageType.Debug, "Getting IE Document");
                var doc = (mshtml.IHTMLDocument2)iedoc.HtmlDocument;
                Logger.Log(LogMessageType.Debug, "Getting HTML Dom");
                foreach (IHTMLElement element in doc.all)
                {
                    if (
                            element.id != null &&
                            element.className != null &&
                            element.className.Equals(className)
                       )
                    {
                        return element;
                    }
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        [FindBy(Id="ctl00_lblInfoUser")]
        protected Span User { get; set; }
    }
}