using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// This class does reflect a DAO object directly, but derives ApplicaitonInformation in order to expose lurker properties that are applicable for a 
    /// Further Advance application.
    /// </summary>
    public partial class FurtherAdvanceApplicationInformation : ApplicationInformation , IFurtherAdvanceApplicationInformation
    {
        /// <summary>
        /// This constructor passes the ApplicationInformation DAO object to the base class.
        /// </summary>
        /// <param name="ApplicationInformation">The application DAO object that this object encapsulates.</param>
        public FurtherAdvanceApplicationInformation(SAHL.Common.BusinessModel.DAO.ApplicationInformation_DAO ApplicationInformation)
            : base(ApplicationInformation)
        {
          
        }

        /// <summary>
        /// 
        /// </summary>
        public IApplicationInformationVariableLoan ApplicationInformationVariableLoan
        {
            get
            {
                if (null == _DAO.ApplicationInformationVariableLoan) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationVariableLoan, ApplicationInformationVariableLoan_DAO>(_DAO.ApplicationInformationVariableLoan);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationVariableLoan = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationVariableLoan = (ApplicationInformationVariableLoan_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IApplicationInformationVarifixLoan ApplicationInformationVarifixLoan
        {
            get
            {
                if (null == _DAO.ApplicationInformationVarifixLoan) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationVarifixLoan, ApplicationInformationVarifixLoan_DAO>(_DAO.ApplicationInformationVarifixLoan);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationVarifixLoan = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationVarifixLoan = (ApplicationInformationVarifixLoan_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IApplicationInformationSuperLoLoan ApplicationInformationSuperLoLoan
        {
            get
            {
                if (null == _DAO.ApplicationInformationSuperLoLoan) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationSuperLoLoan, ApplicationInformationSuperLoLoan_DAO>(_DAO.ApplicationInformationSuperLoLoan);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationSuperLoLoan = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationSuperLoLoan = (ApplicationInformationSuperLoLoan_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }


        public IApplicationInformationEdge ApplicationInformationEdge
        {
            get
            {
                if (null == _DAO.ApplicationInformationEdge) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationEdge, ApplicationInformationEdge_DAO>(_DAO.ApplicationInformationEdge);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationEdge= null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationEdge = (ApplicationInformationEdge_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public IApplicationInformationQuickCash ApplicationInformationQuickCash
        {
            get
            {
                if (null == _DAO.ApplicationInformationQuickCash) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationQuickCash, ApplicationInformationQuickCash_DAO>(_DAO.ApplicationInformationQuickCash);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationQuickCash = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationQuickCash = (ApplicationInformationQuickCash_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }


        /// <summary>
        /// 'Interest Only' information on an application
        /// </summary>
        public IApplicationInformationInterestOnly ApplicationInformationInterestOnly
        {
            get
            {
                if (null == _DAO.ApplicationInformationInterestOnly) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationInterestOnly, ApplicationInformationInterestOnly_DAO>(_DAO.ApplicationInformationInterestOnly);
                }

            }

            set
            {
                if (value == null)
                {
                    _DAO.ApplicationInformationInterestOnly = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ApplicationInformationInterestOnly = (ApplicationInformationInterestOnly_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

    }
}
