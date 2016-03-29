using FluentAssert;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.EventProjection;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    [TestFixture]
    public class when_processing_transactions_for_a_third_party_invoice_payment : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private ThirdPartyInvoiceDataModel _thirdPartyInvoice;
        private ThirdPartyDataModel thirdParty;

        [SetUp]
        public void OnTestSetup()
        {
            decimal totalAmount = 2500;
            decimal vatAmount = Math.Round((totalAmount * 14 / 114), 2);
            decimal amountExVat = totalAmount - vatAmount;
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            _thirdPartyInvoice.InvoiceStatusKey = (int)InvoiceStatus.ProcessingPayment;
            _thirdPartyInvoice.ThirdPartyId = thirdPartyId;
            _thirdPartyInvoice.InvoiceNumber = string.Concat("Test-InvNo-",Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            _thirdPartyInvoice.InvoiceDate = DateTime.Now.AddDays(-1);
            _thirdPartyInvoice.AmountExcludingVAT = amountExVat;
            _thirdPartyInvoice.VATAmount = vatAmount;
            _thirdPartyInvoice.TotalAmountIncludingVAT = totalAmount;
            _thirdPartyInvoice.CapitaliseInvoice = true;
            _thirdPartyInvoice.PaymentReference = string.Concat("PayRef-", Convert.ToBase64String(Guid.NewGuid().ToByteArray())).Remove(30);
            var updateInvoiceCommand = new UpdateThirdPartyInvoiceCommand(_thirdPartyInvoice);
            PerformCommand(updateInvoiceCommand);
            var invoiceLineItems = new List<InvoiceLineItemDataModel>() {
                new InvoiceLineItemDataModel(emptyInvoice.ThirdPartyInvoiceKey, 1, amountExVat, true, vatAmount, totalAmount)
            };
            var command = new InsertInvoiceLineItemsCommand(invoiceLineItems);
            PerformCommand(command);
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(_thirdPartyInvoice.ThirdPartyInvoiceKey);
        }

        [TearDown]
        public void OnTestTearDown()
        {
            if (_thirdPartyInvoice != null)
            {
                var command = new RemoveEmptyThirdPartyInvoiceCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
                base.PerformCommand(command);
                _thirdPartyInvoice = null;
            }
            
        }

        [Test]
        public void when_successful()
        {
            var legalEntity = TestApiClient.Get<LegalEntityDataModel>(new { LegalEntityKey = thirdParty.LegalEntityKey });
            var command = new ProcessTransactionsForThirdPartyInvoicePaymentCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command).WithoutErrors();       

            //Assertions for the financial transactions
            var financialService = TestApiClient.Get<FinancialServiceDataModel>(new { AccountKey = _thirdPartyInvoice.AccountKey, FinancialServiceTypeKey = 1 }).FirstOrDefault();
            var query = new GetFinancialTransactionsQuery(financialService.FinancialServiceKey);
            base.PerformQuery(query);
            var financialTransactions = query.Result.Results;
            CollectionAssert.IsNotEmpty(financialTransactions.Where(x => x.TransactionTypeKey == (int)LoanTransactionTypeEnum.CapitalisedLegalFeeTransaction
                && x.Amount == Convert.ToDecimal(_thirdPartyInvoice.TotalAmountIncludingVAT) && x.Reference == _thirdPartyInvoice.SahlReference));

        }
    }
}