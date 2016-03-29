using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Category_DAO
    /// </summary>
    public partial class Category : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Category_DAO>, ICategory
    {
        public Category(SAHL.Common.BusinessModel.DAO.Category_DAO Category)
            : base(Category)
        {
            this._DAO = Category;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Category_DAO.Value
        /// </summary>
        public Double Value
        {
            get { return _DAO.Value; }
            set { _DAO.Value = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Category_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Category_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}