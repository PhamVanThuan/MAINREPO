using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityCompany_DAO is derived from the LegalEntity_DAO class and is used to instantiate a Legal Entity of type Company.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord(DiscriminatorValue = "3", Lazy = true)]
    public class LegalEntityCompany_DAO : LegalEntity_DAO
    {
        private string _registrationNumber;

        private string _registeredName;

        private string _tradingName;

        /// <summary>
        /// The Company Registration Number
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
        /// The Registered Name of the Company.
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
        /// The name under which the Company trades, this is not always the same as the Registered Name.
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
        /// Gets a list of all companies with a registration number starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<LegalEntityCompany_DAO> FindByRegistrationNumber(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(LegalEntityCompany_DAO), @"
                        from LegalEntityCompany_DAO le
                        where le.RegistrationNumber LIKE ?
                        ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            return new List<LegalEntityCompany_DAO>((LegalEntityCompany_DAO[])ExecuteQuery(q));
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCompany_DAO Find(int id)
        {
            return ActiveRecordBase<LegalEntityCompany_DAO>.Find(id).As<LegalEntityCompany_DAO>();
        }

        public static LegalEntityCompany_DAO Find(object id)
        {
            return ActiveRecordBase<LegalEntityCompany_DAO>.Find(id).As<LegalEntityCompany_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCompany_DAO FindFirst()
        {
            return ActiveRecordBase<LegalEntityCompany_DAO>.FindFirst().As<LegalEntityCompany_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static LegalEntityCompany_DAO FindOne()
        {
            return ActiveRecordBase<LegalEntityCompany_DAO>.FindOne().As<LegalEntityCompany_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static new LegalEntityCompany_DAO[] FindAllByProperty(string property, object value)
        {
            return ActiveRecordBase<LegalEntityCompany_DAO>.FindAllByProperty(property, value);
        }

        #endregion Static Overrides
    }
}