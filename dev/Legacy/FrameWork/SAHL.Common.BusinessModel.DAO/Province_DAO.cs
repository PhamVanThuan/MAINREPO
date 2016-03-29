using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Province", Schema = "dbo", Lazy = true)]
    public partial class Province_DAO : DB_2AM<Province_DAO>
    {
        private string _description;

        private int _key;

        private IList<City_DAO> _cities;

        private Country_DAO _country;

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

        [PrimaryKey(PrimaryKeyType.Native, "ProvinceKey", ColumnType = "Int32")]
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

        [HasMany(typeof(City_DAO), ColumnKey = "ProvinceKey", Table = "City", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<City_DAO> Cities
        {
            get
            {
                return this._cities;
            }
            set
            {
                this._cities = value;
            }
        }

        [BelongsTo("CountryKey", NotNull = true)]
        [ValidateNonEmpty("Country is a mandatory field")]
        public virtual Country_DAO Country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }
    }
}