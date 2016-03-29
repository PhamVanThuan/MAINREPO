using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ACBBank", Schema = "dbo")]
    public partial class ACBBank_DAO : DB_2AM<ACBBank_DAO>
    {
        private string _aCBBankDescription;

        //private IList<ACBBranch_DAO> _aCBBranches;

        private int _key;

        /// <summary>
        /// Contains the description of the bank i.e. Nedbank, ABSA etc
        /// </summary>
        [Property("ACBBankDescription", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ACB Bank Description is a mandatory field")]
        public virtual string ACBBankDescription
        {
            get
            {
                return this._aCBBankDescription;
            }
            set
            {
                this._aCBBankDescription = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ACBBankCode", ColumnType = "Int32")]
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

        // this shouldn't be here - there are too many branches on some banks to retrieve timeously so rather
        // use a repository method and perform ajax like searches
        //[HasMany(typeof(ACBBranch_DAO), ColumnKey = "ACBBankCode", Table = "ACBBranch", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        //public virtual IList<ACBBranch_DAO> ACBBranches
        //{
        //    get
        //    {
        //        return this._aCBBranches;
        //    }
        //    set
        //    {
        //        this._aCBBranches = value;
        //    }
        //}
    }
}