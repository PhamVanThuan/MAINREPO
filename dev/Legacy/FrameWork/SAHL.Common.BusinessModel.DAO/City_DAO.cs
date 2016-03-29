using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("City", Schema = "dbo", Lazy = true)]
    public partial class City_DAO : DB_2AM<City_DAO>
    {
        private string _description;

        private int _key;

        private IList<PostOffice_DAO> _postOffices;

        private IList<Suburb_DAO> _suburbs;

        private Province_DAO _province;

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

        [PrimaryKey(PrimaryKeyType.Native, "CityKey", ColumnType = "Int32")]
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

        [HasMany(typeof(PostOffice_DAO), ColumnKey = "CityKey", Table = "PostOffice", Lazy = true)]
        public virtual IList<PostOffice_DAO> PostOffices
        {
            get
            {
                return this._postOffices;
            }
            set
            {
                this._postOffices = value;
            }
        }

        [HasMany(typeof(Suburb_DAO), ColumnKey = "CityKey", Table = "Suburb", Lazy = true)]
        public virtual IList<Suburb_DAO> Suburbs
        {
            get
            {
                return this._suburbs;
            }
            set
            {
                this._suburbs = value;
            }
        }

        [BelongsTo("ProvinceKey", NotNull = true)]
        [ValidateNonEmpty("Province is a mandatory field")]
        public virtual Province_DAO Province
        {
            get
            {
                return this._province;
            }
            set
            {
                this._province = value;
            }
        }
    }
}