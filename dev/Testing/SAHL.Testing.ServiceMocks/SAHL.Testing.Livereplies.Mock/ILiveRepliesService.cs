using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services;

namespace SAHL.Testing.Livereplies.Mock
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.18020")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "LiveRepliesServiceSoap", Namespace = "http://www.comcorplivereplies.com/Comcorp.LiveReplies/LiveRepliesService")]
    public interface ILiveRepliesServiceSoap
    {

        /// <remarks/>
        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.comcorplivereplies.com/Comcorp.LiveReplies/LiveRepliesService/ProcessB" +
            "ankLiveReplies", RequestNamespace = "http://www.comcorplivereplies.com/Comcorp.LiveReplies/LiveRepliesService", ResponseNamespace = "http://www.comcorplivereplies.com/Comcorp.LiveReplies/LiveRepliesService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        string ProcessBankLiveReplies(string requestXML);
    }
}
