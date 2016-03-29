using NUnit.Framework;
using SAHL.Services.ITC.TransUnion;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace SAHL.Services.ITC.Tests.TransUnion
{
    [TestFixture]
    public class ITCReaderTest
    {
        [Test]
        [Ignore]
        public void GetITCProfileTest()
        {
            XmlDocument doc = new XmlDocument();
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ExternalServices/TestITCs", "itc_with_judgements.xml");
            doc.Load(path);
            XDocument root = XDocument.Parse(doc.OuterXml);
            ItcReader itcReader = new ItcReader(root);
            var itcProfile = itcReader.GetITCProfile;

            Debug.WriteLine("******************EmpiricaScore******************");
            Debug.WriteLine("EmpiricaScore: {0}", (itcProfile.EmpericaScore.HasValue ? itcProfile.EmpericaScore.Value : 0));

            Debug.WriteLine("******************Judgments******************");
            foreach (var item in itcProfile.Judgments)
            {
                Debug.WriteLine("JudgmentDate: {0} Amount:{1}", item.JudgmentDate, item.Amount);
            }

            Debug.WriteLine("******************defaults******************");
            foreach (var item in itcProfile.Defaults)
            {
                Debug.WriteLine("InformationDate: {0} DefaultAmount:{1}", item.InformationDate, item.DefaultAmount);
            }

            Debug.WriteLine("******************notices******************");
            foreach (var item in itcProfile.Notices)
            {
                Debug.WriteLine("NoticeDate: {0} NoticeType: {1} NoticeTypeCode: {2}", item.NoticeDate, item.NoticeType, item.NoticeTypeCode);
            }

            Debug.WriteLine("******************paymentProfiles******************");
            foreach (var item in itcProfile.PaymentProfiles)
            {
                Debug.WriteLine("SupplierName: {0},LastUpdateDate: {1}", item.SupplierName, item.LastUpdateDate);

                Debug.WriteLine("******************PaymentHistories******************");

                foreach (var subItem in item.PaymentHistories)
                {
                    Debug.WriteLine("Date: {0} StatusCode: {1}", subItem.Date, subItem.StatusCode);
                }
            }
        }
    }
}