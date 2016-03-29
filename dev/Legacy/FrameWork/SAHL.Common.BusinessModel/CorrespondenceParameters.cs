using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO
    /// </summary>
    public partial class CorrespondenceParameters : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO>, ICorrespondenceParameters
    {
        public CorrespondenceParameters(SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO CorrespondenceParameters)
            : base(CorrespondenceParameters)
        {
            this._DAO = CorrespondenceParameters;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.Correspondence
        /// </summary>
        public ICorrespondence Correspondence
        {
            get
            {
                if (null == _DAO.Correspondence) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICorrespondence, Correspondence_DAO>(_DAO.Correspondence);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Correspondence = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Correspondence = (Correspondence_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.ReportParameterValue
        /// </summary>
        public String ReportParameterValue
        {
            get { return _DAO.ReportParameterValue; }
            set { _DAO.ReportParameterValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceParameters_DAO.ReportParameter
        /// </summary>
        public IReportParameter ReportParameter
        {
            get
            {
                if (null == _DAO.ReportParameter) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IReportParameter, ReportParameter_DAO>(_DAO.ReportParameter);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ReportParameter = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ReportParameter = (ReportParameter_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}