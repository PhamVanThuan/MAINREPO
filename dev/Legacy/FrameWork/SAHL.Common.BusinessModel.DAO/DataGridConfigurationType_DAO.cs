using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("DataGridConfigurationType", Schema = "dbo", Lazy = true)]
    public class DataGridConfigurationType_DAO : DB_2AM<DataGridConfigurationType_DAO>
    {
        private int _key;

        private string _description;

        private IList<DataGridConfiguration_DAO> _dataGridConfigurations;

        [PrimaryKey(PrimaryKeyType.Native, "DataGridConfigurationTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(DataGridConfiguration_DAO), ColumnKey = "DataGridConfigurationTypeKey", Table = "DataGridConfiguration", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<DataGridConfiguration_DAO> DataGridConfigurations
        {
            get
            {
                return this._dataGridConfigurations;
            }
            set
            {
                this._dataGridConfigurations = value;
            }
        }
    }
}