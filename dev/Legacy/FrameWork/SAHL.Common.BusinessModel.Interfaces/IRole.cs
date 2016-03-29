using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Role_DAO is used in order to link an SAHL Account to the Legal Entities which play a role in the Account. This relationship
    /// is defined in the AccountRole table.
    /// </summary>
    public partial interface IRole : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the status of the Role was changed.
        /// </summary>
        System.DateTime StatusChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the Account table. Each AccountRole that is defined belongs to an AccountKey.
        /// </summary>
        IAccount Account
        {
            get;
            set;
        }

        /// <summary>
        /// The foreign key reference to the LegalEntity table. Each AccountRole that is defined belongs to an LegalEntityKey.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }
    }
}