using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ContactDetailDataModel :  IDataModel
    {
        public ContactDetailDataModel(Guid contactDetailTypeEnumId)
        {
            this.ContactDetailTypeEnumId = contactDetailTypeEnumId;
		
        }

        public ContactDetailDataModel(Guid id, Guid contactDetailTypeEnumId)
        {
            this.Id = id;
            this.ContactDetailTypeEnumId = contactDetailTypeEnumId;
		
        }		

        public Guid Id { get; set; }

        public Guid ContactDetailTypeEnumId { get; set; }
    }
}