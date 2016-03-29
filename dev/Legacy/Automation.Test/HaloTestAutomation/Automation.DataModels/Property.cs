using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataModels
{
    public class Property : IComparable<Property>
    {
        public int PropertyKey { get; set; }

        public string PropertyId { get; set; }

        public PropertyTypeEnum PropertyTypeKey { get; set; }

        public TitleTypeEnum TitleTypeKey { get; set; }

        public AreaClassificationEnum AreaClassificationKey { get; set; }

        public OccupancyTypeEnum OccupancyTypeKey { get; set; }

        public Address PropertyAddress { get; set; }

        public int AddressKey { get; set; }

        public string PropertyDescription1 { get; set; }

        public string PropertyDescription2 { get; set; }

        public string PropertyDescription3 { get; set; }

        public double DeedsOfficeValue { get; set; }

        public DateTime CurrentBondDate { get; set; }

        public string ErfNumber { get; set; }

        public string ErfPortionNumber { get; set; }

        public string SectionalSchemeName { get; set; }

        public string SectionalUnitNumber { get; set; }

        public DeedsPropertyTypeEnum DeedsPropertyTypeKey { get; set; }

        public string ErfSuburbDescription { get; set; }

        public string ErfMetroDescription { get; set; }

        public int FinancialServiceKey { get; set; }

        public DataProviderEnum DataProviderKey { get; set; }

        #region Other

        public string PropertyTypeDescription { get; set; }

        public string TitleTypeDescription { get; set; }

        public string AreaClassificationDescription { get; set; }

        public string OccupancyTypeDescription { get; set; }

        public string DeedsPropertyTypeDescription { get; set; }

        public string TitleDeedNumber { get; set; }

        public string BondAccountNumber { get; set; }

        public string DeedsOfficeName { get; set; }

        public string FormattedPropertyAddress { get; set; }

        public IEnumerable<Valuation> Valuations { get; set; }

        #endregion Other

        public int CompareTo(Property other)
        {
            if (!this.DeedsPropertyTypeDescription.Equals(other.DeedsPropertyTypeDescription))
                return 0;
            if (!this.AreaClassificationDescription.Equals(other.AreaClassificationDescription))
                return 0;
            if (!this.OccupancyTypeDescription.Equals(other.OccupancyTypeDescription))
                return 0;
            if (!this.TitleTypeDescription.Equals(other.TitleTypeDescription))
                return 0;
            if (!this.PropertyTypeDescription.Equals(other.PropertyTypeDescription))
                return 0;

            if (!this.PropertyDescription1.Equals(other.PropertyDescription1))
                return 0;
            if (!this.PropertyDescription2.Equals(other.PropertyDescription2))
                return 0;
            if (!this.PropertyDescription3.Equals(other.PropertyDescription3))
                return 0;

            if (this.TitleDeedNumber == null && other.TitleDeedNumber != null)
                return 0;

            if (this.TitleDeedNumber != null && other.TitleDeedNumber != null)
            {
                if (!this.TitleDeedNumber.Equals(other.TitleDeedNumber))
                    return 0;
            }

            if (this.ErfNumber != null && other.ErfNumber != null)
            {
                if (!this.ErfNumber.Equals(other.ErfNumber))
                    return 0;
            }

            if (this.ErfPortionNumber == null && other.ErfPortionNumber != null)
                return 0;
            if (this.ErfPortionNumber != null && other.ErfPortionNumber != null)
            {
                if (!this.ErfPortionNumber.Equals(other.ErfPortionNumber))
                    return 0;
            }

            if (this.ErfSuburbDescription == null && other.ErfSuburbDescription != null)
                return 0;
            if (this.ErfSuburbDescription != null && other.ErfSuburbDescription != null)
            {
                if (!this.ErfSuburbDescription.Equals(other.ErfSuburbDescription))
                    return 0;
            }

            if (this.ErfMetroDescription == null && other.ErfMetroDescription != null)
                return 0;
            if (this.ErfMetroDescription != null && other.ErfMetroDescription != null)
            {
                if (!this.ErfMetroDescription.Equals(other.ErfMetroDescription))
                    return 0;
            }

            if (this.SectionalSchemeName == null && other.SectionalSchemeName != null)
                return 0;
            if (this.SectionalSchemeName != null && other.SectionalSchemeName != null)
            {
                if (!this.SectionalSchemeName.Equals(other.SectionalSchemeName))
                    return 0;
            }

            if (this.SectionalUnitNumber == null && other.SectionalUnitNumber != null)
                return 0;
            if (this.SectionalUnitNumber != null && other.SectionalUnitNumber != null)
            {
                if (!this.SectionalUnitNumber.Equals(other.SectionalUnitNumber))
                    return 0;
            }
            return 1;
        }
    }
}