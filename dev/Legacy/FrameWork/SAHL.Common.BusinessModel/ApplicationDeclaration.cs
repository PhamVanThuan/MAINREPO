using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO
    /// </summary>
    public partial class ApplicationDeclaration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO>, IApplicationDeclaration
    {
        public ApplicationDeclaration(SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO ApplicationDeclaration)
            : base(ApplicationDeclaration)
        {
            this._DAO = ApplicationDeclaration;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationRole
        /// </summary>
        public IApplicationRole ApplicationRole
        {
            get
            {
                if (null == _DAO.ApplicationRole) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationRole, ApplicationRole_DAO>(_DAO.ApplicationRole);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationRole = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationRole = (ApplicationRole_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationDate
        /// </summary>
        public DateTime? ApplicationDeclarationDate
        {
            get { return _DAO.ApplicationDeclarationDate; }
            set { _DAO.ApplicationDeclarationDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationAnswer
        /// </summary>
        public IApplicationDeclarationAnswer ApplicationDeclarationAnswer
        {
            get
            {
                if (null == _DAO.ApplicationDeclarationAnswer) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationDeclarationAnswer, ApplicationDeclarationAnswer_DAO>(_DAO.ApplicationDeclarationAnswer);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationDeclarationAnswer = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationDeclarationAnswer = (ApplicationDeclarationAnswer_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDeclaration_DAO.ApplicationDeclarationQuestion
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
    }
}