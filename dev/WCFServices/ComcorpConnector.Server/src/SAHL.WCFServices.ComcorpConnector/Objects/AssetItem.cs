using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class AssetItem
    {
        [DataMember]
        public decimal AssetOutstandingLiability { get; set; }

        [DataMember]
        public DateTime DateAssetAcquired { get; set; }

        [DataMember]
        public string SAHLAssetDesc { get; set; }

        [DataMember]
        public decimal SAHLAssetValue { get; set; }
        
        [DataMember]
        public string AssetCompanyName { get; set; }

        [DataMember]
        public string AssetDescription { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address City cannot be longer than 50 characters.")]
        public string AssetPhysicalAddressCity { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Code cannot be longer than 50 characters.")]
        public string AssetPhysicalAddressCode { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Line 1 cannot be longer than 50 characters.")]
        public string AssetPhysicalAddressLine1 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Suburb cannot be longer than 50 characters.")]
        public string AssetPhysicalAddressSuburb { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Line 2 cannot be longer than 50 characters.")]
        public string AssetPhysicalAddressLine2 { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Country cannot be longer than 50 characters.")]
        public string AssetPhysicalCountry { get; set; }

        [DataMember]
        [MaxLength(50, ErrorMessage = "Asset Address Province cannot be longer than 50 characters.")]
        public string AssetPhysicalProvince { get; set; }

    }
}