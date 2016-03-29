using SAHL.Core.Messaging.Shared;
using SAHL.Core.Services;
using SAHL.Core.Validation;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.CATS.Commands
{
    public class GenerateCatsFileCommand : ServiceCommand, ICATSServiceCommand
    {
        [Required]
        public CATsEnvironment CATsEnvironment { get; protected set; }
        [Required]
        public String OutputFileName { get; protected set; }
        [Required]
        [StringLength(5, ErrorMessage = "The field Profile must be a string with a maximum length of 5.")]
        public string Profile { get; protected set; }
        [Required]
        [Range(1, 99999)]
        public int FileSequenceNo { get; protected set; }
        [Required]
        public DateTime Date { get; protected set; }
        [Required]
        public DateTime ActionedDate { get; protected set; }
        [Required]
        public IEnumerable<PaymentBatch> PaymentBatch { get; protected set; }

        public GenerateCatsFileCommand(CATsEnvironment catsEnvironment
            , String outputFileName
            , string profile, int fileSequenceNo
            , DateTime date, DateTime actionedDate
            , IEnumerable<PaymentBatch> paymentBatch)
        {
            CATsEnvironment = catsEnvironment;
            OutputFileName = outputFileName;
            Profile = profile;
            FileSequenceNo = fileSequenceNo;
            Date = date;
            ActionedDate = actionedDate;
            PaymentBatch = paymentBatch;
        }
    }
}
