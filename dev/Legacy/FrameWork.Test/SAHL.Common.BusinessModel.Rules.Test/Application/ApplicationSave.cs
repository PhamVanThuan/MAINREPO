using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Rules.Application.ApplicationSave;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.Application
{
    [TestFixture]
    public class ApplicationSave : RuleBase
    {
        [NUnit.Framework.SetUp()]
        public void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void ApplicationOpenSaveTest()
        {
            ApplicationOpenSave rule = new ApplicationOpenSave();

            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationStatus appStatus = _mockery.StrictMock<IApplicationStatus>();
            IApplicationStatus currAppStatus = _mockery.StrictMock<IApplicationStatus>();

            // if the key is 0 (new application, should exit out
            SetupResult.For(app.Key).Return(0);
            ExecuteRule(rule, 0, app);

            // if current application status is null, should exit out
            SetupResult.For(app.Key).Return(1);
            SetupResult.For(app.IsOpen).Return(false);
            SetupResult.For(app.ApplicationStatusPrevious).Return(null);
            ExecuteRule(rule, 0, app);

            // try each of the offer statuses - only when it's open should it allow a change
            Array offerStatuses = Enum.GetValues(typeof(OfferStatuses));
            for (int i = 0; i < offerStatuses.Length; i++)
            {
                int errorCount = 1;
                int offerStatusKey = (int)offerStatuses.GetValue(i);
                SetupResult.For(app.Key).Return(1);
                SetupResult.For(app.ApplicationStatusPrevious).Return(appStatus);
                SetupResult.For(appStatus.Key).Return(offerStatusKey);
                SetupResult.For(app.ApplicationStatus).Return(appStatus);
                if (offerStatusKey == (int)OfferStatuses.Open)
                {
                    SetupResult.For(app.IsOpen).Return(true);
                    errorCount = 0;
                }
                else
                {
                    SetupResult.For(app.IsOpen).Return(false);
                }

                ExecuteRule(rule, errorCount, app);
            }
        }

        [NUnit.Framework.Test]
        public void ApplicationInformationAcceptedSaveTest()
        {
            ApplicationInformationAcceptedSave rule = new ApplicationInformationAcceptedSave();

            IApplication app = _mockery.StrictMock<IApplication>(); 
            IApplicationInformation appInfo = _mockery.StrictMock<IApplicationInformation>();
            IApplicationInformationType appInfoType = _mockery.StrictMock<IApplicationInformationType>();

            // if the key is 0 (new application, should exit out
            SetupResult.For(appInfo.Key).Return(0);
            ExecuteRule(rule, 0, appInfo);

            // if current application status is null, should exit out
            SetupResult.For(appInfo.Key).Return(1);
            SetupResult.For(appInfo.ApplicationInformationTypePrevious).Return(null);
            ExecuteRule(rule, 0, appInfo);

            // try each of the offer statuses - only when it's open should it allow a change
            Array offerInfoTypes = Enum.GetValues(typeof(OfferInformationTypes));
            for (int i = 0; i < offerInfoTypes.Length; i++)
            {
                int errorCount = 0;
                int offerInfoTypeKey = (int)offerInfoTypes.GetValue(i);
                SetupResult.For(appInfo.Key).Return(1);
                SetupResult.For(appInfo.ApplicationInformationTypePrevious).Return(appInfoType);
                SetupResult.For(appInfoType.Key).Return(offerInfoTypeKey);
                SetupResult.For(appInfo.Application).Return(app);
                SetupResult.For(app.Key).Return(0).IgnoreArguments();

                if (offerInfoTypeKey == (int)OfferInformationTypes.AcceptedOffer)
                    errorCount = 1;
                ExecuteRule(rule, errorCount, appInfo);
            }
        }
        

    }
}
