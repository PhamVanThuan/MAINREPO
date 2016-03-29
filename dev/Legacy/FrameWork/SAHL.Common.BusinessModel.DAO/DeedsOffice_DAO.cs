using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DeedsOffice", Schema = "dbo")]
    public partial class DeedsOffice_DAO : DB_2AM<DeedsOffice_DAO>
    {
        private string _description;

        private int _Key;

        private IList<Attorney_DAO> _attorneys;

        //private IList<Bond_DAO> _bonds;

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

        [PrimaryKey(PrimaryKeyType.Native, "DeedsOfficeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(Attorney_DAO), ColumnKey = "DeedsOfficeKey", Table = "Attorney")]
        public virtual IList<Attorney_DAO> Attorneys
        {
            get
            {
                return this._attorneys;
            }
            set
            {
                this._attorneys = value;
            }
        }

        // commented, this is a lookup.
        //[HasMany(typeof(Bond_DAO), ColumnKey = "DeedsOfficeKey", Table = "Bond")]
        //public virtual IList<Bond_DAO> Bonds
        //{
        //    get
        //    {
        //        return this._bonds;
        //    }
        //    set
        //    {
        //        this._bonds = value;
        //    }
        //}
    }
}