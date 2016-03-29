using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityCloseCorporation_DAO is derived from LegalEntity_DAO and is used to instantiate a Legal Entity of type Close
    /// Corporation.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "4", Lazy = true)]
    public class LegalEntityCloseCorporation_DAO : LegalEntity_DAO
    {
        private string _registrationNumber;

        private string _registeredName;

        private string _tradingName;

        /// <summary>
        /// The Registration number of the Close Corporation.
        /// </summary>
        [Lurker]
        [Property("RegistrationNumber", ColumnType = "String")]
        public virtual string RegistrationNumber
        {
            get
            {
                return this._registrationNumber;
            }
            set
            {
                this._registrationNumber = value;
            }
        }

        /// <summary>
        /// The Registered Name of the Close Corporation.
        /// </summary>
        [Lurker]
        [Property("RegisteredName", ColumnType = "String")]
        public virtual string RegisteredName
        {
            get
            {
                return this._registeredName;
            }
            set
            {
                this._registeredName = value;
            }
        }

        /// <summary>
        /// The name under which the Close Corporation trades, this is not always the same as the Registered Name.
        /// </summary>
        [Property("TradingName", ColumnType = "String", Unique = true)]
        public virtual string TradingName
        {
            get
            {
                return this._tradingName;
            }
            set
            {
                this._tradingName = value;
            }
        }

        /// <summary>
        /// Gets a list of all closed corporations with a registration number starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<LegalEntityCloseCorporation_DAO> FindByRegistrationNumber(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(LegalEntityCloseCorporation_DAO), @"
                        from LegalEntityCloseCorporation_DAO le
                        where le.RegistrationNumber LIKE ?
                        ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            return new List<LegalEntityCloseCorporation_DAO>((LegalEntityCloseCorporation_DAO[])ExecuteQuery(q));
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCloseCorporation_DAO Find(int id)
        {
            return ActiveRecordBase<LegalEntityCloseCorporation_DAO>.Find(id).As<LegalEntityCloseCorporation_DAO>();
        }

        public new static LegalEntityCloseCorporation_DAO Find(object id)
        {
            return ActiveRecordBase<LegalEntityCloseCorporation_DAO>.Find(id).As<LegalEntityCloseCorporation_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCloseCorporation_DAO FindFirst()
        {
            return ActiveRecordBase<LegalEntityCloseCorporation_DAO>.FindFirst().As<LegalEntityCloseCorporation_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCloseCorporation_DAO FindOne()
        {
            return ActiveRecordBase<LegalEntityCloseCorporation_DAO>.FindOne().As<LegalEntityCloseCorporation_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static new LegalEntityCloseCorporation_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<LegalEntityCloseCorporation_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}