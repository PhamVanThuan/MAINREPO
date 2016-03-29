using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Country", Schema = "dbo", Lazy = true)]
    public partial class Country_DAO : DB_2AM<Country_DAO>
    {
        private string _description;

        private bool _allowFreeTextFormat;

        private int _key;

        private IList<Province_DAO> _provinces;

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

        [Property("AllowFreeTextFormat", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Allow Free Text Format is a mandatory field")]
        public virtual bool AllowFreeTextFormat
        {
            get
            {
                return this._allowFreeTextFormat;
            }
            set
            {
                this._allowFreeTextFormat = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CountryKey", ColumnType = "Int32")]
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

        /// <summary>
        /// A collection of provinces that apply to the country.
        /// </summary>
        [HasMany(typeof(Province_DAO), ColumnKey = "CountryKey", Table = "Province", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Province_DAO> Provinces
        {
            get
            {
                return this._provinces;
            }
            set
            {
                this._provinces = value;
            }
        }
    }
}