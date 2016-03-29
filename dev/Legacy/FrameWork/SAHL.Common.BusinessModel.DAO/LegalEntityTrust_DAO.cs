using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityTrust_DAO is derived from the LegalEntity_DAO class. It is used to instantiate a Legal Entity of type "Trust".
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "5", Lazy = true)]
    public class LegalEntityTrust_DAO : LegalEntity_DAO
    {
        private string _registrationNumber;

        private string _registeredName;

        private string _tradingName;

        /// <summary>
        /// The Registration Number of the Trust
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
        /// The Registered Name of the Trust.
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
        /// The Trading Name of the Trust.
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
        /// Gets a list of all trusts with a registration number starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<LegalEntityTrust_DAO> FindByRegistrationNumber(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(LegalEntityTrust_DAO), @"
                        from LegalEntityTrust_DAO le
                        where le.RegistrationNumber LIKE ?
                        ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            return new List<LegalEntityTrust_DAO>((LegalEntityTrust_DAO[])ExecuteQuery(q));
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityTrust_DAO Find(int id)
        {
            return ActiveRecordBase<LegalEntityTrust_DAO>.Find(id).As<LegalEntityTrust_DAO>();
        }

        public new static LegalEntityTrust_DAO Find(object id)
        {
            return ActiveRecordBase<LegalEntityTrust_DAO>.Find(id).As<LegalEntityTrust_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityTrust_DAO FindFirst()
        {
            return ActiveRecordBase<LegalEntityTrust_DAO>.FindFirst().As<LegalEntityTrust_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityTrust_DAO FindOne()
        {
            return ActiveRecordBase<LegalEntityTrust_DAO>.FindOne().As<LegalEntityTrust_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static new LegalEntityTrust_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<LegalEntityTrust_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}