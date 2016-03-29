using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// RoleType_DAO contains the different types of Roles that a Legal Entity can play on an Account at SAHL. This would include:
    /// <list type="bullet">
    /// <item>
    /// <description>Main Applicant</description>
    /// </item>
    /// <item>
    /// <description>Suretor</description>
    /// </item>
    /// <item>
    /// <description>Previous Insurer</description>
    /// </item>
    /// <item>
    /// <description>Assured Life</description>
    /// </item>
    /// </list>
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("RoleType", Schema = "dbo", Lazy = true)]
    public partial class RoleType_DAO : DB_2AM<RoleType_DAO>
    {
        private string _description;

        private int _roleTypeKey;

        //private IList<Role_DAO> _roles;
        /// <summary>
        /// The description of the Role Type
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
        [PrimaryKey(PrimaryKeyType.Assigned, "RoleTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._roleTypeKey;
            }
            set
            {
                this._roleTypeKey = value;
            }
        }

        // This is a lookup
        //[HasMany(typeof(Role_DAO), ColumnKey = "RoleTypeKey", Table = "Role", Lazy=true)]
        //public virtual IList<Role_DAO> Roles
        //{
        //    get
        //    {
        //        return this._roles;
        //    }
        //    set
        //    {
        //        this._roles = value;
        //    }
        //}
    }
}