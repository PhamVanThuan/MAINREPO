using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CreditMatrix", Schema = "dbo", Lazy = true)]
    public partial class CreditMatrix_DAO : DB_2AM<CreditMatrix_DAO>
    {
        private char _newBusinessIndicator;

        private DateTime? _implementationDate;

        private int _key;

        private IList<CreditCriteria_DAO> _creditCriterias;

        IList<OriginationSourceProduct_DAO> _originationSourceProducts;

        [Property("NewBusinessIndicator", ColumnType = "AnsiChar", NotNull = true)]
        [ValidateNonEmpty("New Business Indicator is a mandatory field")]
        public virtual char NewBusinessIndicator
        {
            get
            {
                return this._newBusinessIndicator;
            }
            set
            {
                this._newBusinessIndicator = value;
            }
        }

        [Property("ImplementationDate")]
        public virtual DateTime? ImplementationDate
        {
            get
            {
                return this._implementationDate;
            }
            set
            {
                this._implementationDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CreditMatrixKey", ColumnType = "Int32")]
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

        [HasMany(typeof(CreditCriteria_DAO), Lazy = true, ColumnKey = "CreditMatrixKey", Table = "CreditCriteria", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CreditCriteria_DAO> CreditCriterias
        {
            get
            {
                return this._creditCriterias;
            }
            set
            {
                this._creditCriterias = value;
            }
        }

        [HasAndBelongsToMany(typeof(OriginationSourceProduct_DAO), ColumnRef = "OriginationSourceProductKey", ColumnKey = "CreditMatrixKey", Lazy = true, Schema = "dbo", Table = "OriginationSourceProductCreditMatrix")]
        public virtual IList<OriginationSourceProduct_DAO> OriginationSourceProducts
        {
            get
            {
                return this._originationSourceProducts;
            }
            set
            {
                this._originationSourceProducts = value;
            }
        }
    }
}