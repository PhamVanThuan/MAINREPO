using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using System.Net.Mail;
using SAHL.Common.Globals;
using SAHL.Common.Exceptions;
using System.Reflection;

namespace SAHL.Common.Service.Test
{
    [TestFixture]
    public class MessageServiceTest : TestBase
    {
        [SetUp()]
        public void Setup()
        {
            base.SetRepositoryStrategy(TypeFactoryStrategy.Default);
        }

        [NUnit.Framework.Test]
        public void InternalTestEmail_EmailAddress_Fail()
        {
            // The 'to' email address IS NOT contained in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // Result : Email should NOT be sent
            string to = "craigf@sahomeloans.com"; 

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("",to,"","","","");

            Assert.IsTrue(emailSent == false);
        }

        [Ignore]
        [NUnit.Framework.Test]
        public void InternalTestEmail_EmailAddress_Success()
        {
            // The 'to' email address IS contained in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // Result : Email should be sent
            string to = "halotest@sahomeloans.com";

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("", to, "", "", "", "");

            Assert.IsTrue(emailSent == true);
        }

        [Ignore]
        [NUnit.Framework.Test]
        public void InternalTestEmail_EmailAddress_FilterOut_Success()
        {
            // One of the 'to' email addresses is in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // None of the cc or bcc email addresses are in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // Result : Email should be sent
            string to = "halotest@sahomeloans.com,craigf@sahomeloans.com";
            string cc = "craigf@sahomeloans.com";
            string bcc = "craigf@sahomeloans.com,matts@sahomeloans.com";

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("", to, cc, bcc, "", "");

            Assert.IsTrue(emailSent == true);
        }

        [NUnit.Framework.Test]
        public void InternalTestEmail_EmailAddress_FilterOut_Fail()
        {
            // None of the to, cc or bcc email addresses are in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // Result : Email should NOT be sent
            string to = "matts@sahomeloans.com,craigf@sahomeloans.com";
            string cc = "craigf@sahomeloans.com";
            string bcc = "craigf@sahomeloans.com,matts@sahomeloans.com";

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("", to, cc, bcc, "", "");

            Assert.IsTrue(emailSent == false);
        }

        [NUnit.Framework.Test]
        public void InternalTestEmail_FilterPhrase_Fail()
        {
            // The 'to' email address IS contained in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // The body contains a phrase in the 'InternalTestEmailFilterPhrases' list in the Framework.Test.config
            // Result : Email should NOT be sent
            string to = "halotest@sahomeloans.com";
            string body = "This is a reminder that case";

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("", to, "", "", "", body);

            Assert.IsTrue(emailSent == false);
        }

        [Ignore]
        [NUnit.Framework.Test]
        public void InternalTestEmail_FilterPhrase_Success()
        {
            // The 'to' email address IS contained in the 'InternalTestEmailRecipients' list in the Framework.Test.config
            // The body does not contain a phrase in the 'InternalTestEmailFilterPhrases' list in the Framework.Test.config
            // Result : Email should be sent
            string to = "halotest@sahomeloans.com";
            string body = "test body";

            IMessageService messageService = ServiceFactory.GetService<IMessageService>();
            bool emailSent = messageService.SendEmailInternal("", to, "", "", "", body);

            Assert.IsTrue(emailSent == true);
        }

        [Ignore]
        [NUnit.Framework.Test]
        public void HALO_Exception_Email_Test()
        {            
            try
            {
                ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                ILegalEntityNaturalPerson np = leRepo.GetEmptyLegalEntity(LegalEntityTypes.NaturalPerson) as ILegalEntityNaturalPerson;
                leRepo.SaveLegalEntity(np, false);
            }
            catch (DomainValidationException dex)
            {
                SendExceptionEmail(dex);
            }
            catch (Exception ex)
            {
                SendExceptionEmail( ex);
            }
        }

        private void SendExceptionEmail(Exception exception)
        {
            ICommonRepository commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.HaloException);

            try 
	        {
                MailMessage email = new MailMessage();
                string to = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOExceptionEmailAddress).ControlText;
                string subject = template.Subject + " THIS IS A TEST - please ignore";
                string body = String.Format(template.Template, this.GetType().FullName, MethodBase.GetCurrentMethod().Name, exception);

                string from = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOEmailAddress).ControlText;
                email.From = new MailAddress(from, from);
                email.To.Add(to);
                email.Subject = subject;

                email.Body = body;
                email.BodyEncoding = Encoding.Default;


                // send email
                string smtpServerHost = "192.168.11.28";
                int smtpServerPort = 25;

                SmtpClient smtpMail = new SmtpClient(smtpServerHost, smtpServerPort);
                smtpMail.Credentials = new System.Net.NetworkCredential("sqlservice2", "W0rdpass", "sahl");
                smtpMail.Send(email);
	        }
	        catch (Exception ex)
	        {
		
		        throw;
	        }

        }
    }
}
