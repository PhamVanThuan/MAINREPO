using System;
using SQLQuerying;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;
using BuildingBlocks;
using BuildingBlocks.SAHLWebsite;
using WatiN.Core;
using CommonData.Constants;
using SQLQuerying.DataHelper;
using System.Threading.Tasks;

namespace WebsiteTests.PageTests
{
    [TestFixture,RequiresSTA]
    public class ClientSurvey
    {
        #region PrivateVar
        private Dictionary<int, Uri> accountkeyUri = new Dictionary<int, Uri>();
        #endregion PrivateVar

        [Test]
        public void SimultaneousSurveySubmit()
        {
             //Create two surveys
            Common.WatiNExtensions.CloseAllOpenIEBrowsers();
            List<Browser> browserList = new List<Browser>();
            List<string> urlList = new List<string>();
            try
            {
                //List of browser sessions foreach URL
                List<string> comments = new List<string>();
                comments.Add("1+1=2");
                comments.Add("c:\temp");
                comments.Add("<input></input>");
                comments.Add(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                comments.Add("<TAG\b[^>]*>(.*?)</TAG>");
                comments.Add("tested''");
                comments.Add("!@#$%%))(!@*#)!@&#(_*!@#_+!@(&#+)(!@&$+)!@%^!@+$!@#!+_@(#&)!@#&_!*@$&$%^%%(%^+!@$_!@(#!@!@$(+*@!+%&!@&#*^!@#^!@$@!_$");
                comments.Add(@"/'''//////""""//  `,', `;' ");
                comments.Add(@"'''");
                comments.Add(@"'awdawd''awdawd");
                comments.Add(@"select * from dbo.account");
                comments.Add(@"+ '' + ''");

                foreach (string url in Helpers.GetSurveyUrlList(comments.Count))
                {
                    Browser b = new IE();
                    browserList.Add(b);
                    b.GoTo(url);
                    urlList.Add(url);
                    b.WaitForComplete();
                    string specialCharList = String.Empty;
                    Pages.SAHLWebsiteClientSurvey.DefaultPopulate(b, comments[0]);
                    if (comments.Count > 1)
                        comments.RemoveAt(0);
                }
                Action<Browser> action = new Action<Browser>(SurveySubmit);
                Parallel.ForEach<Browser>(browserList, action);
                
                //Assert
                foreach (string url2 in urlList)
                {
                    string[] strSplit = url2.Split('=');
                    string guid = strSplit[1];
                    string message = Assertions.ClientSurvey.AssertClientSurveyAnswers(guid);
                    if (!message.Contains("All answers was saved for client questionnair"))
                        throw new WatiN.Core.Exceptions.WatiNException(String.Format(@"{0}", message));
                }
            }
            catch(WatiN.Core.Exceptions.WatiNException)
            {
                foreach (Browser b in browserList)
                    b.Dispose();
                throw;
            }
            catch
            {
                foreach (Browser b in browserList)
                    b.Dispose();
                throw;
            }
        }

        #region Helper
        private void SurveySubmit(Browser b)
        {
            Pages.SAHLWebsiteClientSurvey.Submit(b);
         
        }
        #endregion Helper
    }
}
