using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityExceptionReason_DAO is used to store the reasons why a Legal Entity  failed validation.
    /// </summary>
    [ActiveRecord("LegalEntityExceptionReason", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityExceptionReason_DAO : DB_2AM<LegalEntityExceptionReason_DAO>
    {
        private string _description;

        private byte _priority;

        private int _key;

        private IList<LegalEntity_DAO> _legalEntityExceptionReasons;

        /// <summary>
        /// The description of the exception reason. e.g. Missing Salutation, Missing Surname
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// The priority of the exception reason.
        /// </summary>
        [Property("Priority", ColumnType = "Byte", NotNull = true)]
        [ValidateNonEmpty("Priority is a mandatory field")]
        public virtual byte Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityExceptionReasonKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [HasAndBelongsToMany(typeof(LegalEntity_DAO), ColumnRef = "LegalEntityKey", ColumnKey = "LegalEntityExceptionReasonKey", Schema = "dbo", Lazy = true, Table = "LegalEntityException")]
        public virtual IList<LegalEntity_DAO> LegalEntityExceptionReasons
        {
            get
            {
                return this._legalEntityExceptionReasons;
            }
            set
            {
                this._legalEntityExceptionReasons = value;
            }
        }
    }
}