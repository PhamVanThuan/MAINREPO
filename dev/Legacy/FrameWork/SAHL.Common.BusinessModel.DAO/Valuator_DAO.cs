using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Valuator_DAO describes a Valuator who carries out the property valuations.
    /// </summary>
    /// <seealso cref="LegalEntity_DAO"/>
    [ActiveRecord("Valuator", Schema = "dbo", Lazy = true)]
    public partial class Valuator_DAO : DB_2AM<Valuator_DAO>
    {
        private string _valuatorContact;

        private string _valuatorPassword;

        private byte _limitedUserGroup;

        private int _Key;

        // TODO: implement hasandbelongstomany relationship to address.
        //private IList<ValuatorAddress> _valuatorAddresses;

        private LegalEntity_DAO _legalEntity;

        private GeneralStatus_DAO _generalStatus;

        private IList<OriginationSource_DAO> _originationSources;

        /// <summary>
        /// Contact Person at the Valuator
        /// </summary>
        [Property("ValuatorContact", ColumnType = "String")]
        public virtual string ValuatorContact
        {
            get
            {
                return this._valuatorContact;
            }
            set
            {
                this._valuatorContact = value;
            }
        }

        [Property("ValuatorPassword", ColumnType = "String")]
        public virtual string ValuatorPassword
        {
            get
            {
                return this._valuatorPassword;
            }
            set
            {
                this._valuatorPassword = value;
            }
        }

        [Property("LimitedUserGroup", ColumnType = "Byte")]
        public virtual byte LimitedUserGroup
        {
            get
            {
                return this._limitedUserGroup;
            }
            set
            {
                this._limitedUserGroup = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ValuatorKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        //[HasMany(typeof(Address_DAO), ColumnKey = "ValuatorKey", Table = "ValuatorAddress")]
        //public virtual IList<Address_DAO> ValuatorAddresses
        //{
        //    get
        //    {
        //        return this._valuatorAddresses;
        //    }
        //    set
        //    {
        //        this._valuatorAddresses = value;
        //    }
        //}
        /// <summary>
        /// The status of the Valuator e.g. Active or Inactive.
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// Foreign Key reference to the Legal Entity table. Each Valuator exists as a Legal Entity on the database.
        /// </summary>
        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [HasAndBelongsToMany(typeof(OriginationSource_DAO), Table = "OriginationSourceValuator", ColumnKey = "ValuatorKey", ColumnRef = "OriginationSourceKey", Lazy = true)]
        public virtual IList<OriginationSource_DAO> OriginationSources
        {
            get
            {
                return _originationSources;
            }
            set
            {
                _originationSources = value;
            }
        }
    }
}