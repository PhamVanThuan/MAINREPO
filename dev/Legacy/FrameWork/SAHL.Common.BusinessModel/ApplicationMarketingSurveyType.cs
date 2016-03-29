using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO
    /// </summary>
    public partial class ApplicationMarketingSurveyType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO>, IApplicationMarketingSurveyType
    {
        public ApplicationMarketingSurveyType(SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO ApplicationMarketingSurveyType)
            : base(ApplicationMarketingSurveyType)
        {
            this._DAO = ApplicationMarketingSurveyType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationMarketingSurveyType_DAO.ApplicationMarketingSurveyTypeGroup
        /// </summary>
        public IApplicationMarketingSurveyTypeGroup ApplicationMarketingSurveyTypeGroup
        {
            get
            {
                if (null == _DAO.ApplicationMarketingSurveyTypeGroup) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationMarketingSurveyTypeGroup, ApplicationMarketingSurveyTypeGroup_DAO>(_DAO.ApplicationMarketingSurveyTypeGroup);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationMarketingSurveyTypeGroup = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationMarketingSurveyTypeGroup = (ApplicationMarketingSurveyTypeGroup_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}