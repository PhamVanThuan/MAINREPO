using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IEmploymentRepository
    {
        /// <summary>
        /// Gets a list of accounts whose PTI is affected by the employment record - if the result set contains
        /// any accounts then the employment record should not be deactivated or deleted.
        /// </summary>
        /// <param name="employment">The employment record that is being deactivated.</param>
        /// <returns>A list of accounts that deactivating the employment record will affect.</returns>
        IList<IAccount> GetAccountsForPTI(IEmployment employment);

        /// <summary>
        /// Gets an employer entity by a specified key.
        /// </summary>
        /// <param name="employerKey">The unique identifier for the employer record.</param>
        /// <returns>An <see cref="IEmployer"/> object matching the supplied <c>employerKey</c>, or null if no employer record is found.</returns>
        IEmployer GetEmployerByKey(int employerKey);

        /// <summary>
        /// Gets a list of employers according to a specified <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        IReadOnlyEventList<IEmployer> GetEmployersByPrefix(string prefix, int maxRowCount);

        /// <summary>
        /// Gets a list of employment records for an application.
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="incomeContributorsOnly"></param>
        /// <returns></returns>
        IEventList<IEmployment> GetEmploymentByApplicationKey(int applicationKey, bool incomeContributorsOnly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="includeDaySuffix"></param>
        /// <returns></returns>
        IDictionary<ILegalEntity, string> GetSalaryPaymentDaysByGenericKey(int genericKey, int genericKeyTypeKey, bool includeDaySuffix = true);

        /// <summary>
        /// Gets an employment entity by a specified key.
        /// </summary>
        /// <param name="employmentKey">The unique identifier for the employment record.</param>
        /// <returns>An <see cref="IEmployment"/> object matching the supplied <c>employmentKey</c>, or null if no employment record is found.</returns>
        IEmployment GetEmploymentByKey(int employmentKey);

        /// <summary>
        /// Gets a new <see cref="IEmployer"/> that can be populated.
        /// </summary>
        IEmployer GetEmptyEmployer();

        /// <summary>
        /// Gets a new <see cref="IEmployment"/> that can be populated.
        /// </summary>
        /// <param name="employmentType">The employment type key.</param>
        IEmployment GetEmptyEmploymentByType(EmploymentTypes employmentType);

        /// <summary>
        /// Gets a new <see cref="IEmployment"/> that can be populated.
        /// </summary>
        /// <param name="employmentType">The employment type.</param>
        IEmployment GetEmptyEmploymentByType(IEmploymentType employmentType);

        /// <summary>
        /// Gets a new <see cref="ISubsidy"/> that can be populated.
        /// </summary>
        ISubsidy GetEmptySubsidy();

        /// <summary>
        /// Gets a subsidy by a specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the subsidy record.</param>
        /// <returns>An <see cref="ISubsidy"/> object matching the supplied <c>key</c>, or null if no subsidy record is found.</returns>
        ISubsidy GetSubsidyByKey(int key);

        /// <summary>
        /// Gets a subsidy provider by a specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the subsidy provider record.</param>
        /// <returns>An <see cref="ISubsidyProvider"/> object matching the supplied <c>key</c>, or null if no subsidy provider record is found.</returns>
        ISubsidyProvider GetSubsidyProviderByKey(int key);

        /// <summary>
        /// Gets a list of subsidy providers according to a specified <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        ReadOnlyCollection<ISubsidyProvider> GetSubsidyProvidersByPrefix(string prefix, int maxRowCount);

        ///// <summary>
        ///// Gets a list of subsidy providers according to a specified <c>prefix</c> and <c>type</c>.
        ///// </summary>
        ///// <param name="prefix"></param>
        ///// <param name="type"></param>
        ///// <param name="maxRowCount"></param>
        ///// <returns></returns>
        //IReadOnlyEventList<ISubsidyProvider> GetSubsidyProvidersByPrefixAndType(string prefix, SubsidyProviderTypes type, int maxRowCount);

        ISubsidyProvider CreateEmptySubsidyProvider();

        void SaveSubsidyProvider(ISubsidyProvider sp);

        /// <summary>
        /// Saves an employer to the database.
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="principal">The currently user.</param>
        void SaveEmployer(IEmployer employer, SAHLPrincipal principal);

        /// <summary>
        /// Get list of Subsidies by Legal Entity Key
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <returns></returns>
        IList<ISubsidy> GetSubsidiesByLegalEntityKey(int legalEntityKey);

        /// <summary>
        /// Saves legal entity employment information to the database.
        /// </summary>
        /// <param name="employment"></param>
        void SaveEmployment(IEmployment employment);

        /// <summary>
        /// Gets a list of employers according to a specified <c>prefix</c>.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        IReadOnlyEventList<IEmployer> GetEmployers(string prefix);

        /// <summary>
        ///
        /// </summary>
        /// <param name="employment"></param>
        /// <returns></returns>
        DataTable GetVerificationProcessDT(IEmployment employment);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEmploymentVerificationProcess GetEmptyEmploymentVerificationProcess();

        /// <summary>
        ///
        /// </summary>
        /// <param name="subsidy"></param>
        void SaveSubsidy(ISubsidy subsidy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="highestIncomeContributer"></param>
        /// <returns></returns>
        EmploymentTypes DetermineHighestIncomeContributersEmploymentType(ILegalEntityNaturalPerson highestIncomeContributer);
    }
}