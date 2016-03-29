using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using System.Security.Principal;
using SAHL.Web.Services.Internal.DataModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Threading;
using SAHL.Common.Security;

namespace SAHL.Web.Services.Internal
{
    public abstract class AttorneyBase : WebServiceBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lekey"></param>
        /// <returns></returns>
        protected bool IsValidWebUser(int lekey)
        {
            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {

                //Get list of roles
                IReadOnlyEventList<IExternalRole> leroles = legalEntityRepository.GetExternalRolesByLegalEntity(lekey);
                //Determine whether the legal entity is a Litigation Attorney Contact is active and in role WebAccess
                foreach (IExternalRole role in leroles)
                {
                    if (role.GeneralStatus.Key == (int)GeneralStatuses.Active &&
                    role.GenericKeyType.Key == (int)GenericKeyTypes.Attorney &&
                    role.ExternalRoleType.Key == (int)ExternalRoleTypes.WebAccess)
                    {
                        //Then check Attorny is active
                        SAHL.Common.BusinessModel.Interfaces.IAttorney attorney = legalEntityRepository.GetAttorney(role);
                        return attorney.AttorneyLitigationInd.Value && attorney.GeneralStatus.Key == (int)GeneralStatuses.Active;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Can Register User
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        protected bool CanRegisterUser(string emailAddress)
        {
            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                string errorMessage = String.Empty;
                //Determine whether the user is a Legal Entity Natural Person
                ILegalEntityNaturalPerson person = legalEntityRepository.GetWebAccessLegalEntity(emailAddress);

                if (person == null)
                {
                    errorMessage = "This user is either deactivated or not setup for attorney web access, Contact SA Home Loans.";
                    SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                    return false;
                }
                
                //If the person has already registered, don't attempt to register them again
                if (person.LegalEntityLogin != null && person.LegalEntityLogin.GeneralStatus.Key == (int)GeneralStatuses.Active)
                {
                    errorMessage = "The user has already been registered, please use the 'Forgot My Password' feature to recover your password.";
                    SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                    return false;
                }

                //Seperated this from the bottom if statement so that we can return more descriptive registration messages to the user
                if (person != null && person.LegalEntityLogin != null && person.LegalEntityLogin.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                {
                    errorMessage = "The user associated with this account has been disabled, Please Contact SA Home Loans.";
                    SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                    return false;
                }

                return IsValidWebUser(person.Key);
            }
        }
    }
}