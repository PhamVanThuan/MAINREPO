using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("HelpDeskCategory", Schema = "dbo")]
    public partial class HelpDeskCategory_DAO : DB_2AM<HelpDeskCategory_DAO>
    {
        private string _description;

        private int _key;

        //private IList<HelpDeskQuery_DAO> _helpDeskQueries;

        private GeneralStatus_DAO _generalStatus;

        [Property("Description", ColumnType = "String", NotNull = true, Length = 50)]
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

        [PrimaryKey(PrimaryKeyType.Native, "HelpDeskCategoryKey", ColumnType = "Int32")]
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }
    }
}