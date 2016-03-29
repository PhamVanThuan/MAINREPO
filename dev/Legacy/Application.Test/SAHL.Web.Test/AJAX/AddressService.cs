using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.AJAX;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using AjaxControlToolkit;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using System.Data;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class AddressService : TestViewBase
    {

        private Address _addressService;


        [SetUp]
        public void Setup()
        {
            _addressService = new Address();
        }

        [TearDown]
        public void TearDown()
        {
            _addressService = null;
        }

        [NUnit.Framework.Test]
        public void GetProvincesByCountry()
        {
            using (new SessionScope())
            {
                Country_DAO c = Country_DAO.FindFirst();
                CascadingDropDownNameValue[] values = _addressService.GetProvincesByCountry(AjaxHelpers.ConvertToDictionaryEntry(c.Key), "");
                Assert.AreEqual(c.Provinces.Count, values.Length, "The number of provinces returned does not equal the number of provinces attached to the country");
            }
        }

        [NUnit.Framework.Test]
        public void GetPostOffices()
        {
            using (new SessionScope())
            {
                PostOffice_DAO po = PostOffice_DAO.FindFirst();
                string prefix = po.Description.Substring(0, po.Description.Length - 2);
                SAHLAutoCompleteItem[] items = _addressService.GetPostOffices(prefix);

                bool found = false;
                foreach (SAHLAutoCompleteItem item in items)
                {
                    if (item.Value == po.Key.ToString())
                        found = true;
                }

                if (!found)
                    Assert.Fail(String.Format("GetPostOffices did not find post office '{0}' using prefix '{1}'.", po.Description, prefix));
            }
        }

        [NUnit.Framework.Test]
        public void GetSuburbsByProvince()
        {
            string query = @"select top 1 s.SuburbKey, s.Description, p.ProvinceKey from [2am].[dbo].Suburb s (NOLOCK) 
                inner join [2am].[dbo].City c (NOLOCK) on c.CityKey = s.CityKey 
                inner join [2am].[dbo].Province p (NOLOCK)  on c.ProvinceKey = p.ProvinceKey";
            DataTable dt = base.GetQueryResults(query);

            if (dt.Rows.Count == 0)
                Assert.Ignore("No data");

            int suburbKey = Convert.ToInt32(dt.Rows[0][0]);
            string suburbDesc = dt.Rows[0][1].ToString();
            int provinceKey = Convert.ToInt32(dt.Rows[0][2]);
            dt.Dispose();

            using (new SessionScope())
            {

                Province_DAO province = Province_DAO.Find(provinceKey);
                Suburb_DAO suburb = Suburb_DAO.Find(suburbKey);

                bool found = false;
                SAHLAutoCompleteItem[] items = _addressService.GetSuburbsByProvince(suburbDesc, province.Key.ToString());
                foreach (SAHLAutoCompleteItem item in items)
                {
                    if (item.Value == suburb.Key.ToString())
                        found = true;
                }
                Assert.IsTrue(found, String.Format("Suburb '{0}' not found when using prefix '{1}' and key '{2}'.", suburbDesc, suburbDesc, province.Key.ToString()));
            }
        }

        [NUnit.Framework.Test]
        public void GetSuburbDetails()
        {
            using (new SessionScope())
            {
                Suburb_DAO suburb = Suburb_DAO.FindFirst();

                string[] details = _addressService.GetSuburbDetails(suburb.Key);

                Assert.AreEqual(details[0], suburb.City.Description, "Array item 0 was not the same as Suburb.City.Description as expected");
                Assert.AreEqual(details[1], (suburb.PostalCode == null ? String.Empty : suburb.PostalCode), "Array item 1 was not the same as Suburb.PostalCode as expected");
            }

        }

        [NUnit.Framework.Test]
        public void SearchAddresses()
        {
            using (new SessionScope())
            {
                // box address
                AddressBox_DAO box = AddressBox_DAO.FindFirst();
                string[] boxValues = { 
                    String.Format("BoxNumber={0}", box.BoxNumber) ,
                    "PostOfficeKey="
                };
                SearchAddressFormat(AddressFormats.Box, boxValues, box);

                // cluster box address
                AddressClusterBox_DAO cbox = AddressClusterBox_DAO.FindFirst();
                string[] cboxValues = { 
                    String.Format("ClusterBoxNumber={0}", cbox.ClusterBoxNumber) ,
                    "PostOfficeKey="
                };
                SearchAddressFormat(AddressFormats.ClusterBox, cboxValues, cbox);

                // free text address
                AddressFreeText_DAO freetext = AddressFreeText_DAO.FindFirst();
                string[] freetextValues = { 
                    String.Format("FreeTextLine1={0}", freetext.FreeText1) ,
                    String.Format("FreeTextLine2={0}", freetext.FreeText2) ,
                    String.Format("FreeTextLine3={0}", freetext.FreeText3) ,
                    String.Format("FreeTextLine4={0}", freetext.FreeText4) ,
                    "FreeTextLine5=",
                    "Country="
                };
                SearchAddressFormat(AddressFormats.FreeText, freetextValues, freetext);

                // postnet suite address
                AddressPostnetSuite_DAO pns = AddressPostnetSuite_DAO.FindFirst();
                string[] pnsValues = { 
                    String.Format("PostnetSuiteNumber={0}", pns.SuiteNumber) ,
                    String.Format("PrivateBagNumber={0}", pns.PrivateBagNumber) ,
                    "PostOfficeKey="
                };
                SearchAddressFormat(AddressFormats.PostNetSuite, pnsValues, pns);

                // private bag address
                AddressPrivateBag_DAO pbag = AddressPrivateBag_DAO.FindFirst();
                string[] pbagValues = { 
                    String.Format("PrivateBagNumber={0}", pbag.PrivateBagNumber) ,
                    "PostOfficeKey="
                };
                SearchAddressFormat(AddressFormats.PrivateBag, pbagValues, pbag);

                // street address
                AddressStreet_DAO street = AddressStreet_DAO.FindFirst();
                string[] streetValues = { 
                    String.Format("BuildingNumber={0}", street.BuildingNumber) ,
                    String.Format("BuildingName={0}", street.BuildingName) ,
                    String.Format("Country={0}", street.RRR_CountryDescription) ,
                    String.Format("Province={0}", street.RRR_ProvinceDescription) ,
                    String.Format("StreetNumber={0}", street.StreetNumber) ,
                    String.Format("StreetName={0}", street.StreetName) ,
                    String.Format("SuburbKey={0}", street.Suburb.Key) ,
                    String.Format("UnitNumber={0}", street.UnitNumber)
                };
                SearchAddressFormat(AddressFormats.Street, streetValues, street);
            }

        }

        private void SearchAddressFormat(AddressFormats addressFormat, string[] inputValues, Address_DAO address)
        {
            AjaxItem[] items = _addressService.SearchAddresses(((int)addressFormat).ToString(), inputValues);
            bool found = false;

            foreach (AjaxItem ai in items)
            {
                if (ai.Value == address.Key.ToString())
                    found = true;
            }

            // we can only raise a failure if the count is less than 10 and the item is not found
            if (!found && items.Length < 10)
                Assert.Fail("Address search did not find address.");
        }

    }
}
