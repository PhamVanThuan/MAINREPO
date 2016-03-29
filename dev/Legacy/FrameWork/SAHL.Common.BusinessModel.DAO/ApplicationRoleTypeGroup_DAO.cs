using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Groups application role types.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferRoleTypeGroup", Schema = "dbo", Lazy = true)]
    public partial class ApplicationRoleTypeGroup_DAO : DB_2AM<ApplicationRoleTypeGroup_DAO>
    {
        private string _description;

        private int _Key;

        /// <summary>
        /// The description of the Application Role Type Group
        /// </summary>
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "OfferRoleTypeGroupKey", ColumnType = "Int32")]
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
    }
}