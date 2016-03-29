using NUnit.Framework;
using SAHL.Services.Interfaces.ITC.Models;
using SAHL.Services.ITC.TransUnion;
using System.Diagnostics;

namespace SAHL.Services.ITC.Tests.TransUnion
{
    [TestFixture]
    public class ITCServiceTest
    {
        [Ignore("transunion web service call")]
        [Test]
        public void PerformITCEnquiry()
        {
            ItcRequest request = new ItcRequest()
            {
                Forename1 = "Seethiah",
                Forename2 = "Danny",
                Forename3 = string.Empty,
                Surname = "Appanna",
                BirthDate = "19490814",
                IdentityNo1 = "4908145134086",
                Sex = "M",
                Title = "Mr",
                MaritalStatus = "Married - Community of Property",
                HomeTelCode = string.Empty,
                HomeTelNo = string.Empty,
                WorkTelCode = "011",
                WorkTelNo = "3155035",
                CellNo = "0836414368",
                EmailAddress = "fm@constantiahotel.co.za",
                AddressLine1 = string.Empty,
                AddressLine2 = "43 Terrace road",
                Suburb = "Eastleigh",
                City = "Edenvale",
                PostalCode = "1609",
                Address2Line1 = string.Empty,
                Address2Line2 = string.Empty,
                Address2Suburb = string.Empty,
                Address2City = string.Empty,
                Address2PostalCode = string.Empty
            };

            ITransUnionServiceConfiguration config = new TransUnionServiceConfiguration();
            ITransUnionService itcService = new TransUnionService(config);
            var response = itcService.PerformRequest(request);
            Debug.WriteLine(response);
        }
    }
}