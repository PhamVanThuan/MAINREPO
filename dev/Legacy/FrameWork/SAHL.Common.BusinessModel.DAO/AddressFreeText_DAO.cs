using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Free Text format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "5", Lazy = true)]
    public class AddressFreeText_DAO : Address_DAO
    {
        private string _freeText1;

        private string _freeText2;

        private string _freeText3;

        private string _freeText4;

        private string _freeText5;

        PostOffice_DAO _postOffice;

        /// <summary>
        /// Free Text Line 1
        /// </summary>
        [Property("FreeText1", ColumnType = "String")]
        public virtual string FreeText1
        {
            get
            {
                return this._freeText1;
            }
            set
            {
                this._freeText1 = value;
            }
        }

        /// <summary>
        /// Free Text Line 2
        /// </summary>
        [Property("FreeText2", ColumnType = "String")]
        public virtual string FreeText2
        {
            get
            {
                return this._freeText2;
            }
            set
            {
                this._freeText2 = value;
            }
        }

        /// <summary>
        /// Free Text Line 3
        /// </summary>
        [Property("FreeText3", ColumnType = "String")]
        public virtual string FreeText3
        {
            get
            {
                return this._freeText3;
            }
            set
            {
                this._freeText3 = value;
            }
        }

        /// <summary>
        /// Free Text Line 4
        /// </summary>
        [Property("FreeText4", ColumnType = "String")]
        public virtual string FreeText4
        {
            get
            {
                return this._freeText4;
            }
            set
            {
                this._freeText4 = value;
            }
        }

        /// <summary>
        /// Free Text Line 5
        /// </summary>
        [Property("FreeText5", ColumnType = "String")]
        public virtual string FreeText5
        {
            get
            {
                return this._freeText5;
            }
            set
            {
                this._freeText5 = value;
            }
        }

        /// <summary>
        /// The Post Office which the Address belongs to.
        /// </summary>
        [BelongsTo("PostOfficeKey", NotNull = false)]
        public virtual PostOffice_DAO PostOffice
        {
            get
            {
                return this._postOffice;
            }
            set
            {
                this._postOffice = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressFreeText_DAO Find(int id)
        {
            return ActiveRecordBase<AddressFreeText_DAO>.Find(id).As<AddressFreeText_DAO>();
        }

        public new static AddressFreeText_DAO Find(object id)
        {
            return ActiveRecordBase<AddressFreeText_DAO>.Find(id).As<AddressFreeText_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressFreeText_DAO FindFirst()
        {
            return ActiveRecordBase<AddressFreeText_DAO>.FindFirst().As<AddressFreeText_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressFreeText_DAO FindOne()
        {
            return ActiveRecordBase<AddressFreeText_DAO>.FindOne().As<AddressFreeText_DAO>();
        }

        #endregion Static Overrides
    }
}