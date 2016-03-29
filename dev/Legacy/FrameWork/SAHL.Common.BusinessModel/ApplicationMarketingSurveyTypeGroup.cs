using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO
    /// </summary>
    public partial class ApplicationMarketingSurveyTypeGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO>, IApplicationMarketingSurveyTypeGroup
    {
        public ApplicationMarketingSurveyTypeGroup(SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO ApplicationMarketingSurveyTypeGroup)
            : base(ApplicationMarketingSurveyTypeGroup)
        {
            this._DAO = ApplicationMarketingSurveyTypeGroup;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO.ApplicationMarketingSurveyTypes
        /// </summary>
        private DAOEventList<ApplicationMarketingSurveyType_DAO, IApplicationMarketingSurveyType, ApplicationMarketingSurveyType> _ApplicationMarketingSurveyTypes;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyTypeGroup_DAO.ApplicationMarketingSurveyTypes
        /// </summary>
        public IEventList<IApplicationMarketingSurveyType> ApplicationMarketingSurveyTypes
        {
            get
            {
                if (null == _ApplicationMarketingSurveyTypes)
                {
                    if (null == _DAO.ApplicationMarketingSurveyTypes)
                        _DAO.ApplicationMarketingSurveyTypes = new List<ApplicationMarketingSurveyType_DAO>();
                    _ApplicationMarketingSurveyTypes = new DAOEventList<ApplicationMarketingSurveyType_DAO, IApplicationMarketingSurveyType, ApplicationMarketingSurveyType>(_DAO.ApplicationMarketingSurveyTypes);
                    _ApplicationMarketingSurveyTypes.BeforeAdd += new EventListHandler(OnApplicationMarketingSurveyTypes_BeforeAdd);
                    _ApplicationMarketingSurveyTypes.BeforeRemove += new EventListHandler(OnApplicationMarketingSurveyTypes_BeforeRemove);
                    _ApplicationMarketingSurveyTypes.AfterAdd += new EventListHandler(OnApplicationMarketingSurveyTypes_AfterAdd);
                    _ApplicationMarketingSurveyTypes.AfterRemove += new EventListHandler(OnApplicationMarketingSurveyTypes_AfterRemove);
                }
                return _ApplicationMarketingSurveyTypes;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationMarketingSurveyTypes = null;
        }
    }
}