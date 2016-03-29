using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicationMailingAddressModel : IDataModel
    {
        public ApplicationMailingAddressModel(AddressModel address, CorrespondenceLanguage correspondenceLanguage, OnlineStatementFormat onlineStatementFormat, 
                                              CorrespondenceMedium correspondenceMedium, int? clientToUseForEmailCorrespondence, bool onlineStatementRequired)
        {
            this.Address                           = address;
            this.CorrespondenceLanguage            = correspondenceLanguage;
            this.OnlineStatementRequired           = onlineStatementRequired;
            this.OnlineStatementFormat             = onlineStatementFormat;
            this.CorrespondenceMedium              = correspondenceMedium;
            this.ClientToUseForEmailCorrespondence = clientToUseForEmailCorrespondence;

            if (!clientToUseForEmailCorrespondence.HasValue)
            {
                this.CorrespondenceMedium = CorrespondenceMedium.Post;
            }
        }

        [DataMember]
        public AddressModel Address { get; set; }

        [DataMember]
        public CorrespondenceLanguage CorrespondenceLanguage { get; set; }

        [DataMember]
        public bool OnlineStatementRequired { get; set; }

        [DataMember]
        public OnlineStatementFormat OnlineStatementFormat { get; set; }

        [DataMember]
        public CorrespondenceMedium CorrespondenceMedium { get; set; }

        [DataMember]
        public int? ClientToUseForEmailCorrespondence { get; set; }

        [DataMember]
        public int ApplicationNumber { get; set; }
    }
}
