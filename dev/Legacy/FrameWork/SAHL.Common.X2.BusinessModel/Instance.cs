using System;
using System.Collections.Generic;
using SAHL.Common.Collections;
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
    public partial class Instance : IEntityValidation, IInstance, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.Instance_DAO _Instance;

        public Instance(SAHL.Common.X2.BusinessModel.DAO.Instance_DAO Instance)
        {
            this._Instance = Instance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="Instance_DAO"/></returns>
        public object GetDAOObject()
        {
            return _Instance;
        }

        /// <summary>
        ///
        /// </summary>
        public String Name
        {
            get { return _Instance.Name; }
            set { _Instance.Name = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String Subject
        {
            get { return _Instance.Subject; }
            set { _Instance.Subject = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String WorkFlowProvider
        {
            get { return _Instance.WorkFlowProvider; }
            set { _Instance.WorkFlowProvider = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String CreatorADUserName
        {
            get { return _Instance.CreatorADUserName; }
            set { _Instance.CreatorADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime CreationDate
        {
            get { return _Instance.CreationDate; }
            set { _Instance.CreationDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? StateChangeDate
        {
            get { return _Instance.StateChangeDate; }
            set { _Instance.StateChangeDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? DeadlineDate
        {
            get { return _Instance.DeadlineDate; }
            set { _Instance.DeadlineDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? ActivityDate
        {
            get { return _Instance.ActivityDate; }
            set { _Instance.ActivityDate = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ActivityADUserName
        {
            get { return _Instance.ActivityADUserName; }
            set { _Instance.ActivityADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? ActivityID
        {
            get { return _Instance.ActivityID; }
            set { _Instance.ActivityID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? Priority
        {
            get { return _Instance.Priority; }
            set { _Instance.Priority = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int64 ID
        {
            get { return _Instance.ID; }
            set { _Instance.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<InstanceActivitySecurity_DAO, IInstanceActivitySecurity, InstanceActivitySecurity> _InstanceActivitySecurities;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IInstanceActivitySecurity> InstanceActivitySecurities
        {
            get
            {
                if (null == _InstanceActivitySecurities)
                {
                    if (null == _Instance.InstanceActivitySecurities)
                        _Instance.InstanceActivitySecurities = new List<InstanceActivitySecurity_DAO>();
                    _InstanceActivitySecurities = new DAOEventList<InstanceActivitySecurity_DAO, IInstanceActivitySecurity, InstanceActivitySecurity>(_Instance.InstanceActivitySecurities);
                    _InstanceActivitySecurities.BeforeAdd += new EventListHandler(OnInstanceActivitySecurities_BeforeAdd);
                    _InstanceActivitySecurities.BeforeRemove += new EventListHandler(OnInstanceActivitySecurities_BeforeRemove);
                }
                return _InstanceActivitySecurities;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private DAOEventList<WorkList_DAO, IWorkList, WorkList> _WorkLists;

        /// <summary>
        ///
        /// </summary>
        public IEventList<IWorkList> WorkLists
        {
            get
            {
                if (null == _WorkLists)
                {
                    if (null == _Instance.WorkLists)
                        _Instance.WorkLists = new List<WorkList_DAO>();
                    _WorkLists = new DAOEventList<WorkList_DAO, IWorkList, WorkList>(_Instance.WorkLists);
                    _WorkLists.BeforeAdd += new EventListHandler(OnWorkLists_BeforeAdd);
                    _WorkLists.BeforeRemove += new EventListHandler(OnWorkLists_BeforeRemove);
                }
                return _WorkLists;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IInstance ParentInstance
        {
            get
            {
                if (null == _Instance.ParentInstance) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IInstance, Instance_DAO>(_Instance.ParentInstance);
                }
            }

            set
            {
                if (value == null)
                {
                    _Instance.ParentInstance = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Instance.ParentInstance = (Instance_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IState State
        {
            get
            {
                if (null == _Instance.State) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IState, State_DAO>(_Instance.State);
                }
            }

            set
            {
                if (value == null)
                {
                    _Instance.State = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Instance.State = (State_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IWorkFlow WorkFlow
        {
            get
            {
                if (null == _Instance.WorkFlow) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IWorkFlow, WorkFlow_DAO>(_Instance.WorkFlow);
                }
            }

            set
            {
                if (value == null)
                {
                    _Instance.WorkFlow = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _Instance.WorkFlow = (WorkFlow_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Int64? SourceInstanceID
        {
            get { return _Instance.SourceInstanceID; }
            set { _Instance.SourceInstanceID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32? ReturnActivityID
        {
            get { return _Instance.ReturnActivityID; }
            set { _Instance.ReturnActivityID = value; }
        }
    }
}