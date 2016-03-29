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
    /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO
    /// </summary>
    public partial class Correspondence : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Correspondence_DAO>, ICorrespondence
    {
        public Correspondence(SAHL.Common.BusinessModel.DAO.Correspondence_DAO Correspondence)
            : base(Correspondence)
        {
            this._DAO = Correspondence;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.DestinationValue
        /// </summary>
        public String DestinationValue
        {
            get { return _DAO.DestinationValue; }
            set { _DAO.DestinationValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.DueDate
        /// </summary>
        public DateTime? DueDate
        {
            get { return _DAO.DueDate; }
            set { _DAO.DueDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CompletedDate
        /// </summary>
        public DateTime? CompletedDate
        {
            get { return _DAO.CompletedDate; }
            set { _DAO.CompletedDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.ChangeDate
        /// </summary>
        public DateTime ChangeDate
        {
            get { return _DAO.ChangeDate; }
            set { _DAO.ChangeDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.OutputFile
        /// </summary>
        public String OutputFile
        {
            get { return _DAO.OutputFile; }
            set { _DAO.OutputFile = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceMedium
        /// </summary>
        public ICorrespondenceMedium CorrespondenceMedium
        {
            get
            {
                if (null == _DAO.CorrespondenceMedium) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICorrespondenceMedium, CorrespondenceMedium_DAO>(_DAO.CorrespondenceMedium);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CorrespondenceMedium = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CorrespondenceMedium = (CorrespondenceMedium_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.GenericKeyType
        /// </summary>
        public IGenericKeyType GenericKeyType
        {
            get
            {
                if (null == _DAO.GenericKeyType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.GenericKeyType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.ReportStatement
        /// </summary>
        public IReportStatement ReportStatement
        {
            get
            {
                if (null == _DAO.ReportStatement) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IReportStatement, ReportStatement_DAO>(_DAO.ReportStatement);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ReportStatement = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ReportStatement = (ReportStatement_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceParameters
        /// </summary>
        private DAOEventList<CorrespondenceParameters_DAO, ICorrespondenceParameters, CorrespondenceParameters> _CorrespondenceParameters;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceParameters
        /// </summary>
        public IEventList<ICorrespondenceParameters> CorrespondenceParameters
        {
            get
            {
                if (null == _CorrespondenceParameters)
                {
                    if (null == _DAO.CorrespondenceParameters)
                        _DAO.CorrespondenceParameters = new List<CorrespondenceParameters_DAO>();
                    _CorrespondenceParameters = new DAOEventList<CorrespondenceParameters_DAO, ICorrespondenceParameters, CorrespondenceParameters>(_DAO.CorrespondenceParameters);
                    _CorrespondenceParameters.BeforeAdd += new EventListHandler(OnCorrespondenceParameters_BeforeAdd);
                    _CorrespondenceParameters.BeforeRemove += new EventListHandler(OnCorrespondenceParameters_BeforeRemove);
                    _CorrespondenceParameters.AfterAdd += new EventListHandler(OnCorrespondenceParameters_AfterAdd);
                    _CorrespondenceParameters.AfterRemove += new EventListHandler(OnCorrespondenceParameters_AfterRemove);
                }
                return _CorrespondenceParameters;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Correspondence_DAO.CorrespondenceDetail
        /// </summary>
        public ICorrespondenceDetail CorrespondenceDetail
        {
            get
            {
                if (null == _DAO.CorrespondenceDetail) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICorrespondenceDetail, CorrespondenceDetail_DAO>(_DAO.CorrespondenceDetail);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CorrespondenceDetail = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CorrespondenceDetail = (CorrespondenceDetail_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _CorrespondenceParameters = null;
        }
    }
}