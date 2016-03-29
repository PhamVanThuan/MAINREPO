using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// OfferCondition_DAO is instantiated in order to find the Conditions which are associated to a particular Application.
    /// </summary>
    public partial class ApplicationCondition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO>, IApplicationCondition
    {
        public ApplicationCondition(SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO ApplicationCondition)
            : base(ApplicationCondition)
        {
            this._DAO = ApplicationCondition;
        }

        /// <summary>
        /// The Condition which is associated to the Application.
        /// </summary>
        public ICondition Condition
        {
            get
            {
                if (null == _DAO.Condition) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICondition, Condition_DAO>(_DAO.Condition);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Condition = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Condition = (Condition_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Each Condition is related to a TranslatableItem, from which the translated version of the condition can be retrieved.
        /// </summary>
        public ITranslatableItem TranslatableItem
        {
            get
            {
                if (null == _DAO.TranslatableItem) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ITranslatableItem, TranslatableItem_DAO>(_DAO.TranslatableItem);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.TranslatableItem = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.TranslatableItem = (TranslatableItem_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO.ApplicationConditionTokens
        /// </summary>
        private DAOEventList<ApplicationConditionToken_DAO, IApplicationConditionToken, ApplicationConditionToken> _ApplicationConditionTokens;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO.ApplicationConditionTokens
        /// </summary>
        public IEventList<IApplicationConditionToken> ApplicationConditionTokens
        {
            get
            {
                if (null == _ApplicationConditionTokens)
                {
                    if (null == _DAO.ApplicationConditionTokens)
                        _DAO.ApplicationConditionTokens = new List<ApplicationConditionToken_DAO>();
                    _ApplicationConditionTokens = new DAOEventList<ApplicationConditionToken_DAO, IApplicationConditionToken, ApplicationConditionToken>(_DAO.ApplicationConditionTokens);
                    _ApplicationConditionTokens.BeforeAdd += new EventListHandler(OnApplicationConditionTokens_BeforeAdd);
                    _ApplicationConditionTokens.BeforeRemove += new EventListHandler(OnApplicationConditionTokens_BeforeRemove);
                    _ApplicationConditionTokens.AfterAdd += new EventListHandler(OnApplicationConditionTokens_AfterAdd);
                    _ApplicationConditionTokens.AfterRemove += new EventListHandler(OnApplicationConditionTokens_AfterRemove);
                }
                return _ApplicationConditionTokens;
            }
        }

        /// <summary>
        /// The Application Number from the Offer table
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationCondition_DAO.ConditionSet
        /// </summary>
        public IConditionSet ConditionSet
        {
            get
            {
                if (null == _DAO.ConditionSet) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IConditionSet, ConditionSet_DAO>(_DAO.ConditionSet);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ConditionSet = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ConditionSet = (ConditionSet_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationConditionTokens = null;
        }
    }
}