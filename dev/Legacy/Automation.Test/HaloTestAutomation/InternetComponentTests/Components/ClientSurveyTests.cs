using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.InternetComponents;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WatiN.Core;

namespace InternetComponentTests
{
    [TestFixture, RequiresSTA]
    public class ClientSurveyTests : TestBase<BasePage>
    {
        [Test]
        public void SimultaneousSurveySubmit()
        {
            //Create two surveys
            Service<IWatiNService>().CloseAllOpenIEBrowsers();
            List<TestBrowser> browserList = new List<TestBrowser>();
            List<string> urlList = new List<string>();
            try
            {
                //List of browser sessions foreach URL
                var comments = new List<string>
                                    {
                                        "1+1=2",
                                        "c:\temp",
                                        "<input></input>",
                                        @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b",
                                        "<TAG\b[^>]*>(.*?)</TAG>",
                                        "tested''",
                                        "!@#$%%))(!@*#)!@&#(_*!@#_+!@(&#+)(!@&$+)!@%^!@+$!@#!+_@(#&)!@#&_!*@$&$%^%%(%^+!@$_!@(#!@!@$(+*@!+%&!@&#*^!@#^!@$@!_$",
                                        @"/'''//////""""//  `,', `;' ",
                                        @"'''",
                                        @"'awdawd''awdawd",
                                        @"select * from dbo.account",
                                        @"+ '' + ''"
                                    };

                foreach (string url in Helpers.GetSurveyUrlList(comments.Count))
                {
                    var browser = new TestBrowser(string.Empty, string.Empty, url);
                    urlList.Add(url);
                    browser.WaitForComplete();
                    string specialCharList = String.Empty;
                    browser.Page<ClientSurvey>().DefaultPopulate(comments[0]);
                    if (comments.Count > 1)
                        comments.RemoveAt(0);
                }
                Action<TestBrowser> action = new Action<TestBrowser>(SurveySubmit);
                Parallel.ForEach<TestBrowser>(browserList, action);

                //Assert
                foreach (string url2 in urlList)
                {
                    string[] strSplit = url2.Split('=');
                    string guid = strSplit[1];
                    string message = ClientSurveyAssertions.AssertClientSurveyAnswers(guid);
                    if (!message.Contains("All answers was saved for client questionnair"))
                        throw new WatiN.Core.Exceptions.WatiNException(String.Format(@"{0}", message));
                }
            }
            catch (WatiN.Core.Exceptions.WatiNException)
            {
                foreach (TestBrowser b in browserList)
                    b.Dispose();
                throw;
            }
            catch
            {
                foreach (TestBrowser b in browserList)
                    b.Dispose();
                throw;
            }
        }

        #region Helper

        private void SurveySubmit(TestBrowser browser)
        {
            browser.Page<ClientSurvey>().Submit();
        }

        #endregion Helper
    }
}