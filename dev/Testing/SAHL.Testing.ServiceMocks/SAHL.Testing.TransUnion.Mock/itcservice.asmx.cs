using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace SAHL.Testing.TransUnion.Mock
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class itcservice : SAHL.Testing.TransUnion.Mock.IConsumerSoap
    {

        public BureauResponse ProcessRequestTrans01(BureauEnquiry01 BureauEnquiry01, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans07(BureauEnquiry07 BureauEnquiry07)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans12(BureauEnquiry12 BureauEnquiry12)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans13(BureauEnquiry13 BureauEnquiry13, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans17(BureauEnquiry17 BureauEnquiry17, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans22(BureauEnquiry22 BureauEnquiry22)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans23(BureauEnquiry23 BureauEnquiry23)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans41(BureauEnquiry41 BureauEnquiry41, Destination Destination)
        {
            string fileName = String.Empty;
            string fragment = BureauEnquiry41.Surname;
            switch (fragment)
            {
                case "AdminOrder":
                    fileName = "Admin Order ITC.xml";
                    break;
                case "CodeZ":
                    fileName = "Code Z - Consumer is deceased ITC.xml";
                    break;
                case "DebtCounselling":
                    fileName = "Debt Counselling ITC.xml";
                    break;
                case "HeaderOnly":
                    fileName = "Header only ITC.xml";
                    break;
                case "Judgement1_10000":
                    fileName = "Judgements 1 and value 10 000 ITC.xml";
                    break;
                case "Judgement1_9999":
                    fileName = "Judgements 1 and value 9999 ITC.xml";
                    break;
                case "Judgement2_10000":
                    fileName = "Judgements 2 and value 10 000 ITC.xml";
                    break;
                case "Judgement3_10000":
                    fileName = "Judgements 3 and value 10 000 ITC.xml";
                    break;
                case "Judgement3_10001":
                    fileName = "Judgements 3 and value 10 001 ITC.xml";
                    break;
                case "Judgement3_9999":
                    fileName = "Judgements 3 and value 9999 ITC.xml";
                    break;
                case "Judgement3_Random":
                    fileName = "Judgements 3 and value random ITC.xml";
                    break;
                case "Judgement4_Random":
                    fileName = "Judgements 4 and value random ITC.xml";
                    break;
                case "Judgement5_Random":
                    fileName = "Judgements 5 and value random ITC.xml";
                    break;
                case "NonSettledJudgements_14999":
                    fileName = "Judgements of 14999 and non settles for 36 months (05-01-11) ITC.xml";
                    break;
                case "NonSettledJudgements_15001":
                    fileName = "Judgements of 15001 and non settles for 36 months (05-05-11) ITC.xml";
                    break;
                case "Sequestration":
                    fileName = "Sequestration ITC.xml";
                    break;
                case "Defaults3In2Yrs":
                    fileName = "Unsettled defaults of 3 within two years ITC.xml";
                    break;
                case "Defaults4In2Yrs":
                    fileName = "Unsettled defaults of 4 within two years ITC.xml";
                    break;
                case "Defaults5In2Yrs":
                    fileName = "Unsettled defaults of 5 within two years ITC.xml";
                    break;
                default:
                    fileName = "ProcessRequestTrans41.xml";
                    break;
            }

            switch (BureauEnquiry41.Surname)
            {
                case "Error":
                    fileName = "Error.xml";
                    break;
                case "Large":
                    fileName = "Large.xml";
                    break;
                case "ThrowException":
                    throw new Exception("Catch me if you can");
            }

            if (BureauEnquiry41.Surname.Contains("Sleep"))
            {
                var time = BureauEnquiry41.Surname.Split('_')[1];
                System.Threading.Thread.Sleep(Convert.ToInt32(time));
            }

            var path = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/App_Data"));
            var reader = XmlReader.Create(string.Format(@"{0}\{1}",path,fileName));
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SAHL.Testing.TransUnion.Mock.BureauResponse), "https://secure.transunion.co.za/TUBureau");
            var response = (SAHL.Testing.TransUnion.Mock.BureauResponse)serializer.Deserialize(reader);
            reader.Close();

            var errorResponseList = new List<string>() { "Error", "Large", "TimeOut", "ThrowException" };

            if (!errorResponseList.Contains(BureauEnquiry41.Surname))
            {
                response.UniqueRefGuid = Guid.NewGuid();
                response.ConsumerInfoNO04.IdentityNo1 = BureauEnquiry41.IdentityNo1;

                var date = DateTime.Now;
                switch (fileName)
                {
                    case "Judgements 1 and value 10 000 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 1 and value 9999 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 2 and value 10 000 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 3 and value 10 000 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[20].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 3 and value 10 001 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[2].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 3 and value 9999 ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[2].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 3 and value random ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[2].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 4 and value random ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-1).ToString("yyyyMMdd");
                        response.JudgementsNJ07[2].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[3].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements 5 and value random ITC.xml":
                        response.JudgementsNJ07[0].JudgmentDate = date.ToString("yyyyMMdd");
                        response.JudgementsNJ07[1].JudgmentDate = date.AddDays(1).AddYears(-1).ToString("yyyyMMdd");
                        response.JudgementsNJ07[2].JudgmentDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.JudgementsNJ07[3].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        response.JudgementsNJ07[4].JudgmentDate = date.AddDays(1).AddYears(-3).ToString("yyyyMMdd");
                        break;
                    case "Judgements of 14999 and non settles for 36 months (05-01-11) ITC.xml":
                        foreach (var judgement in response.JudgementsNJ07)
                        {
                            judgement.JudgmentDate = date.AddMonths(-36).ToString("yyyyMMdd");
                        }
                        break;
                    case "Judgements of 15001 and non settles for 36 months (05-05-11) ITC.xml":
                        foreach (var judgement in response.JudgementsNJ07)
                        {
                            judgement.JudgmentDate = date.AddMonths(-36).ToString("yyyyMMdd");
                        }
                        break;
                    case "Unsettled defaults of 3 within two years ITC.xml":
                        response.DefaultsD701Part1[0].WrittenOffDate = date.ToString("yyyyMMdd");
                        response.DefaultsD701Part1[1].WrittenOffDate = date.AddDays(1).AddYears(-1).ToString("yyyyMMdd");
                        response.DefaultsD701Part1[2].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        break;
                    case "Unsettled defaults of 4 within two years ITC.xml":
                        response.DefaultsD701Part1[0].WrittenOffDate = date.ToString("yyyyMMdd");
                        response.DefaultsD701Part1[1].WrittenOffDate = date.AddDays(1).AddYears(-1).ToString("yyyyMMdd");
                        response.DefaultsD701Part1[2].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        response.DefaultsD701Part1[3].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyyMMdd");
                        break;
                    case "Unsettled defaults of 5 within two years ITC.xml":
                        response.DefaultsD701Part1[0].WrittenOffDate = date.ToString("yyyMMdd");
                        response.DefaultsD701Part1[1].WrittenOffDate = date.AddDays(1).AddYears(-1).ToString("yyyMMdd");
                        response.DefaultsD701Part1[2].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyMMdd");
                        response.DefaultsD701Part1[3].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyMMdd");
                        response.DefaultsD701Part1[4].WrittenOffDate = date.AddDays(1).AddYears(-2).ToString("yyyMMdd");
                        break;
                    case "ProcessRequestTrans41.xml":
                        var surname = BureauEnquiry41.Surname;
                        string score = "00000";
                        string empiricaScoreTest = "Empirica_";
                        if (surname.Contains(empiricaScoreTest))
                        {
                            score = surname.Split('_')[1];
                            response.EmpiricaEM07.EmpiricaScore = score;
                        }
                        break;
                    default:
                        break;
                }
            }

            return response;
        }

        public BureauResponse ProcessRequestTrans42(BureauEnquiry42 BureauEnquiry42, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans43(BureauEnquiry43 BureauEnquiry43, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauPingResponse BureauPing(BureauPing BureauPing1)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTransC4(BureauEnquiryC4 BureauEnquiryC4, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTransC3(BureauEnquiryC3 BureauEnquiryC3, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans91(BureauEnquiry91 BureauEnquiry91, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public BureauResponse ProcessRequestTrans92(BureauEnquiry92 BureauEnquiry92, Destination Destination)
        {
            throw new NotImplementedException();
        }

        public string HelloWorld()
        {
            throw new NotImplementedException();
        }

        private SAHL.Testing.TransUnion.Mock.BureauResponse BuildResponse(SAHL.Testing.TransUnion.Mock.BureauEnquiry41 BureauEnquiry41)
        {
            var response = new SAHL.Testing.TransUnion.Mock.BureauResponse();
            response.ResponseStatus = SAHL.Testing.TransUnion.Mock.ResponseStatus.Success;
            response.ProcessingStartDate = DateTime.Now;
            response.UniqueRefGuid = Guid.NewGuid();

            SAHL.Testing.TransUnion.Mock.v1Seg[] v1Seg = {
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "00", Value = "01" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "01", Value = "02" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NA", Value = "07" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NC", Value = "04" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "D7", Value = "01" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NE", Value = "50" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NJ", Value = "07" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NK", Value = "04" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NM", Value = "04" },
                                               new SAHL.Testing.TransUnion.Mock.v1Seg() { Code = "NO", Value = "04" }
                                           };
            response.V1Segment = new SAHL.Testing.TransUnion.Mock.V1Segment();
            response.V1Segment.v1Segs = v1Seg;

            response.ConsEnqTransInfo0102 = new SAHL.Testing.TransUnion.Mock.ConsEnqTransInfo0102
            {
                DefiniteMatchCount = "1",
                PossibleMatchCount = "00",
                MatchedConsumerNo = "633072787",
            };

            response.EchoData0001 = new SAHL.Testing.TransUnion.Mock.EchoData0001
            {
                SubscriberCode = BureauEnquiry41.SubscriberCode,
                ClientReference = BureauEnquiry41.ClientReference
            };

            response.ConsumerInfoNO04 = new SAHL.Testing.TransUnion.Mock.ConsumerInfoNO04
            {
                RecordSeq = "01",
                PartSeq = "001",
                IdentityNo1 = BureauEnquiry41.IdentityNo1
            };

            return response;
        }
    }
}