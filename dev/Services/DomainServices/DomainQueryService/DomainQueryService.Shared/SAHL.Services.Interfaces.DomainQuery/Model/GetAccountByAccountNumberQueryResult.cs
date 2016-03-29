using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DomainQuery.Model
{
    public class GetAccountByAccountNumberQueryResult
    {
        public int AccountKey { get; set; }

        public double FixedPayment { get; set; }

        public int AccountStatusKey { get; set; }

        public DateTime InsertedDate { get; set; }

        public int? OriginationSourceProductKey { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public int? RRR_ProductKey { get; set; }

        public int? RRR_OriginationSourceKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public int? SPVKey { get; set; }

        public int? ParentAccountKey { get; set; }
    }

}
