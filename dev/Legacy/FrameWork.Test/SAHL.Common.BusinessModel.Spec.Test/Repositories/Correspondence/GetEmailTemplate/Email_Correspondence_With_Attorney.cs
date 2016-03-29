using Castle.ActiveRecord;
using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.Correspondence.GetEmailTemplate
{
    [Subject("Email_Correspondence_With_Attorney")]
    internal class Email_Correspondence_With_Attorney : WithFakes
    {
        internal static CorrespondenceTestBase _testBase;
        internal static ILegalEntity _legalEntity;
        internal static int _accountKey;
        internal static string _body;
        internal static string _subject;
        internal static string _expectedResult;


        internal Establish context = () =>
        {
            _testBase = new CorrespondenceTestBase();
            _accountKey = Param<int>.IsAnything;
            _legalEntity = An<ILegalEntity>();
            _legalEntity.WhenToldTo(x => x.GetLegalName(Param<LegalNameFormat>.IsAnything)).Return("");
            _expectedResult = string.Format(_testBase.GetActualTemplate(CorrespondenceTemplates.EmailCorrespondenceAttorney).Template, "", "", "");
        };

        internal Because of = () =>
        {
            _testBase.GetEmailTemplate(_legalEntity, "", "", _accountKey, CorrespondenceTemplates.EmailCorrespondenceAttorney, out _subject, out _body);
        };

        internal It should_equal_expected_result = () =>
        {
            _expectedResult.ShouldEqual(_body);
        };

        internal It should_not_return_empty_body = () =>
        {
            _body.ShouldNotBeNull();
        };

        internal It should_not_return_empty_subject = () =>
        {
            _subject.ShouldNotBeNull();
        };
    }
}
