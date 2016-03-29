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
    public partial class StageActivity : IEntityValidation, IStageActivity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.StageActivity_DAO _StageActivity;

        public StageActivity(SAHL.Common.X2.BusinessModel.DAO.StageActivity_DAO StageActivity)
        {
            this._StageActivity = StageActivity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="StageActivity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _StageActivity;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 StageDefinitionKey
        {
            get { return _StageActivity.StageDefinitionKey; }
            set { _StageActivity.StageDefinitionKey = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _StageActivity.ID; }
            set { _StageActivity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivity Activity
        {
            get
            {
                if (null == _StageActivity.Activity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivity, Activity_DAO>(_StageActivity.Activity);
                }
            }

            set
            {
                if (value == null)
                {
                    _StageActivity.Activity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _StageActivity.Activity = (Activity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}