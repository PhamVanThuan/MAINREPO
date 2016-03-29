using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HOCInsurer", Schema = "dbo")]
    public partial class HOCInsurer_DAO : DB_2AM<HOCInsurer_DAO>
    {
        private string _description;

        private short? _hOCInsurerStatus;

        private int _key;

        private IList<HOCRates_DAO> _hOCRates;

        // commented, this is a lookup.
        // private IList<HOC> _hOCs;

        // commented, this is a lookup.
        //private IList<HOCHistory> _hOCHistories;

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

        [Property("HOCInsurerStatus", ColumnType = "Int16")]
        public virtual short? HOCInsurerStatus
        {
            get
            {
                return this._hOCInsurerStatus;
            }
            set
            {
                this._hOCInsurerStatus = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "HOCInsurerKey", ColumnType = "Int32")]
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

        [HasMany(typeof(HOCRates_DAO), ColumnKey = "HOCInsurerKey", Table = "HOCRates", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<HOCRates_DAO> HOCRates
        {
            get
            {
                return _hOCRates;
            }
            set
            {
                _hOCRates = value;
            }
        }

        // commented, this is a lookup.
        //[HasMany(typeof(HOC), ColumnKey = "HOCInsurerKey", Table = "HOC")]
        //public virtual IList<HOC> HOCs
        //{
        //    get
        //    {
        //        return this._hOCs;
        //    }
        //    set
        //    {
        //        this._hOCs = value;
        //    }
        //}

        //[HasMany(typeof(HOCHistory), ColumnKey = "HOCInsurerKey", Table = "HOCHistory")]
        //public virtual IList<HOCHistory> HOCHistories
        //{
        //    get
        //    {
        //        return this._hOCHistories;
        //    }
        //    set
        //    {
        //        this._hOCHistories = value;
        //    }
        //}
    }
}