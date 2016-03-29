using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantAssetLiabilityModel : IDataModel
    {
        public ApplicantAssetLiabilityModel(AssetLiabilityType assetLiabilityType, AssetLiabilitySubType? assetLiabilitySubType, AddressModel address, 
                                            double? assetValue, double? liabilityValue, string companyName, double? cost, DateTime? date, string description)
        {
            this.AssetLiabilityType    = assetLiabilityType;
            this.AssetLiabilitySubType = assetLiabilitySubType;
            this.Address               = address;
            this.AssetValue            = assetValue;
            this.LiabilityValue        = liabilityValue;
            this.CompanyName           = companyName;
            this.Cost                  = cost;
            this.Date                  = date;
            this.Description           = description;

        }

        [DataMember]
        public AssetLiabilityType AssetLiabilityType { get; set; }

        [DataMember]
        public AssetLiabilitySubType? AssetLiabilitySubType { get; set; }

        [DataMember]
        public AddressModel Address { get; set; }

        [DataMember]
        public double? AssetValue { get; set; }

        [DataMember]
        public double? LiabilityValue { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public double? Cost { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
