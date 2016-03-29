using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The Control Group DAO Object specifies what type of control is being used.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ControlGroup", Schema = "dbo")]
    public partial class ControlGroup_DAO : DB_2AM<ControlGroup_DAO>
    {
        #region variables

        private string _description;
        private int _key;

        #endregion variables

        #region properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description of the control being used.
        /// </value>
        [Property("Description", ColumnType = "String")]
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
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key is the Primary index column of the table.
        /// This is a managed key.
        /// </value>
        [PrimaryKey(PrimaryKeyType.Assigned, "ControlGroupKey", ColumnType = "Int32")]
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

        #endregion properties
    }
}