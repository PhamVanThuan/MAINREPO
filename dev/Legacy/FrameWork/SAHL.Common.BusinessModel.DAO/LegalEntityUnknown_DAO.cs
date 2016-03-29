using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityUnknown_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Unknown".
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "1", Lazy = true)]
    public class LegalEntityUnknown_DAO : LegalEntity_DAO
    {
        private Salutation_DAO _salutation;

        private string _firstNames;

        private string _surname;

        private string _initials;

        /// <summary>
        /// The foreign key reference to the Salutation table. A Unknown Legal Entity belongs to a single Saluation type.
        /// </summary>
        [Lurker]
        [BelongsTo("Salutationkey")]
        public virtual Salutation_DAO Salutation
        {
            get
            {
                return this._salutation;
            }
            set
            {
                this._salutation = value;
            }
        }

        /// <summary>
        /// The First Names of the Unknown Legal Entity.
        /// </summary>
        [Lurker]
        [Property("FirstNames", ColumnType = "String")]
        public virtual string FirstNames
        {
            get
            {
                return this._firstNames;
            }
            set
            {
                this._firstNames = value;
            }
        }

        /// <summary>
        /// The Initials of the Unknown Legal Entity.
        /// </summary>
        [Lurker]
        [Property("Initials", ColumnType = "String", Length = 5)]
        public virtual string Initials
        {
            get
            {
                return this._initials;
            }
            set
            {
                this._initials = value;
            }
        }

        /// <summary>
        /// The Surname of the Unknown Legal Entity.
        /// </summary>
        [Lurker]
        [Property("Surname", ColumnType = "String")]
        public virtual string Surname
        {
            get
            {
                return this._surname;
            }
            set
            {
                this._surname = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityUnknown_DAO Find(int id)
        {
            return ActiveRecordBase<LegalEntityUnknown_DAO>.Find(id).As<LegalEntityUnknown_DAO>();
        }

        public new static LegalEntityUnknown_DAO Find(object id)
        {
            return ActiveRecordBase<LegalEntityUnknown_DAO>.Find(id).As<LegalEntityUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityUnknown_DAO FindFirst()
        {
            return ActiveRecordBase<LegalEntityUnknown_DAO>.FindFirst().As<LegalEntityUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityUnknown_DAO FindOne()
        {
            return ActiveRecordBase<LegalEntityUnknown_DAO>.FindOne().As<LegalEntityUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static new LegalEntityUnknown_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<LegalEntityUnknown_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}