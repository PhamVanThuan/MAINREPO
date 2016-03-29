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
    public partial class ActiveExternalActivity : IEntityValidation, IActiveExternalActivity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.ActiveExternalActivity_DAO _ActiveExternalActivity;

        public ActiveExternalActivity(SAHL.Common.X2.BusinessModel.DAO.ActiveExternalActivity_DAO ActiveExternalActivity)
        {
            this._ActiveExternalActivity = ActiveExternalActivity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="ActiveExternalActivity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _ActiveExternalActivity;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 WorkFlowID
        {
            get { return _ActiveExternalActivity.WorkFlowID; }
            set { _ActiveExternalActivity.WorkFlowID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int64? ActivatingInstanceID
        {
            get { return _ActiveExternalActivity.ActivatingInstanceID; }
            set { _ActiveExternalActivity.ActivatingInstanceID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime ActivationTime
        {
            get { return _ActiveExternalActivity.ActivationTime; }
            set { _ActiveExternalActivity.ActivationTime = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ActivityXMLData
        {
            get { return _ActiveExternalActivity.ActivityXMLData; }
            set { _ActiveExternalActivity.ActivityXMLData = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String WorkFlowProviderName
        {
            get { return _ActiveExternalActivity.WorkFlowProviderName; }
            set { _ActiveExternalActivity.WorkFlowProviderName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _ActiveExternalActivity.ID; }
            set { _ActiveExternalActivity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IExternalActivity ExternalActivity
        {
            get
            {
                if (null == _ActiveExternalActivity.ExternalActivity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExternalActivity, ExternalActivity_DAO>(_ActiveExternalActivity.ExternalActivity);
                }
            }

            set
            {
                if (value == null)
                {
                    _ActiveExternalActivity.ExternalActivity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _ActiveExternalActivity.ExternalActivity = (ExternalActivity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}