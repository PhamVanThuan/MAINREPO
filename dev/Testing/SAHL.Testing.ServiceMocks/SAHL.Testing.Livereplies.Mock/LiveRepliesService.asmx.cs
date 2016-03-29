using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace SAHL.Testing.Livereplies.Mock
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [System.Web.Services.WebServiceBindingAttribute(Name = "LiveRepliesServiceSoap", Namespace = "http://localhost/SAHL.Testing.Livereplies.Mock/")]
    public class LiveRepliesService : ILiveRepliesServiceSoap
    {
        public string ProcessBankLiveReplies(string requestXML)
        {
            string result = @"<ProcessBankLiveReplies.Reply>
  <Service.Header>
    <Reply.DateTime>######</Reply.DateTime>
    <Service.Result>1</Service.Result>
  </Service.Header>
</ProcessBankLiveReplies.Reply>";

            result = result.Replace("######", DateTime.Now.ToShortDateString());
            return result;
        }
    }
}
