using DomainService2.IOC;
using DomainService2.Workflow.Origination.ApplicationCapture;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using X2DomainService.Interface.Common;

namespace DomainService2.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            //var config = new SAHL.Communication.EasyNetQMessageBusConfigurationProvider();
            //SAHL.Communication.EasyNetQMessageBus mb = new SAHL.Communication.EasyNetQMessageBus(config);
            //DomainServiceLoader.ProcessName = "myProcess";
            //    var ioc = DomainServiceLoader.Instance;
            //    using (var tran = new TransactionScope())
            //    {
            //        var ruleHandler = DomainServiceLoader.Instance.DomainServiceIOC.GetCommandHandler<CheckBranchSubmitApplicationRulesCommand>();
            //        var rule = new CheckBranchSubmitApplicationRulesCommand(1464644, true);
            //        ruleHandler.Handle(new DomainMessageCollection(), rule);

            //        var handler = DomainServiceLoader.Instance.DomainServiceIOC.GetCommandHandler<PromoteLeadToMainApplicantCommand>();
            //        PromoteLeadToMainApplicantCommand command = new PromoteLeadToMainApplicantCommand(1464644);
            //        handler.Handle(new DomainMessageCollection(), command);
            //        //tran.Complete();
            //    }

            //    System.Console.WriteLine("Happyness");
            //    System.Console.ReadLine();
            //}
            //catch (Exception ex)
            //{
            //    System.Console.WriteLine("Not so much happyness");
            //    string s = ex.ToString();
            //    throw;
            //}

            var ioc = DomainServiceLoader.Instance;
            for (int i = 0; i < 4; i++)
            {

                try
                {
                    IDomainMessageCollection messages = new DomainMessageCollection();
                    int applicationKey = 1612792;
                    bool isBondExceptionAction = false;
                    var commonHost = DomainServiceLoader.Instance.Get<ICommon>();
                    //x2 node
                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        //domain service - map
                        commonHost.CalculateAndSaveApplication(messages, applicationKey, isBondExceptionAction);
                        transactionScope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }
        }
    }
}
