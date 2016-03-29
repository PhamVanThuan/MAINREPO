using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("CorrespondenceMedium", Schema = "dbo")]
    public partial class CorrespondenceMedium_DAO : DB_2AM<CorrespondenceMedium_DAO>
    {
        private string _description;

        private int _Key;

        // commented, this is a lookup.
        //private IList<Correspondence_DAO> _correspondences;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "CorrespondenceMediumKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(Correspondence_DAO), ColumnKey = "CorrespondenceMediumKey", Table = "Correspondence")]
        //public virtual IList<Correspondence_DAO> Correspondences
        //{
        //    get
        //    {
        //        return this._correspondences;
        //    }
        //    set
        //    {
        //        this._correspondences = value;
        //    }
        //}
    }
}