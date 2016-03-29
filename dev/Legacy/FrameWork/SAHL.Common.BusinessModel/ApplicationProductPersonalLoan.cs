using System;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Validation;
using System.Linq;

namespace SAHL.Common.BusinessModel
{
    public class ApplicationProductPersonalLoan : ApplicationProductUnsecuredLending, IApplicationProductPersonalLoan
    {
        public ApplicationProductPersonalLoan(IApplication application, bool createNew)
            : base(application, createNew)
        {
            if (createNew)
            {
                ApplicationInformationPersonalLoan_DAO PL = new ApplicationInformationPersonalLoan_DAO();
                PL.ApplicationInformation = _appInfoDAO;
                _appInfoDAO.ApplicationInformationPersoanlLoan = PL;

                if (clone)
                {
                    if (_appInfoDAOPrevious.ApplicationInformationPersoanlLoan != null)
                        _appInfoDAOPrevious.ApplicationInformationPersoanlLoan.Clone(PL);

                    GetLatestApplicationInformation().ApplicationInsertDate = DateTime.Now;
                }
            }
        }

		public ApplicationProductPersonalLoan(IApplicationInformation applicationInformation) : base(applicationInformation)
        {
        }

        public IApplicationInformationPersonalLoan ApplicationInformationPersonalLoan
        {
            get
            {
                if (_appInfoDAO != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationPersonalLoan, ApplicationInformationPersonalLoan_DAO>(_appInfoDAO.ApplicationInformationPersoanlLoan);
                }
                throw new NullReferenceException("Application Information could not be retrieved.");
            }
        }

        /// <summary>
        /// We should have one external policy per account
        /// </summary>
        public IExternalLifePolicy ExternalLifePolicy
        {
            get
            {
                IExternalLifePolicy externalLifePolicy = null;
                if (_appDAO.ExternalLifePolicy != null && _appDAO.ExternalLifePolicy.Count > 0)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    externalLifePolicy = BMTM.GetMappedType<IExternalLifePolicy, ExternalLifePolicy_DAO>(_appDAO.ExternalLifePolicy.First());
                }
                return externalLifePolicy;
            }
            set
            {
                if (value == null)
                {
                    _appDAO.ExternalLifePolicy = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                {
                    // Only a single value is expected as the get logic only fetches the first element
                    _appDAO.ExternalLifePolicy.Clear();
                    _appDAO.ExternalLifePolicy.Add((ExternalLifePolicy_DAO)obj.GetDAOObject());
                }
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}