using System.Collections.Generic;

namespace InternetComponentTests
{
    public static class Helpers
    {
        public static List<string> GetSurveyUrlList(int noURL)
        {
            int count = 0;
            List<string> urlList = new List<string>();
            //SQLQuerying.DataContext dataContext
            //    = new SQLQuerying.DataContext(InternetComponentTests.Default.SAHLDataBaseServer, "2AM", schema: "survey");
            //SQLQuerying.QueryResults results = dataContext.GetTable("ClientQuestionnaire", 1000);
            //results = results.Filter<string>("DateReceived","");
            //List<string> guidList = results.GetColumnValueList<String>("guid");
            //foreach (string guid in guidList)
            //{
            //    count++;
            //    UriBuilder uriBuild = new UriBuilder();
            //    uriBuild.Scheme = "http";
            //    uriBuild.Host = InternetComponentTests.Default.WebsiteHost;
            //    uriBuild.Path = "survey.aspx";
            //    uriBuild.Query = String.Format("survey={0}", guid);
            //    urlList.Add(uriBuild.Uri.AbsoluteUri);
            //    if (count == noURL)
            //        break;
            //}
            return urlList;
        }
    }
}