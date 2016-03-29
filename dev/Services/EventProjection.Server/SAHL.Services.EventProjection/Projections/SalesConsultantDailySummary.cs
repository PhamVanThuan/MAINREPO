using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events.Projections;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Models;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Common;
using SAHL.Services.Interfaces.LegacyEventGenerator.Events.Workflow.Origination.ApplicationProgress;
using System;
using System.Linq;

namespace SAHL.Services.EventProjection.Projections
{
    public class SalesConsultantDailyPipeline :
        ITableProjector<BusinessDayCompletedLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressInApplicationCaptureLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressInApplicationManagementLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressInValuationLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressReturnToManageAppFromValuationLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressInCreditLegacyEvent, SalesConsultantDailyPipelineDataModel>,
        ITableProjector<AppProgressInLOALegacyEvent, SalesConsultantDailyPipelineDataModel>
    {
        public void Handle(BusinessDayCompletedLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            using (var db = new Db().InAppContext())
            {
                db.Update<SalesConsultantDailyPipelineDataModel>(new EmptyProjectionSqlStatement());
                db.Complete();
            }
        }

        public void Handle(AppProgressInApplicationCaptureLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            var currentStage = "Application Capture";
            using (var db = new Db().InAppContext())
            {
                var applicationsCapturedForConsultant = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = currentStage });
                var offerInformationRecord = db.SelectOneWhere<OfferInformationVariableLoanDataModel>("OfferInformationKey = @OfferInformationKey",
                    new { OfferInformationKey = @event.ApplicationInformationKey });
                if (!applicationsCapturedForConsultant.Any())
                {
                    db.Insert<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, 1, 1,
                        offerInformationRecord.LoanAgreementAmount ?? 0));
                }
                else
                {
                    var numberOfApplicationsAtStage = applicationsCapturedForConsultant.First().NumberOfApplicationsAtStage + 1;
                    var totalLoanAgreementAmountForConsultant = applicationsCapturedForConsultant.First().SalesValue + offerInformationRecord.LoanAgreementAmount;
                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, numberOfApplicationsAtStage,
                        1, totalLoanAgreementAmountForConsultant ?? 0));
                }
                db.Complete();
            }
            //make call to projection service
        }

        public void Handle(AppProgressInApplicationManagementLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            var currentStage = "Application Management";
            var previousStage = "Application Capture";
            using (var db = new Db().InAppContext())
            {
                var applicationsAtPreviousStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = previousStage });
                var applicationsAtCurrentStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = currentStage });

                var offerInformationRecord = db.SelectOneWhere<OfferInformationVariableLoanDataModel>("OfferInformationKey = @OfferInformationKey",
                    new { OfferInformationKey = @event.ApplicationInformationKey });
                if (!applicationsAtCurrentStage.Any())
                {
                    db.Insert<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, 1, 1,
                        offerInformationRecord.LoanAgreementAmount ?? 0));
                }
                else
                {
                    var numberOfApplicationsAtCurrentStage = applicationsAtCurrentStage.First().NumberOfApplicationsAtStage + 1;
                    var numberOfApplicationsAtPreviousStage = applicationsAtPreviousStage.First().NumberOfApplicationsAtStage - 1;
                    var totalLoanAgreementAmountForConsultantAtCurrentStage = applicationsAtCurrentStage.First().SalesValue + offerInformationRecord.LoanAgreementAmount;
                    var totalLoanAgreementAmountForConsultantAtPreviousStage = applicationsAtPreviousStage.First().SalesValue - totalLoanAgreementAmountForConsultantAtCurrentStage;

                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, numberOfApplicationsAtCurrentStage,
                        1, totalLoanAgreementAmountForConsultantAtCurrentStage ?? 0));
                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, previousStage, numberOfApplicationsAtPreviousStage,
                        1, totalLoanAgreementAmountForConsultantAtPreviousStage ?? 0));
                }
                db.Complete();
            }
            //make call to projection service
        }

        public void Handle(AppProgressInCreditLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            var currentStage = "Credit";
            var previousStage = "Application Management";
            using (var db = new Db().InAppContext())
            {
                var applicationsAtPreviousStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = previousStage });
                var applicationsAtCurrentStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = currentStage });

                var offerInformationRecord = db.SelectOneWhere<OfferInformationVariableLoanDataModel>("OfferInformationKey = @OfferInformationKey",
                    new { OfferInformationKey = @event.ApplicationInformationKey });
                if (!applicationsAtCurrentStage.Any())
                {
                    db.Insert<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, 1, 1,
                        offerInformationRecord.LoanAgreementAmount ?? 0));
                }
                else
                {
                    var numberOfApplicationsAtCurrentStage = applicationsAtCurrentStage.First().NumberOfApplicationsAtStage + 1;
                    var numberOfApplicationsAtPreviousStage = applicationsAtPreviousStage.First().NumberOfApplicationsAtStage - 1;
                    var totalLoanAgreementAmountForConsultantAtCurrentStage = applicationsAtCurrentStage.First().SalesValue + offerInformationRecord.LoanAgreementAmount;
                    var totalLoanAgreementAmountForConsultantAtPreviousStage = applicationsAtPreviousStage.First().SalesValue - totalLoanAgreementAmountForConsultantAtCurrentStage;

                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, numberOfApplicationsAtCurrentStage,
                        1, totalLoanAgreementAmountForConsultantAtCurrentStage ?? 0));
                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, previousStage, numberOfApplicationsAtPreviousStage,
                        1, totalLoanAgreementAmountForConsultantAtPreviousStage ?? 0));
                }
                db.Complete();
            }
            //make call to projection service
        }

        public void Handle(AppProgressInLOALegacyEvent @event, IServiceRequestMetadata metadata)
        {
            var currentStage = "LOA";
            var previousStage = "Credit";
            using (var db = new Db().InAppContext())
            {
                var applicationsAtPreviousStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage", 
                    new { Consultant = @event.AssignedADUser, ApplicationStage = previousStage });
                var applicationsAtCurrentStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage", 
                    new { Consultant = @event.AssignedADUser, ApplicationStage = currentStage });

                var offerInformationRecord = db.SelectOneWhere<OfferInformationVariableLoanDataModel>("OfferInformationKey = @OfferInformationKey", 
                    new { OfferInformationKey = @event.ApplicationInformationKey });
                if (!applicationsAtCurrentStage.Any())
                {
                    db.Insert<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, 1, 1, 
                        offerInformationRecord.LoanAgreementAmount ?? 0));
                }
                else
                {
                    var numberOfApplicationsAtCurrentStage = applicationsAtCurrentStage.First().NumberOfApplicationsAtStage + 1;
                    var numberOfApplicationsAtPreviousStage = applicationsAtPreviousStage.First().NumberOfApplicationsAtStage - 1;
                    var totalLoanAgreementAmountForConsultantAtCurrentStage = applicationsAtCurrentStage.First().SalesValue + offerInformationRecord.LoanAgreementAmount;
                    var totalLoanAgreementAmountForConsultantAtPreviousStage = applicationsAtPreviousStage.First().SalesValue - totalLoanAgreementAmountForConsultantAtCurrentStage;

                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, numberOfApplicationsAtCurrentStage, 
                        1, totalLoanAgreementAmountForConsultantAtCurrentStage ?? 0));
                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, previousStage, numberOfApplicationsAtPreviousStage,
                        1, totalLoanAgreementAmountForConsultantAtPreviousStage ?? 0));
                }
                db.Complete();
            }
            //make call to projection service
        }

        public void Handle(AppProgressInValuationLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            var currentStage = "Valuations";
            var previousStage = "LOA";
            using (var db = new Db().InAppContext())
            {
                var applicationsAtPreviousStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = previousStage });
                var applicationsAtCurrentStage = db.SelectWhere<SalesConsultantDailyPipelineDataModel>("Consultant = @Consultant and ApplicationStage = @ApplicationStage",
                    new { Consultant = @event.AssignedADUser, ApplicationStage = currentStage });

                var offerInformationRecord = db.SelectOneWhere<OfferInformationVariableLoanDataModel>("OfferInformationKey = @OfferInformationKey",
                    new { OfferInformationKey = @event.ApplicationInformationKey });
                if (!applicationsAtCurrentStage.Any())
                {
                    db.Insert<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, 1, 1,
                        offerInformationRecord.LoanAgreementAmount ?? 0));
                }
                else
                {
                    var numberOfApplicationsAtCurrentStage = applicationsAtCurrentStage.First().NumberOfApplicationsAtStage + 1;
                    var numberOfApplicationsAtPreviousStage = applicationsAtPreviousStage.First().NumberOfApplicationsAtStage - 1;
                    var totalLoanAgreementAmountForConsultantAtCurrentStage = applicationsAtCurrentStage.First().SalesValue + offerInformationRecord.LoanAgreementAmount;
                    var totalLoanAgreementAmountForConsultantAtPreviousStage = applicationsAtPreviousStage.First().SalesValue - totalLoanAgreementAmountForConsultantAtCurrentStage;

                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, currentStage, numberOfApplicationsAtCurrentStage,
                        1, totalLoanAgreementAmountForConsultantAtCurrentStage ?? 0));
                    db.Update<SalesConsultantDailyPipelineDataModel>(new SalesConsultantDailyPipelineDataModel(@event.AssignedADUser, previousStage, numberOfApplicationsAtPreviousStage,
                        1, totalLoanAgreementAmountForConsultantAtPreviousStage ?? 0));
                }
                db.Complete();
            }
            //make call to projection service
        }

        public void Handle(AppProgressReturnToManageAppFromValuationLegacyEvent @event, IServiceRequestMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}