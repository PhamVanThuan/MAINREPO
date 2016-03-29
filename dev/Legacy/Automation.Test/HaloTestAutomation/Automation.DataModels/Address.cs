using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Address : IComparable<Address>
    {
        public int AddressKey { get; set; }

        public AddressFormatEnum AddressFormatKey { get; set; }

        public string AddressFormatDescription { get; set; }

        public AddressTypeEnum AddressType { get; set; }

        public string AddressTypeDescription { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Line4 { get; set; }

        public string Line5 { get; set; }

        public string FreeText1 { get; set; }

        public string FreeText2 { get; set; }

        public string FreeText3 { get; set; }

        public string FreeText4 { get; set; }

        public string FreeText5 { get; set; }

        public string RRR_CountryDescription { get; set; }

        public string SuiteNumber { get; set; }

        public string PrivateBag { get; set; }

        public string UnitNumber { get; set; }

        public string BoxNumber { get; set; }

        public string BuildingNumber { get; set; }

        public string BuildingName { get; set; }

        public string StreetNumber { get; set; }

        public string StreetName { get; set; }

        public string RRR_SuburbDescription { get; set; }

        public string RRR_ProvinceDescription { get; set; }

        public string RRR_CityDescription { get; set; }

        public string RRR_PostalCode { get; set; }

        public string PostalCode { get; set; }

        public string ConcatenatedAddressString { get; set; }

        public string PostnetSuiteNumber { get; set; }

        public string PostOfficeDescription { get; set; }

        public int SuburbKey { get; set; }

        public object UserID { get; set; }

        public object ChangeDate { get; set; }

        #region Other

        public string FormattedAddress { get; set; }

        #endregion Other

        public int CompareTo(Address other)
        {
            if (!this.AddressFormatKey.Equals(other.AddressFormatKey))
                return 0;
            if (!this.AddressFormatDescription.Equals(other.AddressFormatDescription))
                return 0;
            if (!this.RRR_CountryDescription.Equals(other.RRR_CountryDescription))
                return 0;
            if (!this.UnitNumber.Equals(other.UnitNumber))
                return 0;
            if (!this.BuildingNumber.Equals(other.BuildingNumber))
                return 0;
            if (!this.BuildingName.Equals(other.BuildingName))
                return 0;
            if (!this.StreetNumber.Equals(other.StreetNumber))
                return 0;
            if (!this.StreetName.Equals(other.StreetName))
                return 0;
            if (!this.RRR_SuburbDescription.Equals(other.RRR_SuburbDescription))
                return 0;
            if (!this.RRR_ProvinceDescription.Equals(other.RRR_ProvinceDescription))
                return 0;
            if (!this.RRR_CityDescription.Equals(other.RRR_CityDescription))
                return 0;
            if (!this.RRR_PostalCode.Equals(other.RRR_PostalCode))
                return 0;
            return 1;
        }
    }
}