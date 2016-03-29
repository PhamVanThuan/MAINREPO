using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO
    /// </summary>
    public partial class ApplicationDeclarationQuestionAnswerConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO>, IApplicationDeclarationQuestionAnswerConfiguration
    {
        public ApplicationDeclarationQuestionAnswerConfiguration(SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO ApplicationDeclarationQuestionAnswerConfiguration)
            : base(ApplicationDeclarationQuestionAnswerConfiguration)
        {
            this._DAO = ApplicationDeclarationQuestionAnswerConfiguration;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO.ApplicationDeclarationQuestion
        /// </summary>
        public IApplicationDeclarationQuestion ApplicationDeclarationQuestion
        {
            get
            {
                if (null == _DAO.ApplicationDeclarationQuestion) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationDeclarationQuestion, ApplicationDeclarationQuestion_DAO>(_DAO.ApplicationDeclarationQuestion);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationDeclarationQuestion = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationDeclarationQuestion = (ApplicationDeclarationQuestion_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO.GenericKey
        /// </summary>
        public Int32 GenericKey
        {
            get { return _DAO.GenericKey; }
            set { _DAO.GenericKey = value; }
        } 

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO.GenericKeyType
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclarationQuestionAnswerConfiguration_DAO.OriginationSourceProduct
        /// </summary>
        public IOriginationSourceProduct OriginationSourceProduct
        {
            get
            {
                if (null == _DAO.OriginationSourceProduct) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOriginationSourceProduct, OriginationSourceProduct_DAO>(_DAO.OriginationSourceProduct);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OriginationSourceProduct = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OriginationSourceProduct = (OriginationSourceProduct_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}