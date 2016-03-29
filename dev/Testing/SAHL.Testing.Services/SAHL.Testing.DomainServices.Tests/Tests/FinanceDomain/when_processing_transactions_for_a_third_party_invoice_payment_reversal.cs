using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models.FETest;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.FinanceDomain;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Enum;
using SAHL.Services.Interfaces.FrontEndTest.Commands;
using SAHL.Services.Interfaces.FrontEndTest.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.FinanceDomain
{
    public class when_processing_transactions_for_a_third_party_invoice_payment_reversal : ServiceTestBase<IFinanceDomainServiceClient>
    {
        private ThirdPartyInvoiceDataModel _thirdPartyInvoice;

        [SetUp]
        public void OnTestSetup()
        {
            decimal totalAmount = 2500;
            decimal vatAmount = Math.Round((totalAmount * 14 / 114), 2);
            decimal amountExVat = totalAmount - vatAmount;
            var thirdParty = TestApiClient.GetAny<ThirdPartyDataModel>(new { ThirdPartyTypeKey = (int)ThirdPartyType.Attorney }, 1);
            Guid thirdPartyId = thirdParty.Id;
            var emptyInvoice = TestApiClient.GetAny<EmptyThirdPartyInvoicesDataModel>();
            _thirdPartyInvoice = TestApiClient.GetByKey<ThirdPartyInvoiceDataModel>(emptyInvoice.ThirdPartyInvoiceKey);
            _thirdPartyInvoice.InvoiceStatusKey = (int)InvoiceStatus.ProcessingPayment;
            _thirdPartyInvoice.ThirdPartyId = thirdPartyId;
            _thirdPartyInvoice.InvoiceNumber = string.Concat("Test-InvNo-", Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
            _thirdPartyInvoice.InvoiceDate = DateTime.Now.AddDays(-1);
            _thirdPartyInvoice.AmountExcludingVAT = amountExVat;
            _thirdPartyInvoice.VATAmount = vatAmount;
            _thirdPartyInvoice.TotalAmountIncludingVAT = totalAmount;
            _thirdPartyInvoice.CapitaliseInvoice = false;
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
        public void given_a_legal_fee_transaction_exists_it_should_post_a_legal_fee_reversal_transaction()
        {
            var command = new ProcessTransactionsForThirdPartyInvoicePaymentCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(command).WithoutErrors();

            var reversalCommand = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            base.Execute(reversalCommand).WithoutErrors();

            //Assertions for the financial transactions
            var financialService = TestApiClient.Get<FinancialServiceDataModel>(new { AccountKey = _thirdPartyInvoice.AccountKey, FinancialServiceTypeKey = 1 }).FirstOrDefault();
            var query = new GetFinancialTransactionsQuery(financialService.FinancialServiceKey);
            base.PerformQuery(query);
            var financialTransactions = query.Result.Results;
            CollectionAssert.IsNotEmpty(financialTransactions.Where(x => x.TransactionTypeKey == (int)LoanTransactionTypeEnum.NonCapitalisedLegalFeeReversalTransaction
                && x.Amount == (Convert.ToDecimal(_thirdPartyInvoice.TotalAmountIncludingVAT) * -1)));
        }

        [Test]
        public void given_a_legal_fee_transaction_does_not_exists_it_should_error()
        {
            var reversalCommand = new ProcessTransactionsForThirdPartyInvoicePaymentReversalCommand(_thirdPartyInvoice.ThirdPartyInvoiceKey);
            try
            {
                base.Execute(reversalCommand).AndExpectThatMessagesContain(new SystemMessage("Failed to retrieve the financial transaction key", SystemMessageSeverityEnum.Warning));
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("ThirdPartyInvoiceKey: {0} | {1}", _thirdPartyInvoice.ThirdPartyInvoiceKey, e.Message));
            }

            //Assertions for the financial transactions
            var financialService = TestApiClient.Get<FinancialServiceDataModel>(new { AccountKey = _thirdPartyInvoice.AccountKey, FinancialServiceTypeKey = 1 }).FirstOrDefault();
            var query = new GetFinancialTransactionsQuery(financialService.FinancialServiceKey);
            base.PerformQuery(query);
            var financialTransactions = query.Result.Results;
            CollectionAssert.IsEmpty(financialTransactions.Where(x => x.TransactionTypeKey == (int)LoanTransactionTypeEnum.NonCapitalisedLegalFeeReversalTransaction
                && x.Amount == (Convert.ToDecimal(_thirdPartyInvoice.TotalAmountIncludingVAT) * -1)));
        }
    }
}