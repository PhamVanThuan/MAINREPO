using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Globals;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ILoanAgreementRepository))]
    public class LoanAgreementRepository : AbstractRepositoryBase, ILoanAgreementRepository
    {
        ///// <summary>
        ///// Implements <see cref="ILoanAgreementRepository.GetEmptyLoanAgreement(IDomainMessageCollection)"></see>.
        ///// </summary>
        //public ILoanAgreement GetEmptyLoanAgreement()
        //{
        //    if (Messages == null)
        //        throw new ArgumentNullException(StaticMessages.NullDomainCollection);

        //    return new LoanAgreement(new LoanAgreement_DAO());
        //}

        /// <summary>
        /// Implements <see cref="ILoanAgreementRepository.CreateLoanAgreement"></see>.
        /// </summary>
        public ILoanAgreement CreateLoanAgreement(DateTime AgreementDate, double Amount, DateTime ChangeDate, IBond Bond, string UserName)
        {
            LoanAgreement LA = new LoanAgreement(new LoanAgreement_DAO());
            LoanAgreement_DAO LADAO = (LoanAgreement_DAO)LA.GetDAOObject();
            Bond B = (Bond)Bond;
            Bond_DAO BDAO = (Bond_DAO)B.GetDAOObject();

            LADAO.AgreementDate = AgreementDate;
            LADAO.Amount = Amount;
            LADAO.ChangeDate = ChangeDate;
            LADAO.Bond = BDAO;
            LADAO.UserName = UserName;

            LADAO.CreateAndFlush();

            // do this after the LA create because of the rule checking
            // BondLoanAgreementAmount must be updated with the LoanAgreement Amount
            Bond.BondLoanAgreementAmount = Amount;
            //Loan agreement is saved, so save the bond also
            BDAO.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

            return (ILoanAgreement)(LA);
        }
    }
}

