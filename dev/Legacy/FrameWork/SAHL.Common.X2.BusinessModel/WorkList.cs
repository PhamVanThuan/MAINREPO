using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class WorkList : IEntityValidation, IWorkList, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.WorkList_DAO _WorkList;

        public WorkList(SAHL.Common.X2.BusinessModel.DAO.WorkList_DAO WorkList)
        {
            this._WorkList = WorkList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="WorkList_DAO"/></returns>
        public object GetDAOObject()
        {
            return _WorkList;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _WorkList.ID; }
            set { _WorkList.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ADUserName
        {
            get { return _WorkList.ADUserName; }
            set { _WorkList.ADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime ListDate
        {
            get { return _WorkList.ListDate; }
            set { _WorkList.ListDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Message
        {
            get { return _WorkList.Message; }
            set { _WorkList.Message = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IInstance Instance
        {
            get
            {
                if (null == _WorkList.Instance) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IInstance, Instance_DAO>(_WorkList.Instance);
                }
            }

            set
            {
                if (value == null)
                {
                    _WorkList.Instance = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _WorkList.Instance = (Instance_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}