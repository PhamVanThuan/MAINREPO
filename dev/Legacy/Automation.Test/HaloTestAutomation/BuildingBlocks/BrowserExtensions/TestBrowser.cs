using BuildingBlocks.Services.Contracts;
using Common;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.NavigationControls;
using ObjectMaps.Pages;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core;
using WatiN.Core.DialogHandlers;
using WatiN.Core.Exceptions;

namespace BuildingBlocks
{
    /// <summary>
    /// TestAutomation Browser that extends the WaTiN browser.
    /// </summary>
    public class TestBrowser : IE
    {
        /// <summary>
        /// Initialize an instance of a browser that doesn't need login, will use url configuration to navigate.
        /// </summary>
        public TestBrowser(bool isSahomeloansWebsite)
            : this(string.Empty, string.Empty, GlobalConfiguration.TestWebsite)
        {
        }

        /// <summary>
        ///  Create Defualt IE navigating to the configured url and password
        /// </summary>
        /// <param name="testusername"></param>
        public TestBrowser(string testusername, string password)
            : this(testusername, password, GlobalConfiguration.HaloURL)
        {
        }

        public TestBrowser(string testusername)
            : this(testusername, TestUsers.Password, GlobalConfiguration.HaloURL)
        {
        }
        
        /// <summary>
        /// Create Defualt IE and navigate to URL and use username and password to login
        /// </summary>
        /// <param name="testusername"></param>
        /// <param name="testuserpassword"></param>
        /// <param name="url"></param>
        public TestBrowser(string testusername, string testuserpassword, string url)
            : base(true)
        {
            var properties = typeof(TestUsers).GetFields();
            var exists = properties.Any(x => x.GetValue(null).ToString().ToUpper() == testusername.ToUpper());
            if (exists || string.IsNullOrEmpty(testusername))
            {
                this.Initialize(testusername, testuserpassword, url);
            }
            else
            {
                base.Dispose();
                throw new WatiNException(string.Format("Test cannot use a user not defined as a test user. Attempted to use: {0}", testusername));
            }
        }

        #region OurOwnStuff

        /// <summary>
        /// Common initialization of the browser shared bewtween constructors
        /// </summary>
        /// <param name="testUsername"></param>
        /// <param name="testUserPassword"></param>
        /// <param name="url"></param>

        private void Initialize(string testUsername, string testUserPassword, string url)
        {
            this.LoggedInUser = testUsername;
            Assert.True(!String.IsNullOrEmpty(url), "Url was not provided.");
            LogonDialogHandler ldh = null;
            TestBrowserLoginHandle tbldh = null;
            OperatingSystem osInfo = Environment.OSVersion;
            Version vs = osInfo.Version;
            if (osInfo.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 5: //win XP
                        ldh = new LogonDialogHandler(testUsername, testUserPassword);
                        base.DialogWatcher.Add(ldh);
                        break;

                    case 6: //win vista and 7
                        tbldh = new TestBrowserLoginHandle(testUsername, testUserPassword, url);
                        base.DialogWatcher.Add(tbldh);
                        break;
                }
            }
            base.GoTo(url);
            if (ldh != null)
                base.DialogWatcher.Remove(ldh);
            if (tbldh != null)
            {
                Thread.Sleep(2000);
                base.DialogWatcher.HandleWindow(base.DialogWatcher.MainWindow);
                base.DialogWatcher.Remove(tbldh);
            }
            WaitUntilFrameExists();
        }

        private void WaitUntilFrameExists()
        {
            try
            {
                WatiN.Core.Frame frame = null;
                while (frame != null)
                {
                    frame = base.DomContainer.Frames[0];
                }
            }
            catch (Exception ex)
            {
                WaitUntilFrameExists();
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TNavigation"></typeparam>
        /// <param name="hasFrame"></param>
        /// <param name="frameIndex"></param>
        /// <returns></returns>
        public TNavigation Navigate<TNavigation>(bool hasFrame = true, int frameIndex = 0)
            where TNavigation : BaseNavigation, new()
        {
            return GotoPage<TNavigation>(hasFrame, frameIndex);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TPage"></typeparam>
        /// <param name="hasFrame"></param>
        /// <param name="frameIndex"></param>
        /// <returns></returns>
        public TPage Page<TPage>(bool hasFrame = true, int frameIndex = 0)
            where TPage : BasePageControls, new()
        {
            return GotoPage<TPage>(hasFrame, frameIndex);
        }

        private TPage GotoPage<TPage>(bool hasFrame, int frameIndex)
              where TPage : Page, new()
        {
            if (base.Frames.Count == 0 && hasFrame)
                throw new WatiN.Core.Exceptions.FrameNotFoundException("Frames.Count == 0");
            
            var pageInstance = default(TPage);
            if (!hasFrame)
                pageInstance = base.Page<TPage>();
            else
                pageInstance = (TPage)base.Frames[frameIndex].Page<TPage>();
            return pageInstance;
        }

        /// <summary>
        ///
        /// </summary>
        public override DialogWatcher DialogWatcher
        {
            get { return base.DialogWatcher; }
        }

        public string LoggedInUser { get; private set; }

        #endregion OurOwnStuff

        public void ClickAction(string workflowActivity)
        {
            base.Frames[0].Link(Find.ByText(workflowActivity)).Click();
        }

        public void ClickWorkflowLoanNode(string workflow)
        {
            var regex = new Regex(string.Format(@"^Loan\: [\x20-\x7E]* \({0}\)$", workflow));
            FindActionByRegexAndClick(regex);
        }

        private void FindActionByRegexAndClick(Regex regex)
        {
            try
            {
                base.Frames[0].Link(Find.ByText(regex: regex)).Click();
            }
            catch (ElementNotFoundException)
            {
                base.Frames[0].Link(Find.ByTitle(regex: regex)).Click();
            }
        }

        public void ClickWorkflowLoanNode(string workflowState, int accountKey)
        {
            var regex = new Regex(string.Format(@"{0} \: {1} \({0}\)", workflowState, accountKey));
            FindActionByRegexAndClick(regex);
        }

        public void BypassSSLCertificateWarning()
        {
            var overrideLink = base.Link(Find.ById("overridelink"));
            if (overrideLink.Exists)
            {
                overrideLink.Click();
            }
        }

        public bool ActionExists(string actionName)
        {
            return base.Frames[0].Link(Find.ByTitle(actionName)).Exists;
        }
    }
}