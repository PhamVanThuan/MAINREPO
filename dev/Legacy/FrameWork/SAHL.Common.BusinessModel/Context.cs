using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Context_DAO
    /// </summary>
    public partial class Context : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Context_DAO>, IContext
    {
        public Context(SAHL.Common.BusinessModel.DAO.Context_DAO Context)
            : base(Context)
        {
            this._DAO = Context;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Context_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Context_DAO.TableName
        /// </summary>
        public String TableName
        {
            get { return _DAO.TableName; }
            set { _DAO.TableName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Context_DAO.Alias
        /// </summary>
        public String Alias
        {
            get { return _DAO.Alias; }
            set { _DAO.Alias = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Context_DAO.PrimaryKeyColumn
        /// </summary>
        public String PrimaryKeyColumn
        {
            get { return _DAO.PrimaryKeyColumn; }
            set { _DAO.PrimaryKeyColumn = value; }
        }
    }
}